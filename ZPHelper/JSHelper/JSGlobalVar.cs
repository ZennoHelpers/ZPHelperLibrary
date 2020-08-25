using System;
using JetBrains.Annotations;
using ZennoLab.CommandCenter;
using static ZPHelper.JSHelper;

namespace ZPHelper
{
    // ReSharper disable once InconsistentNaming
    public class JSGlobalVar
    {
        private readonly Document _doc;
        // ReSharper disable once InconsistentNaming
        public string JsVar { get; }

        public JSGlobalVar(Document doc, string jsVar)
        {
            _doc = doc;
            JsVar = jsVar;
        }

        [NotNull]
        public string Value
        {
            get => Eval(_doc, "return " + JsVar);
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                Eval(_doc, JsVar + " = '" + value + "';");
            }
        }
    }
}
