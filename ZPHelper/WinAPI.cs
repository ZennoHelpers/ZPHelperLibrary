using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ZPHelper
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class WinAPI
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern short VkKeyScanEx(char ch, IntPtr dwhkl);

        [DllImport("user32.dll")]
        internal static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);
    }
}
