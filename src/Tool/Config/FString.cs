using Tool.Import;
using System.IO;
using System.Xml;
using Tool.Wrap;
using System.Collections.Generic;

namespace Tool.Config
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
        public override int ExportBinary(ref byte[] bytes, int offset)
        {
            int length = MessagePackBinary.WriteString(ref bytes, offset, Value);
            return length;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj is FString)
                return (obj as FString).Value == Value;
            else if (obj is string)
                return (string)obj == Value;
            else
                return false;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
