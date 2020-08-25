using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using JetBrains.Annotations;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.Enums.Browser;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZPHelper;

namespace ZPHelper
{
    public class KeyboardHelper
    {
        private readonly ZPHelper _z;

        // Для определения нажатия shift'а
        //private LayoutHelper _layoutHelper;
        //private readonly StringCollection _langs; // языки
        private int _langNum; // Номер текущего языка ввода

        private readonly (int min, int max) _inputLatency;

        public KeyboardHelper(ZPHelper zpHelper, /*StringCollection langs,*/ int langNum, (int min, int max) inputLatency)
        {
            _z = zpHelper;

            //_langs = langs;
            _langNum = langNum;

            _inputLatency = inputLatency;
        }

//        private LayoutHelper LayoutHelper
//        {
//            get => _layoutHelper ?? (_layoutHelper = new LayoutHelper(_langs[_langNum]));
//            set
//            {
//                if (_layoutHelper != null)
//                    throw new Exception($"Попытка переинициализировать свойство {nameof(LayoutHelper)}");
//                _layoutHelper = value;
//            }
//        }

        //public void SwitchLayout([NotNull] string lang) { }

//        public void SwitchLayout()
//        {
//            int langsCount = _langs.Count;
//            bool a = langsCount == 1;
//
//            if (!a)
//            {
//                if (langsCount == _langNum)
//                    _langNum = 0;
//                else
//                    _langNum++;
//
//
//                LayoutHelper.SetLayout(_langs[_langNum]);
//            }
//
//            _z.Log.SendDebugMsg("Изменение раскладки " + (a ? "не " : "") + "произведено.\n" +
//                                      string.Join("\n", _langs));
//        }

//        public void SwitchLayoutEmulation(int min, int max)
//        {
//            bool modKey = _z.Rnd.NextDouble() >= 0.5;
//            bool keyEvent = true;
//
//            for (int i = 0; i <= 1; i++, keyEvent = !keyEvent)
//            {
//                ModifierKeyEvent(modKey, keyEvent);
//                InputWait();
//                ModifierKeyEvent(!modKey, keyEvent);
//                InputWait();
//            }
//
//            void ModifierKeyEvent(bool a, bool b) => InputKey("",
//                a ? KeyModifier.Shift : KeyModifier.Alt,
//                b ? KeyEvent.Down : KeyEvent.Up);
//
//            void InputWait() => Thread.Sleep(_z.Rnd.Next(min, max));
//        }
//
//        public void AddLayout(string layout)
//        {
//            if (string.IsNullOrWhiteSpace(layout)) throw new ArgumentException("Недопустимое значение", layout);
//            _langs.Add(layout);
//        }

        #region InputText

        public void InputTextByXPath(string xPath, string text, int latency = 75)
        {
            _z.Mouse.ClickByXPath(xPath);
            _z.Logic.Pause();
            InputText(text, latency);
        }

        public void InputText(string text, int latency = 75)
        {
            _z.Browser.Instance.SendText(text, latency);
        }
//        public void InputText(string text)
//        {
//            // Получение состояния shift'а и ввод
//            foreach (char ch in text)
//            {
//                // Проверка раскладки и её переключение
//                if (!LayoutHelper.CheckScanCode(ch, out bool shift))
//                {
//                    InputKey(KeyModifier.Shift, KeyModifier.Alt);
//                    SwitchLayout();
//                }
//
//                InputKey(ch.ToString(), shift ? KeyModifier.Shift : KeyModifier.None);
//
//                Thread.Sleep(_z.Rnd.Next(_inputLatency.min,
//                    _inputLatency.max));
//            }
//        }



        public void InputKey(string key,
            [ValueProvider("ZymlexHelper.ZP.KeyModifier")] string keyModifier = KeyModifier.None,
            [ValueProvider("ZymlexHelper.ZP.KeyEvent")] string keyEvent = KeyEvent.Press)
        {
            _z.Browser.ActiveTab.KeyEvent(key, keyEvent, keyModifier);
        }

        #endregion
    }
}
