using Android.Graphics.Pdf;
using Android.Util;
using Android.Webkit;
using Forms.DependencyServices;
using Forms.Droid.DependencyServices;
using Java.IO;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(PDFDependencyService_Android))]
namespace Forms.Droid.DependencyServices
{
    public class PDFDependencyService_Android : Java.Lang.Object, IPdfService
    {
        public PDFDependencyService_Android() { }

        public string ConvertHtmlToPDF(string html, string fileName)
        {
            var dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Download/");
            var file = new Java.IO.File(dir + "/" + fileName + ".pdf");

            if (!dir.Exists())
                dir.Mkdirs();

            int x = 0;
            while (file.Exists())
            {
                x++;
                file = new Java.IO.File(dir + "/" + fileName + "(" + x + ")" + ".pdf");
            }

            var webpage = new Android.Webkit.WebView(MainActivity.Instance);
            //var windowManager = MainActivity.Instance.GetSystemService(Android.Content.Context.WindowService);
            //DisplayMetrics outMetrics = new DisplayMetrics();
            //windowManager.DefaultDisplay.GetMetrics(outMetrics);
            //int widthPixels = outMetrics.WidthPixels;
            //int heightPixels = outMetrics.HeightPixels;

            //int width = widthPixels;
            //int height = heightPixels;
            int width = 2102;
            int height = 2973;

            webpage.Layout(0, 0, width, height);
            //webpage.SetWebViewClient(new WebViewCallBack(file.ToString()));
            webpage.SetWebViewClient(new WebViewCallBack(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Download/test.pdf"));
            webpage.LoadDataWithBaseURL("", html, "text/html", "UTF-8", null);

            //return file.ToString();
            return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Download/test.pdf";
        }

        class WebViewCallBack : WebViewClient
        {
            string fileNameWithPath = null;

            public WebViewCallBack(string path)
            {
                this.fileNameWithPath = path;
            }

            public override void OnPageFinished(Android.Webkit.WebView myWebview, string url)
            {
                PdfDocument document = new PdfDocument();
                PdfDocument.Page page = document.StartPage(new PdfDocument.PageInfo.Builder(2120, 3000, 1).Create());

                myWebview.Draw(page.Canvas);
                document.FinishPage(page);

                Stream filestream = null;
                FileOutputStream fos = null;
                try
                {
                    filestream = new MemoryStream();
                    fos = new Java.IO.FileOutputStream(fileNameWithPath, false);
                    document.WriteTo(filestream);
                    fos.Write(((MemoryStream)filestream).ToArray(), 0, (int)filestream.Length);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (null != filestream)
                        filestream.Close();

                    if (null != fos)
                        fos.Close();
                }

                document.Close();
            }
        }
    }
}