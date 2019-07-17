using CoreGraphics;
using Forms.DependencyServices;
using Forms.iOS.DependencyServices;
using Foundation;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(PDFDependencyService_IOS))]
namespace Forms.iOS.DependencyServices
{
    public class PDFDependencyService_IOS : IPdfService
    {
        public PDFDependencyService_IOS() { }

        public async Task<string> ConvertHtmlToPDF(string html, string filename)
        {
            UIWebView webView = new UIWebView(new CGRect(0, 0, 6.5 * 72, 9 * 72));

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var file = Path.Combine(documents, filename + ".pdf");

            var webDelegate = new WebViewCallBack(file);
            webView.Delegate = webDelegate;
            webView.ScalesPageToFit = true;
            webView.UserInteractionEnabled = false;
            webView.BackgroundColor = UIColor.White;

            var tokenSource = new CancellationTokenSource();
            var task = Task.Run(async () =>
            {
                if (tokenSource.Token.IsCancellationRequested) return;
                while (true)
                {
                    await Task.Delay(1);
                    if (tokenSource.Token.IsCancellationRequested) break;
                }
            }, tokenSource.Token);

            webDelegate.OnPageLoadFinished += (s, e) =>
            {
                tokenSource.Cancel();
            };

            webView.LoadHtmlString(html, null);

            await task;

            return file;
        }

        class WebViewCallBack : UIWebViewDelegate
        {

            string filename = null;
            public event EventHandler OnPageLoadFinished;
            public WebViewCallBack(string path)
            {
                this.filename = path;
            }

            public override void LoadingFinished(UIWebView webView)
            {
                double height, width;
                int header, sidespace;

                width = 595.2;
                height = 841.8;
                header = 10;
                sidespace = 10;

                UIEdgeInsets pageMargins = new UIEdgeInsets(header, sidespace, header, sidespace);
                webView.ViewPrintFormatter.ContentInsets = pageMargins;

                UIPrintPageRenderer renderer = new UIPrintPageRenderer();
                renderer.AddPrintFormatter(webView.ViewPrintFormatter, 0);

                CGSize pageSize = new CGSize(width, height);
                CGRect printableRect = new CGRect(sidespace,
                                  header,
                                  pageSize.Width - (sidespace * 2),
                                  pageSize.Height - (header * 2));
                CGRect paperRect = new CGRect(0, 0, width, height);
                renderer.SetValueForKey(NSValue.FromObject(paperRect), (NSString)"paperRect");
                renderer.SetValueForKey(NSValue.FromObject(printableRect), (NSString)"printableRect");
                NSData file = PrintToPDFWithRenderer(renderer, paperRect);
                File.WriteAllBytes(filename, file.ToArray());

                OnPageLoadFinished?.Invoke(this, new EventArgs());
            }

            private NSData PrintToPDFWithRenderer(UIPrintPageRenderer renderer, CGRect paperRect)
            {
                NSMutableData pdfData = new NSMutableData();
                UIGraphics.BeginPDFContext(pdfData, paperRect, null);

                renderer.PrepareForDrawingPages(new NSRange(0, renderer.NumberOfPages));

                CGRect bounds = UIGraphics.PDFContextBounds;

                for (int i = 0; i < renderer.NumberOfPages; i++)
                {
                    UIGraphics.BeginPDFPage();
                    renderer.DrawPage(i, paperRect);
                }
                UIGraphics.EndPDFContent();

                return pdfData;
            }

        }
    }
}