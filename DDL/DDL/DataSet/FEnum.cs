using Import;
using Wrap;
using System.Xml;

namespace DataSet
{
    public class FEnum : Data
    {
        public readonly string EnumName;
        public readonly int Value;

        public FEnum(FClass host, FieldWrap define, ImportExcel excel) : base(host, define)
        {
            EnumWrap info = EnumWrap.Enums[define.OriginalType];
            string name = excel.GetEnum();
            EnumName = info.GetEnumName(name);
            EnumName = EnumName.IsEmpty() ? name : EnumName;
            Value = EnumWrap.Enums[define.OriginalType].GetEnumValue(EnumName);
        }
        public FEnum(FClass host, FieldWrap define, XmlElement xml) : base(host, define)
        {
            EnumName = xml.InnerText;
            Value = EnumWrap.Enums[define.OriginalType].GetEnumValue(EnumName);
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
                return obj is FEnum ? (obj as FEnum).Value == Value : false;
        }
        public override int GetHashCode()
        {
            return EnumName.GetHashCode();
        }
    }

}
