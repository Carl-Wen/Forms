using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Forms.DependencyServices;
using Forms.iOS.DependencyServices;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PrintService_iOS))]
namespace Forms.iOS.DependencyServices
{
    public class PrintService_iOS : IPrintService
    {
        private IPrintServiceCallBack _callBack;

        public PrintService_iOS() { }

        public void PrintDocument(string filePath)
        {
            var printInfo = UIKit.UIPrintInfo.PrintInfo;
            printInfo.Duplex = UIKit.UIPrintInfoDuplex.LongEdge;
            printInfo.OutputType = UIKit.UIPrintInfoOutputType.General;
            printInfo.JobName = "AppPrint";
            var printer = UIKit.UIPrintInteractionController.SharedPrintController;
            printer.PrintInfo = printInfo;
            printer.PrintingItem = Foundation.NSData.FromFile(filePath);
            printer.ShowsPageRange = true;
            printer.Present(true, (handler, completed, err) =>
            {
                if (!completed && err != null)
                {
                    System.Diagnostics.Debug.WriteLine("Printer Error");
                }
            });
        }

        public void PrintHtml(string html)
        {
            var webview = new UIWebView(new CGRect(0f, 0f, UIApplication.SharedApplication.KeyWindow.Screen.Bounds.Width, UIApplication.SharedApplication.KeyWindow.Screen.Bounds.Height));
            webview.LoadFinished += _loadFinished;
            webview.ScalesPageToFit = true;
            webview.LoadHtmlString(html, null);
        }

        private void _loadFinished(object sender, EventArgs e)
        {
            var webview = sender as UIWebView;
            UIPrintInteractionController controller = UIPrintInteractionController.SharedPrintController;
            if (null == controller)
            {
                return;
            }

            // 设置打印机的一些默认信息
            UIPrintInfo printInfo = UIPrintInfo.PrintInfo;
            // 输出类型
            printInfo.OutputType = UIPrintInfoOutputType.General;
            // 打印队列名称
            printInfo.JobName = "HtmlDemo";
            // 是否单双面打印
            printInfo.Duplex = UIPrintInfoDuplex.LongEdge;
            // 设置默认打印信息
            controller.PrintInfo = printInfo;

            // 显示页码范围
            controller.ShowsPageRange = true;

            // 预览设置
            UIPrintPageRenderer myRenderer = new UIPrintPageRenderer();

            // To draw the content of each page, a UIViewPrintFormatter is used.
            // 生成html格式
            UIViewPrintFormatter viewFormatter = webview.ViewPrintFormatter;
            myRenderer.AddPrintFormatter(viewFormatter, 0);
            // 渲染html
            controller.PrintPageRenderer = myRenderer;

            controller.Present(true, (handler, completed, err) =>
            {
                if (!completed && err != null)
                {
                    System.Diagnostics.Debug.WriteLine("Printer Error");
                }
            });
        }

        public void SetPrintServiceCallBack(IPrintServiceCallBack callBack)
        {
            _callBack = callBack;
        }
    }
}