using Tool.Import;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Tool.Wrap;

namespace Tool.Config
{
    public class FList : Data
    {
        private static Dictionary<FieldWrap, HashSet<Data>> _indexs = new Dictionary<FieldWrap, HashSet<Data>>();
        /// <summary>
        /// 检查数据索引
        /// </summary>
        /// <param name="key">FullName.Index</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool ContainsIndex(FieldWrap index, Data data)
        {
            return _indexs[index].Contains(data);
        }
        public static void AddIndex(FieldWrap index, Data data)
        {
            if (_indexs.ContainsKey(index))
                _indexs[index].Add(data);
            else
                _indexs.Add(index, new HashSet<Data>() { data });
        }

        public List<Data> Values { get { return _values; } }

        private List<Data> _values = new List<Data>();
        private FieldWrap _item;

        public FList(FClass host, FieldWrap define) : base(host, define)
        {
            _item = define.GetItemDefine();
        }
        public FList(FClass host, FieldWrap define, ImportExcel excel) : this(host, define)
        {
            excel.GetList(this, define);
        }
        public void LoadMultiRecord(XmlElement data)
        {
            XmlNodeList list = data.ChildNodes;
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i] as XmlElement;
                Values.Add(Data.Create(Host, _item, item));
            }
        }
        public void LoadOneRecord(XmlElement data)
        {
            Values.Add(Data.Create(_host, _item, data));
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
        public override int ExportBinary(ref byte[] bytes, int offset)
        {
            int length = MessagePackBinary.WriteArrayHeader(ref bytes, offset, Values.Count);
            for (int i = 0; i < Values.Count; i++)
                length += ExportBinary(ref bytes, offset + length);
            return length;
        }
    }
}
