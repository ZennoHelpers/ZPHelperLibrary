using System;
using System.Collections.Generic;
using System.Threading;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.Enums.Browser;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZPHelper;

namespace ZPHelper
{
    public class LogicHelper
    {
        private readonly ZPHelper _z;

        // Диапазон паузы
        private (int min, int max) _pause;

        public LogicHelper(ZPHelper zpHelper, (int min, int max) pauseMinMax)
        {
            _z = zpHelper;

            _pause = pauseMinMax;
        }

        public void CallWithProbability(int percent, Action lambdaExpression)
        {
            if (percent < -100 | percent > 100)
                throw new ArgumentOutOfRangeException(nameof(percent), percent, null);

            int a = _z.Rnd.Next(100);
            if (a < percent)
            {
                lambdaExpression.Invoke();
            }

            _z.Log.WriteInfo(a.ToString());
        }

        public void Pause()
        {
            Pause(_pause.min, _pause.max);
        }

        public void Pause(int min, int max)
        {
            int i = _z.Rnd.Next(min, max);
            _z.Log.SendDebugMsg($@"{i.ToString()} мс");
            Thread.Sleep(i);
        }

        public void SetPause(int min, int max)
        {
            _pause = (min, max);
        }
    }
}
