using ConfigGen.Import;
using ConfigGen.TypeInfo;
using System.Collections.Generic;
using System.Xml;

namespace ConfigGen.Config
{
    public class FList : Data
    {
        public List<Data> Values { get { return _values; } }

        private List<Data> _values = new List<Data>();
        private FieldInfo _item;

        public FList(FClass host, FieldInfo define) : base(host, define)
        {
            _item = define.GetItemDefine();
        }
        public FList(FClass host, FieldInfo define, XmlElement data) : base(host, define)
        {
            _item = define.GetItemDefine();

            XmlNodeList list = data.ChildNodes;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i] as XmlElement;
                Values.Add(Data.Create(Host, _item, item));
            }
        }
    }
}
