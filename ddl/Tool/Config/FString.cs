using Description.Import;
using Wrap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Description.Wrap
{
    public class FString : Data
    {
        public readonly string Value;

        public FString(FClass host, FieldWrap define, ImportExcel excel) : base(host, define)
        {
            Value = excel.GetString();
        }
        public FString(FClass host, FieldWrap define, XmlElement value) : base(host, define)
        {
            Value = value.InnerText;
        }
        public override string ExportData()
        {
            return Value;
        }
        public override void VerifyData()
        {
            base.VerifyData();
            if (_define.RefPaths == null) return;

            for (int i = 0; i < _define.RefPaths.Length; i++)
            {
                string path = Util.GetAbsPath(_define.RefPaths[i]);
                path = path.Replace("*", Value);
                if (!File.Exists(path))
                    Util.LogWarningFormat("Class:{0} {1} {2}文件不存在", _host.FullType, _define, Util.GetRelPath(path));
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else
                return obj is FString ? (obj as FString).Value == Value : false;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
