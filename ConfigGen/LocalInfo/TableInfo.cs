﻿using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections;

namespace ConfigGen.LocalInfo
{
    //public enum DefineType
    //{
    //    Class,
    //    Enum,
    //}
    public enum SheetType
    {
        None,
        Data,
        Define,
    }

    /// <summary>
    /// Sheet数据内容
    /// </summary>
    abstract class TableInfo
    {
        public SheetType SType { get; protected set; }
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
        public abstract bool Analyze();
    }

    class TableDataInfo : TableInfo
    {
        public TableDataInfo(string relPath, DataTable data, string className)
            : base(relPath, data)
        {//className: 首张define表信息中类的名字
            SType = SheetType.Data;
            DataFields = new List<TableFieldInfo>();

            DataTable dt = TableDataSet;
            DataClassInfo = new ClassInfo();
            DataClassInfo.NamespaceName = TypeInfo.GetNamespaceName(RelPath);
            string checkRule = dt.Rows[Values.DataSheetCheckIndex][0].ToString();
            string ruleRef = CheckRuleType.Define.ToString();
            int startIndex = checkRule.IndexOf(ruleRef, StringComparison.OrdinalIgnoreCase);
            if (startIndex > -1)
            {
                //注:[define:type]数据表引用类型一般是在同命名空间下,字段引用类型可不在同一命名空间下.
                int endIndex = checkRule.IndexOf(Values.CheckRuleSplitFlag, startIndex);
                startIndex += ruleRef.Length;
                DataClassInfo.Name = checkRule.Substring(startIndex, endIndex - startIndex);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(className))
                    DataClassInfo.Name = className;
                else
                {
                    Util.LogErrorFormat("数据类型解析时,无法找到正确的类型定义,文件名:{0}", RelPath);
                }
            }

            string name = DataClassInfo.GetClassName();
            ClassInfo define = LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict[name];
            DataClassInfo = define.Clone();
            DataClassInfo.UpdateToDict();
        }

        public string ClassName { get { return DataClassInfo.GetClassName(); } }
        public ClassInfo DataClassInfo { get; private set; }
        public List<TableFieldInfo> DataFields { get; private set; }
        public override bool Analyze()
        {
            bool isOK = true;
            DataTable dt = TableDataSet;
            var dataFieldDict = new Dictionary<string, TableFieldInfo>();

            var fieldInfoDict = DataClassInfo.GetFieldInfoDict();
            //解析检查类定义
            object[] tableFields = dt.Rows[Values.DataSheetFieldIndex].ItemArray;
            for (int i = 0; i < tableFields.Length; i++)
            {
                string field = tableFields[i].ToString();
                string fieldType = dt.Rows[Values.DataSheetTypeIndex][i].ToString();

                if (!string.IsNullOrWhiteSpace(field) && !string.IsNullOrWhiteSpace(fieldType))
                {
                    TableFieldInfo tableFieldInfo = new TableFieldInfo();
                    if (fieldInfoDict.ContainsKey(field))
                    {
                        //解析数据类字段
                        FieldInfo fieldInfo = fieldInfoDict[field];
                        string check = dt.Rows[Values.DataSheetCheckIndex][i].ToString();
                        check = string.IsNullOrWhiteSpace(check) ? fieldInfo.Check : check;
                        tableFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Des, check, fieldInfo.Group);

                        ////检查类型
                        //if (!fieldType.Equals(fieldInfo.Type))
                        //{
                        //    Util.LogErrorFormat("在{0}类型的数据表中,{1}字段的类{2}与实际定义字段类型{3}不一致,错误位置{4}[{5}{6}]",
                        //       DataClassInfo.Name, tableFieldInfo.Name, tableFieldInfo.Type, fieldInfo.Type, RelPath, Util.GetColumnName(i + 1), (Values.DataSheetTypeIndex + 1).ToString());
                        //    isOK = false;
                        //}                        

                        //解析类字段数据
                        i = AnalyzeFieldAndData(dt, i, tableFieldInfo, out isOK);
                        if (!isOK)
                        {
                            Util.LogErrorFormat("在{0}类型的数据表中,错误位置{1}[{2}{3}]",
                                DataClassInfo.Name, Util.GetColumnName(i + 1), (Values.DataSheetCheckIndex + 1).ToString());
                        }

                        if (!dataFieldDict.ContainsKey(tableFieldInfo.Name))
                            dataFieldDict.Add(tableFieldInfo.Name, tableFieldInfo);
                    }
                    else
                    {
                        Util.LogErrorFormat("在{0}类型的数据表中,{1}字段名与实际定义不一致,错误位置{2}[{3}{4}]",
                            DataClassInfo.Name, field, RelPath, Util.GetColumnName(i + 1), (Values.DataSheetFieldIndex + 1).ToString());
                        isOK = false;
                    }
                }
                else
                {
                    Util.LogErrorFormat("在{0}类型的数据表中,解析异常,错误位置{1}[{2}{3}]",
                            DataClassInfo.Name, RelPath, Util.GetColumnName(i + 1), (Values.DataSheetFieldIndex + 1).ToString());
                    isOK = false;
                }
            }

            DataFields.AddRange(dataFieldDict.Values);
            return isOK;
        }


        /// <summary>
        /// 解析类字段数据
        /// </summary>
        /// <param name="column">当前字段列号</param>
        /// <param name="fieldInfo">当前表字段信息</param>
        /// <returns>数值范围:当前列号,或者最后一个子字段列号</returns>
        private int AnalyzeFieldAndData(DataTable dt, int column, TableFieldInfo fieldInfo, out bool isOK)
        {
            isOK = true;
            //解析检查规则
            if (TableChecker.ParseCheckRule(fieldInfo))
            {
                Util.LogWarningFormat("在{0}类型的数据表中,规则填写错误,错误位置{1}[{2}{3}]",
                    DataClassInfo.Name, Util.GetColumnName(column + 1), (Values.DataSheetCheckIndex + 1).ToString());
            }
            fieldInfo.TypeType = TypeInfo.GetFieldTypeType(fieldInfo.Type);
            fieldInfo.ColumnIndex = column;
            switch (fieldInfo.TypeType)
            {
                case FieldTypeType.Base://单列
                case FieldTypeType.Enum:
                    fieldInfo.Data = new List<object>(AnalyzeColumnData(dt, fieldInfo, out isOK));
                    if (!isOK)
                    {
                        Util.LogErrorFormat("{0}字段数据解析错误,错误位置{1}[{2}{3}]",
                           fieldInfo.Name, RelPath, Util.GetColumnName(column + 1), (Values.DataSheetFieldIndex + 1).ToString());
                    }
                    return column;
                case FieldTypeType.Class://多列
                    fieldInfo.ChildFields = new List<TableFieldInfo>();
                    column = AnalyzeClassChildField(dt, column + 1, fieldInfo, out isOK);
                    break;
                case FieldTypeType.List:
                    fieldInfo.ChildFields = new List<TableFieldInfo>();
                    column = AnalyzeListChildField(dt, column + 1, fieldInfo, out isOK);
                    break;
                case FieldTypeType.Dict:
                    fieldInfo.ChildFields = new List<TableFieldInfo>();
                    column = AnalyzeDictChildField(dt, column + 1, fieldInfo, out isOK);
                    break;
                case FieldTypeType.None:
                default:
                    Util.LogErrorFormat("{0}字段的类型{1}不合法", fieldInfo.Name, fieldInfo.Type);
                    break;
            }

            if (!isOK)
            {
                Util.LogErrorFormat("{0}字段数据解析错误,错误位置{1}[{2}{3}]",
                   fieldInfo.Name, RelPath, Util.GetColumnName(column), (Values.DataSheetFieldIndex + 1).ToString());
            }
            return column;
        }
        
        private int AnalyzeClassChildField(DataTable dt, int nextColumn, TableFieldInfo parentFieldInfo, out bool isOk)
        {
            isOk = true;
            string error = TableChecker.CheckType(parentFieldInfo.Type);
            if (string.IsNullOrWhiteSpace(error))
            {
                Util.LogErrorFormat("{0}字段{1}", error);
                isOk = false;
                return nextColumn;
            }
            ClassInfo classInfo = LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict[parentFieldInfo.Type];
            int lastColumn = -1;
            for (int i = 0; i < classInfo.Fields.Count; i++)
            {
                FieldInfo fieldInfo = classInfo.Fields[i];
                TableFieldInfo childField = new TableFieldInfo();
                childField.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Des, fieldInfo.Check, fieldInfo.Group);
                lastColumn = AnalyzeFieldAndData(dt, nextColumn + i, childField, out isOk);
                if (!isOk)
                {
                    Util.LogErrorFormat("{0}类型的子字段{1}解析错误,错误位置{2}[{3}{4}]",
                  classInfo.Name, RelPath, Util.GetColumnName(lastColumn + 1), (Values.DataSheetFieldIndex + 1).ToString());
                }
                parentFieldInfo.ChildFields.Add(childField);
            }
            return lastColumn;
        }
        private int AnalyzeListChildField(DataTable dt, int column, TableFieldInfo parentFieldInfo, out bool isOK)
        {
            isOK = true;
            for (int i = column; i < dt.Rows.Count; i++)
            {

            }
            return 1;
        }
        private int AnalyzeDictChildField(DataTable dt, int column, TableFieldInfo parentFieldInfo, out bool isOK)
        {
            isOK = true;
            for (int i = column; i < dt.Rows.Count; i++)
            {

            }
            return 1;
        }
        /// <summary>
        /// 解析字段列数据
        /// </summary>
        /// <param name="dt">数据表Sheet</param>
        /// <param name="tableFieldInfo">当前字段信息</param>
        /// <returns></returns>
        private List<object> AnalyzeColumnData(DataTable dt, TableFieldInfo fieldInfo, out bool isOK)
        {
            isOK = true;
            List<object> columnData = new List<object>();
            for (int i = Values.DataSheetDataStartIndex; i < dt.Rows.Count; i++)
            {
                object value = dt.Rows[fieldInfo.ColumnIndex];
                string error = TableChecker.CheckFieldData(fieldInfo, value);
                if (string.IsNullOrWhiteSpace(error))
                    columnData.Add(value);
                else
                {
                    Util.LogErrorFormat("{0},数据错误位置[{1}{2}]", error,
                       Util.GetColumnName(fieldInfo.ColumnIndex + 1), i.ToString());
                    isOK = false;
                    return new List<object>();
                }
            }
            return columnData;
        }

        /// <summary>
        /// 只查询数据表中类型定义
        /// </summary>
        public override bool Exist(string content)
        {
            return false;
        }
        /// <summary>
        /// 只做数据表中数据类型相关参数替换
        /// </summary>
        public override bool Replace(string arg1, string arg2)
        {
            return false;
        }
    }
    class TableDefineInfo : TableInfo
    {
        public TableDefineInfo(string relPath, DataTable data)
            : base(relPath, data)
        {
            SType = SheetType.Define;
            ClassInfoDict = new Dictionary<string, ClassInfo>();
            EnumInfoDict = new Dictionary<string, EnumInfo>();
        }
        public string FirstName { get; private set; }
        public Dictionary<string, ClassInfo> ClassInfoDict { get; private set; }
        public Dictionary<string, EnumInfo> EnumInfoDict { get; private set; }

        public string GetFirstName()
        {
            string firstName = null;
            DataTable dt = TableDataSet;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string firstColumn = dt.Rows[i][0].ToString();
                if (!firstColumn.StartsWith(Values.DefineTypeFlag)) continue;

                string defineTypeStr = firstColumn.TrimStart(Values.DefineTypeFlag.ToCharArray());
                if (defineTypeStr.Equals("class"))
                {
                    firstName = dt.Rows[i][1].ToString();
                    break;
                }
            }
            return firstName;
        }


        /// <summary>
        /// 只查询定义表中类型信息
        /// </summary>
        public override bool Exist(string content)
        {
            return false;
        }
        /// <summary>
        /// 只替换定义表中类型相关参数
        /// </summary>
        public override bool Replace(string arg1, string arg2)
        {
            return false;
        }
        public override bool Analyze()
        {
            bool isOK = true;
            DataTable dt = TableDataSet;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string firstColumn = dt.Rows[i][0].ToString();
                if (!firstColumn.StartsWith(Values.DefineTypeFlag)) continue;

                //开始定义类型
                string defineTypeStr = firstColumn.TrimStart(Values.DefineTypeFlag.ToCharArray());
                string name = dt.Rows[i][1].ToString();
                string group = dt.Rows[i][2].ToString();
                i += 2;
                if (defineTypeStr.Equals("class"))
                {
                    ClassInfo classInfo = new ClassInfo();
                    classInfo.RelPath = RelPath;
                    classInfo.NamespaceName = TypeInfo.GetNamespaceName(RelPath);
                    classInfo.Name = name;
                    classInfo.Group = group;
                    if (string.IsNullOrWhiteSpace(FirstName))
                        FirstName = classInfo.Name;
                    for (int j = i; string.IsNullOrWhiteSpace(dt.Rows[j][0].ToString()); j++, i++)
                    {
                        FieldInfo fieldInfo = new FieldInfo();
                        fieldInfo.Name = dt.Rows[j][0].ToString();
                        string errorString = null;
                        string fieldType = dt.Rows[j][1].ToString();
                        int endIndex = fieldType.LastIndexOf('.');
                        if (endIndex == -1)
                            fieldType = string.Format("{0}.{1}", classInfo.NamespaceName, fieldType);
                        errorString = TableChecker.CheckFieldClassName(fieldType);
                        if (!string.IsNullOrWhiteSpace(errorString))
                        {
                            Util.LogErrorFormat("数据类型{0}的字段{1}类型定义错误:{2},错误位置{3}[{4}{5}]",
                                classInfo.GetClassName(), fieldInfo.Name, errorString, RelPath, Util.GetColumnName(2 + 1), (j + 1).ToString());
                            isOK = false;
                        }
                        fieldInfo.Type = fieldType;
                        fieldInfo.Des = dt.Rows[j][2].ToString();
                        fieldInfo.Check = dt.Rows[j][3].ToString();
                        fieldInfo.Group = dt.Rows[j][4].ToString();
                        classInfo.Fields.Add(fieldInfo);
                    }
                    ClassInfoDict.Add(name, classInfo);
                }
                else if (defineTypeStr.Equals("enum"))
                {
                    EnumInfo enumInfo = new EnumInfo();
                    enumInfo.RelPath = RelPath;
                    enumInfo.NamespaceName = TypeInfo.GetNamespaceName(RelPath);
                    enumInfo.Name = name;
                    enumInfo.Group = group;
                    for (int j = i; string.IsNullOrWhiteSpace(dt.Rows[j][0].ToString()); j++, i++)
                    {
                        EnumKeyValue kv = new EnumKeyValue();
                        kv.Key = dt.Rows[j][0].ToString();
                        kv.Value = dt.Rows[j][1].ToString();
                        kv.Des = dt.Rows[j][2].ToString();
                        enumInfo.KeyValuePair.Add(kv);
                    }
                    EnumInfoDict.Add(name, enumInfo);
                }
                else
                {
                    Util.LogErrorFormat("当前定义只能是class,enum等类型,Type:{0}", defineTypeStr);
                    isOK = false;
                }
            }

            return isOK;
        }
    }
    /// <summary>
    /// 字段信息及数据信息
    /// </summary>
    class TableFieldInfo : FieldInfo
    {
        public FieldTypeType TypeType { get; set; }
        /// <summary>
        /// 基础/枚举类型单列数据存储在Data
        /// Class类型多列数据存储在ChildFields
        /// List类型多列数据存储在ChildFields
        /// Dict类型多列数据key/value存储在ChildFields
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
        public void Set(string name, string type, string des, string check, string group)
        {
            Name = name;
            Type = type;
            Des = des;
            Check = check;
            Group = group;
        }

        /// <summary>
        /// 不带命名空间,纯类名
        /// </summary>
        public string GetTypeName()
        {
            int endIndex = Type.LastIndexOf('.');
            return Type.Substring(endIndex);
        }
        public ClassInfo GetClassInfo()
        {
            string errorString = TableChecker.CheckType(Type);
            if (!string.IsNullOrWhiteSpace(errorString))
                return null;

            return LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict[Type];
        }
        public string WriteCsv()
        {
            return null;
        }
    }
}
