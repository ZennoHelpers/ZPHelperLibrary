using System;

namespace ZPHelper
{
    // ReSharper disable once InconsistentNaming
    internal class JSPropertyNotExistException : Exception
    {
        public JSPropertyNotExistException(string msg)
            : base(msg) { }

        public JSPropertyNotExistException(string jsObjVar, string jsPropName)
            : base(MsgPrep(jsObjVar, jsPropName)) { }

        public JSPropertyNotExistException(string jsObjVar, string jsPropName, Exception inner)
            : base(MsgPrep(jsObjVar, jsPropName), inner) { }

        private static string MsgPrep(string jsObjVar, string jsPropName)
            => $"Свойство \"{jsPropName}\" отсутствует в объекте \"{jsObjVar}\".";
    }
}
