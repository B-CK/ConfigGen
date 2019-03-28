using ConfigGen.TypeInfo;
using System;
using System.Xml;

namespace ConfigGen.Data
{
    public class FFloat : Data
    {
        public readonly float Value;

        public FFloat(FClass host, FieldInfo define, float value) : base(host, define)
        {
            Value = value;
        }
        public FFloat(FClass host, FieldInfo define, string value) : this(host, define, Convert.ToSingle(value))
        { }
        public FFloat(FClass host, FieldInfo define, XmlElement value) : this(host, define, value.InnerText)
        { }
    }
}
