using System.Collections.Generic;

namespace ZPHelper.Captcha.CaptchaModule
{
    public interface ICaptchaModule
    {
        string SolveCaptcha(string base64, IReadOnlyDictionary<string, string> additParams = null);

        string SolveReCaptcha(string pageurl, string sitekey, string proxy = null, string proxytype = null, IReadOnlyDictionary<string, string> additParams = null);

        string GetBalance();
    }
}
