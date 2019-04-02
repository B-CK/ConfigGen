using ConfigGen.Import;
using ConfigGen.TypeInfo;
using System;
using System.Xml;

namespace ConfigGen.Config
{
    public class FBool : Data
    {
        public readonly bool Value;

        public FBool(FClass host, FieldInfo define, ImportExcel excel) : base(host, define)
        {
            Value = excel.GetBool();
        }
        public FBool(FClass host, FieldInfo define, XmlElement xml) : base(host, define)
        {
            string v = xml.InnerText;
            if (v.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                Value = true;
            else if (v.Equals("false", StringComparison.CurrentCultureIgnoreCase))
                Value = false;
            else
            {
                Util.Error("{0}非bool类型", v);
                Value = false;
            }
        }
        public override string ExportData()
        {
            return Value.ToString();
        }
    }
}
