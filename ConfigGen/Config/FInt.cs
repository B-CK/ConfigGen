using ConfigGen.Import;
using ConfigGen.TypeInfo;
using System;
using System.Xml;

namespace ConfigGen.Config
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
    }
}
