using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using JetBrains.Annotations;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;

namespace ZPHelper
{
    // ReSharper disable once InconsistentNaming
    public class ZPTask
    {
        private readonly Guid _guid;

        public string InputSettings
        {
            get => ZennoPoster.ExportInputSettings(_guid);
            set => ZennoPoster.ImportInputSettings(_guid, value);
        }

        internal ZPTask(Guid projectGuid)
        {
            _guid = projectGuid;
        }

//        public bool GetInfo(out TaskSettings settings)
//        {
//            string taskInfo = ZennoPoster.GetTaskInfo(_guid);
//
//            if (taskInfo == null)
//            {
//                settings = null;
//                return false;
//            }
//
//            XmlReader reader;
//            StringReader sr;
//            try
//            {
//                sr = new StringReader(taskInfo);
//                reader = XmlReader.Create(sr);
//            }
//            catch (Exception e)
//            {
//                throw new Exception("Ошибка при подготовке к чтению xml.", e);
//            }
//
//            reader.ReadElementContentAsString();
//
//
//            reader.Dispose();
//            sr.Dispose();
//        }

        public void Start() => ZennoPoster.StartTask(_guid);

        public void Stop() => ZennoPoster.StopTask(_guid);

        public void Interrupt() => ZennoPoster.InterruptTask(_guid);

        public void AddTries(int count) => ZennoPoster.AddTries(_guid, count);

        public void RemoveTask() => ZennoPoster.RemoveTask(_guid);
    }
}
