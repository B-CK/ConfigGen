using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections;

namespace ConfigGen.LocalInfo
{
    public enum SheetType
    {
        None,
        Data,
        Define,
    }

    /// <summary>
    /// Sheet数据内容
    /// </summary>
    public abstract class TableInfo
    {
        /// <summary>
        /// 文件相对路径
        /// </summary>
        public string RelPath { get; private set; }
        public DataTable TableDataSet { get; private set; }

        public TableInfo(string relPath, DataTable data)
        {
            RelPath = relPath;
            TableDataSet = data;
        }
        public abstract bool Exist(string content);
        public abstract bool Replace(string arg1, string arg2);
        public abstract void Analyze();
    }

    public class Data
    {

    }

    public class DataBase : Data
    {
        public object Data;

        public static implicit operator double(DataBase value)
        {
            return Convert.ToDouble(value.Data);
        }
        public static implicit operator long(DataBase value)
        {
            return Convert.ToInt64(value.Data);
        }
        public static implicit operator string(DataBase value)
        {
            return Convert.ToString(value.Data);
        }
    }
    public class DataClass : Data
    {
        public string Type { get; set; }
        public Dictionary<string, Data> Fields;
        public DataClass()
        {
            Fields = new Dictionary<string, Data>();
        }
    }
    public class DataList : Data
    {
        public List<Data> Elements;
        public DataList()
        {
            Elements = new List<Data>();
        }
    }
    public class DataDict : Data
    {
        public List<KeyValuePair<DataBase, Data>> Pairs { get; set; }
        public DataDict()
        {
            Pairs = new List<KeyValuePair<DataBase, Data>>();
        }
    }
}
