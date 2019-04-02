using ConfigGen.Import;
using ConfigGen.TypeInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConfigGen.Config
{
    public class FString : Data
    {
        public readonly string Value;

        public FString(FClass host, FieldInfo define, ImportExcel excel) : base(host, define)
        {
            Value = excel.GetString();
        }
        public FString(FClass host, FieldInfo define, XmlElement value) : base(host, define)
        {
            Value = value.InnerText;
        }
        public override string ExportData()
        {
            return Value;
        }
    }
}
