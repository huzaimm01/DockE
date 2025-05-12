using System;
using System.Windows;
using Drawing = System.Drawing;
using Forms = System.Windows.Forms;
using Application = System.Windows.Application;

namespace DockE.Helpers
{
    public static class TrayIcon
    {
        private static Forms.NotifyIcon? _notifyIcon;

        public static void Initialize()
        {
            if (_notifyIcon != null) return;

            _notifyIcon = new Forms.NotifyIcon
            {
                Icon = new Drawing.Icon("Assets/DockE.ico"),
                Visible = true,
                Text = "DockE - Emoji Clipboard",
                ContextMenuStrip = new Forms.ContextMenuStrip()
            };

            _notifyIcon.ContextMenuStrip.Items.Add("Show Dock", null, (_, __) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var dock = new MainWindow
                    {
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };
                    dock.Show();
                });
            });

            _notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (_, __) =>
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
                Application.Current.Shutdown();
            });
        }
    }
}
