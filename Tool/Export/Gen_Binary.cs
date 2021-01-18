using Tool.Wrap;
using System.IO;

namespace Tool.Export
{
    public class Gen_Binary
    {
        public static void Gen()
        {
            var configs = ConfigWrap.GetExports();
            for (int i = 0; i < configs.Count; i++)
            {
                var cfg = configs[i];
                byte[] bytes = null;
                int length = cfg.Data.ExportBinary(ref bytes, 0);
                string filePath = Path.Combine(Setting.BinaryDir, cfg.OutputFile + Setting.DataFileExt);
                byte[] content = new byte[length];
                System.Buffer.BlockCopy(bytes, 0, content, 0, length);
                Util.SaveFile(filePath.ToLower(), content);
            }
        }
    }
}
