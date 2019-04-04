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
            return Value ? "1" : "0";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else
                return obj is FBool ? (obj as FBool).Value == Value : false;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
