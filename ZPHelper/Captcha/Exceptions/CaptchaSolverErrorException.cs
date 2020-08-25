using System;

namespace ZPHelper.Captcha
{
    public class CaptchaSolverErrorException : Exception
    {
        public CaptchaSolverErrorException(string error, string text)
            : base(MsgPrep(error, text)) { }

        public CaptchaSolverErrorException(string error, string text, Exception inner)
            : base(MsgPrep(error, text), inner) { }

        private static string MsgPrep(string error, string text)
        {
            return "Сервис вернул ошибку: '" + error + "' - '" + text + "'.";
        }
    }
}
