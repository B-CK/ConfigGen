using ConfigGen.TypeInfo;
using System;
using System.Xml;

namespace ConfigGen.Data
{
    public class FLong : Data
    {
        public readonly long Value;

        public FLong(FClass host, FieldInfo define, long value) : base(host, define)
        {
            Value = value;
        }
        public FLong(FClass host, FieldInfo define, string value) : this(host, define, Convert.ToInt64(value))
        { }
        public FLong(FClass host, FieldInfo define, XmlElement value) : this(host, define, value.InnerText)
        { }
    }
}
