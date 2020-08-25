using System;
using System.Collections.Generic;
using System.Threading;
using Global.ZennoLab.Json.Linq;
using JetBrains.Annotations;

namespace ZPHelper.Captcha.CaptchaModule
{
    public class RuCaptcha : ICaptchaModule
    {
        private readonly string _key;
        private static int _attepmts;

        internal RuCaptcha(string key)
        {
            _key = key;
            _attepmts = 20;
        }

        public string SolveCaptcha(string base64, IReadOnlyDictionary<string, string> additParams = null)
        {
            Dictionary<string, string> paramPairs = new Dictionary<string, string>
            {
                {"method", "base64"},
                {"body", base64}
            };

            AddParams(ref paramPairs, additParams);

            string taskId = Request("in", _key, paramPairs);
            Thread.Sleep(5000); // TODO Переделать на динамический

            return GetTaskResult(taskId);
        }

        public string SolveReCaptcha(string pageurl, string sitekey, string proxy = null, string proxytype = null, IReadOnlyDictionary<string, string> additParams = null)
        {
            Dictionary<string, string> paramPairs = new Dictionary<string, string>
            {
                {"method", "userrecaptcha"},
                {"pageurl", pageurl},
                {"googlekey", sitekey},
            };

            if (!string.IsNullOrEmpty(proxy))
                paramPairs.Add("proxy", proxy);

            if (!string.IsNullOrEmpty(proxytype))
                paramPairs.Add("proxytype", proxytype);

            AddParams(ref paramPairs, additParams);

            string taskId = Request("in", _key, paramPairs);
            Thread.Sleep(7500); // TODO Переделать на динамический

            return GetTaskResult(taskId);
        }

        private string GetTaskResult(string id) =>
            Request("res", _key, new Dictionary<string, string>
            {
                {"action", "get"},
                {"id", id}
            });

        public string GetBalance() => Request("res", _key, new Dictionary<string, string> {{"action", "getbalance"}});

        private static void AddParams(ref Dictionary<string, string> paramPairs, IReadOnlyDictionary<string, string> additParams)
        {
            if (additParams != null)
            {
                foreach (KeyValuePair<string, string> pair in additParams)
                {
                    paramPairs.Add(pair.Key, pair.Value);
                }
            }
        }

        private static string Request(string type, string key, [NotNull] IReadOnlyDictionary<string, string> additData)
        {
            string url = $@"http://rucaptcha.com/{type}.php?key={key}&soft_id=2362&json=1";
            string apiRequest = string.Empty;

            foreach (KeyValuePair<string, string> param in additData)
            {
                url = url + "&" + param.Key + "=" + param.Value;
            }

            for (int i = 0; i < _attepmts; i++)
            {
                if (GetResponse(ExtRequest.Get(url), out apiRequest))
                    return apiRequest;

                ErrorState errorState = GetErrorInfo(apiRequest, out int timeout, out string errorText);

                if (errorState.HasFlag(ErrorState.Unknown))
                    throw new UnknownCaptchaSolverErrorException(errorText);
                if (errorState.HasFlag(ErrorState.Critical))
                    throw new CaptchaSolverErrorException(apiRequest, errorText);
                if (errorState.HasFlag(ErrorState.LowBet))
                    throw new CaptchaSolverErrorException(apiRequest, errorText); // TODO Доработать логику с подстройкой цены
                if (errorState.HasFlag(ErrorState.CaptchaNotReady))
                    if (additData.TryGetValue("method", out string v) && v == "userrecaptcha")
                        timeout = 15000; // TODO Переделать на получение из конфига + добавить сохранение в него
                    else
                        timeout = 5000;

                if (timeout != 0) Thread.Sleep(timeout);
            }

            throw new Exception("Попытки истекли.\n" + apiRequest);
        }

        private static bool GetResponse(ExtResponse response, out string apiRequest)
        {
            if (!response.ContentType(out string type) || !type.Contains("json"))
                throw new Exception("Ответ не является json:\n" + response);

            JObject o = JObject.Parse(response.Value);

            string status = (string)o["status"];
            apiRequest = (string)o["request"];

            switch (status)
            {
                case "1":
                    return true;
                case "0":
                    return false;
                default:
                    throw new Exception("Сервис вернул невалидный ответ:\n" + apiRequest);
            }
        }

        private static ErrorState GetErrorInfo(string error, out int timeout, out string errorText)
        {
            ErrorState state = ErrorState.Critical; // default for all TODO Переделать?!
            timeout = 0;

            switch (error)
            {
                case "CAPCHA_NOT_READY":
                    errorText = "Капча ещё не распознана.";
                    state &= ~ErrorState.Critical; // not critical
                    state |= ErrorState.CaptchaNotReady;
                    timeout = 5000;
                    break;
                case "ERROR_WRONG_USER_KEY":
                    errorText = "Параметра key в неверном формате, ключ должен содержать 32 символа.";
                    break;
                case "ERROR_KEY_DOES_NOT_EXIST":
                    errorText = "Указанный ключ не существует.";
                    break;
                case "ERROR_ZERO_BALANCE":
                    errorText = "На счету недостаточно средств.";
                    break;
                case "ERROR_PAGEURL":
                    errorText = "Параметр pageurl не задан в запросе";
                    break;
                case "ERROR_NO_SLOT_AVAILABLE":
                    errorText = "Очередь переполнена. Возможно, низкая ставка.";
                    break;
                case "ERROR_ZERO_CAPTCHA_FILESIZE":
                    errorText = "Размер отправленного изображения менее 100 байт.";
                    break;
                case "ERROR_TOO_BIG_CAPTCHA_FILESIZE":
                    errorText = "Размер отправленного изображения более 100 Кбайт.";
                    break;
                case "ERROR_WRONG_FILE_EXTENSION":
                    errorText = "Файл имеет неподдерживаемое расширение. Требуется: jpg, jpeg, gif, png.";
                    break;
                case "ERROR_IMAGE_TYPE_NOT_SUPPORTED":
                    errorText = "Сервер не может опознать тип вашего файла.";
                    break;
                case "ERROR_UPLOAD":
                    errorText = "Сервер не может прочитать файл из вашего POST-запроса.";
                    break;
                case "ERROR_IP_NOT_ALLOWED":
                    errorText = "IP запроса не добавлен в белый список сервиса.";
                    break;
                case "IP_BANNED":
                    errorText = "IP-адрес заблокирован за чрезмерное количество попыток авторизации с неверным ключом авторизации.";
                    break;
                case "ERROR_BAD_TOKEN_OR_PAGEURL":
                    errorText = "Невалидная пара значений googlekey и pageurl.";
                    break;
                case "ERROR_GOOGLEKEY":
                    errorText = "sitekey в запросе пустой или имеет некорректный формат.";
                    break;
                case "ERROR_CAPTCHAIMAGE_BLOCKED":
                    errorText = "Изображение помечено в нашей базе данных как не распознаваемое.";
                    break;
                case "MAX_USER_TURN":
                    errorText = "Вы делаете больше 60 обращений к in.php в течение 3 секунд. Ваш ключ API заблокирован на 10 секунд. Блокировка будет снята автоматически.";
                    state &= ~ErrorState.Critical; // not critical
                    timeout = 10000;
                    break;
                case "ERROR_EMPTY_METHOD":
                    errorText = "Передано пустое, либо невалидное значение action.";
                    break;
                case "ERROR: 1001":
                    errorText = "Вы получили 120 ответов ERROR_NO_SLOT_AVAILABLE за одну минуту из-за того, что ваша максимальная ставка ниже, чем текущая ставка на сервере.";
                    state &= ~ErrorState.Critical; // not critical
                    timeout = 3000;
                    break;
                case "ERROR: 1002":
                    errorText = "Вы получили 120 ответов ERROR_ZERO_BALANCE за одну минуту из-за того, что на вашем счету недостаточно средств.";
                    break;
                case "ERROR: 1003":
                    errorText = "Вы получаете ответ ERROR_NO_SLOT_AVAILABLE из-за того, что на сервере скопилась большая очередь из ваших капч, которые не распределены работникам." +
                                "Вы получили в три раза больше ошибок, чем число капч, которое вы загрузили (но не менее 120 ошибок). Увеличьте тайм-аут, если вы получаете этот код ошибки.";
                    state &= ~ErrorState.Critical; // not critical
                    timeout = 3000;
                    break;
                case "ERROR: 1004":
                    errorText = "Ваш IP-адрес заблокирован, потому что мы получили 5 запросов с некорректным ключом API с вашего IP.";
                    break;
                case "ERROR: 1005":
                    errorText = "Вы делаете слишком много запросов к res.php для получения ответов.";
                    break;
                default:
                    state |= ErrorState.Unknown;
                    errorText = string.Empty;
                    break;
            }

            return state;
        }
    }
}
