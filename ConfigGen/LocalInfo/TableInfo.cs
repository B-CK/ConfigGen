using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections;

namespace ConfigGen.LocalInfo
{
    public enum DefineType
    {
        Class,
        Enum,
    }
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
        public SheetType SType { get; private set; }
        public string RelPath { get; private set; }
        public DataTable TableDataSet { get; private set; }

        public TableInfo(SheetType type, string relPath, DataTable data)
        {
            SType = type;
            RelPath = relPath;
            TableDataSet = data;
        }
        public abstract bool Exist(string content);
        public abstract bool Replace(string arg1, string arg2);
        public abstract bool Analyze();
    }

    class TableDataInfo : TableInfo
    {
        private string _className;
        public TableDataInfo(SheetType type, string relPath, DataTable data, string className)
            : base(type, relPath, data)
        {
            _className = className;
            DataFields = new List<TableFieldInfo>();
            _dataFieldDict = new Dictionary<string, TableFieldInfo>();
        }

        public ClassInfo DataClassInfo { get; private set; }
        public List<TableFieldInfo> DataFields { get; private set; }
        private Dictionary<string, TableFieldInfo> _dataFieldDict;
        private void AddField(TableFieldInfo tableField)
        {

        }
        public override bool Analyze()
        {
            bool isOK = true;
            DataTable dt = TableDataSet;
            DataClassInfo = new ClassInfo();
            DataClassInfo.NamespaceName = TypeInfo.GetNamespaceName(RelPath);
            string checkRule = dt.Rows[Values.DataSheetCheckIndex][0].ToString();
            string ruleRef = CheckRuleType.Define.ToString();
            int startIndex = checkRule.IndexOf(ruleRef, StringComparison.OrdinalIgnoreCase);
            string className = "";
            if (startIndex > -1)
            {
                //注:[ref:type]数据表引用类型一般是在同命名空间下,字段引用类型可不在同一命名空间下.
                int endIndex = checkRule.IndexOf(Values.CheckRuleSplitFlag, startIndex);
                startIndex += ruleRef.Length;
                DataClassInfo.Name = checkRule.Substring(startIndex, endIndex - startIndex);
                className = DataClassInfo.GetClassName();
                if (!TableChecker.CheckClass(className))
                {
                    Util.LogErrorFormat("TableDataInfo数据表引用{0}类型不存在,表名:{1}", className, RelPath);
                    isOK = false;
                }
            }
            else if (!string.IsNullOrWhiteSpace(_className))
                DataClassInfo.Name = _className;
            else
            {
                Util.LogErrorFormat("TableDataInfo数据类型解析时,无法找到正确的类型定义,表名:{0}", RelPath);
                isOK = false;
            }
            className = DataClassInfo.GetClassName();
            ClassInfo define = LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict[className];
            DataClassInfo = define.Clone();
            DataClassInfo.UpdateToDict();
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
                        tableFieldInfo.Name = fieldInfo.Name;
                        tableFieldInfo.Des = fieldInfo.Des;
                        tableFieldInfo.Group = fieldInfo.Group;
                        string check = dt.Rows[Values.DataSheetCheckIndex][i].ToString();
                        tableFieldInfo.Check = string.IsNullOrWhiteSpace(check) ? fieldInfo.Check : check;
                        //解析检查规则

                        int endIndex = fieldType.LastIndexOf('.');
                        if (endIndex == -1)
                            fieldType = string.Format("{0}.{1}", DataClassInfo.NamespaceName, fieldType);
                        if (!tableFieldInfo.Type.Equals(fieldInfo.Type))
                        {
                            Util.LogErrorFormat("在{0}类型的数据表中,{1}字段的类{2}与实际定义字段类型{3}不一致,错误位置{4}[{5}{6}]"
                              , DataClassInfo.Name, tableFieldInfo.Name, tableFieldInfo.Type, fieldInfo.Type, RelPath, Util.GetColumnName(i + 1), (Values.DataSheetTypeIndex + 1).ToString());
                            isOK = false;
                        }
                        tableFieldInfo.Type = fieldInfo.Type;
                        tableFieldInfo.TypeType = TypeInfo.GetFieldTypeType(tableFieldInfo.Type);
                        tableFieldInfo.ColumnIndex = i;

                        switch (tableFieldInfo.TypeType)
                        {
                            case FieldTypeType.Base://单列
                            case FieldTypeType.Enum:
                                tableFieldInfo.Data = new List<List<object>>();
                                if (!AnalyzeColumnData(dt, i, tableFieldInfo))
                                {
                                    Util.LogErrorFormat("在{0}类型的数据表中,{1}字段的数据解析错误,错误位置{3}[{4}{5}]",
                                       DataClassInfo.Name, tableFieldInfo.Name, tableFieldInfo.Type, RelPath, Util.GetColumnName(i + 1), (Values.DataSheetTypeIndex + 1).ToString());
                                    isOK = false;
                                }
                                break;
                            case FieldTypeType.Class://多列
                            case FieldTypeType.List:
                            case FieldTypeType.Dict:
                                break;
                            case FieldTypeType.None:
                            default:
                                Util.LogErrorFormat("在{0}类型的数据表中,{1}字段的类型{2}定义不合法,错误位置{3}[{4}{5}]",
                                   DataClassInfo.Name, tableFieldInfo.Name, tableFieldInfo.Type, RelPath, Util.GetColumnName(i + 1), (Values.DataSheetTypeIndex + 1).ToString());
                                isOK = false;
                                break;
                        }
                        if (tableFieldInfo.TypeType != FieldTypeType.None)
                        {
                            tableFieldInfo.Data = new List<List<object>>();
                            if (!AnalyzeColumnData(dt, i, tableFieldInfo))
                            {
                                Util.LogErrorFormat("在{0}类型的数据表中,{1}字段的数据解析错误,错误位置{3}[{4}{5}]",
                                   DataClassInfo.Name, tableFieldInfo.Name, tableFieldInfo.Type, RelPath, Util.GetColumnName(i + 1), (Values.DataSheetTypeIndex + 1).ToString());
                                isOK = false;
                            }
                        }
                        else
                        {

                        }

                        if (!_dataFieldDict.ContainsKey(tableFieldInfo.Name))
                            _dataFieldDict.Add(tableFieldInfo.Name, tableFieldInfo);
                    }
                    else
                    {
                        Util.LogErrorFormat("在{0}类型的数据表中,{1}字段名与实际定义不一致,错误位置{4}[{2}{3}]",
                            DataClassInfo.Name, field, RelPath, Util.GetColumnName(i + 1), (Values.DataSheetFieldIndex + 1).ToString());
                        isOK = false;
                    }
                }
                ////解析数据类字段的子字段.一般为集合或者自定义类型
                //else if (!string.IsNullOrWhiteSpace(field) && string.IsNullOrWhiteSpace(fieldType))
                //{                    

                //}
            }

            //解析类字段数据





            for (int i = Values.DataSheetDataStartIndex; i < dt.Rows.Count; i++)
            {
                //存数据到类字段
                object[] cells = dt.Rows[i].ItemArray;
                for (int j = 0; j < cells.Length; j++)
                {
                    object cell = cells[j];
                    FieldInfo fieldInfo = new FieldInfo();
                    switch (i)
                    {
                        case Values.DataSheetCheckIndex:
                            break;
                        case Values.DataSheetFieldIndex:
                            break;
                        case Values.DataSheetTypeIndex:
                            break;
                        default:
                            break;
                    }
                }

                //1.ref确定类型
                //2.查后续表是否有
                for (int j = i; string.IsNullOrWhiteSpace(dt.Rows[j][0].ToString()); j++, i++)
                {
                    FieldInfo fieldInfo = new FieldInfo();
                    fieldInfo.Name = dt.Rows[j][0].ToString();
                    fieldInfo.Type = dt.Rows[j][1].ToString();
                    fieldInfo.Des = dt.Rows[j][2].ToString();
                    fieldInfo.Check = dt.Rows[j][3].ToString();
                }
            }
            DataFields.AddRange(_dataFieldDict.Values);
            return isOK;
        }
        private bool AnalyzeChildField(DataTable dt, int column, TableFieldInfo tableFieldInfo)
        {
            return false;
        }
        private bool AnalyzeColumnData(DataTable dt, int column, TableFieldInfo tableFieldInfo)
        {
            List<object> columnData = new List<object>();
            for (int i = Values.DataSheetDataStartIndex; i < dt.Rows.Count; i++)
            {

            }
            tableFieldInfo.Data.Add(columnData);
            return false;
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
        public TableDefineInfo(SheetType type, string relPath, DataTable data)
            : base(type, relPath, data)
        {
            ClassInfoDict = new Dictionary<string, ClassInfo>();
            EnumInfoDict = new Dictionary<string, EnumInfo>();
        }

        public Dictionary<string, ClassInfo> ClassInfoDict { get; private set; }
        public Dictionary<string, EnumInfo> EnumInfoDict { get; private set; }


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
            DefineType defineType = DefineType.Class;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string firstColumn = dt.Rows[i][0].ToString();
                if (!firstColumn.StartsWith(Values.DefineTypeFlag)) continue;

                //开始定义类型
                string defineTypeStr = firstColumn.TrimStart(Values.DefineTypeFlag.ToCharArray());
                string name = dt.Rows[i][1].ToString();
                string group = dt.Rows[i][2].ToString();
                i += 2;
                if (Enum.TryParse(defineTypeStr, out defineType))
                {
                    switch (defineType)
                    {
                        case DefineType.Class:
                            ClassInfo classInfo = new ClassInfo();
                            classInfo.RelPath = RelPath;
                            classInfo.NamespaceName = TypeInfo.GetNamespaceName(RelPath);
                            classInfo.Name = name;
                            classInfo.Group = group;
                            for (int j = i; string.IsNullOrWhiteSpace(dt.Rows[j][0].ToString()); j++, i++)
                            {
                                FieldInfo fieldInfo = new FieldInfo();
                                fieldInfo.Name = dt.Rows[j][0].ToString();
                                string errorString = null;
                                string fieldType = dt.Rows[j][1].ToString();
                                int endIndex = fieldType.LastIndexOf('.');
                                if (endIndex == -1)
                                    fieldType = string.Format("{0}.{1}", classInfo.NamespaceName, fieldType);
                                if (!TableChecker.CheckFieldClassName(fieldType, out errorString))
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
                            break;
                        case DefineType.Enum:
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
                            break;
                    }

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
        /// 集合类型数据,子元素为空时表示结束.
        /// 其他情况均要将类型各个字段填充值
        /// </summary>
        public List<List<object>> Data { get; set; }
        /// <summary>
        /// 子字段
        /// </summary>
        public List<TableFieldInfo> ChildFields { get; set; }
        /// <summary>
        /// 列索引
        /// </summary>
        public int ColumnIndex { get; set; }

        public CheckRuleType RuleType { get; set; }
        public string[] RuleArgs { get; set; }

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
            if (!TableChecker.CheckClass(Type))
                return null;
            return LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict[Type];
        }
        public string WriteCsv()
        {
            return null;
        }
    }
}
