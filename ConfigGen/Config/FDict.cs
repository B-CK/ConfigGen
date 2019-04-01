using ConfigGen.TypeInfo;
using System.Collections.Generic;
using System.Xml;

namespace ConfigGen.Config
{
    public class FDict : Data
    {
        public Dictionary<Data, Data> Values { get { return _values; } }

        private Dictionary<Data, Data> _values = new Dictionary<Data, Data>();
        private FieldInfo _key;
        private FieldInfo _value;

        public FDict(FClass host, FieldInfo define) : base(host, define)
        {
            _key = define.GetKeyDefine();
            _value = define.GetValueDefine();
        }
        public FDict(FClass host, FieldInfo define, XmlElement data) : base(host, define)
        {
            _key = define.GetKeyDefine();
            _value = define.GetValueDefine();

            XmlNodeList dict = data.ChildNodes;
            for (int i = 0; i < dict.Count; i++)
            {
                XmlNode pair = dict[i];
                XmlElement key = pair[Consts.KEY];
                XmlElement value = pair[Consts.VALUE];
                var dk = Data.Create(Host, _key, key);
                var dv = Data.Create(Host, _value, value);
                if (!Values.ContainsKey(dk))
                    Values.Add(dk, dv);
                else
                    Util.Error("字段:{0} Key:{1} 重复", define.Name, _key.Name);

            }
        }
    }
}
