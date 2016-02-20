using CefSharp;
using System.Windows;
using System.IO;
using System;

namespace WebExpress
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string Webexpresspath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebExpress");

        public string Userdatapath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebExpress\\user data");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!Directory.Exists(Webexpresspath))
            {
                Directory.CreateDirectory(Webexpresspath);
                Directory.CreateDirectory(Userdatapath);
            }
            if (!Directory.Exists(Userdatapath))
            {
                Directory.CreateDirectory(Userdatapath);
            }
            if (!System.IO.Directory.Exists("Extensions"))
            {
                System.IO.Directory.CreateDirectory("Extensions");
            }
            var cefSettings = new CefSettings();
            cefSettings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebExpress\\user data\\Cache");
            cefSettings.SetOffScreenRenderingBestPerformanceArgs();
            cefSettings.UserDataPath = Userdatapath;
            cefSettings.BrowserSubprocessPath = "WebExpress.exe";
            cefSettings.PersistSessionCookies = true;

            //Make sure all dependencies are present when the application runs, may wish to include a nicer error message
            Cef.Initialize(cefSettings, shutdownOnProcessExit:true, performDependencyCheck:true);
        }
    }
}
