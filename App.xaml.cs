using System.Windows;
using DockE.Helpers;

namespace DockE
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Start tray icon service
            TrayIcon.Initialize();

           
            var window = new MainWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.Show();
        }
    }
}
