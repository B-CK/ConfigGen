using Import;
using TypeInfo;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Wrap
{
    public class FList : Data
    {
        private static Dictionary<FieldInfo, HashSet<Data>> _indexs = new Dictionary<FieldInfo, HashSet<Data>>();
        /// <summary>
        /// 检查数据索引
        /// </summary>
        /// <param name="key">FullName.Index</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool ContainsIndex(FieldInfo index, Data data)
        {
            return _indexs[index].Contains(data);
        }
        public static void AddIndex(FieldInfo index, Data data)
        {
            if (_indexs.ContainsKey(index))
                _indexs[index].Add(data);
            else
                _indexs.Add(index, new HashSet<Data>() { data });
        }

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
                builder.AppendFormat("{0}{1}", Setting.CsvSplitFlag, _values[i].ExportData());
            return builder.ToString();
        }
        public override void VerifyData()
        {
            for (int i = 0; i < Values.Count; i++)
                Values[i].VerifyData();
        }
    }
}
