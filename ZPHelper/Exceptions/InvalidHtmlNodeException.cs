using System;

namespace ZPHelper.Exceptions
{
    internal class InvalidHtmlNodeException : Exception
    {
        public InvalidHtmlNodeException(string message)
            : base(message) { }

        public InvalidHtmlNodeException(string message, Exception inner)
            : base(message, inner) { }
    }
}
