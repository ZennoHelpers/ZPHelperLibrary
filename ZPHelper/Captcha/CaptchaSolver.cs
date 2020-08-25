using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using JetBrains.Annotations;
using ZennoLab.CommandCenter;
using ZPHelper.Captcha.CaptchaModule;

namespace ZPHelper.Captcha
{
    [UsedImplicitly]
    public class CaptchaSolver
    {
        private ZPHelper _z;
        private Document _doc;
        private ICaptchaModule _service;

        public CaptchaSolver(ZPHelper zpHelper, Document doc, string serviceName = Services.RuCaptcha)
        {
            _z = zpHelper;
            _doc = doc;
            _service = GetService(serviceName);
        }

        private static ICaptchaModule GetService(string serviceName)
        {
            ICaptchaModule service;

            switch (serviceName)
            {
                case Services.RuCaptcha:
                    service = new RuCaptcha(GetServiceKey(serviceName));
                    break;
//                case AntiCaptcha:
//                    serviceObj = new AntiCaptcha(GetServiceKey(serviceName));;
//                    break;
                default:
                    throw new Exception("Неизвестное имя сервиса разгадки капчи.");
            }

            return service;
        }

        public string SolveCaptcha(string base64) => _service.SolveCaptcha(base64);

        public void SolveReCaptchaByXPath(string xPath)
        {
            HtmlNode targetNode = _z.HtmlNodeHelper.GetHtmlNodeByXPath(xPath);
            HtmlNode frameNode = targetNode.GetChildByXPath(".//iframe[contains(@src, 'https://www.google.com/recaptcha/api2/anchor')]");
            string urlStr = frameNode.GetAttributeValue("src");

            Uri url = new Uri(urlStr);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(url.Query);
            string sitekey = nameValueCollection.Get("k");

            // TODO Узнать точно, как надёжнее получить домен элемента во фрейме!
            string domain = targetNode.DocDomain;

            HtmlNode textareaNode = targetNode.GetChildByXPath(".//textarea[@id='g-recaptcha-response' or @class='g-recaptcha-response' or @name='g-recaptcha-response']");

            textareaNode.Value = _service.SolveReCaptcha(domain, sitekey);
        }

//        public void AutoSolveReCaptcha()
//        {
//            Tab tab = _tab;
//
//            JSHelper.CheckPropertyExist(tab, "window", "___grecaptcha_cfg");
//            string count = /*int.Parse(*/new JSGlobalVar(tab, "___grecaptcha_cfg.count").Value /*)*/;
//            if (count != "1") throw new Exception("___grecaptcha_cfg.count вернул недопустимое значение.");
//
//            JSHelper.Eval(tab,
//            // @formatter:off
//                ""+
//                "let fn = function (obj, props) {" +
//                  "let splitted = props.split('.');" +
//                  "let tmp = obj;" +
//                  "for (let index in splitted) {" +
//                    "let propName = splitted[index];" +
//                    "if (typeof tmp[propName] === 'undefined') {" +
//                      "throw Error('Свойство ' + propName + ' отсутствует.');" +
//                    "}" +
//                    "tmp = tmp[propName];" +
//                  "}" +
//                "};");
//            // @formatter:on
//        }

        private static string GetServiceKey(string serviceName) => GetConfigSettings(serviceName)["key"].Value;

        private static KeyValueConfigurationCollection GetConfigSettings(string serviceName)
        {
            string currentDllConfig = Environment.GetEnvironmentVariable("AppData") + $@"\ZennoLab\Configs\{serviceName}.dll.config";

            ExeConfigurationFileMap map = new ExeConfigurationFileMap {ExeConfigFilename = currentDllConfig};
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            return config.AppSettings.Settings;
        }
    }
}
