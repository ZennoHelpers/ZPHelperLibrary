using System;

namespace ZPHelper.Captcha
{
    public class UnknownCaptchaSolveServiceAnswerException : Exception
    {
        public UnknownCaptchaSolveServiceAnswerException(string answer)
            : base(MsgPrep(answer)) { }

        public UnknownCaptchaSolveServiceAnswerException(string answer, Exception inner)
            : base(MsgPrep(answer), inner) { }

        private static string MsgPrep(string answer)
        {
            return "Сервис вернул неизвестный ответ:\n" + answer;
        }
    }
}
