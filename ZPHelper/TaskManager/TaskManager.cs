using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using ZennoLab.CommandCenter;

namespace ZPHelper
{
    public class TaskManager
    {
        private readonly ZPHelper _z;

        public TaskManager(ZPHelper helper) => _z = helper;

        public ZPTask AddTask(string name, string path)
            => AddTask(new TaskSettings {Name = name, ProjectFileLocation = path});

        public ZPTask AddTask(TaskSettings taskSettings)
        {
            Random rnd = _z.Rnd;

            StringWriter sw;
            XmlWriter writer;

            try
            {
                sw = new StringWriter();
                writer = XmlWriter.Create(sw, new XmlWriterSettings {ConformanceLevel = ConformanceLevel.Fragment});
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при начальном создании xml.", e);
            }

            // @formatter:off
            int year    = rnd.Next(1, 10000);
            int month   = rnd.Next(1, 13);
            int days    = rnd.Next(1, DateTime.DaysInMonth(year, month) + 1);
            int hour    = rnd.Next(0, 24);
            int minute  = rnd.Next(0, 60);
            int seconds = rnd.Next(0, 60);
            // @formatter:on

            DateTime dateTime = new DateTime(year, month, days, hour, minute, seconds);

            Guid guid = Guid.NewGuid();

            WriteXmlElements(writer, null, new Dictionary<string, string>
            {
                {"Id", guid.ToString()},
                {"Name", taskSettings.Name},
                {"IsNewbie", "False"},
                {"IsEnable", "True"},
                {"CreateTime", dateTime.ToString(CultureInfo.InvariantCulture)},
                {"SettingsType", taskSettings.SettingsType},
            });

            // ExecutionSettings
            WriteXmlElements(writer, "ExecutionSettings", new Dictionary<string, string>
            {
//                {"Id", Guid.NewGuid().ToString()},
                {"LimitOfThreads", taskSettings.LimitOfThreads.ToString()},
                {"MaxAllowOfThreads", taskSettings.MaxAllowOfThreads.ToString()},
//                {"DoneSuccesfully", "0"},
//                {"DoneAll", "0"},
//                {"NumberOfTries", "0"},
//                {"LastNumberOfTries", "0"},
//                {"Priority", "50"},
//                {"Proxy", "DoNotUseProxy"},
//                {"Status", "Newbie"},
//                {"ProxyLabels", string.Empty},
//                {"ShouldBeExecutedRandomly", "False"},
//                {"GroupLabels", string.Empty},
//                {"GroupStates", string.Empty},
//                {"MaxNumOfSuccesStop", "-1"},
//                {"Timeout", "-1"},
//                {"MaxNumOfFailStop", "-1"},
//                {"NumOfFailStop", "0"},
//                {"ShowTask", "False"},
//                {"TraceTask", "False"},
            });
            // SchedulerSettings
//            WriteXmlElements(writer, "SchedulerSettings", new Dictionary<string, string>
//            {
//                {"Id", Guid.NewGuid().ToString()},
//                {"StartDate", DateTime.Now.ToString(CultureInfo.InvariantCulture)},
//                {"ShedulerOnDate", "01/01/0001 00:00:00"},
//                {"EndDate", DateTime.Now.ToString(CultureInfo.InvariantCulture)},
//                {"RepetitionCount", "1"},
//                {"ScheduleType", "EveryMinutes"},
//                {"ActivateTime", "01/01/0001 00:00:00"},
//                {"ActivateWorkTime", "EveryMinutes"},
//                {"ScheduleType", "01/01/0001 00:00:00"},
//                {"IsActive", "EveryMinutes"},
//                {"ScheduleType", "False"},
//                {"NumberOfTries", "0"},
//                {"Minutes", "1"},
//                {"Days", "1"},
//                {"LastScheduleDate", "01/01/0001 00:00:00"},
//                {"NextScheduleDate", "null"},
//                {"IsClearSucces", "False"},
//                {"GroupName", string.Empty},
//            });
            // TriggerSettings
//            WriteXmlElements(writer, "TriggerSettings", new Dictionary<string, string>
//            {
//                {"Id", Guid.NewGuid().ToString()},
//                {"CheckFileExistanse", "False"},
//                {"CheckFilePath", string.Empty},
//                {"RemoveCheckFile", "True"},
//                {"NumberOfTries", "0"},
//                {"IsClearSucces", "False"},
//            });
            // Project
            WriteXmlElements(writer, "Project", new Dictionary<string, string>
            {
                {"ProjectFileLocation", taskSettings.ProjectFileLocation},
                {"ProjectType", taskSettings.ProjectType},
//                {"PurchaseState", "None"},
            });

            writer.Flush();
            writer.Dispose();
            // Dispose StringWriter after XmlWriter
            sw.Dispose();

            ZennoPoster.AddTask(sw.ToString());

            return new ZPTask(guid);
        }

        private static void WriteXmlElements(XmlWriter writer, string startEl, Dictionary<string, string> dict)
        {
            if (startEl != null) writer.WriteStartElement(startEl);

            foreach (KeyValuePair<string, string> a in dict)
            {
                writer.WriteElementString(a.Key, a.Value);
            }

            if (startEl != null) writer.WriteEndElement();
        }

        public void RunSubProject(string projectPath, IEnumerable<Tuple<string, string>> varibleMapping,
            bool useBrowser = false, bool mapOnBadExist = false, bool passProjectContext = true)
        {
            bool isSuccess = _z.Project.ExecuteProject(projectPath, varibleMapping, mapOnBadExist, passProjectContext,
                useBrowser);
            _z.Log.SendDebugMsg($@"Путь к проекту: ""{projectPath}""");
            if (!isSuccess) throw new Exception($@"Подпроект {projectPath} не выполнен.");
        }
    }
}
