using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ZPHelper
{
    public static class Other
    {
        internal static string GetResourceData(Type targetType, string resName)
        {
            Stream stream = Assembly.GetAssembly(targetType).GetManifestResourceStream(resName);
            if (stream == null) return null;
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        internal static string GetCalledMethodName()
        {
            StackFrame frame = new StackFrame(2);
            MethodBase methodBase = frame.GetMethod();
            return methodBase.Name;
        }
    }
}
