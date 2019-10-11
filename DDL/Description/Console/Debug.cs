using System;
using System.IO;
using Description;
using System.Text;
using System.Collections.Generic;

public class Debug
{
    const string logFile = "info.log";

    static Stream stream;
    static StreamWriter writer;

    static StringBuilder _builder = new StringBuilder();
    static Queue<Log> _cache = new Queue<Log>();
    static List<Log> _logs = new List<Log>();
    public static void Init()
    {
        if (!File.Exists(logFile))
            File.Create(logFile);
        else
        {
            string[] lines = File.ReadAllLines(logFile);
            if (lines.Length >= 1000)
            {
                string[] cut = new string[500];
                Array.Copy(lines, 500, cut, 0, 500);
                File.WriteAllLines(logFile, cut);
            }
        }
        stream = File.Open(logFile, FileMode.Append, FileAccess.Write);
        writer = new StreamWriter(stream);
        writer.AutoFlush = true;
    }
    public static void Dispose()
    {
        Clear();
        _cache.Clear();
        stream.Close();
        stream.Dispose();
        stream = null;
        writer = null;
        _cache = null;
        _logs = null;
    }
    public static void Clear()
    {
        for (int i = 0; i < _logs.Count; i++)
            _cache.Enqueue(_logs[i]);
        _logs.Clear();
    }
    public static List<Log> GetLogs() { return _logs; }
    public static void Log(object msg) { Log(msg.ToString()); }
    public static void LogWarning(object msg) { LogWarning(msg.ToString()); }
    public static void LogError(object msg) { LogError(msg.ToString()); }
    public static void Log(string msg)
    {
        SendMessage(LogType.Info, msg);
    }
    public static void LogWarning(string msg)
    {
        SendMessage(LogType.Warn, msg);
    }
    public static void LogError(string msg)
    {
        SendMessage(LogType.Error, msg);
    }
    public static void LogFormat(string format, params object[] logString)
    {
        Log(Util.Format(format, logString));
    }
    public static void LogWarningFormat(string format, params object[] warningString)
    {
        LogWarning(Util.Format(format, warningString));
    }
    public static void LogErrorFormat(string format, params object[] errorString)
    {
        LogError(Util.Format(format, errorString));
    }
    private static void SendMessage(LogType type, string msg = "")
    {
        int line = _logs.Count;
        Log log = null;
        if (_cache.Count == 0)
        {
            log = new Log(line, type, msg);
        }
        else
        {
            log = _cache.Dequeue();
            log.Reset(line, type, msg);
        }

        _logs.Add(log);
        if (writer != null)
            writer.WriteLine(log.TimeMsg);

        if (ConsoleDock.Ins != null)
            ConsoleDock.Ins.ShowMessage(log);
    }
}
