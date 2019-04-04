﻿using ConfigGen.Import;
using ConfigGen.TypeInfo;
using System;
using System.Xml;

namespace ConfigGen.Config
{
    public class FFloat : Data
    {
        public readonly float Value;

        public FFloat(FClass host, FieldInfo define, ImportExcel excel) : base(host, define)
        {
            Value = excel.GetFloat();
        }
        public FFloat(FClass host, FieldInfo define, XmlElement xml) : base(host, define)
        {
            string v = xml.InnerText;
            float r;
            if (!float.TryParse(v, out r))
            {
                Util.Error("{0}非float类型", v);
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