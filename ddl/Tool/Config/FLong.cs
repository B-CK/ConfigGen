using Tool.Import;
using System.Xml;
using Tool.Wrap;

namespace Tool.Config
{
    public class FLong : Data
    {
        public readonly long Value;

        public FLong(FClass host, FieldWrap define, ImportExcel excel) : base(host, define)
        {
            Value = excel.GetLong();
        }
        public FLong(FClass host, FieldWrap define, XmlElement xml) : base(host, define)
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
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else
                return obj is FLong ? (obj as FLong).Value == Value : false;
        }
        public override string ExportData()
        {
            return Value.ToString();
        }
        public override int ExportBinary(ref byte[] bytes, int offset)
        {
            int length = MessagePackBinary.WriteInt64(ref bytes, offset, Value);
            return length;
        }
    }
}
