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
  
  
    /// <summary>
    /// 字段信息及数据信息
    /// </summary>
    public class TableFieldInfo : FieldInfo
    {
        /// <summary>
        /// 基础/枚举类型单列数据存储在Data
        /// <para>Class类型多列数据存储在ChildFields</para>
        /// <para>List类型多列数据存储在ChildFields,以下表为索引</para>
        /// <para>Dict类型多列数据pair存储在ChildFields,以下表为索引;key/value存储在pair,pair[0]:key,pair[1]:value</para>
        /// </summary>
        public List<object> Data { get; set; }
        /// <summary>
        /// 子字段
        /// </summary>
        public List<TableFieldInfo> ChildFields { get; set; }
        /// <summary>
        /// 列索引
        /// </summary>
        public int ColumnIndex { get; set; }

        public Dictionary<CheckRuleType, List<string>> RuleDict { get; set; }

        public TableFieldInfo()
        {
            RuleDict = new Dictionary<CheckRuleType, List<string>>();
        }
        public void Set(string name, string type, string des, string check, string group, int index)
        {
            Name = name;
            Type = type;
            Des = des;
            Check = check;
            Group = group;
            ColumnIndex = index;
        }
        public void AsChildSet(string name, string type, int index)
        {
            Name = name;
            Type = type;
            ColumnIndex = index;
        }

        /// <summary>
        /// 不带命名空间,纯类名
        /// </summary>
        public string GetTypeName()
        {
            int endIndex = Type.LastIndexOf('.');
            return Type.Substring(endIndex);
        }
    }
}
