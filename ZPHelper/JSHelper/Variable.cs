using System;
using ZennoLab.CommandCenter;

namespace ZPHelper
{
    // ReSharper disable once InconsistentNaming
    public static partial class JSHelper
    {
        public static void ThrowIfNullOrUndef(Document doc, string jsVar)
        {
            if (IsNullOrUndef(doc, jsVar)) throw new JSNullOrUndefException(jsVar);
        }

        public static bool IsNullOrUndef(Document doc, string jsVar)
        {
            string result = String.Empty;
            
            try
            {
                result = Eval(doc, $@"return {jsVar} == null;");
                return bool.Parse(result);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message + "\r\n" + result, e);
            }
        }

        public static void CheckInstanceOf(Document doc, string jsObjVar, string jsClass)
        {
            if (!IsInstanceOf(doc, jsObjVar, jsClass))
                throw new Exception("Элемент " + jsObjVar + " не относится к " + jsClass);
        }

        public static bool IsInstanceOf(Document doc, string jsObjVar, string jsClass) => bool.Parse(Eval(doc, $@"return {jsObjVar} instanceof {jsClass};"));

        public static bool IsVariableExist(Document doc, string jsVar) => bool.Parse(Eval(doc, $"return typeof {jsVar} !== 'undefined';"));
    }
}
