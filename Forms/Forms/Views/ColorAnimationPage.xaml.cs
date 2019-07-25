using Forms.Extensions;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorAnimationPage : ContentPage
    {
        public ICommand TestCommand { get; private set; }

        public ColorAnimationPage()
        {
            InitializeComponent();

            TestCommand = new Command(TestCommandAction);
        }

        private void TestCommandAction()
        {

        }

        private void TapGestureRecognizer_Clicked(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Pressed(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Released(object sender, EventArgs e)
        {

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