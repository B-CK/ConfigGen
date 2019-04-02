using ConfigGen.Import;
using ConfigGen.TypeInfo;
using System;
using System.Xml;

namespace ConfigGen.Config
{
    public class FLong : Data
    {
        public readonly long Value;

        public FLong(FClass host, FieldInfo define, ImportExcel excel) : base(host, define)
        {
            Value = excel.GetLong();
        }
        public FLong(FClass host, FieldInfo define, XmlElement xml) : base(host, define)
        {
            string v = xml.InnerText;
            long r;
            if (!long.TryParse(v, out r))
            {
                Util.Error("{0}非long类型", v);
                Value = -1;
            }
            Value = r;
        }

        public override string ExportData()
        {
            return Value.ToString();
        }
    }
}
