using Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

public abstract class CfgObject : ScriptableObject { }

/// <summary>
/// 数据解析类
/// <summary>
public partial class DataStream
{
    private string DataDir => $"{Application.dataPath}/../../Config/csv/";
    private void Error(string err)
    {
        throw new Exception(err);
    }
#if !BinaryConfig
    private readonly string[] _line;
    private int _index;
    public DataStream(string path, Encoding encoding)
    {
        _line = File.ReadAllLines($"{DataDir}{path}");
        _index = 0;
    }
    public string GetNext()
    {
        return _index < _line.Length ? _line[_index++] : null;
    }
    private string GetNextAndCheckNotEmpty()
    {
        string v = GetNext();
        if (v == null)
        {
            Error("Unable to read!");
        }
        return v;
    }
    public string GetString()
    {
        return GetNextAndCheckNotEmpty();
    }
    public float GetFloat()
    {
        return float.Parse(GetNextAndCheckNotEmpty());
    }
    public int GetInt()
    {
        return int.Parse(GetNextAndCheckNotEmpty());
    }
    public long GetLong()
    {
        return long.Parse(GetNextAndCheckNotEmpty());
    }
    public bool GetBool()
    {
        string v = GetNextAndCheckNotEmpty();
        if (v == "true")
        {
            return true;
        }
        if (v == "false")
        {
            return false;
        }
        Error(v + " isn't bool");
        return false;
    }
    public CfgObject GetObject(string name)
    {
        return (CfgObject)Type.GetType(name).GetConstructor(new[] { typeof(DataStream) }).Invoke(new object[] { this });
    }
    public int GetArrayLength()
    {
        return GetInt();
    }
    public int GetMapLength()
    {
        return GetInt();
    }
    /// <summary>
    /// 加载数据列表(多条数据)
    /// </summary>
    /// <param name="path">绝对路径</param>
    /// <param name="constructor">constructor参数为指定类型的构造函数</param>
    /// <typeparam name="T">数据结构类型</typeparam>
    /// <returns></returns>
    public static Dictionary<K, V> Load<K, V>(string path, Action<DataStream, Dictionary<K, V>> constructor)
    {
        if (!File.Exists(path))
        {
            UnityEngine.Debug.LogError(path + "文件不存在!");
            return null;
        }
        var dict = new Dictionary<K, V>();
        try
        {
            DataStream data = new DataStream(path, Encoding.UTF8);
            int length = data.GetInt();
            for (int i = 0; i < length; i++)
                constructor(data, dict);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"{path}解析异常~\n{e.Message}");
#if UNITY_EDITOR
            UnityEngine.Debug.LogError($"最后一条数据Key:{dict.Last().Key}.");
#endif
        }
        return dict;
    }
#else
    private readonly byte[] _bytes;
    private int _offset;
    public DataStream(string path, Encoding encoding)
    {
        _bytes = File.ReadAllBytes($"{DataDir}{path}");
        _offset = 0;
    }
    public string GetString()
    {
        string value = MessagePackBinary.ReadString(_bytes, _offset, out int length);
        _offset += length;
        return value;
    }
    public float GetFloat()
    {
        float value = MessagePackBinary.ReadSingle(_bytes, _offset, out int length);
        _offset += length;
        return value;
    }
    public int GetInt()
    {
        int value = MessagePackBinary.ReadInt32(_bytes, _offset, out int length);
        _offset += length;
        return value;
    }
    public long GetLong()
    {
        long value = MessagePackBinary.ReadInt64(_bytes, _offset, out int length);
        _offset += length;
        return value;
    }
    public bool GetBool()
    {
        bool value = MessagePackBinary.ReadBoolean(_bytes, _offset, out int length);
        _offset += length;
        return value;
    }
    public int GetArrayLength()
    {
        int value = MessagePackBinary.ReadArrayHeader(_bytes, _offset, out int length);
        _offset += length;
        return value;
    }
    public int GetMapLength()
    {
        int value = MessagePackBinary.ReadMapHeader(_bytes, _offset, out int length);
        _offset += length;
        return value;
    }
    public CfgObject GetObject(string name)
    {
        return (CfgObject)Type.GetType(name).GetConstructor(new[] { typeof(DataStream) }).Invoke(new object[] { this });
    }
#endif

}
