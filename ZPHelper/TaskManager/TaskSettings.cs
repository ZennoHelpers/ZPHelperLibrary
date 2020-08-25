using System;
using JetBrains.Annotations;

namespace ZPHelper
{
    public class TaskSettings
    {
        public string Name { get; internal set; }
        public string SettingsType { get; internal set; }
        public int LimitOfThreads { get; internal set; }
        public int MaxAllowOfThreads { get; internal set; }
        public string ProjectFileLocation { get; internal set; }
        public string ProjectType { get; internal set; }

        public TaskSettings()
        {
            SettingsType = "None";
            LimitOfThreads = 1;
            MaxAllowOfThreads = 1;
            ProjectType = "Assembly";
        }
    }
}
