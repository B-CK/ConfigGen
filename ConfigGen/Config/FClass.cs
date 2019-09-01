using Description.Import;
using Description.TypeInfo;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Description.Wrap
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

        public FClass(FClass host, FieldInfo define, ImportExcel excel) : base(host, define)
        {
            _fullType = define.FullType;
            excel.GetClass(this, ClassInfo.Get(define.FullType));
        }
        public FClass(FClass host, FieldInfo define, XmlElement value) : base(host, define)
        {
            _fullType = define.FullType;

            ClassInfo info = ClassInfo.Get(define.FullType);
            Load(info, value);
        }
        /// <summary>
        /// 依据实际数据修正类型
        /// </summary>
        public void SetCurrentType(string fullType)
        {
            _fullType = fullType;
        }
        void Load(ClassInfo info, XmlElement data)
        {
            if (info.IsDynamic())
            {
                if (!info.Inherit.IsEmpty())
                {
                    ClassInfo parent = ClassInfo.Get(info.Inherit);
                    Load(parent, data);
                }

                string type = data.GetAttribute("Type");
                ClassInfo dynamic = ClassInfo.Get(type);
                if (dynamic == null)
                    Util.Error("多态类型{0}未知", type);
                if (!info.HasChild(type))
                    Util.Error("数据类型{0}非{2}子类", type, info.FullType);
                SetCurrentType(type);
                info = dynamic;
            }

            var fields = info.Fields;
            for (int i = 0; i < fields.Count; i++)
                Values.Add(Data.Create(this, fields[i], data));
        }

        public override string ExportData()
        {
            StringBuilder builder = new StringBuilder();
            bool isDynamic = _define.IsDynamic;
            if (isDynamic)
                builder.Append(FullType + Setting.CsvSplitFlag);
            for (int i = 0; i < _values.Count; i++)
                builder.AppendFormat("{0}{1}", i > 0 ? Setting.CsvSplitFlag : "", _values[i].ExportData());
            return builder.ToString();
        }
        public override void VerifyData()
        {
            for (int i = 0; i < Values.Count; i++)
                Values[i].VerifyData();
        }
    }
}
