using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.Enums.Browser;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZPHelper.Captcha;

namespace ZPHelper
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ZPHelper
    {
        private readonly BrowserType BrowserType;

        private BrowserHelper _browserHelper;

        public ZPHelper(IZennoPosterProjectModel project, bool debugMode = true)
            : this(project, null, BrowserType.WithoutBrowser, debugMode) { }

        public ZPHelper(IZennoPosterProjectModel project, Instance instance, BrowserType browserType = BrowserType.Firefox52, bool debugMode = true)
        {
            Project = project;
            BrowserType = browserType;
            Instance = instance;

            // Общий рандом
            Rnd = new Random(Guid.NewGuid().GetHashCode());

            // Остальное
            TaskManager = new TaskManager(this);

            Log = new LogHelper(
                zpHelper: this,
                debugMode: debugMode,
                debugLogType: LogLevel.Normal,
                showDebugInPoster: true);

            Logic = new LogicHelper(
                zpHelper: this,
                pauseMinMax: (2000, 3000));
        }

        #region Properties

        public Random Rnd { get; }
        public IZennoPosterProjectModel Project { get; }
        public Instance Instance { get; }
        public TaskManager TaskManager { get; }
        public LogHelper Log { get; }
        public LogicHelper Logic { get; }

        public BrowserHelper Browser
        {
            get
            {
                if (_browserHelper != null)
                    return _browserHelper;

                if (BrowserType == BrowserType.WithoutBrowser)
                    throw new Exception("Запуск браузера невозможен, без явного указания его типа.");

                return _browserHelper = new BrowserHelper(this, Instance);
            }
        }

//        public ProjectHelper ProjectHlp
//          => _projectHlp ?? (_projectHlp = new ProjectHelper(this));

//        public ElementHelper ElementHelper
//          => _elementHelper ?? (_elementHelper = new ElementHelper(helper: this, heAttempts: 10, heDelay: 1000));

        public CaptchaSolver CaptchaSolver => Browser.CaptchaSolver;
        public HtmlNodeHelper HtmlNodeHelper => Browser.HtmlNodeHelper;
        public KeyboardHelper Keyboard => Browser.KeyboardHlp;
        public MouseHelper Mouse => Browser.MouseHlp;

        #endregion
    }
}
