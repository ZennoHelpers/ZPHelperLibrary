using System;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.Enums.Browser;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZPHelper;

namespace ZPHelper
{
    public class LogHelper
    {
        private readonly ZPHelper _z;

        private bool _debugMode;
        private readonly LogLevel _debugLogType;
        private bool _showDebugInPoster;

        private string _lastActionDescription;

        public LogHelper(ZPHelper zpHelper, bool debugMode, LogLevel debugLogType, bool showDebugInPoster)
        {
            _z = zpHelper;

            if (debugMode)
            {
                StartDebugMode(showDebugInPoster);
                SendDebugMsg("Инициализация библиотеки " + nameof(ZPHelper));
            }

            // Режим отладки
            _debugMode = debugMode;

            // Тип сообщений отладки
            _debugLogType = debugLogType;
        }

        public void StartDebugMode(bool showInPoster = true)
        {
            _debugMode = true;
            _showDebugInPoster = showInPoster;
            SetLogOptions(_z.Project.Directory + @"\Logs\DebugLog.txt");
            SendDebugMsg("Отладочные сообщения включены");
        }

        public void SendDebugMsg(string message)
        {
            if (!_debugMode) return;

            WriteInfo(_debugLogType, $@"{Other.GetCalledMethodName()}", message, _showDebugInPoster);
        }

        public void WriteError(object message, bool showInPoster = true)
        {
            WriteWarn(string.Empty, message, showInPoster);
        }

        public void WriteError(object title, object message, bool showInPoster = true)
        {
            WriteInfo(LogLevel.Error, title, message, showInPoster);
        }

        public void WriteWarn(object message, bool showInPoster = true)
        {
            WriteWarn(string.Empty, message, showInPoster);
        }

        public void WriteWarn(object title, object message, bool showInPoster = true)
        {
            WriteInfo(LogLevel.Warning, title, message, showInPoster);
        }

        public void WriteInfo(object message, bool showInPoster = true)
        {
            WriteInfo(string.Empty, message, showInPoster);
        }

        public void WriteInfo(object title, object message, bool showInPoster = true)
        {
            WriteInfo(LogLevel.Normal, title, message, showInPoster);
        }

        public void WriteInfo(LogLevel logLevel, object title, object message, bool showInPoster = true)
        {
            if (title == null) title = "▫";
            if (message == null) message = "▫";

            IZennoPosterProjectModel project = _z.Project;

            switch (logLevel)
            {
                case LogLevel.Normal:
                    project.SendInfoToLog(message.ToString(), title.ToString(), showInPoster);
                    break;
                case LogLevel.Warning:
                    project.SendWarningToLog(message.ToString(), title.ToString(), showInPoster);
                    break;
                case LogLevel.Error:
                    project.SendErrorToLog(message.ToString(), title.ToString(), showInPoster);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, nameof(logLevel));
            }
        }

        private void SetLogOptions([PathReference] string path, bool separateLog = true)
        {
            IZennoPosterProjectModel project = _z.Project;

            project.LogOptions.LogFile = path;
            project.LogOptions.SplitLogByThread = separateLog;
        }

        public void ActionDescription(string description, bool showInPoster = false)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException("Описание не может быть пустым", nameof(description));

            if (description == _lastActionDescription)
                throw new ArgumentException("Описание не может повторяться", nameof(description));

            _z.Project.SendInfoToLog(description, "Выполняется:", showInPoster);
            _lastActionDescription = description;
        }
    }
}
