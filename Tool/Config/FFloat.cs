using Tool.Import;
using Tool.Wrap;
using System.Xml;

namespace Tool.Config
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
                Util.Error("{0}非float类型或者数值溢出", v);
                Value = -1;
            }
            Value = r;
        }

        public override string ExportData()
        {
            return Value.ToString();
        }
        public override int ExportBinary(ref byte[] bytes, int offset)
        {
            int length = MessagePackBinary.WriteSingle(ref bytes, offset, Value);
            return length;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj is FFloat)
                return (obj as FFloat).Value == Value;
            else if (obj is float)
                return (float)obj == Value;
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
