using System.Windows;

namespace DockE
{
    public static class ClipboardHelper
    {
        public static void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
