using ConfigGen.Import;
using ConfigGen.TypeInfo;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ConfigGen.Config
{
    public class FList : Data
    {
        public List<Data> Values { get { return _values; } }

        private List<Data> _values = new List<Data>();
        private FieldInfo _item;

        private FList(FClass host, FieldInfo define) : base(host, define)
        {
            _item = define.GetItemDefine();
        }
        public FList(FClass host, FieldInfo define, ImportExcel excel) : this(host, define)
        {
            excel.GetList(this, define);
        }
        public FList(FClass host, FieldInfo define, XmlElement data) : this(host, define)
        {
            XmlNodeList list = data.ChildNodes;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i] as XmlElement;
                Values.Add(Data.Create(Host, _item, item));
            }
        }

        public override string ExportData()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_values.Count);
            for (int i = 0; i < _values.Count; i++)
                builder.AppendFormat("{0}{1}", Consts.CsvSplitFlag, _values[i].ExportData());
            return builder.ToString();
        }
    }
}
