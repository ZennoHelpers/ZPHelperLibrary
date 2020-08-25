using System;
using System.Collections.Generic;
using System.Threading;
using Global.ZennoLab.Json.Linq;
using JetBrains.Annotations;
using ZennoLab.CommandCenter;
using ZPHelper;

namespace ZPHelper
{
    // ReSharper disable once InconsistentNaming
    public static partial class JSHelper
    {
        public static string Eval(Document doc, string jsCode, bool throwAllowed = true, bool altRun = false)
        {
            jsCode = "'use strict'; try{" +
                        jsCode +
                     "}catch(e){ return e }";

            try
            {
                return doc.EvaluateScript(jsCode, true, altRun);
            }
            catch (Exception e)
            {
                if (throwAllowed) throw;
                return e.Message;
            }
        }
    }
}
