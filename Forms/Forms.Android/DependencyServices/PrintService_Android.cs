using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Print;
using Android.Print.Pdf;
using Android.PrintServices;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Forms.DependencyServices;
using Forms.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(PrintService_Android))]
namespace Forms.Droid.DependencyServices
{
    public class PrintService_Android : IPrintService
    {
        public PrintManager PrintManager;
        private IPrintServiceCallBack _callBack;

        public PrintService_Android()
        {
            PrintManager = (PrintManager)MainActivity.Instance.GetSystemService(Context.PrintService);
        }

        public void PrintHtml(string html)
        {
            var webview = new Android.Webkit.WebView(MainActivity.Instance);
            webview.SetWebViewClient(new PrintHTMLWebClient());
            webview.LoadDataWithBaseURL(null, html, "text/HTML", "UTF-8", null);
        }

        public void PrintDocument(string filePath)
        {
            var attr = new PrintAttributes.Builder()
                .SetColorMode(PrintColorMode.Color)
                .SetMediaSize(PrintAttributes.MediaSize.IsoA4)
                .Build();
            var jobName = Android.App.Application.Context.PackageName + ".PrintDocument";
            var job = PrintManager.Print(jobName, new PrintAdapter(filePath), null);
            PrintManager.PrintJobs.Add(job);
        }

        public void SetPrintServiceCallBack(IPrintServiceCallBack callBack)
        {
            _callBack = callBack;
        }
    }

    public class PrintHTMLWebClient : WebViewClient
    {
        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
        {
            return false; // base.ShouldOverrideUrlLoading(view, url);
        }

        public override void OnPageFinished(Android.Webkit.WebView webView, string url)
        {
            base.OnPageFinished(webView, url);

            var printManager = (PrintManager)webView.Context.GetSystemService(Context.PrintService);
            string jobName = webView.Context.PackageName + ".PrintHtml";
            PrintDocumentAdapter printAdapter = webView.CreatePrintDocumentAdapter(jobName);
            Android.Print.PrintJob printJob = printManager.Print(jobName, printAdapter, new PrintAttributes.Builder().Build());
            printManager.PrintJobs.Add(printJob);
        }
    }

    public class PrintAdapter : PrintDocumentAdapter
    {
        private string _filePath;
        private IPrintServiceCallBack _callBack;
        private int _totalpages;
        private int _pageHeight;
        private int _pageWidth;

        public PrintAdapter(string filePath) : base()
        {
            _filePath = filePath;
        }

        public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes, CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
        {
            if (cancellationSignal.IsCanceled)
            {
                callback.OnLayoutCancelled();
                return;
            }

            ParcelFileDescriptor mFileDescriptor = null;
            PdfRenderer pdfRender = null;

            try
            {
                mFileDescriptor = ParcelFileDescriptor.Open(new Java.IO.File(_filePath), ParcelFileMode.ReadOnly);
                if (mFileDescriptor != null)
                    pdfRender = new PdfRenderer(mFileDescriptor);

                _totalpages = pdfRender.PageCount;
            }
            catch (FileNotFoundException e)
            {

            }
            catch (IOException e)
            {

            }
            finally
            {
                if (null != mFileDescriptor)
                    mFileDescriptor.Close();
                if (null != pdfRender)
                    pdfRender.Close();
            }

            if (_totalpages > 0)
            {
                PrintDocumentInfo.Builder builder = new PrintDocumentInfo
                        .Builder("快速入门.pdf")
                        .SetContentType(PrintContentType.Document)
                        .SetPageCount(_totalpages);  //构建文档配置信息

                PrintDocumentInfo info = builder.Build();
                callback.OnLayoutFinished(info, true);
            }
            else
            {
                callback.OnLayoutFailed("Page count is zero.");
            }
        }

        public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination, CancellationSignal cancellationSignal, WriteResultCallback callback)
        {
            ;
        }
    }
}