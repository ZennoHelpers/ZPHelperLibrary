using System;

namespace ZPHelper
{
    internal class JSNullOrUndefException : Exception
    {
        public JSNullOrUndefException(string jsObjVar)
            : base("Переменная JS '" + jsObjVar + "' равна null или undefined.") { }

        public JSNullOrUndefException(string jsObjVar, Exception inner)
            : base("Переменная JS '" + jsObjVar + "' равна null или undefined.", inner) { }
    }
}
