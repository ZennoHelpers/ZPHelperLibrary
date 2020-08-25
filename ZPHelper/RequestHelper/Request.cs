using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.Enums.Http;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace ZPHelper
{
    public static class Request
    {
        #region Get

        public static string Get(
            string url,
            ResponceType respType = ResponceType.BodyOnly,
            string proxy = "",
            string userAgent = "",
            string contentPostingType = "application/x-www-form-urlencoded",
            string encoding = "UTF-8",
            string[] additionalHeaders = null,
            ICookieContainer cookieContainer = null,
            bool useRedirect = true,
            int maxRedirectCount = 5,
            int timeout = 30000,
            string downloadPath = null,
            bool useOriginalUrl = false,
            bool throwExceptionOnError = true)
        {
            return ZennoPoster.HTTP.Request(
                method: HttpMethod.GET,
                url: url,
                contentPostingType: contentPostingType,
                proxy: proxy,
                Encoding: encoding,
                respType: respType,
                Timeout: timeout,
                UserAgent: userAgent,
                UseRedirect: useRedirect,
                MaxRedirectCount: maxRedirectCount,
                AdditionalHeaders: additionalHeaders,
                DownloadPath: downloadPath,
                UseOriginalUrl: useOriginalUrl,
                throwExceptionOnError: throwExceptionOnError,
                cookieContainer: cookieContainer);
        }

        #endregion

        #region Post

        public static string Post(
            string url,
            string content,
            string proxy = "",
            ICookieContainer cookieContainer = null,
            ResponceType respType = ResponceType.BodyOnly,
            string userAgent = "",
            string contentPostingType = "application/x-www-form-urlencoded",
            string encoding = "UTF-8",
            string[] additionalHeaders = null,
            bool useRedirect = true,
            int maxRedirectCount = 5,
            int timeout = 30000,
            string downloadPath = null,
            bool useOriginalUrl = false,
            bool throwExceptionOnError = true)
        {
            return ZennoPoster.HTTP.Request(
                method: HttpMethod.POST,
                url: url,
                content: content,
                contentPostingType: contentPostingType,
                proxy: proxy,
                Encoding: encoding,
                respType: respType,
                Timeout: timeout,
                UserAgent: userAgent,
                UseRedirect: useRedirect,
                MaxRedirectCount: maxRedirectCount,
                AdditionalHeaders: additionalHeaders,
                DownloadPath: downloadPath,
                UseOriginalUrl: useOriginalUrl,
                throwExceptionOnError: throwExceptionOnError,
                cookieContainer: cookieContainer);
        }

        public static string Post(
            string url,
            byte[] content,
            string proxy = "",
            ICookieContainer cookieContainer = null,
            ResponceType respType = ResponceType.BodyOnly,
            string userAgent = "",
            string contentPostingType = "application/x-www-form-urlencoded",
            string encoding = "UTF-8",
            string[] additionalHeaders = null,
            bool useRedirect = true,
            int maxRedirectCount = 5,
            int timeout = 30000,
            string downloadPath = null,
            bool useOriginalUrl = false,
            bool throwExceptionOnError = true)
        {
            return ZennoPoster.HTTP.Request(
                method: HttpMethod.POST,
                url: url,
                content: content,
                contentPostingType: contentPostingType,
                proxy: proxy,
                Encoding: encoding,
                respType: respType,
                Timeout: timeout,
                UserAgent: userAgent,
                UseRedirect: useRedirect,
                MaxRedirectCount: maxRedirectCount,
                AdditionalHeaders: additionalHeaders,
                DownloadPath: downloadPath,
                UseOriginalUrl: useOriginalUrl,
                throwExceptionOnError: throwExceptionOnError,
                cookieContainer: cookieContainer);
        }

        public static string Post(
            string url,
            string content,
            string proxy = "",
            string cookie = "",
            ResponceType respType = ResponceType.BodyOnly,
            string userAgent = "",
            string contentPostingType = "application/x-www-form-urlencoded",
            string encoding = "UTF-8",
            string[] additionalHeaders = null,
            bool useRedirect = true,
            int maxRedirectCount = 5,
            int timeout = 30000,
            string downloadPath = null,
            bool useOriginalUrl = false,
            bool throwExceptionOnError = true)
        {
            return ZennoPoster.HTTP.Request(
                method: HttpMethod.POST,
                url: url,
                content: content,
                contentPostingType: contentPostingType,
                proxy: proxy,
                Encoding: encoding,
                respType: respType,
                Timeout: timeout,
                Cookies: cookie,
                UserAgent: userAgent,
                UseRedirect: useRedirect,
                MaxRedirectCount: maxRedirectCount,
                AdditionalHeaders: additionalHeaders,
                DownloadPath: downloadPath,
                UseOriginalUrl: useOriginalUrl,
                throwExceptionOnError: throwExceptionOnError,
                cookieContainer: null);
        }

        public static string Post(
            string url,
            byte[] content,
            string proxy = "",
            string cookie = "",
            ResponceType respType = ResponceType.BodyOnly,
            string userAgent = "",
            string contentPostingType = "application/x-www-form-urlencoded",
            string encoding = "UTF-8",
            string[] additionalHeaders = null,
            bool useRedirect = true,
            int maxRedirectCount = 5,
            int timeout = 30000,
            string downloadPath = null,
            bool useOriginalUrl = false,
            bool throwExceptionOnError = true)
        {
            return ZennoPoster.HTTP.Request(
                method: HttpMethod.POST,
                url: url,
                content: content,
                contentPostingType: contentPostingType,
                proxy: proxy,
                Encoding: encoding,
                respType: respType,
                Timeout: timeout,
                Cookies: cookie,
                UserAgent: userAgent,
                UseRedirect: useRedirect,
                MaxRedirectCount: maxRedirectCount,
                AdditionalHeaders: additionalHeaders,
                DownloadPath: downloadPath,
                UseOriginalUrl: useOriginalUrl,
                throwExceptionOnError: throwExceptionOnError,
                cookieContainer: null);
        }

        #endregion
    }
}
