﻿using Wrap;
using System.IO;
using System.Text;
using Description;

namespace Tool.Export
{
    public class Gen_Data
    {
        public static void Gen()
        {
            Util.TryDeleteDirectory(Setting.DataDir);

            StringBuilder builder = new StringBuilder();
            var cit = ConfigWrap.Configs.GetEnumerator();
            while (cit.MoveNext())
            {
                var cfg = cit.Current.Value;
                builder.AppendLine(cfg.Data.ExportData());
                string filePath = Path.Combine(Setting.DataDir, cfg.OutputFile + Setting.DataFileExt);
                Util.SaveFile(filePath.ToLower(), builder.ToString());
                builder.Clear();
            }
        }
    }
}
