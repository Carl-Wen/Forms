using Android.Content;
using Android.OS;
using Android.Print;
using Android.Webkit;
using Forms.DependencyServices;
using Forms.Droid.DependencyServices;
using Java.IO;
using Java.Lang;
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
                .SetDuplexMode(DuplexMode.LongEdge)
                .Build();
            var jobName = Android.App.Application.Context.PackageName + ".PrintDocument";
            var job = PrintManager.Print(jobName, new PrintAdapter(filePath), attr);
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

        public PrintAdapter(string filePath) : base()
        {
            _filePath = filePath;
        }

        public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes, CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
        {
            if (cancellationSignal.IsCanceled)
            {
                callback?.OnLayoutCancelled();
            }
            else
            {
                var builder = new PrintDocumentInfo.Builder("filename.pdf");
                builder.SetContentType(PrintContentType.Document)
                    .SetPageCount(PrintDocumentInfo.PageCountUnknown);
                callback.OnLayoutFinished(builder.Build(), !(newAttributes == oldAttributes));
            }
        }

        public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination, CancellationSignal cancellationSignal, WriteResultCallback callback)
        {
            InputStream input = null;
            OutputStream output = null;
            try
            {
                File file = new File(_filePath);
                input = new FileInputStream(file);
                output = new FileOutputStream(destination.FileDescriptor);

                byte[] buf = new byte[16384];
                int size;

                while ((size = input.Read(buf)) >= 0 && !cancellationSignal.IsCanceled)
                {
                    output.Write(buf, 0, size);
                }

                if (cancellationSignal.IsCanceled)
                {
                    callback.OnWriteCancelled();
                }
                else
                {
                    callback.OnWriteFinished(new PageRange[] { PageRange.AllPages });
                }
            }
            catch (Exception e)
            {
                callback.OnWriteFailed(e.Message);
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                try
                {
                    input?.Close();
                    output?.Close();
                }
                catch (IOException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }
    }
}
