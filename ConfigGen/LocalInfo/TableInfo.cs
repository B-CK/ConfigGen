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



    public class DataBaseInfo : FieldInfo
    {//Name,Type,Check,Group
        public List<object> Data { get; set; }//多行数据
        public DataBaseInfo()
        {
            Data = new List<object>();
        }
    }
    public class DataClassInfo : FieldInfo
    {
        public Dictionary<string, FieldInfo> Fields { get; set; }//多列数据
        public List<string> Types { get; set; }//多态中标识数据类型,每条数据的类型
        public DataClassInfo()
        {
            Fields = new Dictionary<string, FieldInfo>();
            Types = new List<string>();
        }
    }
    public class DataListInfo : FieldInfo
    {
        public int MaxIndex = 0;
        public List<DataElementInfo> DataSet { get; set; }//多行数据
        public DataListInfo()
        {
            DataSet = new List<DataElementInfo>();
        }
    }
    //一行数据,无数据填充##结束符
    public class DataElementInfo : FieldInfo
    {
        public List<FieldInfo> Elements { get; set; }//多列数据
        public DataElementInfo()
        {
            Elements = new List<FieldInfo>();
        }
    }
    public class DataDictInfo : FieldInfo
    {
        public List<DataPairInfo> DataSet { get; set; }//多行数据
        public DataDictInfo()
        {
            DataSet = new List<DataPairInfo>();
        }
    }
    //一行数据,无数据填充##结束符
    public class DataPairInfo : FieldInfo
    {
        public List<KeyValuePair<DataBaseInfo, FieldInfo>> Pairs { get; set; }//多列数据
        public DataPairInfo()
        {
            Pairs = new List<KeyValuePair<DataBaseInfo, FieldInfo>>();
        }
    }


    public class DataBase : FieldInfo
    {
        public object Data;
    }
    public class DataClass : FieldInfo
    {
        public Dictionary<string, FieldInfo> Fields;
        public DataClass()
        {
            Fields = new Dictionary<string, FieldInfo>();
        }
    }
    public class DataList : FieldInfo
    {
        public List<FieldInfo> Elements;
        public DataList()
        {
            Elements = new List<FieldInfo>();
        }
    }
    public class DataDict : FieldInfo
    {
        public List<KeyValuePair<DataBase, FieldInfo>> Pairs { get; set; }
        public DataDict()
        {
            Pairs = new List<KeyValuePair<DataBase, FieldInfo>>();
        }
    }
}
