using Forms.Extensions;
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
    public partial class ColorAnimationPage : ContentPage
    {
        public ColorAnimationPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            frame.ColorTo(Color.Accent, Color.Green, CallBack, length: 500, easing: Easing.Linear, finished: FinishedCallBack, repeat: () => true);
        }

        private void CallBack(Color color)
        {
            frame.BackgroundColor = color;
        }

        private void FinishedCallBack(Color color, bool finished)
        {
            System.Console.WriteLine(color.ToString() + finished.ToString());
        }
    }
}