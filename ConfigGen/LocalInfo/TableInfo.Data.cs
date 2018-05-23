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
        {
            if (classType != null)
                ClassTypeInfo = classType;
            else
                Util.LogErrorFormat("数据表结构没有指明类型,文件名:{0}", RelPath);
        }

        public ClassTypeInfo ClassTypeInfo { get; protected set; }
        /// <summary>
        /// 数据行数
        /// </summary>
        public int DataLength { get; protected set; }
        public DataClassInfo DataClassInfo { get; protected set; }
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
            DataClassInfo = new DataClassInfo();
            DataClassInfo.Set(ClassTypeInfo.Name, ClassTypeInfo.GetClassName(), null, null);

            AnalyzeClassField(dt, DataClassInfo, -1);
            DataLength = dt.Rows.Count - Values.DataSheetDataStartIndex;
        }
        private string GetErrorSite(int c, int r)
        {
            return Util.GetErrorSite(RelPath, c, r);
        }

        private int AnalyzeClassField(DataTable dt, DataClassInfo classFieldInfo, int index)
        {
            int startColumn = index + 1;
            classFieldInfo.Fields = new Dictionary<string, FieldInfo>();
            ClassTypeInfo classInfo = classFieldInfo.BaseInfo as ClassTypeInfo;
            classInfo.UpdateToDict();
            var classFieldDict = classInfo.GetFieldInfoDict();
            if (classInfo.HasSubClass)
                throw new Exception(string.Format("Excel中无法解析继承类型的数据,字段名:{0},{1}",
                   classFieldInfo.Name, GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1)));
            for (int i = 0; i < classFieldDict.Count; i++)
            {
                startColumn = startColumn + i;
                string fieldName = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
                if (!classFieldDict.ContainsKey(fieldName))
                {
                    throw new Exception(string.Format("在{0}类型的数据表中,{1}字段名与实际定义不一致,{2}",
                            ClassTypeInfo.Name, fieldName, GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1)));
                }
                if (string.IsNullOrWhiteSpace(fieldName))
                {
                    throw new Exception(string.Format("数据表对应的类型字段名必须填写完整,{1}",
                            GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1)));
                }

                //解析数据类字段
                FieldInfo fieldInfo = classFieldDict[fieldName];
                string check = dt.Rows[Values.DataSheetCheckIndex][startColumn].ToString();
                check = string.IsNullOrWhiteSpace(check) ? fieldInfo.Check : check;

                BaseTypeInfo baseType = fieldInfo.BaseInfo;
                switch (baseType.TypeType)
                {
                    case TypeType.Base:
                    case TypeType.Enum:
                        DataBaseInfo baseFieldInfo = new DataBaseInfo();
                        baseFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, check, fieldInfo.Group);
                        classFieldInfo.Fields.Add(baseFieldInfo.Name, baseFieldInfo);
                        startColumn = AnalyzeBaseField(dt, baseFieldInfo, startColumn);
                        break;
                    case TypeType.Class://类的检查规则需要自定义
                        DataClassInfo subClassFieldInfo = new DataClassInfo();
                        subClassFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, check, fieldInfo.Group);
                        classFieldInfo.Fields.Add(subClassFieldInfo.Name, subClassFieldInfo);
                        startColumn = AnalyzeClassField(dt, subClassFieldInfo, startColumn);
                        break;
                    case TypeType.List://集合中元素的规则可手动逐个重写
                        DataListInfo listFieldInfo = new DataListInfo();
                        listFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, check, fieldInfo.Group);
                        classFieldInfo.Fields.Add(listFieldInfo.Name, listFieldInfo);
                        startColumn = AnalyzeListField(dt, listFieldInfo, startColumn);
                        break;
                    case TypeType.Dict://集合中元素的规则可手动逐个重写
                        DataDictInfo dictFieldInfo = new DataDictInfo();
                        dictFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, check, fieldInfo.Group);
                        classFieldInfo.Fields.Add(dictFieldInfo.Name, dictFieldInfo);
                        startColumn = AnalyzeDictField(dt, dictFieldInfo, startColumn);
                        break;
                    case TypeType.None:
                    default:
                        break;
                }
            }

            return startColumn;
        }
        private int AnalyzeBaseField(DataTable dt, DataBaseInfo fieldInfo, int index)
        {
            int column = index;
            if (TableChecker.ParseCheckRule(fieldInfo))
            {
                Util.LogWarningFormat("在{0}类型的数据表中,{1}字段规则{2}填写错误,{3}",
                    ClassTypeInfo.Name, fieldInfo.Name, fieldInfo.Check, GetErrorSite(column + 1, Values.DataSheetFieldIndex + 1));
            }
            fieldInfo.Data = new List<object>();
            for (int i = Values.DataSheetDataStartIndex; i < dt.Rows.Count; i++)
            {
                object value = dt.Rows[i][column];
                string error = TableChecker.CheckFieldData(fieldInfo, value);
                if (string.IsNullOrWhiteSpace(error))
                    fieldInfo.Data.Add(value);
                else
                {
                    Util.LogErrorFormat("{0},数据错误位置[{1}{2}]", error,
                       Util.GetColumnName(column + 1), i.ToString());
                    break;
                }
            }
            return column;
        }
        private int AnalyzeListField(DataTable dt, DataListInfo listFieldInfo, int index)
        {
            int startColumn = index + 1;
            listFieldInfo.Elements = new List<FieldInfo>();
            ListTypeInfo listTypeInfo = listFieldInfo.BaseInfo as ListTypeInfo;
            BaseTypeInfo elemTypeInfo = TypeInfo.GetTypeInfo(listTypeInfo.ItemType);
            string check = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
            for (int i = 0; !check.StartsWith(Values.DataSetEndFlag); i++)
            {
                switch (elemTypeInfo.TypeType)
                {
                    case TypeType.List:
                    case TypeType.Dict:
                        Util.LogErrorFormat("数据表中{0}字段定义非法,List不允许直接嵌套集合,{1}",
                            listFieldInfo.Name, GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1));
                        break;
                    case TypeType.Class:
                        DataClassInfo classFieldInfo = new DataClassInfo();
                        classFieldInfo.Set(elemTypeInfo.Name, elemTypeInfo.GetClassName(), check, listFieldInfo.Group);
                        startColumn = AnalyzeClassField(dt, classFieldInfo, startColumn - 1);
                        listFieldInfo.Elements.Add(classFieldInfo);
                        break;
                    case TypeType.Base:
                    case TypeType.Enum:
                    case TypeType.None:
                    default:
                        DataBaseInfo baseFieldInfo = new DataBaseInfo();
                        baseFieldInfo.Set(i.ToString(), elemTypeInfo.GetClassName(), check, listFieldInfo.Group);
                        startColumn = AnalyzeBaseField(dt, baseFieldInfo, startColumn);
                        listFieldInfo.Elements.Add(baseFieldInfo);
                        break;
                }

                startColumn++;
                check = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
            }
            return startColumn;
        }
        private int AnalyzeDictField(DataTable dt, DataDictInfo dictFieldInfo, int index)
        {
            int startColumn = index + 1;
            dictFieldInfo.Pairs = new List<KeyValuePair<object, FieldInfo>>();
            DictTypeInfo dictTypeInfo = dictFieldInfo.BaseInfo as DictTypeInfo;
            BaseTypeInfo keyTypeInfo = TypeInfo.GetTypeInfo(dictTypeInfo.KeyType);
            BaseTypeInfo valueTypeInfo = TypeInfo.GetTypeInfo(dictTypeInfo.ValueType);
            string check = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
            for (int i = 0; !check.StartsWith(Values.DataSetEndFlag); i++)
            {
                DataBaseInfo keyInfo = new DataBaseInfo();
                keyInfo.Set(Values.KEY, keyTypeInfo.GetClassName(), check, dictTypeInfo.Group);
                startColumn = AnalyzeBaseField(dt, keyInfo, startColumn);

                startColumn = startColumn + 1;
                switch (valueTypeInfo.TypeType)
                {
                    case TypeType.List:
                    case TypeType.Dict:
                        Util.LogErrorFormat("数据表中{0}字段定义非法,Dict value不允许直接嵌套集合,{1}",
                            dictFieldInfo.Name, GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1));
                        break;
                    case TypeType.Class:
                        {
                            DataClassInfo valueInfo = new DataClassInfo();
                            valueInfo.Set(Values.VALUE, valueTypeInfo.GetClassName(), check, dictTypeInfo.Group);
                            startColumn = AnalyzeClassField(dt, valueInfo, startColumn - 1);
                            dictFieldInfo.Pairs.Add(new KeyValuePair<object, FieldInfo>(keyInfo, valueInfo));
                            break;
                        }
                    case TypeType.Base:
                    case TypeType.Enum:
                    case TypeType.None:
                    default:
                        {
                            DataBaseInfo valueInfo = new DataBaseInfo();
                            valueInfo.Set(Values.VALUE, valueTypeInfo.GetClassName(), check, dictTypeInfo.Group);
                            startColumn = AnalyzeBaseField(dt, valueInfo, startColumn);
                            dictFieldInfo.Pairs.Add(new KeyValuePair<object, FieldInfo>(keyInfo, valueInfo));
                            break;
                        }
                }
                startColumn = startColumn + 1;
                check = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
            }

            return startColumn;
        }
    }
   
}
