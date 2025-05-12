using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace DockE.Helpers
{
    public static class DwmBlurHelper
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmEnableBlurBehindWindow(IntPtr hWnd, ref DWM_BLURBEHIND blurBehind);

        [StructLayout(LayoutKind.Sequential)]
        private struct DWM_BLURBEHIND
        {
            public DWM_BB dwFlags;
            public bool fEnable;
            public IntPtr hRgnBlur;
            public bool fTransitionOnMaximized;
        }

        [Flags]
        private enum DWM_BB : uint
        {
            Enable = 0x00000001,
            BlurRegion = 0x00000002,
            TransitionMaximized = 0x00000004
        }

        public static void EnableBlur(Window window)
        {
            var windowHelper = new WindowInteropHelper(window);
            var hwnd = windowHelper.Handle;

            var blur = new DWM_BLURBEHIND
            {
                fEnable = true,
                dwFlags = DWM_BB.Enable,
                hRgnBlur = IntPtr.Zero,
                fTransitionOnMaximized = true
            };

            DwmEnableBlurBehindWindow(hwnd, ref blur);
        }
    }
}
