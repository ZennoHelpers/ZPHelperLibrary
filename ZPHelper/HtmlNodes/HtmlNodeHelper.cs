using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JetBrains.Annotations;
using ZennoLab.CommandCenter;
using ZPHelper;
using ZPHelper.Exceptions;

namespace ZPHelper
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class HtmlNodeHelper
    {
        private readonly ZPHelper _z;
        private readonly string JSNodesStore;

        private readonly Document _workDoc;
        //TODO Проверка кол-ва найденных элементов + проверка их видимости

        internal HtmlNodeHelper(ZPHelper zpHelper, Document doc)
        {
            _z = zpHelper;
            _workDoc = doc;
            JSNodesStore = JSObject.Window;
        }

        internal string CreateJSNodeVar()
            => JSHelper.CreateUniqueProperty(_workDoc, _z.Rnd, JSNodesStore, JSValue.EmptyObject, configurable: JSBool.True, writable: JSBool.True, enumerable: JSBool.False);

        #region ByXPath

//        public void GetNodeByXPathAndSave(string xPath, string elementName, string contextJSNodeVar = JSObject.DocumentElement)
//        {
//            if(_htmlNodeStore.ContainsKey(elementName)) throw new Exception("Элемент с именем \"" + elementName + "\" уже существует.");
//
//            HtmlNode htmlNode = GetHtmlNodeByXPath(xPath, contextJSNodeVar);
//            _htmlNodeStore.Add(elementName, htmlNode);
//            _lastHtmlNodeTarget = elementName;
//        }

        public HtmlNode GetHtmlNodeByXPath(string xPath, string contextJSNodeVar = JSObject.DocumentElement, int attempts = 20, int delay = 500)
        {
            if (TryGetHtmlNodeByXPath(xPath, out HtmlNode htmlNode, contextJSNodeVar, attempts, delay)) return htmlNode;
            throw new HtmlNodeNotFoundException("xPath", xPath);
        }

        public bool TryGetHtmlNodeByXPath(string xPath, out HtmlNode htmlNode, string contextJSNodeVar = JSObject.DocumentElement, int attempts = 20, int delay = 500)
        {
            Document doc = _workDoc;
            string jsXPathResultVar = CreateJSNodeVar();

            for (int i = 0; i < attempts; i++)
            {
                JSHelper.GetXPathResult(doc, jsXPathResultVar, xPath, contextJSNodeVar);
                if (CheckResult(doc, jsXPathResultVar + ".singleNodeValue", out htmlNode)) return true;
                Thread.Sleep(delay);
            }
            htmlNode = null;
            return false;
        }

        #endregion

        #region ById

        public HtmlNode GetHtmlNodeById(string id)
        {
            if (TryGetHtmlNodeById(id, out HtmlNode htmlNode)) return htmlNode;
            throw new HtmlNodeNotFoundException("Id", id);
        }

        public bool TryGetHtmlNodeById(string id, out HtmlNode htmlNode)
        {
            Document doc = _workDoc;

            string jsNodeVar = CreateJSNodeVar();
            JSHelper.GetNodeById(doc, jsNodeVar, id);

            return CheckResult(doc, jsNodeVar, out htmlNode);
        }

        #endregion

        internal bool CheckResult(Document doc, string jsNodeVar, out HtmlNode htmlNode)
        {
            bool result = !JSHelper.IsNullOrUndef(doc, jsNodeVar);
            htmlNode = result ? new HtmlNode(_z, doc, jsNodeVar) : null;

            return result;
        }

        public string SolveCaptchaByXPath(string xPath)
        {
            HtmlNode htmlNode = GetHtmlNodeByXPath(xPath);
            return htmlNode.SolveCaptcha();
        }
    }
}
