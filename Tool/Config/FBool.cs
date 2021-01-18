using Tool.Import;
using System;
using System.Xml;
using Tool.Wrap;

namespace Tool.Config
{
    public class FBool : Data
    {
        public readonly bool Value;

        public FBool(FClass host, FieldWrap define, ImportExcel excel) : base(host, define)
        {
            Value = excel.GetBool();
        }
        public FBool(FClass host, FieldWrap define, XmlElement xml) : base(host, define)
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
            return Value.ToString().ToLower();
        }
        public override int ExportBinary(ref byte[] bytes, int offset)
        {
            bytes[offset] = Value ? MessagePackCode.True : MessagePackCode.False;
            return 1;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj is FBool)
                return (obj as FBool).Value == Value;
            else if (obj is bool)
                return (bool)obj == Value;
            else
                return false;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
