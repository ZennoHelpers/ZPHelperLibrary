using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Global.Threads;
using Global.ZennoExtensions;
using ZennoLab.CommandCenter;
using ZennoLab.CommandCenter.TouchEvents;
using ZennoLab.InterfacesLibrary.Enums.Browser;
using ZennoLab.InterfacesLibrary.Enums.Http;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace ZPTest
{
    public class Main : IZennoCustomCode, IZennoCustomEndCode
    {
        public int ExecuteCode(Instance instance, IZennoPosterProjectModel project)
        {
            var helper = new ZPHelper.ZPHelper(project, instance, BrowserType.Firefox45, true);

            helper.Browser.OpenSite("https://github.com/ZennoHelpers/ZPHelper");

            return 0;
        }

        public void GoodEnd(Instance instance, IZennoPosterProjectModel project) { }

        public void BadEnd(Instance instance, IZennoPosterProjectModel project) { }
    }
}
