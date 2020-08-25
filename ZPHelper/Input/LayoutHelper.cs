using System;
using System.Collections.Generic;
using System.Globalization;

namespace ZPHelper
{
    public class LayoutHelper
    {
        private IntPtr _pointer;

        private readonly Dictionary<string, IntPtr> _intPtrDict;

        internal LayoutHelper(string layout)
        {
            _intPtrDict = new Dictionary<string, IntPtr>();
            SetLayout(layout);
        }

        internal void SetLayout(string layout)
        {
            if (_intPtrDict.Count != 0 && _intPtrDict.TryGetValue(layout, out _pointer)) return;
            GetPointer(layout);
        }

        private void GetPointer(string layout)
        {
            IntPtr pointer = WinAPI.LoadKeyboardLayout(CultureInfo.GetCultureInfo(layout).KeyboardLayoutId.ToString("X8"), 0);
            _pointer = pointer;
            _intPtrDict.Add(layout, pointer);
        }

        internal bool CheckScanCode(char character, out bool shift)
        {
            shift = false;

            short keysStates = WinAPI.VkKeyScanEx(character, _pointer);

            if (keysStates == 0) return false;

            byte[] bytes = BitConverter.GetBytes(keysStates);
            shift = (bytes[1] & 1) == 1;
            return true;
        }
    }
}
