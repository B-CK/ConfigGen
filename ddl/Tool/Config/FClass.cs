using Tool.Import;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Tool.Wrap;

namespace Tool.Config
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

        public FClass(FClass host, FieldWrap define, ImportExcel excel) : base(host, define)
        {
            _fullType = define.FullType;
            excel.GetClass(this, ClassWrap.Get(define.FullType));
        }
        public FClass(FClass host, FieldWrap define, XmlElement value) : base(host, define)
        {
            _fullType = define.FullType;

            ClassWrap info = ClassWrap.Get(define.FullType);
            Load(info, value);
        }
        /// <summary>
        /// 依据实际数据修正类型
        /// </summary>
        public void SetCurrentType(string fullType)
        {
            _fullType = fullType;
        }
        void Load(ClassWrap info, XmlElement data)
        {
            ClassWrap parent = null;
            if (!info.Inherit.IsEmpty())
            {
                parent = ClassWrap.Get(info.Inherit);
                Load(parent, data);
            }

            var fields = info.Fields;
            int offset = parent == null ? 0 : parent.Fields.Count;
            for (int i = 0; i < fields.Count; i++)
                Values.Add(Data.Create(this, fields[i], data.ChildNodes[i + offset] as XmlElement));
        }

        public override string ExportData()
        {
            StringBuilder builder = new StringBuilder();
            if (_define.IsDynamic)
                builder.Append(Util.CorrectFullType(_fullType) + Setting.CsvSplitFlag);
            for (int i = 0; i < _values.Count; i++)
                builder.AppendFormat("{0}{1}", i > 0 ? Setting.CsvSplitFlag : "", _values[i].ExportData());
            return builder.ToString();
        }
        public override void VerifyData()
        {
            for (int i = 0; i < Values.Count; i++)
                Values[i].VerifyData();
        }
        public override int ExportBinary(ref byte[] bytes, int offset)
        {
            int length = 0;
            if (_define.IsDynamic)
            {
                string dynamicName = Util.CorrectFullType(_fullType);
                length += MessagePackBinary.WriteString(ref bytes, offset, dynamicName);
            }
            for (int i = 0; i < _values.Count; i++)
                length += ExportBinary(ref bytes, offset + length);
            return length;
        }
    }
}
