using System;
using System.Threading;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.Enums.Browser;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace ZymlexZPHelper
{
    public class ElementHelper
    {
        // https://help.zennolab.com/en/v5/zennoposter/5.23.0.0/webframe.html#topic121.html
        private readonly Helper _z;

        internal HtmlElement He;
        private bool _isVoid;
        private int _madeAttempts;
        private int _heAttempts;
        private int _heDelay;

        public ElementHelper(Helper helper, int heAttempts, int heDelay)
        {
            _z = helper;

            // Кол-во попыток поиска элемента
            _heAttempts = heAttempts;
            _heDelay = heDelay;
        }

        #region GetByXPath

        public void GetByXPath(string xPath, bool isChild = false)
        {
            HtmlElement ZpFindMethod() => isChild
                ? He.FindChildByXPath(xPath, 0)
                : _z.Browser.WorkTab.FindElementByXPath(xPath, 0);

            GetElement(ZpFindMethod, xPath);
            _z.Log.SendDebugInfo($"xPath: \"{xPath}\",\n" +
                                 $"isChild: {isChild},\n" +
                                 $"Попыток: {_madeAttempts}.");
        }

        #endregion

        // Пытается получить элемент, пока есть попытки
        private void GetElement(Func<HtmlElement> findHeMethod, string xPath)
        {
            HtmlElement he;
            bool isVoid;
            int countAttempts = _heAttempts;
            int delay = _heDelay;
            int i;

            // Ожидание появления
            for (i = 0;; i++)
            {
                he = findHeMethod();
                isVoid = he.IsVoid;

                // Проверка валидности
                if (!isVoid) break;

                // Проверка попыток
                if (i >= countAttempts) break;

                Thread.Sleep(delay);
            }

            _madeAttempts = i;
            _isVoid = isVoid;

            CheckElement(xPath);

            He = he;
        }

        private void CheckElement(string xPath)
        {
            if (_isVoid) throw new Exception("Элемент не найден. " + xPath);
        }

        public void SetCountAttempts(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count,
                                                      @"Отрицательное кол-во попыток.");
            _heAttempts = count;
        }
    }
}
