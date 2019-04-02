using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using ConfigGen.Description;
using ConfigGen.TypeInfo;

namespace ConfigGen.Export
{
    class ExportCsv
    {
        public static void Export()
        {
            StringBuilder builder = new StringBuilder();
            var cit = ConfigInfo.Configs.GetEnumerator();
            while (cit.MoveNext())
            {
                var cfg = cit.Current.Value;
                builder.AppendLine(cfg.Data.ExportData());
                string filePath = string.Format("{0}\\{1}{2}", Consts.DataDir, cfg.Name, Consts.CsvFileExt);
                Util.SaveFile(filePath, builder.ToString());
                builder.Clear();
            }
        }
    }
}
