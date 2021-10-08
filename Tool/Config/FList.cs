using Tool.Import;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Tool.Wrap;

namespace Tool.Config
{
    public class FList : Data
    {
        /// <summary>
        /// key:数据表的index字段
        /// value:数据表的每一条数据
        /// </summary>
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
                Values.Add(Data.Create(_host, _item, item));
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
        public override int ExportBinary(ref byte[] bytes, int offset)
        {
            int length = MessagePackBinary.WriteArrayHeader(ref bytes, offset, Values.Count);
            for (int i = 0; i < _values.Count; i++)
                length += _values[i].ExportBinary(ref bytes, offset + length);
            return length;
        }
        public override void VerifyData()
        {
            if (!IsRoot) 
                base.VerifyData();
            else
                for (int i = 0; i < Values.Count; i++)
                    Values[i].VerifyData();
        }
        public override bool Equals(object obj)
        {
            return obj is FList list &&
                   base.Equals(obj) &&
                   EqualityComparer<List<Data>>.Default.Equals(_values, list._values) &&
                   EqualityComparer<FieldWrap>.Default.Equals(_item, list._item);
        }
        public override int GetHashCode()
        {
            var hashCode = 1294733218;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Data>>.Default.GetHashCode(_values);
            hashCode = hashCode * -1521134295 + EqualityComparer<FieldWrap>.Default.GetHashCode(_item);
            return hashCode;
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Values.Count; i++)
            {
                if (i == 0)
                    builder.Append($"{Values[i]}");
                else
                    builder.Append($", {Values[i]}");
            }
            return builder.ToString();
        }

        /// <summary>
        /// Csv格式数据导出.
        /// 注:实际导出数据与Excel填写有差异,主要差异在于List/Dict类型导出后,会导出count且移除末尾]]符号
        /// </summary>
        public string ExportCsv()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _values.Count; i++)
                builder.AppendFormat("{0}{1}\r\n", _values[i].ExportData(), Setting.CsvSplitFlag);
            return builder.ToString();
        }
    }
}
