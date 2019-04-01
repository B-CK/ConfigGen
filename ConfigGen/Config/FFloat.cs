using ConfigGen.TypeInfo;
using System;
using System.Xml;

namespace ConfigGen.Config
{
    public class FFloat : Data
    {
        public readonly float Value;

        public FFloat(FClass host, FieldInfo define, float value) : base(host, define)
        {
            Value = value;
        }
        public FFloat(FClass host, FieldInfo define, XmlElement value) : base(host, define)
        {
            string v = value.InnerText;
            float r;
            if (!float.TryParse(v, out r))
            {
                Util.Error("{0}非float类型", v);
                Value = -1;
            }
            Value = r;
        }
    }
}
