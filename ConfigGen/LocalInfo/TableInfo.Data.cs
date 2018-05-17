using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace ConfigGen.LocalInfo
{
    //---数据定义表,Excel形式中数据类无继承形式[未定义继承解析]
    public class TableDataInfo : TableInfo
    {
        public TableDataInfo(string relPath, DataTable data, ClassTypeInfo classType)
            : base(relPath, data)
        {//className: 首张define表信息中类的名字
            SType = SheetType.Data;
            DataFields = new List<TableFieldInfo>();

            if (classType != null)
                DataClassInfo = classType;
            else
                Util.LogErrorFormat("数据表结构没有指明类型,文件名:{0}", RelPath);
        }

        public ClassTypeInfo DataClassInfo { get; private set; }
        public int DataLength { get { return TableDataSet.Rows.Count - Values.DataSheetDataStartIndex; } }
        public List<TableFieldInfo> DataFields { get; private set; }
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

        public override void Analyze()
        {
            DataTable dt = TableDataSet;
            DataClassInfo.UpdateToDict();
            var dataFieldDict = new Dictionary<string, TableFieldInfo>();
            var fieldInfoDict = DataClassInfo.GetFieldInfoDict();
            string fieldIndex = dt.Rows[Values.DataSheetFieldIndex][0].ToString();
            DataClassInfo.IndexField = fieldInfoDict[fieldIndex];
            //解析检查类定义
            object[] tableFields = dt.Rows[Values.DataSheetFieldIndex].ItemArray;
            for (int i = 0; i < tableFields.Length; i++)
            {
                string field = tableFields[i].ToString();
                //string fieldType = dt.Rows[Values.DataSheetTypeIndex][i].ToString();
                if (!string.IsNullOrWhiteSpace(field))
                {
                    TableFieldInfo tableFieldInfo = new TableFieldInfo();
                    if (fieldInfoDict.ContainsKey(field))
                    {
                        //解析数据类字段
                        FieldInfo fieldInfo = fieldInfoDict[field];
                        string check = dt.Rows[Values.DataSheetCheckIndex][i].ToString();
                        check = string.IsNullOrWhiteSpace(check) ? fieldInfo.Check : check;
                        tableFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Des, check, fieldInfo.Group, i);

                        //解析类字段数据
                        i = AnalyzeField(dt, tableFieldInfo);

                        if (!dataFieldDict.ContainsKey(tableFieldInfo.Name))
                            dataFieldDict.Add(tableFieldInfo.Name, tableFieldInfo);
                    }
                    else
                    {
                        Util.LogErrorFormat("在{0}类型的数据表中,{1}字段名与实际定义不一致,{2}",
                            DataClassInfo.Name, field, GetErrorSite(i + 1, Values.DataSheetFieldIndex + 1));
                    }
                }
                else
                {
                    Util.LogErrorFormat("在{0}类型的数据表中,解析异常,{1}",
                            DataClassInfo.Name, GetErrorSite(i + 1, Values.DataSheetFieldIndex + 1));
                }
            }

            DataFields.AddRange(dataFieldDict.Values);
        }
        private string GetErrorSite(int c, int r)
        {
            return Util.GetErrorSite(RelPath, c, r);
        }

        /// <summary>
        /// 解析类字段数据
        /// </summary>
        /// <param name="column">当前字段列号</param>
        /// <param name="fieldInfo">当前表字段信息</param>
        /// <returns>数值范围:当前列号,或者最后一个子字段列号</returns>
        private int AnalyzeField(DataTable dt, TableFieldInfo fieldInfo)
        {
            int column = fieldInfo.ColumnIndex;
            ////检查类型存在性
            //string error = TableChecker.CheckType(fieldInfo.Type);
            //if (string.IsNullOrWhiteSpace(error))
            //{
            //    Util.LogErrorFormat("数据表中{0}字段不存在,{1}", fieldInfo.Name, GetErrorSite(column + 1, Values.DataSheetFieldIndex + 1));
            //    return column;
            //}
            //解析检查规则
            if (TableChecker.ParseCheckRule(fieldInfo))
            {
                Util.LogWarningFormat("在{0}类型的数据表中,{1}字段规则{2}填写错误,{3}",
                    DataClassInfo.Name, fieldInfo.Name, fieldInfo.Check, GetErrorSite(column + 1, Values.DataSheetFieldIndex + 1));
            }
            BaseTypeInfo baseInfo = fieldInfo.BaseInfo;
            switch (baseInfo.TypeType)
            {
                case TypeType.Base://单列
                case TypeType.Enum:
                    fieldInfo.Data = new List<object>(AnalyzeColumnData(dt, fieldInfo));
                    break;
                case TypeType.Class://多列
                    column = AnalyzeClassField(dt, fieldInfo);
                    break;
                case TypeType.List:
                    column = AnalyzeListField(dt, fieldInfo);
                    break;
                case TypeType.Dict:
                    column = AnalyzeDictField(dt, fieldInfo);
                    break;
                case TypeType.None:
                default:
                    Util.LogErrorFormat("数据表中{0}字段的类型{1}不合法", fieldInfo.Name, fieldInfo.Type);
                    break;
            }
            return column;
        }
        private int AnalyzeClassField(DataTable dt, TableFieldInfo classFieldInfo)
        {
            int startColumn = classFieldInfo.ColumnIndex + 1;
            classFieldInfo.ChildFields = new List<TableFieldInfo>();
            ClassTypeInfo classInfo = classFieldInfo.BaseInfo as ClassTypeInfo;
            if (classInfo.HasSubClass)
                throw new Exception(string.Format("Excel中无法解析继承类型的数据,字段名:{0},{1}",
                   classFieldInfo.Name, GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1)));
            classInfo.UpdateToDict();
            var classfieldDict = classInfo.GetFieldInfoDict();
            for (int i = 0; i < classfieldDict.Count; i++)
            {
                startColumn = startColumn + i;
                string fieldName = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
                if (!classfieldDict.ContainsKey(fieldName))
                {
                    Util.LogErrorFormat("数据表中{0}类型{1}字段不包含{2}子字段,{3}",
                         classFieldInfo.Type, classFieldInfo.Name, fieldName,
                         GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1));
                }

                FieldInfo fieldInfo = classfieldDict[fieldName];
                TableFieldInfo childField = new TableFieldInfo();
                string check = dt.Rows[Values.DataSheetCheckIndex][startColumn].ToString();
                check = string.IsNullOrWhiteSpace(check) ? fieldInfo.Check : check;
                childField.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Des, check, fieldInfo.Group, startColumn);
                startColumn = AnalyzeField(dt, childField);
                classFieldInfo.ChildFields.Add(childField);
            }

            return startColumn;
        }
        private int AnalyzeListField(DataTable dt, TableFieldInfo listFieldInfo)
        {
            int startColumn = listFieldInfo.ColumnIndex + 1;
            listFieldInfo.ChildFields = new List<TableFieldInfo>();
            ListTypeInfo listTypeInfo = listFieldInfo.BaseInfo as ListTypeInfo;
            BaseTypeInfo elemTypeInfo = TypeInfo.GetTypeInfo(listTypeInfo.ItemType);
            string flag = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
            for (int i = 0; !flag.StartsWith(Values.DataSetEndFlag); i++)
            {
                TableFieldInfo elemInfo = new TableFieldInfo();

                switch (elemTypeInfo.TypeType)
                {
                    case TypeType.List:
                    case TypeType.Dict:
                        Util.LogErrorFormat("数据表中{0}字段定义非法,List不允许直接嵌套集合,{1}",
                            listFieldInfo.Name, GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1));
                        break;
                    case TypeType.Class:
                        elemInfo.AsChildSet(i.ToString(), elemTypeInfo.GetClassName(), startColumn - 1);
                        startColumn = AnalyzeField(dt, elemInfo);
                        break;
                    case TypeType.Base:
                    case TypeType.Enum:
                    case TypeType.None:
                    default:
                        elemInfo.AsChildSet(i.ToString(), elemTypeInfo.GetClassName(), startColumn);
                        startColumn = AnalyzeField(dt, elemInfo);
                        break;
                }
                listFieldInfo.ChildFields.Add(elemInfo);

                startColumn = startColumn + 1;
                flag = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
            }
            return startColumn;
        }
        private int AnalyzeDictField(DataTable dt, TableFieldInfo dictFieldInfo)
        {
            int startColumn = dictFieldInfo.ColumnIndex + 1;
            dictFieldInfo.ChildFields = new List<TableFieldInfo>();
            DictTypeInfo dictTypeInfo = dictFieldInfo.BaseInfo as DictTypeInfo;
            BaseTypeInfo keyTypeInfo = TypeInfo.GetTypeInfo(dictTypeInfo.KeyType);
            BaseTypeInfo valueTypeInfo = TypeInfo.GetTypeInfo(dictTypeInfo.ValueType);
            string flag = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
            for (int i = 0; !flag.StartsWith(Values.DataSetEndFlag); i++)
            {
                TableFieldInfo pair = new TableFieldInfo();
                pair.ChildFields = new List<TableFieldInfo>();

                TableFieldInfo keyInfo = new TableFieldInfo();
                keyInfo.AsChildSet("key", keyTypeInfo.GetClassName(), startColumn);
                startColumn = AnalyzeField(dt, keyInfo);

                startColumn = startColumn + 1;

                TableFieldInfo valueInfo = new TableFieldInfo();
                valueInfo.AsChildSet("value", valueTypeInfo.GetClassName(), startColumn);
                switch (valueTypeInfo.TypeType)
                {
                    case TypeType.List:
                    case TypeType.Dict:
                        Util.LogErrorFormat("数据表中{0}字段定义非法,Dict value不允许直接嵌套集合,{1}",
                            dictFieldInfo.Name, GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1));
                        break;
                    case TypeType.Class:
                        valueInfo.AsChildSet("value", valueTypeInfo.GetClassName(), startColumn - 1);
                        startColumn = AnalyzeField(dt, valueInfo);
                        break;
                    case TypeType.Base:
                    case TypeType.Enum:
                    case TypeType.None:
                    default:
                        valueInfo.AsChildSet("value", valueTypeInfo.GetClassName(), startColumn);
                        startColumn = AnalyzeField(dt, valueInfo);
                        break;
                }
                pair.ChildFields.Add(keyInfo);
                pair.ChildFields.Add(valueInfo);
                dictFieldInfo.ChildFields.Add(pair);

                startColumn = startColumn + 1;
                flag = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
            }

            return startColumn;
        }

        /// <summary>
        /// 解析字段列数据
        /// </summary>
        /// <param name="dt">数据表Sheet</param>
        /// <param name="tableFieldInfo">当前字段信息</param>
        /// <returns></returns>
        private List<object> AnalyzeColumnData(DataTable dt, TableFieldInfo fieldInfo)
        {
            List<object> columnData = new List<object>();
            for (int i = Values.DataSheetDataStartIndex; i < dt.Rows.Count; i++)
            {
                object value = dt.Rows[i][fieldInfo.ColumnIndex];
                string error = TableChecker.CheckFieldData(fieldInfo, value);
                if (string.IsNullOrWhiteSpace(error))
                    columnData.Add(value);
                else
                {
                    Util.LogErrorFormat("{0},数据错误位置[{1}{2}]", error,
                       Util.GetColumnName(fieldInfo.ColumnIndex + 1), i.ToString());
                    return columnData;
                }
            }
            return columnData;
        }
    }
}
