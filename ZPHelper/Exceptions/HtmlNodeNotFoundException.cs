using System;

namespace ZPHelper.Exceptions
{
    internal class HtmlNodeNotFoundException : Exception
    {
        public HtmlNodeNotFoundException(string typeSearch, string searchStr)
            : base(MsgPrep(typeSearch, searchStr)) { }

        public HtmlNodeNotFoundException(string typeSearch, string searchStr, Exception inner)
            : base(MsgPrep(typeSearch, searchStr), inner) { }

        private static string MsgPrep(string type, string str)
        {
            str = type + ": '" + str + "'";

            return "HtmlNode по " + str + " не был найден.";
        }
    }
}
