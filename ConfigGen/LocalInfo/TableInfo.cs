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
        public List<object> Data { get; set; }
          
    }
    public class DataClassInfo : FieldInfo
    {
        public Dictionary<string, FieldInfo> Fields { get; set; }
        public List<string> Types { get; set; }//多态中标识数据类型,每条数据的类型
    }
    public class DataListInfo : FieldInfo
    {
        public List<FieldInfo> Elements { get; set; }
    }
    public class DataDictInfo : FieldInfo
    {
        /// <summary>
        /// list    -   多条数据字典
        /// dict    -   单条数据字典
        /// </summary>
        public List<KeyValuePair<DataBaseInfo, FieldInfo>> Pairs { get; set; }
    }
}
