using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Description.Wrap;
using Description.TypeInfo;

namespace Description.Import
{
    public class ImportXml
    {
        public XmlElement Data { get { return _data; } }

        private XmlElement _data;
        private string _path;
        private int _index;

        public ImportXml(string path)
        {
            _path = path;
            _index = -1;

            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            _data = doc.DocumentElement;
        }

        public void Error(string msg)
        {
            string error = string.Format("错误:{0} \n位置:{1}", msg, _path);
            throw new Exception(error);
        }
    }
}
