using ConfigGen.TypeInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConfigGen.Config
{
    public class FString : Data
    {
        public readonly string Value;

        public FString(FClass host, FieldInfo define, string value) : base(host, define)
        {
            Value = value;
        }
        public FString(FClass host, FieldInfo define, XmlElement value) : this(host, define, value.InnerText)
        { }
    }
}
