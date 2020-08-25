using System;
using JetBrains.Annotations;
using ZennoLab.CommandCenter;

namespace ZPHelper
{
    // ReSharper disable once InconsistentNaming
    public static partial class JSHelper
    {
        #region For JS Property

        #region Creating

        public static string CreateProperty(Document doc, string jsObjVar, string jsPropName,
            [ValueProvider("ZymlexHelper.ZP.JSValue")] string value = JSValue.Null,
            [ValueProvider("ZymlexHelper.ZP.JSBool")] string configurable = JSBool.True,
            [ValueProvider("ZymlexHelper.ZP.JSBool")] string writable = JSBool.True,
            [ValueProvider("ZymlexHelper.ZP.JSBool")] string enumerable = JSBool.True)
        {
            Eval(doc, $"Object.defineProperty({jsObjVar}, '{jsPropName}', {{configurable: {configurable}, enumerable: {enumerable}, writable: {writable}, value: {value}}});");

            if (!IsPropertyExist(doc, jsObjVar, jsPropName))
                throw new JSPropertyNotExistException($"Свойство \"{jsPropName}\" не было создано в объекте \"{jsObjVar}\".");

            return jsObjVar + "." + jsPropName;
        }

        public static string CreateUniqueProperty(Document doc, Random rnd, string jsObjVar,
            [ValueProvider("ZymlexHelper.ZP.JSValue")] string value = JSValue.Null,
            [ValueProvider("ZymlexHelper.ZP.JSBool")] string configurable = JSBool.True,
            [ValueProvider("ZymlexHelper.ZP.JSBool")] string writable = JSBool.True,
            [ValueProvider("ZymlexHelper.ZP.JSBool")] string enumerable = JSBool.True)
        {
            string jsPropName = GetUniquePropName(doc, jsObjVar, rnd);
            return CreateProperty(doc, jsObjVar, jsPropName, value: value, configurable: configurable, writable: writable, enumerable: enumerable);
        }

        public static string GetUniquePropName(Document doc, string jsObjVar, Random rnd)
        {
            for (int i = 0; i < 20; i++)
            {
                string jsPropName = Text.GetRandomString(new[] {"En"}, rnd, rnd.Next(8, 12));
                if (!IsPropertyExist(doc, jsObjVar, jsPropName)) return jsPropName;
            }

            throw new Exception("Не удалось подобрать свободное имя свойству.");
        }

        #endregion

        #region Check exist

        public static bool IsPropertyExist(Document doc, string jsObjVar, string jsPropName)
            => bool.Parse(Eval(doc, $@"return {jsObjVar}.hasOwnProperty('{jsPropName}')"));

        public static void CheckPropertyExist(Document doc, string jsObjVar, string jsPropName)
        {
            if (!IsPropertyExist(doc, jsObjVar, jsPropName)) throw new JSPropertyNotExistException(jsObjVar, jsPropName);
        }

        #endregion

        public static string ModifyProperty(Document doc, string jsObjVar, string jsPropName,
            [ValueProvider("ZymlexHelper.ZP.JSBool")] string configurable = null,
            [ValueProvider("ZymlexHelper.ZP.JSBool")] string writable = null,
            [ValueProvider("ZymlexHelper.ZP.JSBool")] string enumerable = null, string value = null)
        {
            string descriptor = string.Empty;

            Check(ref descriptor, configurable, nameof(configurable));
            Check(ref descriptor, writable, nameof(writable));
            Check(ref descriptor, enumerable, nameof(enumerable));
            Check(ref descriptor, value, nameof(value));

            void Check(ref string str, string paramValue, string paramName)
            {
                if (paramValue == null) return;

                CommaForNotEmptyStr(ref str);
                str += paramName + ": " + paramValue;
            }

            void CommaForNotEmptyStr(ref string str)
            {
                if (str.Length > 0) str += ", ";
            }

            Eval(doc, $"Object.defineProperty({jsObjVar}, '{jsPropName}', {{{descriptor}}});");
            return jsObjVar + "." + jsPropName;
        }

        public static bool TryDeleteProperty(Document doc, string jsObjVar, string jsPropName)
            => bool.Parse(Eval(doc, $"return delete {jsObjVar}['{jsPropName}'];"));

        #endregion
    }
}
