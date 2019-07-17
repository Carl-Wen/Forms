using Android.Graphics.Pdf;
using Android.OS;
using Android.Util;
using Android.Webkit;
using Forms.DependencyServices;
using Forms.Droid.DependencyServices;
using Java.IO;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Com.Android.DX.Stock;
using Android.Print;
using Java.Lang.Reflect;
using Java.Lang;
using File = Java.IO.File;
using Android.Content;
using Android.Views;

[assembly: Dependency(typeof(PDFDependencyService_Android))]
namespace Forms.Droid.DependencyServices
{
    public class PDFDependencyService_Android : Java.Lang.Object, IPdfService
    {
        public PDFDependencyService_Android() { }

        public async Task<string> ConvertHtmlToPDF(string html, string fileName)
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
            int height = 2937;
            webpage.Layout(0, 0, width, height);

            var client = new WebViewCallBack(file.ToString());
            var tokenSource = new CancellationTokenSource();
            var task = Task.Run(() =>
            {
                if (tokenSource.Token.IsCancellationRequested) return;
                while (true)
                {
                    if (tokenSource.Token.IsCancellationRequested) break;
                }
            }, tokenSource.Token);

            client.OnPageLoadFinished += (s, e) =>
            {
                tokenSource.Cancel();
            };
            webpage.SetWebViewClient(client);
            webpage.LoadDataWithBaseURL("", html, "text/html", "UTF-8", null);

            await task;

            return file.ToString();
        }

        class WebViewCallBack : WebViewClient, IInvocationHandler
        {
            string fileNameWithPath = null;

            public event EventHandler OnPageLoadFinished;

            public PrintAttributes.MediaSize MediaSize = PrintAttributes.MediaSize.IsoA4;
            private ParcelFileDescriptor descriptor;
            private PageRange[] ranges;
            private PrintDocumentAdapter printAdapter;

            public WebViewCallBack(string path)
            {
                this.fileNameWithPath = path;
            }

            public override void OnPageFinished(Android.Webkit.WebView myWebview, string url)
            {
                CreatePDF2(myWebview);
            }

            private async void CreatePDF2(Android.Webkit.WebView webview)
            {
                try
                {
                    // 计算webview打印需要的页数
                    int numberOfPages = await GetPDFPageCount(webview);
                    File pdfFile = new File(fileNameWithPath);
                    if (pdfFile.Exists())
                    {
                        pdfFile.Delete();
                    }
                    pdfFile.CreateNewFile();
                    descriptor = ParcelFileDescriptor.Open(pdfFile, ParcelFileMode.ReadWrite);
                    // 设置打印参数
                    var dm = webview.Context.Resources.DisplayMetrics;
                    var d = dm.Density;
                    var dpi = dm.DensityDpi;
                    var height = dm.HeightPixels;
                    var width = dm.WidthPixels;
                    var xdpi = dm.Xdpi;
                    var ydpi = dm.Ydpi;
                    PrintAttributes attributes = new PrintAttributes.Builder()
                            .SetMediaSize(MediaSize)
                            .SetResolution(new PrintAttributes.Resolution("id", Context.PrintService, Convert.ToInt16(xdpi), Convert.ToInt16(ydpi)))
                            .SetColorMode(PrintColorMode.Color)
                            .SetMinMargins(PrintAttributes.Margins.NoMargins)
                            .Build();

                    ranges = new PageRange[] { new PageRange(0, numberOfPages - 1) };
                    // 创建pdf文件缓存目录
                    // 获取需要打印的webview适配器
                    printAdapter = webview.CreatePrintDocumentAdapter("CreatePDF");
                    // 开始打印
                    printAdapter.OnStart();
                    printAdapter.OnLayout(attributes, attributes, new CancellationSignal(), GetLayoutResultCallback(this), new Bundle());
                }
                catch (Java.IO.FileNotFoundException e)
                {
                    System.Console.WriteLine(e.Message);
                }
                catch (Java.IO.IOException e)
                {
                    System.Console.WriteLine(e.Message);
                }
                catch (Java.Lang.Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }

            public Java.Lang.Object Invoke(Java.Lang.Object proxy, Method method, Java.Lang.Object[] args)
            {
                if (method.Name.Equals("onLayoutFinished"))
                {
                    // 监听到内部调用了onLayoutFinished()方法,即打印成功
                    onLayoutSuccess();
                }
                else if (method.Name.Equals("onWriteFinished"))
                {
                    OnPageLoadFinished?.Invoke(this, new EventArgs());
                }
                else
                {
                    // 监听到打印失败或者取消了打印
                }
                return null;
            }

            private void onLayoutSuccess()
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
                {
                    PrintDocumentAdapter.WriteResultCallback callback = GetWriteResultCallback(this);
                    printAdapter.OnWrite(ranges, descriptor, new CancellationSignal(), callback);
                }
            }

            public static PrintDocumentAdapter.LayoutResultCallback GetLayoutResultCallback(IInvocationHandler invocationHandler)
            {
                return (PrintDocumentAdapter.LayoutResultCallback)ProxyBuilder.ForClass(Java.Lang.Class.FromType(typeof(Android.Print.PrintDocumentAdapter.LayoutResultCallback)))
                    .Handler(invocationHandler)
                    .Build();
            }

            public static PrintDocumentAdapter.WriteResultCallback GetWriteResultCallback(IInvocationHandler invocationHandler)
            {
                return (PrintDocumentAdapter.WriteResultCallback)ProxyBuilder.ForClass(Class.FromType(typeof(PrintDocumentAdapter.WriteResultCallback)))
                        .Handler(invocationHandler)
                        .Build();
            }

            /// <summary>
            /// Note: Not stable.
            /// </summary>
            /// <param name="webview"></param>
            private async void CreatePDF1(Android.Webkit.WebView webview)
            {
                //var pageCount = await GetPDFPageCount(webview);
                PdfDocument document = new PdfDocument();

                PdfDocument.Page page = document.StartPage(new PdfDocument.PageInfo.Builder(2120, 3000, 1).Create());

                await Task.Delay(34);
                webview.Draw(page.Canvas);
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
                catch (Java.Lang.Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
                finally
                {
                    filestream?.Close();
                    fos?.Close();
                }

                document.Close();

                OnPageLoadFinished?.Invoke(this, new EventArgs());
            }

            private async Task<int> GetPDFPageCount(Android.Webkit.WebView webview)
            {
                // 计算webview打印需要的页数
                // 延迟一帧的时间让webview render完成，获得正确的contentHeight
                await Task.Delay(34);

                var dm = webview.Context.Resources.DisplayMetrics;
                var d = dm.Density;
                var dpi = dm.DensityDpi;
                var height = dm.HeightPixels;
                var width = dm.WidthPixels;
                var xdpi = dm.Xdpi;
                var ydpi = dm.Ydpi;

                var scale = webview.Scale;
                var h1 = webview.ContentHeight;
                var h2 = MediaSize.HeightMils;
                var count = h1 / (h2 / 2.54);
                var count1 = System.Math.Ceiling(count);
                var count2 = Java.Lang.Math.Ceil(count);
                return (int)count1;
            }
        }
    }
}