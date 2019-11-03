using Description.Import;
using Wrap;
using System;
using System.Xml;

namespace Description.Wrap
{
    public class FFloat : Data
    {
        public readonly float Value;

        public FFloat(FClass host, FieldWrap define, ImportExcel excel) : base(host, define)
        {
            Value = excel.GetFloat();
        }
        public FFloat(FClass host, FieldWrap define, XmlElement xml) : base(host, define)
        {
            string v = xml.InnerText;
            float r;
            if (!float.TryParse(v, out r))
            {
                Util.Error("{0}非float类型", v);
                Value = -1;
            }
            Value = r;
        }

        public override string ExportData()
        {
            return Value.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else
                return obj is FFloat ? (obj as FFloat).Value == Value : false;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
