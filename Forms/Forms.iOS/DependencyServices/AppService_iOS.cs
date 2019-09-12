using System;
using System.Diagnostics;
using Forms.DependencyServices;
using Forms.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppService_iOS))]
namespace Forms.iOS.DependencyServices
{
    public class AppService_iOS : IAppService
    {
        public AppService_iOS()
        {
        }

        public void Quit()
        {
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }
    }
}
