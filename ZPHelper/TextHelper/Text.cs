using System;
using System.Text;

namespace ZPHelper
{
    public static class Text
    {
        public static string GetRandomString(string[] requiredLetters, Random rnd, int length)
        {
            string letters = string.Empty;

            for (int i = 0; i < requiredLetters.Length; i++)
            {
                string lang = requiredLetters[i];

                switch (lang)
                {
                    case "En":
                        letters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
                        break;
                    case "Ru":
                        letters = "АаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя";
                        break;
                    case "Num":
                        letters = "0123456789";
                        break;
                    default:
                        throw new ArgumentException($@"Неизвестный параметр в {nameof(requiredLetters)} #{i}");
                }
            }

            return GenRandomString(letters, rnd, length);
        }

        private static string GenRandomString(string letters, Random rnd, int length)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), length, "Недопустимая длина строки");

            StringBuilder sb = new StringBuilder(length - 1);

            for (int i = 0; i < length; i++)
            {
                int p = rnd.Next(0, letters.Length);
                sb.Append(letters[p]);
            }

            return sb.ToString();
        }
    }
}
