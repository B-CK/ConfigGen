using ConfigGen.TypeInfo;
using System.Collections.Generic;
using System.Xml;

namespace ConfigGen.Data
{
    public class FClass : Data
    {
        /// <summary>
        /// 多态情况下,实际类型,非基类.
        /// </summary>
        public string FullType { get { return _fullType; } }
        public List<Data> Values { get { return _values; } }

        private string _fullType;
        private List<Data> _values = new List<Data>();

        public FClass(FClass host, FieldInfo define, string fullType) : base(host, define)
        {
            _fullType = fullType;
        }
        public FClass(FClass host, FieldInfo define, string fullType, XmlElement cls) : base(host, define)
        {
            _fullType = fullType;
        }
    }
}
