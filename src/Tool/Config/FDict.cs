using Tool.Import;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Tool.Wrap;

namespace Tool.Config
{
    public class FDict : Data
    {
        public Dictionary<Data, Data> Values { get { return _values; } }

        private Dictionary<Data, Data> _values = new Dictionary<Data, Data>();
        private FieldWrap _key;
        private FieldWrap _value;

        public FDict(FClass host, FieldWrap define, ImportExcel excel) : base(host, define)
        {
            _key = define.GetKeyDefine();
            _value = define.GetValueDefine();
            excel.GetDict(this, define);
        }
        public FDict(FClass host, FieldWrap define, XmlElement data) : base(host, define)
        {
            _key = define.GetKeyDefine();
            _value = define.GetValueDefine();

            XmlNodeList dict = data.ChildNodes;
            for (int i = 0; i < dict.Count; i++)
            {
                XmlNode pair = dict[i];
                XmlElement key = pair[Setting.KEY];
                XmlElement value = pair[Setting.VALUE];
                var dk = Data.Create(host, _key, key);
                var dv = Data.Create(host, _value, value);
                if (!Values.ContainsKey(dk))
                    Values.Add(dk, dv);
                else
                    Util.Error("字段:{0} Key:{1} 重复", define.Name, _key.Name);

            }
        }

        public override string ExportData()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_values.Count);
            var dit = _values.GetEnumerator();
            while (dit.MoveNext())
            {
                string key = dit.Current.Key.ExportData();
                string value = dit.Current.Value.ExportData();
                builder.AppendFormat("{0}{1}{0}{2}", Setting.CsvSplitFlag, key, value);
            }
            return builder.ToString();
        }
        public override void VerifyData()
        {
            var dit = Values.GetEnumerator();
            while (dit.MoveNext())
            {
                dit.Current.Key.VerifyData();
                dit.Current.Value.VerifyData();
            }
        }
        public override int ExportBinary(ref byte[] bytes, int offset)
        {
            int length = MessagePackBinary.WriteMapHeader(ref bytes, offset, Values.Count);
            foreach (var item in _values)
            {
                length += item.Key.ExportBinary(ref bytes, offset + length);
                length += item.Value.ExportBinary(ref bytes, offset + length);
            }                
            return length;
        }

        public override bool Equals(object obj)
        {
            return obj is FDict dict &&
                   base.Equals(obj) &&
                   EqualityComparer<Dictionary<Data, Data>>.Default.Equals(_values, dict._values) &&
                   EqualityComparer<FieldWrap>.Default.Equals(_key, dict._key) &&
                   EqualityComparer<FieldWrap>.Default.Equals(_value, dict._value);
        }
        public override int GetHashCode()
        {
            var hashCode = -827054455;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<Data, Data>>.Default.GetHashCode(_values);
            hashCode = hashCode * -1521134295 + EqualityComparer<FieldWrap>.Default.GetHashCode(_key);
            hashCode = hashCode * -1521134295 + EqualityComparer<FieldWrap>.Default.GetHashCode(_value);
            return hashCode;
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in Values)
            {
                if (i == 0)
                    builder.Append($"{item.Key}={item.Value}");
                else
                    builder.Append($", {item.Key}={item.Value}");
            }
            return builder.ToString();
        }
    }
}
