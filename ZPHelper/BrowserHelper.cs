using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.Enums.Browser;
using ZPHelper.Captcha;

namespace ZPHelper
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BrowserHelper
    {
        private readonly ZPHelper _z;
        private HtmlNodeHelper _htmlNodeHelper;
        private KeyboardHelper _keyboardHelper;
        private MouseHelper _mouseHelper;
        private CaptchaSolver _captchaSolver;

        public Instance Instance { get; }

        public Tab ActiveTab => Instance.ActiveTab;

        internal HtmlNodeHelper HtmlNodeHelper => _htmlNodeHelper ??= new HtmlNodeHelper(_z, ActiveTab.MainDocument /*, heAttempts: 10, heDelay: 1000*/);
        internal KeyboardHelper KeyboardHlp => _keyboardHelper ??= new KeyboardHelper(_z, langNum: 0, inputLatency: (300, 500));
        internal MouseHelper MouseHlp => _mouseHelper ??= new MouseHelper(_z, pctOffset: (0, 0), pctScatter: (75, 75));
        internal CaptchaSolver CaptchaSolver => _captchaSolver ??= new CaptchaSolver(_z, ActiveTab.MainDocument);

        internal BrowserHelper(ZPHelper zpHelper, Instance instance)
        {
            _z = zpHelper;
            Instance = instance;
        }

        private Tab CheckTab(Tab tab) => (!tab.IsVoid | !tab.IsNull) ? tab : throw new Exception("Tab недействителен.");

        public void SetBrowser(BrowserType browser)
        {
            Instance.Launch(browser, true);
        }

        public void OpenSite(string url) => OpenSite(url, url);

        [UsedImplicitly]
        public void OpenSite(string url, string referer)
        {
            Tab tab = ActiveTab;
            tab.Navigate(url, referer);
            if (tab.IsBusy) tab.WaitDownloading();

            _z.Log.SendDebugMsg($"URL: '{url}',\nRef: '{referer}'");
        }

        public void ReloadPage() => JSUtils.ReloadPage(ActiveTab.MainDocument);

        public void WaitPageLoad() => ActiveTab.WaitDownloading();

        public void ChangeBrowser(BrowserType browser, bool useProfile = true)
            => Instance.Launch(browser, useProfile);

        public void ClearCookieAndCache(string domainFilter = null)
        {
            Instance.ClearCache(domainFilter);
            Instance.ClearCookie(domainFilter);
        }

        public void DisableSendPings()
        {
            ChangeBrowserPrefs(new Dictionary<string, object>
            {
                {"network.http.speculative-parallel-limit", 0},
                {"browser.send_pings", false},
                {"browser.send_pings.require_same_host", false},
                {"browser.send_pings.max_per_link", 0}
            });
        }

        public void DisableOCSP()
        {
            ChangeBrowserPrefs(new Dictionary<string, object>
            {
                {"security.OCSP.enabled", 0},
                {"security.OCSP.require", false},
                {"security.ssl.enable_ocsp_stapling", false},
                {"security.ssl.enable_ocsp_must_staple", false}
            });
        }

        public void DisableSCP()
        {
            ChangeBrowserPrefs(new Dictionary<string, object>
            {
                {"security.csp.enable", false},
                {"security.csp.enableStrictDynamic", false}
            });
        }

        public void DisableAllCache()
        {
            ChangeBrowserPrefs(new Dictionary<string, object>
            {
                {"browser.cache.disk.enable", false},
                {"browser.cache.offline.enable", false},
                {"browser.cache.memory.enable", false}
            });
        }

        [UsedImplicitly]
        public void ChangeBrowserPrefs(ICollection<KeyValuePair<string, object>> prefs)
        {
            foreach (KeyValuePair<string, object> pref in prefs)
            {
                ChangeBrowserPref(pref.Key, pref.Value);
            }
        }

        [UsedImplicitly]
        public void ChangeBrowserPref(string name, object value)
        {
            bool resultOk = Instance.SetBrowserPreference(name, value);
            if (!resultOk)
                throw new Exception($"Параметр '{name}' со значением '{value}' не был установлен.");
        }

        public void DisableAllFeatures(BrowserFeatures ignore = 0)
        {
            Instance instance = Instance;

            // @formatter:off
            instance.LoadPictures         = ignore.HasFlag(BrowserFeatures.LoadPictures);         // картинки
            instance.DownloadActiveX      = ignore.HasFlag(BrowserFeatures.DownloadActiveX);      // загрузка ActiveX
            instance.RunActiveX           = ignore.HasFlag(BrowserFeatures.RunActiveX);           // запуск ActiveX
            instance.DownloadFrame        = ignore.HasFlag(BrowserFeatures.DownloadFrame);        // фреймы
            instance.UseCSS               = ignore.HasFlag(BrowserFeatures.UseCSS);               // CSS
          //instance.UseJavaScripts       = ignore.HasFlag(BrowserFeatures.UseJavaScripts);       // JS
            instance.UseJsFeatures        = ignore.HasFlag(BrowserFeatures.UseJsFeatures);        // browsers emulation
            instance.UseJavaApplets       = ignore.HasFlag(BrowserFeatures.UseJavaApplets);       // java апплеты
            instance.UsePlugins           = ignore.HasFlag(BrowserFeatures.UsePlugins);           // плагины
            instance.UsePluginsForceWmode = ignore.HasFlag(BrowserFeatures.UsePluginsForceWmode); // плагины wmode
            instance.DownloadVideos       = ignore.HasFlag(BrowserFeatures.DownloadVideos);       // видео
            instance.AllowPopUp           = ignore.HasFlag(BrowserFeatures.AllowPopUp);           // popups
            instance.BackGroundSoundsPlay = ignore.HasFlag(BrowserFeatures.BackGroundSoundsPlay); // звуки
            instance.UseAdds              = ignore.HasFlag(BrowserFeatures.UseAdds);              // реклама
            instance.UseMedia             = ignore.HasFlag(BrowserFeatures.UseMedia);             // медиа
            instance.UseGeoposition       = ignore.HasFlag(BrowserFeatures.UseGeoposition);       // geoposition
            instance.AllowNotification    = ignore.HasFlag(BrowserFeatures.AllowNotification);    // notifications

            // Настройки игнорирования запросов
            instance.IgnoreAjaxRequests    = !ignore.HasFlag(BrowserFeatures.IgnoreAjaxRequests); // AJAX запросы
            instance.IgnoreFrameRequests    = !ignore.HasFlag(BrowserFeatures.IgnoreFrameRequests); // Frame запросы
            instance.IgnoreFlashRequests     = !ignore.HasFlag(BrowserFeatures.IgnoreFlashRequests); // Flash запросы
            instance.IgnoreAdditionalRequests = !ignore.HasFlag(BrowserFeatures.IgnoreAdditionalRequests); // доп. запросы
            // @formatter:on
        }


//        public void LoadPictures(bool isAllow = true)
//        {
//            _z.Instance.LoadPictures = isAllow; // картинки
//        }
//
//        public void DownloadActiveX(bool isAllow = true)
//        {
//            _z.Instance.DownloadActiveX = isAllow; // загрузка ActiveX
//        }
//
//        public void RunActiveX(bool isAllow = true)
//        {
//            _z.Instance.RunActiveX = isAllow; // запуск ActiveX
//        }
//
//        public void DownloadFrame(bool isAllow = true)
//        {
//            _z.Instance.DownloadFrame = isAllow; // фреймы
//        }
//
//        public void UseCSS(bool isAllow = true)
//        {
//            _z.Instance.UseCSS = isAllow; // CSS
//        }
//
//        public void UseJavaScripts(bool isAllow = true)
//        {
//            _z.Instance.UseJavaScripts = isAllow;
//        }
//
//        public void UseJavaApplets(bool isAllow = true)
//        {
//            _z.Instance.UseJavaApplets = isAllow; // java апплеты
//        }
//
//        public void UsePlugins(bool isAllow = true)
//        {
//            _z.Instance.UsePlugins = isAllow; // плагины
//        }
//
//        public void UsePluginsForceWmode(bool isAllow = true)
//        {
//            _z.Instance.UsePluginsForceWmode = isAllow; // плагины wmode
//        }
//
//        public void DownloadVideos(bool isAllow = true)
//        {
//            _z.Instance.DownloadVideos = isAllow; // видео
//        }
//
//        public void AllowPopUp(bool isAllow = true)
//        {
//            _z.Instance.AllowPopUp = isAllow; // попапы
//        }
//
//        public void BackGroundSoundsPlay(bool isAllow = true)
//        {
//            _z.Instance.BackGroundSoundsPlay = isAllow; // звуки
//        }
//
//        public void Adds(bool isAllow = true)
//        {
//            _z.Instance.UseAdds = isAllow; // реклама
//        }
//
//        public void Media(bool isAllow = true)
//        {
//            _z.Instance.UseMedia = isAllow; // медиа
//        }
//
//        public void Geoposition(bool isAllow = true)
//        {
//            _z.Instance.UseGeoposition = isAllow;
//        }
//
//        public void WaitAjaxRequests(bool isAllow = true)
//        {
//            _z.Instance.IgnoreAjaxRequests = isAllow; // AJAX запросы
//        }
//
//        public void WaitFrameRequests(bool isAllow = true)
//        {
//            _z.Instance.IgnoreFrameRequests = isAllow; // Frame запросы
//        }
//
//        public void WaitFlashRequests(bool isAllow = true)
//        {
//            _z.Instance.IgnoreFlashRequests = isAllow; // Flash запросы
//        }
//
//        public void WaitAdditionalRequests(bool isAllow = true)
//        {
//            _z.Instance.IgnoreAdditionalRequests = isAllow; // дополнительные запросы
//        }
    }
}
