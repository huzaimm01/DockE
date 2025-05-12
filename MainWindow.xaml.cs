using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DockE.Helpers;

namespace DockE
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Apply blue (visionOS)
            DwmBlurHelper.EnableBlur(this);

            // Allow drag-
            MouseDown += (s, e) =>
            {
                if (e.ChangedButton == MouseButton.Left)
                    DragMove();
            };

            Loaded += (_, __) =>
            {
                LoadEmojis();
                HotkeyManager.RegisterHotKey(ShowDock);
            };
        }

        private void LoadEmojis()
        {
            try
            {
                if (FindResource("EmojiList") is string[] emojis)
                {
                    foreach (var emoji in emojis)
                    {
                        var btn = new Button
                        {
                            Content = emoji,
                            Style = (Style)FindResource("EmojiButtonStyle")
                        };
                        btn.Click += Emoji_Click;
                        EmojiPanel.Children.Add(btn);
                    }
                }
                else
                {
                    MessageBox.Show("EmojiList not found in resources.", "DockE Resource Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load emojis:\n{ex.Message}", "DockE Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Emoji_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is string emoji)
            {
                ClipboardHelper.CopyToClipboard(emoji);
               
            }
        }

        private void CloseToTray_Click(object sender, RoutedEventArgs e)
        {
            this.Hide(); // Hide. Helps with Shortcut
        }

        protected override void OnClosed(EventArgs e)
        {
            HotkeyManager.UnregisterHotKey();
            base.OnClosed(e);
        }

        public static void ShowDock()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Avoid multiple windows (sometimes happened in early testing)
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is MainWindow existing)
                    {
                        existing.Show();
                        existing.Activate();
                        return;
                    }
                }

                var window = new MainWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                window.Show();
            });
        }
    }
}
