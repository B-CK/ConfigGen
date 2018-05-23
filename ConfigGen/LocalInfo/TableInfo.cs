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
        /// 
        /// <para>Class
        /// -->ChildFields
        /// $type       -           多态类型标识,普通类型无此标识
        ///             -           Data,中存储每一行所对应的类型
        ///             -           ChildFields.中为每个子类字段信息,key-派生类型,value-派生类型信息
        ///             -           派生字段以字段名索引字段数据
        /// key         -           字段名
        /// value       -           字段信息
        /// </para>
        /// 
        /// <para>List
        /// -->Data
        /// 基础类型    -           直接存储在Data(包含枚举)
        /// -->ChildFields
        /// key         -           索引号
        /// value       -           普通类或者派生类
        ///             -->ChildFields
        ///             key         -           字段名
        ///             value       -           字段信息
        /// </para>
        /// 
        /// <para>Dict
        /// -->ChildFields
        /// key         -           索引号
        /// value       -           Pair信息
        ///             -->ChildFields
        ///             DictKey             -           类型为int,long,string,enum
        ///             DictValue           -           value内容
        ///                         -->Data
        ///                         基础类型    -       直接存储在Data(包含枚举)
        ///                         -->ChildFields
        ///                         key         -       字段名
        ///                         value       -       字段信息
        /// </para>
        /// </summary>
        public List<object> Data { get; set; }
        /// <summary>
        /// 子字段
        /// </summary>
        public Dictionary<string, TableFieldInfo> ChildFields { get; set; }
        /// <summary>
        /// 列索引,下标号从0开始
        /// 基础类型        -       当前列号
        /// 
        /// 类类型          -       普通类型/派生类型
        ///                 普通类型        -       无类型标识,字段从下一个列号开始
        ///                 派生类型        -       类型存在当前列,字段从下一个列号开始
        ///                 
        /// List            -       基础类型/类类型
        ///                 基础类型        -       从下一个列号开始
        ///                 类类型          -       从当前列号开始
        ///                
        /// Dict            -       key&value从下一列号
        ///                 value           -       基础类型/类类型
        ///                                 基础类型        -       从下一个列号开始
        ///                                 类类型          -       从当前列号开始
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
        public List<KeyValuePair<object, FieldInfo>> Pairs { get; set; }
    }
}
