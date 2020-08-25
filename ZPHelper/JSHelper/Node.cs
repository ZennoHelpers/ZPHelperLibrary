using JetBrains.Annotations;
using ZennoLab.CommandCenter;

namespace ZPHelper
{
    // ReSharper disable once InconsistentNaming
    public static partial class JSHelper
    {
//        public static void CreateElement(Document doc, string jsVar, string tagName)
//        {
//            Eval(doc, $@"{jsVar} = document.createElement('{tagName}');");
//        }
//
//        public static void AppendChild(Document doc, string jsParentNodeVar, string jsChildNodeVar)
//        {
//            Eval(doc, $@"{jsParentNodeVar}.appendChild({jsChildNodeVar});");
//        }
//
//        public static void SetNodeStyle(Document doc, string jsNodeVar, string styleString)
//        {
//            Eval(doc, $@"{jsNodeVar}.style = `{styleString}`;");
//        }

        public static void GetXPathResult(Document doc, string jsXPathResultVar, string xPath, string contextNode, [ValueProvider("ZymlexHelper.ZP.XPathResultType")] string resultType = "9")
            => Eval(doc, $@"{jsXPathResultVar} = document.evaluate(`{xPath}`, {contextNode}, null, {resultType}, null);");

        public static void GetNodeById(Document doc, string jsNodeVar, string id) => Eval(doc, $@"{jsNodeVar} = document.getElementById('{id}');");

//        public static void GetNodeClientRects(Document doc, string jsNodeVar, out double bottom, out double height, out double left, out double right, out double top, out double width)
//        {
//            string result = Eval(doc, $@"return JSON.stringify({jsNodeVar}.getClientRects(), [...]);");
//
//            JObject o = JObject.Parse(result);
//            // @formatter:off
//            bottom = (double)o["bottom"];
//            height = (double)o["height"];
//            left   = (double)o["left"];
//            right  = (double)o["right"];
//            top    = (double)o["top"];
//            width  = (double)o["width"];
//            // @formatter:on
//        }

        #region InnerHtml

        public static string GetNodeInnerHtml(Document doc, string jsNodeVar) => Eval(doc, $@"return {jsNodeVar}.innerHTML");

        public static void SetNodeInnerHtml(Document doc, string jsNodeVar, string innerHtml) => Eval(doc, $@"{jsNodeVar}.innerHTML = `{innerHtml}`");

        #endregion

        #region OuterHtml

        public static string GetNodeOuterHtml(Document doc, string jsNodeVar) => Eval(doc, $@"return {jsNodeVar}.outerHTML");

        public static void SetNodeOuterHtml(Document doc, string jsNodeVar, string innerHtml) => Eval(doc, $@"{jsNodeVar}.outerHTML = `{innerHtml}`");

        #endregion

        public static string GetTextContent(Document doc, string jsNodeVar) => Eval(doc, $@"return {jsNodeVar}.textContent");

        public static string GetNodeValue(Document doc, string jsNodeVar) => Eval(doc, $@"return {jsNodeVar}.value;");

        public static void SetNodeValue(Document doc, string jsNodeVar, string value) => Eval(doc, $@"{jsNodeVar}.value = `{value}`;");

        public static string GetNodeTagName(Document doc, string jsNodeVar) => Eval(doc, $@"return {jsNodeVar}.tagName;");

        public static string GetNodeName(Document doc, string jsNodeVar) => Eval(doc, $@"return {jsNodeVar}.nodeName;");

        public static string GetNodeType(Document doc, string jsNodeVar) => Eval(doc, $@"return {jsNodeVar}.nodeType;");

        public static string GetNodeDocDomain(Document doc, string jsNodeVar) => Eval(doc, $@"return {jsNodeVar}.ownerDocument.domain");

        public static bool IsNodeAttributesExist(Document doc, string jsNodeVar)
            => bool.Parse(Eval(doc, $@"return {jsNodeVar}.hasAttributes();"));

        public static string GetAttributeValue(Document doc, string jsNodeVar, string attrName)
            => Eval(doc, $@"return {jsNodeVar}.getAttribute('{attrName}');");

        public static void GetFirstChild(Document doc, string jsParentNodeVar, string jsChildNodeVar)
            => Eval(doc, $@"{jsChildNodeVar} = {jsParentNodeVar}.firstChild;");

        public static void GetChildById(Document doc, string childId, string jsParentNodeVar, string jsChildNodeVar)
            => Eval(doc, $@"{jsChildNodeVar} = {jsParentNodeVar}.querySelector(`#{childId}`);");

        public static bool GetImageLoadStatus(Document doc, string jsNodeVar) => bool.Parse(Eval(doc, $@"return {jsNodeVar}.complete;"));

        public static void ReloadImage(Document doc, string jsNodeVar) => Eval(doc, $@"let img = {jsNodeVar}; img.src = img.src;");
    }
}
