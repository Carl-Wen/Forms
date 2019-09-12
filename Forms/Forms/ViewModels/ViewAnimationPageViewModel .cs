using System;
using System.Windows.Input;
using Forms.DependencyServices;
using Xamarin.Forms;

namespace Forms.ViewModels
{
    public class ViewAnimationPageViewModel : BaseViewModel
    {
        public ICommand TestCommand { get; private set; }

        public ViewAnimationPageViewModel() : base()
        {
            Title = "View Animation";

            TestCommand = new Command(TestCommandAction);
        }

        public void TestCommandAction()
        {
            //Device.OpenUri(new Uri("https://xamarin.com/platform")
            var service = DependencyService.Get<IAppService>();
            service?.Quit();
        }
    }
}