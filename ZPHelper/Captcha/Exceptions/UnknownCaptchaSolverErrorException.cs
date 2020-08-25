using System;

namespace ZPHelper.Captcha
{
    public class UnknownCaptchaSolverErrorException : Exception
    {
        public UnknownCaptchaSolverErrorException(string answer)
            : base(MsgPrep(answer)) { }

        public UnknownCaptchaSolverErrorException(string answer, Exception inner)
            : base(MsgPrep(answer), inner) { }

        private static string MsgPrep(string answer)
        {
            return "Сервис вернул неизвестную ошибку: '" + answer + "'.";
        }
    }
}
