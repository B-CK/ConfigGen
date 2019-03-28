using ConfigGen.TypeInfo;
using System;
using System.Xml;

namespace ConfigGen.Data
{
    public class FInt : Data
    {
        public readonly int Value;

        public FInt(FClass host, FieldInfo define, int value) : base(host, define)
        {
            Value = value;
        }
        public FInt(FClass host, FieldInfo define, string value) : this(host, define, Convert.ToInt32(value))
        { }
        public FInt(FClass host, FieldInfo define, XmlElement value) : this(host, define, value.InnerText)
        { }
    }
}
