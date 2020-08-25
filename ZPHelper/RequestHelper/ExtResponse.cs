using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ZPHelper
{
    public class ExtResponse
    {
        public string FullResponse { get; }
        public string Header => GetData(FullResponse, @".*?(?=(?:(?:\r?\n){3}))");
        //TODO Переделать!
        public string Value => GetData(FullResponse, @"(?<=(?:(?:\r?\n){3})).*");

        internal ExtResponse(string response) => FullResponse = response;

        private static string GetData(string response, string regex)
        {
            Match match = Regex.Match(response, regex, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return match.Success
                ? match.Value
                : throw new Exception("Сервер вернул невалидный ответ:\n" + response);
        }

        public bool ContentType(out string type)
        {
            Match m = Regex.Match(Header, @"(?<=Content-Type:\s).*(?=\r)", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            type = m.Success ? m.Value : null;
            return m.Success;
        }

        public override string ToString() => FullResponse;
    }
}
