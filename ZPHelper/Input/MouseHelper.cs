using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ZPHelper
{
    public class MouseHelper
    {
        private readonly ZPHelper _z;

        // Смещение от центра
        private (int X, int Y) _pctOffset;

        // Макс. разброс от смещённого центра клика
        private (int X, int Y) _pctScatter;

        public (int X, int Y) PctScatter
        {
            set => _pctScatter = value;
        }

        public MouseHelper(ZPHelper zpHelper, (int X, int Y) pctOffset, (int X, int Y) pctScatter)
        {
            _z = zpHelper;

            // Смещение от центра
            _pctOffset = pctOffset;

            // Макс. разброс от смещённого центра клика
            _pctScatter = pctScatter;
        }

        #region Move

        public void MoveByXPath(string xPath)
        {
            MoveToHtmlNode(_z.HtmlNodeHelper.GetHtmlNodeByXPath(xPath));
        }

        private void MoveToHtmlNode(HtmlNode htmlNode)
        {
            htmlNode.GetBoundingClientRect(out double pageX, out double pageY, out double width, out double height, out _, out _, out _, out _);
            MoveTo(pageX, pageY, width, height, _pctOffset, _pctScatter);
        }

        [SuppressMessage("ReSharper", "UseDeconstructionOnParameter")]
        public void MoveTo(double nodeMinX, double nodeMinY, double nodeWidth, double nodeHeight, (int X, int Y) pctOffset, (int X, int Y) pctScatter)
        {
            double RandomizeScatter(double a)
            {
                double i;

                for (i = 0; i < 1;)
                {
                    i = _z.Rnd.NextDouble() * a;
                }

                if (_z.Rnd.NextDouble() >= 0.5) i = -i; // минус к числу

                return i;
            }

            // Центр элемента
            double nodeCenterX = nodeMinX + nodeWidth * 0.5;
            double nodeCenterY = nodeMinY + nodeHeight * 0.5;

            // Допустимое расстояние наведения от центра элемента (0:0)
            double maxDistX = nodeWidth / 2 - 1;
            double maxDistY = nodeHeight / 2 - 1;

            // Координата наведения со смещением относительно центра элемента (0:0)
            double offsetX = maxDistX * pctOffset.X;
            double offsetY = maxDistY * pctOffset.Y;

            // Получение допустимого разброса для координат со смещением
            double maxScatterX = (maxDistX - Math.Abs(offsetX)) * (pctScatter.X * 0.01);
            double maxScatterY = (maxDistY - Math.Abs(offsetY)) * (pctScatter.Y * 0.01);

            // Текущий разброс
            double scatterX = RandomizeScatter(maxScatterX * (_pctScatter.X * 0.01));
            double scatterY = RandomizeScatter(maxScatterY * (_pctScatter.Y * 0.01));

            // Итоговые координаты наведения:
            int x = (int)(nodeCenterX + offsetX + scatterX);
            int y = (int)(nodeCenterY + offsetY + scatterY);

            _z.Browser.ActiveTab.FullEmulationMouseMove(x, y);

            _z.Log.SendDebugMsg($"Наведение по: {x}, {y}, разброс: {(int)scatterX}, {(int)scatterY}, " +
                                $"макс. разброс: {maxDistX}, {maxDistY},\nцентр элемента: {nodeCenterX}, {nodeCenterY}, размер: {nodeWidth}, {nodeHeight}.");
        }

        #endregion

        #region Click

        public void ClickByXPath(string xPath, [ValueProvider("ZymlexHelper.ZP.MouseButton")] string button = MouseButton.Left, [ValueProvider("ZymlexHelper.ZP.MouseEvent")] string mouseEvent = MouseEvent.Click)
        {
            MoveByXPath(xPath);
            Click(button, mouseEvent);
        }

        public void Click([ValueProvider("ZymlexHelper.ZP.MouseButton")] string button = MouseButton.Left, [ValueProvider("ZymlexHelper.ZP.MouseEvent")] string mouseEvent = MouseEvent.Click)
        {
            _z.Browser.ActiveTab.FullEmulationMouseClick(button, mouseEvent);
        }

        #endregion
    }
}
