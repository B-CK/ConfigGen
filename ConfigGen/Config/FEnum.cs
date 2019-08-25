using Description.Import;
using Description.TypeInfo;
using System.Xml;

namespace Description.Config
{
    public class FEnum : Data
    {
        public readonly string EnumName;
        public readonly string Value;

        public FEnum(FClass host, FieldInfo define, ImportExcel excel) : base(host, define)
        {
            EnumInfo info = EnumInfo.Enums[define.OriginalType];
            string name = excel.GetEnum();
            EnumName = info.GetEnumName(name);
            EnumName = EnumName.IsEmpty() ? name : EnumName;
            Value = EnumInfo.Enums[define.OriginalType].GetEnumValue(EnumName);
        }
        public FEnum(FClass host, FieldInfo define, XmlElement xml) : base(host, define)
        {
            EnumName = xml.InnerText;
            Value = EnumInfo.Enums[define.OriginalType].GetEnumValue(EnumName);
        }

        public override string ExportData()
        {
            return Value;
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
