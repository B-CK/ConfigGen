using Tool.Wrap;
using System.IO;
using System.Text;

namespace Tool.Export
{
    public class Gen_Data
    {
        public static void Gen()
        {
            Util.TryDeleteDirectory(Setting.DataDir);

            StringBuilder builder = new StringBuilder();
            var configs = ConfigWrap.GetExports();
            for (int i = 0; i < configs.Count; i++)
            {
                var cfg = configs[i];
                builder.AppendLine(cfg.Data.ExportData());
                string filePath = Path.Combine(Setting.DataDir, cfg.OutputFile + Setting.DataFileExt);
                Util.SaveFile(filePath.ToLower(), builder.ToString());
                builder.Clear();
            }
        }
    }
}
