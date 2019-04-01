using ConfigGen.TypeInfo;
using System;
using System.Xml;

namespace ConfigGen.Config
{
    public class FBool : Data
    {
        public readonly bool Value;

        public FBool(FClass host, FieldInfo define, bool value) : base(host, define)
        {
            Value = value;
        }
        public FBool(FClass host, FieldInfo define, XmlElement value) : base(host, define)
        {
            string v = value.InnerText;
            if (v.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                Value = true;
            else if (v.Equals("false", StringComparison.CurrentCultureIgnoreCase))
                Value = false;
            else
            {
                Util.Error("{0}非bool类型", v);
                Value = false;
            }
        }
    }
}
