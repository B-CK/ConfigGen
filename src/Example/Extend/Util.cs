using System;
using System.Text;
using System.IO;

public static class Util {
    public static StringBuilder IntervalLevel (this StringBuilder builder, int n) {
        for (int i = 0; i < n; i++)
            builder.Append ("\t");
        return builder;
    }
    public static void SaveFile (string filePath, string content) {
        string dirPath = Path.GetDirectoryName (filePath);
        if (!Directory.Exists (dirPath))
            Directory.CreateDirectory (dirPath);

        File.WriteAllText (filePath, content, UTF8);
    }
}