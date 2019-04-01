using ConfigGen.TypeInfo;
using System.Xml;

namespace ConfigGen.Config
{
    public class FEnum : Data
    {
        public readonly string EnumName;
        public readonly int Value;

        public FEnum(FClass host, FieldInfo define, string enumName) : base(host, define)
        {
            EnumName = enumName;
            Value = EnumInfo.Enums[define.OriginalType].GetEnumValue(enumName);
        }
        public FEnum(FClass host, FieldInfo define, XmlElement value) : this(host, define, value.InnerText)
        { }
    }

}
