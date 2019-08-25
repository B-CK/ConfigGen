using Description.Import;
using Description.TypeInfo;
using System;
using System.Xml;

namespace Description.Config
{
    public class FInt : Data
    {
        public readonly int Value;

        public FInt(FClass host, FieldInfo define, ImportExcel excel) : base(host, define)
        {
            Value = excel.GetInt();
        }
        public FInt(FClass host, FieldInfo define, XmlElement value) : base(host, define)
        {
            string v = value.InnerText;
            int r;
            if (!int.TryParse(v, out r))
            {
                Util.Error("{0}非int类型", v);
                Value = -1;
            }
            Value = r;
        }

        public override string ExportData()
        {
            return Value.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else
                return obj is FInt ? (obj as FInt).Value == Value : false;
        }
        public override int GetHashCode()
        {
            return Value;
        }
    }
}
