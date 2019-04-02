using ConfigGen.Import;
using ConfigGen.TypeInfo;
using System.Xml;

namespace ConfigGen.Config
{
    public class FEnum : Data
    {
        public readonly string EnumName;
        public readonly string Value;

        public FEnum(FClass host, FieldInfo define, ImportExcel excel) : base(host, define)
        {
            EnumName = excel.GetEnum();
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
    }

}
