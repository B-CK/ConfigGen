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
            _fullType = define.FullName;
            excel.GetClass(this, ClassWrap.Get(define.FullName));
        }
        public FClass(FClass host, FieldWrap define, XmlElement value) : base(host, define)
        {
            _fullType = define.FullName;

            ClassWrap info = ClassWrap.Get(define.FullName);
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
            int offset = 0;
            while (parent != null)
            {
                offset += parent.Fields.Count;
                parent = ClassWrap.Get(parent.Inherit);
            }
            for (int i = 0; i < fields.Count; i++)
                Values.Add(Data.Create(this, fields[i], data.ChildNodes[i + offset] as XmlElement));
        }
        public override string ExportData()
        {
            StringBuilder builder = new StringBuilder();
            if (_define.IsDynamic)
                builder.Append(Util.CorrectFullType(_fullType) + Setting.DataSplitFlag);
            for (int i = 0; i < _values.Count; i++)
                builder.AppendFormat("{0}{1}", i > 0 ? Setting.DataSplitFlag : "", _values[i].ExportData());
            return builder.ToString();
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
            return obj is FClass @class &&
                   base.Equals(obj) &&
                   _fullType == @class._fullType &&
                   EqualityComparer<List<Data>>.Default.Equals(_values, @class._values);
        }
        public override int GetHashCode()
        {
            var hashCode = 509527360;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_fullType);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Data>>.Default.GetHashCode(_values);
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
    }
}
