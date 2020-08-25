using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using ZennoLab.CommandCenter;
using ZPHelper.Captcha;
using ZPHelper.Exceptions;

namespace ZPHelper
{
    public class HtmlNode
    {
        private readonly ZPHelper _z;
        private readonly HtmlNodeHelper _htmlNodeHelper;
        private readonly Document _nodeDoc;
        private readonly string _jsNodeVar;

        private Document NodeTab => CheckTab() ? _nodeDoc : throw new InvalidHtmlNodeException("Попытка обращения к HtmlNode в невалидной вкладке.");

        private string JsNodeVar => CheckJSNodeVarExist() ? _jsNodeVar : throw new InvalidHtmlNodeException(_jsNodeVar);

        public bool IsValid => CheckTab() || CheckJSNodeVarExist();

        internal HtmlNode(ZPHelper zpHelper, Document doc, string jsNodeVar)
        {
            _z = zpHelper;
            _htmlNodeHelper = zpHelper.HtmlNodeHelper;
            _nodeDoc = doc;
            _jsNodeVar = jsNodeVar;
        }

        // ReSharper disable once InconsistentNaming
        private bool CheckJSNodeVarExist() => JSHelper.IsVariableExist(NodeTab, _jsNodeVar);

        // ReSharper disable once InconsistentNaming
        private bool CheckTab() => !(_nodeDoc.IsVoid || _nodeDoc.IsNull);


        public string TextContent => JSHelper.GetTextContent(NodeTab, JsNodeVar);

        public string InnerHtml
        {
            get => JSHelper.GetNodeInnerHtml(NodeTab, JsNodeVar);
            set => JSHelper.SetNodeInnerHtml(NodeTab, JsNodeVar, value);
        }

        public string OuterHtml
        {
            // get only snapshot state
            get => JSHelper.GetNodeOuterHtml(NodeTab, JsNodeVar);
            set => JSHelper.SetNodeOuterHtml(NodeTab, JsNodeVar, value);
        }

        public string Name => JSHelper.GetNodeName(NodeTab, JsNodeVar);

        public string Type => JSHelper.GetNodeType(NodeTab, JsNodeVar);

        public string TagName => JSHelper.GetNodeTagName(NodeTab, JsNodeVar);

        public string Value
        {
            get => JSHelper.GetNodeValue(NodeTab, JsNodeVar);
            set => JSHelper.SetNodeValue(NodeTab, JsNodeVar, value);
        }

        public bool HasAttributes => JSHelper.IsNodeAttributesExist(NodeTab, JsNodeVar);

        public string DocDomain => JSHelper.GetNodeDocDomain(NodeTab, JsNodeVar);

        public string GetAttributeValue(string attrName) => JSHelper.GetAttributeValue(NodeTab, JsNodeVar, attrName);

        #region By xPath

        public HtmlNode GetChildByXPath(string xPath)
            => _htmlNodeHelper.GetHtmlNodeByXPath(xPath, JsNodeVar);

        public bool TryGetChildByXPath(string xPath, out HtmlNode htmlNode)
            => _htmlNodeHelper.TryGetHtmlNodeByXPath(xPath, out htmlNode, JsNodeVar);

        #endregion

        public HtmlNode GetChildById(string id)
        {
            Document doc = NodeTab;

            string jsNewNodeVar = _htmlNodeHelper.CreateJSNodeVar();
            if (_htmlNodeHelper.CheckResult(doc, JsNodeVar, out HtmlNode htmlNode)) return new HtmlNode(_z, doc, jsNewNodeVar);
            throw new HtmlNodeNotFoundException("Id", id);
        }

        internal bool TryGetChildNode(string jsParentNodeVar, out HtmlNode childHtmlNode)
        {
            Document doc = NodeTab;

            string jsChildNodeVar = _htmlNodeHelper.CreateJSNodeVar();
            JSHelper.GetFirstChild(doc, jsParentNodeVar, jsChildNodeVar);

            return _htmlNodeHelper.CheckResult(doc, jsChildNodeVar, out childHtmlNode);
        }

        public void GetBoundingClientRect(out double pageX, out double pageY, out double width, out double height, out double screenX, out double screenY, out double bottom, out double right)
            => JSUtils.GetNodeBoundingClientRect(NodeTab, JsNodeVar, out pageX, out pageY, out width, out height, out screenX, out screenY, out bottom, out right);

        public void WaitImageLoad(int loadAttempts = 3, int waitAttempts = 5, int sleepTime = 500)
        {
            Document doc = NodeTab;
            string jsNodeVar = JsNodeVar;

            for (int i = 1, x = 1; x <= waitAttempts & i <= loadAttempts; i++)
            {
                if (JSHelper.GetImageLoadStatus(doc, jsNodeVar)) return;
                if (i == loadAttempts)
                {
                    JSHelper.ReloadImage(doc, jsNodeVar);
                    x++;
                }

                Thread.Sleep(sleepTime * i);
            }

            throw new Exception("Изображение на странице не прогрузилось.");
        }

        public string ToBase64() => JSUtils.ConvertImageToBase64(NodeTab, _jsNodeVar);

        public string SolveCaptcha()
        {
            WaitImageLoad();
            string base64 = ToBase64();

            return _z.CaptchaSolver.SolveCaptcha(base64);
        }
    }
}
