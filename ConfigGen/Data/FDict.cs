using ConfigGen.TypeInfo;
using System.Collections.Generic;
using System.Xml;

namespace ConfigGen.Data
{
    public class FDict : Data
    {
        public Dictionary<Data, Data> Values { get { return _values; } }

        private Dictionary<Data, Data> _values = new Dictionary<Data, Data>();
        private FieldInfo _keyDefine;
        private FieldInfo _valueDefine;

        public FDict(FClass host, FieldInfo define) : base(host, define)
        {
            _keyDefine = define.GetKeyDefine();
            _valueDefine = define.GetValueDefine();
        }
        public FDict(FClass host, FieldInfo define, XmlElement dict) : base(host, define) { }
    }
}
