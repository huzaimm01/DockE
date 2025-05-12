using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace DockE.Helpers
{
    public static class HotkeyManager
    {
        private const int HOTKEY_ID = 9000;
        private static HwndSource? _source;
        private static Action? _onHotkey;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const uint MOD_CTRL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint VK_OEM_PERIOD = 0xBE; // "." 

        public static void RegisterHotKey(Action onHotkey)
        {
            _onHotkey = onHotkey;

            var hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            _source = HwndSource.FromHwnd(hwnd);
            _source?.AddHook(HwndHook);

            if (!RegisterHotKey(hwnd, HOTKEY_ID, MOD_CTRL | MOD_SHIFT, VK_OEM_PERIOD))
            {
                MessageBox.Show("Failed to register Ctrl+Shift+.");
            }
        }

        public static void UnregisterHotKey()
        {
            var hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            UnregisterHotKey(hwnd, HOTKEY_ID);
            _source?.RemoveHook(HwndHook);
        }

        private static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;

            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                _onHotkey?.Invoke();
                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}
