using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Xml;
using TypeInfo;

namespace Export
{
    class ExportCsv
    {
        public static void Export()
        {
            Util.TryDeleteDirectory(Setting.DataDir);

            StringBuilder builder = new StringBuilder();
            var cit = ConfigInfo.Configs.GetEnumerator();
            while (cit.MoveNext())
            {
                var cfg = cit.Current.Value;
                builder.AppendLine(cfg.Data.ExportData());
                string filePath = Path.Combine(Setting.DataDir, cfg.OutputFile + Setting.CsvFileExt);
                Util.SaveFile(filePath.ToLower(), builder.ToString());
                builder.Clear();
            }
        }
    }
}
