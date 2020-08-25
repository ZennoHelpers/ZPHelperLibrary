using System;

namespace ZPHelper
{
    public enum LogLevel
    {
        Normal,
        Warning,
        Error
    }

    public enum PageState
    {
        FullDownload,
        Interactive,
        PreComplete
    }

    [Flags]
    public enum BrowserFeatures
    {
        // @formatter:off
        LoadPictures = 1,   // картинки
        DownloadActiveX = 2, // загрузка ActiveX
        RunActiveX = 4,       // запуск ActiveX
        DownloadFrame = 8,     // фреймы
        UseCSS = 16,            // CSS
      //UseJavaScripts = 32,     // JS
        UseJsFeatures = 64,       // browsers emulation
        UseJavaApplets = 128,      // java апплеты
        UsePlugins = 256,           // плагины
        UsePluginsForceWmode = 512,  // плагины wmode
        DownloadVideos = 1024,       // видео
        AllowPopUp = 2048,           // popups
        BackGroundSoundsPlay = 4096, // звуки
        UseAdds = 8192,              // реклама
        UseMedia = 16384,            // медиа
        UseGeoposition = 32768,      // geoposition
        AllowNotification = 262144,  // notifications
        IgnoreAjaxRequests = 524288, // AJAX запросы
        IgnoreFrameRequests = 1048576, // Frame запросы
        IgnoreFlashRequests = 2097152, // Flash запросы
        IgnoreAdditionalRequests = 4194304 // доп. запросы
        // @formatter:on
    }

//    public enum WaitDownloadType
//    {
//        Download,
//        Interactive,
//        PreComplete
//    }
}
