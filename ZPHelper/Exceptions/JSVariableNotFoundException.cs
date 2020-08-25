using System;

namespace ZPHelper.Exceptions
{
    internal class JSVariableNotFoundException : Exception
    {
        public JSVariableNotFoundException(string jsObjVar)
            : base(MsgPrep(jsObjVar)) { }

        public JSVariableNotFoundException(string jsObjVar, Exception inner)
            : base(MsgPrep(jsObjVar), inner) { }

        private static string MsgPrep(string jsObjVar)
            => "Переменная JS: '" + jsObjVar + "' отсутствует.";
    }
}
