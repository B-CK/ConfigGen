using ConfigGen.TypeInfo;
using System.Xml;

namespace ConfigGen.Data
{
    public class FEnum : Data
    {
        public readonly string EnumName;
        public readonly int Value;

        public FEnum(FClass host, FieldInfo define, string name) : base(host, define)
        {
            EnumName = name;
            Value = EnumInfo.Enums[define.OriginalType].GetEnumValue(name);
        }
        public FEnum(FClass host, FieldInfo define, XmlElement value) : this(host, define, value.InnerText)
        { }
    }

}
