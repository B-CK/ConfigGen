using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

public static class Util
{
    /// <summary>
    /// 数据表数据占位符,仅用于基础类型;不填写数据时,使用null占位.
    /// 默认值int,long,float="";string=""[无"null"字符串];bool=false;
    /// </summary>
    public const string Null = "null";
    public const string BOOL = "bool";
    public const string INT = "int";
    public const string FLOAT = "float";
    public const string LONG = "long";
    public const string STRING = "string";
    public const string LIST = "list";
    public const string DICT = "dict";
    //枚举可继承类型(暂时无用)
    public static readonly Dictionary<string, string> EnumInhert = new Dictionary<string, string>()
        {
            { "ubyte", "ubyte[0, 255]" },
            { "byte", "byte[-128, 127]" },
            { "ushort", "ushort[0, 65535]" },
            { "short", "short[-32768, 32767]" },
            { "uint", "uint[0, 4294967295]" },
            { "int", "int[-2147483648, 2147483647]" },
        };

    public static readonly string[] BaseTypes = new string[] { BOOL, INT, LONG, FLOAT, STRING, LIST, DICT };
    public static readonly HashSet<string> BaseHash = new HashSet<string>(BaseTypes);

    private static readonly string[] KeyTypes = new string[] { INT, LONG, STRING };
    public static readonly HashSet<string> RawTypes = new HashSet<string>() { BOOL, INT, FLOAT, LONG, STRING };
    public static readonly HashSet<string> ContainerTypes = new HashSet<string>() { LIST, DICT };
    /// <summary>
    /// 多参数分隔符[:],适用检查规则,分组,键值对
    /// </summary>
    public static readonly char[] ArgsSplitFlag = new char[] { ':' };
    /// <summary>
    /// 类型名称分隔符[.]
    /// </summary>
    public static readonly char[] DotSplit = new char[] { '.' };
    private static readonly char[] PathSplit = new char[] { '/' };

    public static void Error(string fmt, params object[] msg)
    {
        new Exception(string.Format(fmt, msg));
    }
    public static void Log(object logString, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(logString);
    }
    public static void LogWarning(object warningString)
    {
        Log(warningString, ConsoleColor.Yellow);
    }
    public static void LogError(object errorString)
    {
        Log(errorString, ConsoleColor.Red);
    }
    public static void LogFormat(string format, params object[] logString)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(format, logString);
    }
    public static void LogWarningFormat(string format, params object[] warningString)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(format, warningString);
    }
    public static void LogErrorFormat(string format, params object[] errorString)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(format, errorString);
    }
    public static void TryDeleteDirectory(string path)
    {
        if (!Directory.Exists(path)) return;

        var fs = Directory.GetFiles(path, "*.*");
        for (int i = 0; i < fs.Length; i++)
            File.Delete(fs[i]);
        var ds = Directory.GetDirectories(path, "*");
        for (int i = 0; i < ds.Length; i++)
            TryDeleteDirectory(ds[i]);

        Directory.Delete(path, true);
    }
    /// <summary>
    /// 创建文本
    /// </summary>
    public static void SaveFile(string filePath, string content)
    {
        string dirPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        File.WriteAllText(filePath, content, Encoding.UTF8);
    }
    public static string NormalizePath(string patth)
    {
        return patth.Replace("/", @"\");
    }

    /// <summary>
    /// 各种空
    /// </summary>
    public static bool IsEmpty(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
    public static string FirstCharUpper(this string name)
    {
        return Char.ToUpper(name[0]) + name.Substring(1);
    }
    public static string ToString(this object[] array, string split = ",")
    {
        return string.Join(split, array);
    }
    public static string ToString(this List<string> list, string split = ",")
    {
        return ToString(list.ToArray(), split);
    }
}
