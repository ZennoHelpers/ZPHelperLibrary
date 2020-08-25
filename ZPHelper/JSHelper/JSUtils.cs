using System;
using Global.ZennoLab.Json.Linq;
using ZennoLab.CommandCenter;

namespace ZPHelper
{
    // ReSharper disable once InconsistentNaming
    public static class JSUtils
    {
        public static void ReloadPage(Document doc) => JSHelper.Eval(doc, "window.location.reload(true);");

        public static void CreateCanvasField(Document doc, string jsContextVar)
        {
            JSHelper.Eval(doc, @"let canvas = document.createElement('canvas');
            document.body.appendChild(canvas);

            canvas.style.cssText = 'position: absolute; left: 0px; top: 0px;';
            canvas.width = window.innerWidth;
            canvas.height = window.innerHeight;" +
            jsContextVar + " = canvas.getContext('2d');");
        }

//        public static void DrawLine(Document doc, string jsContextVar, Axis axis, int value)
//        {
//            string valueStr = value.ToString();
//            bool a = axis == Axis.X;
//
//            string moveToX = a ? valueStr : "0";
//            string lineToX = a ? valueStr : "99999";
//            string moveToY = a ? "0" : valueStr;
//            string lineToY = a ? "99999" : valueStr;
//
//            JSHelper.Eval(doc, $@"let ctx = {jsContextVar};
//                        ctx.beginPath();
//                        ctx.moveTo({moveToX},{moveToY});
//                        ctx.lineTo({lineToX},{lineToY});
//                        ctx.stroke();");
//        }
// language=JS
        public static void GetNodeBoundingClientRect(Document doc, string jsNodeVar, out double pageX, out double pageY, out double width, out double height, out double screenX, out double screenY, out double bottom, out double right)
        {
            // language=JS
            string result = JSHelper.Eval(doc,
                $@"let node = {jsNodeVar}, range, value;" +
                "if (node instanceof Text) {" +
                  "range = document.createRange();" +
                  "range.selectNode(node);" +
                  "value = range.getClientRects()[0];}" +
                "else value = node.getBoundingClientRect();" +
                "return JSON.stringify({bottom: value.bottom, height: value.height, left: value.left, right: value.right, " +
                "top: value.top, width: value.width, pageY: value.top + pageYOffset, pageX: value.left + pageXOffset}," +
                "['bottom', 'height', 'left', 'right', 'top', 'width', 'pageX', 'pageY']);");

            JObject o = JObject.Parse(result);
            // @formatter:off
            height  = (double)o["height"];
            width   = (double)o["width"];
            screenX = (double)o["left"];
            screenY = (double)o["top"];
            right   = (double)o["right"];
            bottom  = (double)o["bottom"];
            pageX   = (double)o["pageX"];
            pageY   = (double)o["pageY"];
            // @formatter:on
            // Альтернатива парсинга json:
            // JavaScriptSerializer serializer = new JavaScriptSerializer();
            // Dictionary<string, string> Loc = serializer.Deserialize<Dictionary<string, string>>("{'_a':'a'}");
        }

        // language=JS
        public static string ConvertImageToBase64(Document doc, string jsNodeVar)
        {
            return JSHelper.Eval(doc,
                $"const img = {jsNodeVar};\n" +
                 "const canvas = document.createElement('canvas');\n" + // create DOM element without delete
                 "canvas.width = img.width;\n" +
                 "canvas.height = img.height;\n" +
                 "const ctx = canvas.getContext('2d');\n" +
                 "ctx.drawImage(img, 0, 0);\n" +
                 "return canvas.toDataURL('image/png').split(',')[1];");
        }
    }
}
