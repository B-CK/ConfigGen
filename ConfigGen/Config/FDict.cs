using Description.Import;
using Description.TypeInfo;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Description.Config
{
    public class FDict : Data
    {
        public Dictionary<Data, Data> Values { get { return _values; } }

        private Dictionary<Data, Data> _values = new Dictionary<Data, Data>();
        private FieldInfo _key;
        private FieldInfo _value;

        public FDict(FClass host, FieldInfo define, ImportExcel excel) : base(host, define)
        {
            _key = define.GetKeyDefine();
            _value = define.GetValueDefine();
            excel.GetDict(this, define);
        }
        public FDict(FClass host, FieldInfo define, XmlElement data) : base(host, define)
        {
            _key = define.GetKeyDefine();
            _value = define.GetValueDefine();

            XmlNodeList dict = data.ChildNodes;
            for (int i = 0; i < dict.Count; i++)
            {
                XmlNode pair = dict[i];
                XmlElement key = pair[Setting.KEY];
                XmlElement value = pair[Setting.VALUE];
                var dk = Data.Create(Host, _key, key);
                var dv = Data.Create(Host, _value, value);
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
    }
}
