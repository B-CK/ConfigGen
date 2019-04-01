using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ConfigGen.Config;
using ConfigGen.TypeInfo;

namespace ConfigGen.Import
{
    public class ImportXml
    {
        private List<XmlElement> _datas = new List<XmlElement>();
        private string _relPath;
        private int _index;

        public ImportXml(string relPath)
        {
            _relPath = relPath;
            _index = -1;

            string absPath = Util.GetAbsPath(relPath);
            string[] files = Directory.GetFiles(absPath, "*.xml");
            XmlDocument doc = new XmlDocument();
            for (int i = 0; i < files.Length; i++)
            {
                doc.Load(files[i]);
                _datas.Add(doc.DocumentElement);
            }
        }
        public XmlElement GetNext()
        {
            ++_index;
            return _datas[_index];
        }
        public void Error(string msg)
        {
            string error = string.Format("错误:{0} \n位置:{1}", msg, _relPath);
            throw new Exception(error);
        }
    }
}
