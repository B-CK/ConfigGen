using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
namespace Example
{
    public abstract class CfgObject { }

    /// <summary>
    /// 数据解析类
    /// <summary>
    public class DataStream
    {
        private readonly string[] _line;
        private int _index;
        public DataStream(string path, Encoding encoding)
        {
            _line = File.ReadAllLines(path);
            _index = 0;
        }

        public string GetNext()
        {
            return _index < _line.Length ? _line[_index++] : null;
        }

        private void Error(string err)
        {
            throw new Exception(err);
        }

        private string GetNextAndCheckNotEmpty()
        {
            string v = GetNext();
            if (v == null)
            {
                Error("read not enough");
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

        /// <summary>
        /// 加载数据列表(多条数据)
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="constructor">constructor参数为指定类型的构造函数</param>
        /// <typeparam name="T">数据结构类型</typeparam>
        /// <returns></returns>
        public static List<T> Load<T>(string path, Func<DataStream, T> constructor)
        {
            if (!File.Exists(path))
            {
                UnityEngine.Debug.LogError(path + "文件不存在!");
                return new List<T>();
            }
            DataStream data = new DataStream(path, Encoding.UTF8);
            List<T> list = new List<T>();
            int length = data.GetInt();
            for (int i = 0; i < length; i++)
                list.Add(constructor(data));
            return list;
        }

    }
}