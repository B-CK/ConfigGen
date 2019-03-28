using ConfigGen.TypeInfo;
using System;
using System.Xml;

namespace ConfigGen.Data
{
    public class FBool : Data
    {
        public readonly bool Value;

        public FBool(FClass host, FieldInfo define, bool value) : base(host, define)
        {
            Value = value;
        }
        public FBool(FClass host, FieldInfo define, string value) : this(host, define, Convert.ToBoolean(value))
        { }
        public FBool(FClass host, FieldInfo define, XmlElement value) : this(host, define, value.InnerText)
        { }
    }
}
