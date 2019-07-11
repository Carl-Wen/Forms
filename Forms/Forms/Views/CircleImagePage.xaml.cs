using Forms.DependencyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CircleImagePage : ContentPage, IPrintServiceCallBack
    {
        private static string htmlStr = "<!DOCTYPE html><html><head><title>这是个标题</title></head><body><h1>这是个标题1</h1><h1>这是一个一个简单的HTML</h1><p>Hello World！</p><h1>这是个标题2</h1><h1>这是一个一个简单的HTML</h1><p>Hello World！</p><h1>这是个标题3</h1><h1>这是一个一个简单的HTML</h1><p>Hello World！</p><h1>这是个标题4</h1><h1>这是一个一个简单的HTML</h1><p>Hello World！</p><h1>这是个标题5</h1><h1>这是一个一个简单的HTML</h1><p>Hello World！</p><h1>这是个标题6</h1><h1>这是一个一个简单的HTML</h1><p>Hello World！</p><h1>这是个标题7</h1><h1>这是一个一个简单的HTML</h1><p>Hello World！</p><h1>这是个标题8</h1><h1>这是一个一个简单的HTML</h1><p>Hello World！</p><h1>这是个标题9</h1><h1>这是一个一个简单的HTML</h1><p>Hello World！</p><h1>这是个标题10</h1><h1>这是一个一个简单的HTML</h1><p>Hello World！</p></body></html>";

        public CircleImagePage()
        {
            InitializeComponent();

            var tap1 = new TapGestureRecognizer();
            tap1.Tapped += _tappedHTML;
            imageHtml.GestureRecognizers.Add(tap1);

            var tap2 = new TapGestureRecognizer();
            tap2.Tapped += _tappedDoc;
            imageDoc.GestureRecognizers.Add(tap2);
        }

        public void OnFailed(string errorMsg)
        {
            Console.WriteLine("Print Failed.");
        }

        public void OnFinish()
        {
            Console.WriteLine("Print Finish.");
        }

        private void _tappedHTML(object sender, EventArgs e)
        {
            var printService = DependencyService.Resolve<IPrintService>();
            var pdfService = DependencyService.Resolve<IPdfService>();

            if (null != printService && null != pdfService)
            {
                //var path = pdfService.ConvertHtmlToPDF(htmlStr, "test");
                printService.SetPrintServiceCallBack(this);
                //printService.PrintDocument(path);
                printService.PrintHtml(htmlStr);
            }
        }

        private async void _tappedDoc(object sender, EventArgs e)
        {
            var printService = DependencyService.Resolve<IPrintService>();
            var pdfService = DependencyService.Resolve<IPdfService>();

            if (null != printService && null != pdfService)
            {
                var path = await pdfService.ConvertHtmlToPDF(htmlStr, "test");
                printService.SetPrintServiceCallBack(this);
                printService.PrintDocument(path);
            }
        }
    }
}