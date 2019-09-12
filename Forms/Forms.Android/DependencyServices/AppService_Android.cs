using System;
using Forms.DependencyServices;
using Forms.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppService_Android))]
namespace Forms.Droid.DependencyServices
{
    public class AppService_Android : IAppService
    {
        public AppService_Android()
        {
        }

        public void Quit()
        {
            MainActivity.Instance?.Finish();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}
