using ConfigGen.TypeInfo;
using System;
using System.Xml;

namespace ConfigGen.Config
{
    public class FLong : Data
    {
        public readonly long Value;

        public FLong(FClass host, FieldInfo define, long value) : base(host, define)
        {
            Value = value;
        }
        public FLong(FClass host, FieldInfo define, XmlElement value) : base(host, define)
        {
            string v = value.InnerText;
            long r;
            if (!long.TryParse(v, out r))
            {
                Util.Error("{0}非long类型", v);
                Value = -1;
            }
            Value = r;
        }
    }
}
