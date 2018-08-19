using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace ConfigGen.LocalInfo
{
    //注:集合中##或者null均表示数据结束,集合数据从左至右填充,遇空则结束.
    //##结束符的功能预留,可扩展

    //---数据定义表,Excel形式中数据类无继承形式[未定义继承解析]
    public class TableDataInfo : TableInfo
    {
        public DataTable TableDataSet { get; private set; }

        public TableDataInfo(string absPath, DataTable data, ClassTypeInfo classType)
            : base(absPath, classType)
        {
            TableDataSet = data;
        }

        public override void Analyze()
        {
            DataTable dt = TableDataSet;
            Datas = new List<DataClass>();
            var fieldDict = ClassTypeInfo.FieldDict;
            for (int row = Values.DataSheetDataStartIndex; row < dt.Rows.Count; row++)
            {
                DataClass dataClass = new DataClass();
                string key = dt.Rows[row][0].ToString();
                if (string.IsNullOrWhiteSpace(key)) continue;

                int column = 0;
                for (int i = 0; i < ClassTypeInfo.Fields.Count; i++, column++)
                {
                    string fieldName = dt.Rows[Values.DataSheetFieldIndex][column].ToString();
                    if (!fieldDict.ContainsKey(fieldName))
                        Util.LogWarningFormat("{0}.{1}为定义", ClassTypeInfo.GetFullName(), fieldName, GetErrorSite(column + 1, Values.DataSheetFieldIndex + 1));
                    FieldInfo fieldInfo = fieldDict[fieldName];
                    Data dataField = AnalyzeField(dt, fieldInfo, row, ref column);
                    dataClass.Fields.Add(fieldName, dataField);
                }
                Datas.Add(dataClass);
            }

        }
        private string GetErrorSite(int c, int r)
        {
            return Util.GetErrorSite(AbsPath, c, r);
        }

        private Data AnalyzeField(DataTable dt, FieldInfo info, int row, ref int column)
        {
            Data dataField = null;
            BaseTypeInfo baseType = info.BaseInfo;
            switch (baseType.EType)
            {
                case TypeType.Base:
                    dataField = AnalyzeBase(dt, info, row, ref column);
                    break;
                case TypeType.Enum:
                    dataField = AnalyzeEnum(dt, info, row, ref column);
                    break;
                case TypeType.Class:
                    dataField = AnalyzeClass(dt, info, row, ref column);
                    break;
                case TypeType.List:
                    dataField = AnalyzeList(dt, info, row, ref column);
                    break;
                case TypeType.Dict:
                    dataField = AnalyzeDict(dt, info, row, ref column);
                    break;
                case TypeType.None:
                default:
                    break;
            }
            return dataField;
        }
        private Data AnalyzeBase(DataTable dt, FieldInfo info, int row, ref int column)
        {
            DataBase dataBase = new DataBase();
            //dataBase.Set(info.Name, info.Type, info.Check, info.Group);
            dataBase.Data = dt.Rows[row][column];
            string key = dataBase.Data as string;
            if (key != null && key.Equals(Values.DataSetEndFlag))
                return null;
            return dataBase;
        }
        private Data AnalyzeEnum(DataTable dt, FieldInfo info, int row, ref int column)
        {
            DataBase dataBase = new DataBase();
            EnumTypeInfo enumType = info.BaseInfo as EnumTypeInfo;
            dataBase.Data = dt.Rows[row][column];
            string key = dataBase.Data as string;
            if (key != null && !key.Equals(Values.DataSetEndFlag))
            {
                if (enumType.AliasDict.ContainsKey(key))
                    dataBase.Data = enumType.AliasDict[key].Value;
                else if (enumType.EnumDict.ContainsKey(key))
                    dataBase.Data = enumType.EnumDict[key].Value;
                else
                    Util.LogErrorFormat("{0}.{1}不存在,{2}", enumType.GetFullName(), key
                        , GetErrorSite(column + 1, row + 1));
            }
            else
                dataBase = null;

            return dataBase;
        }
        private Data AnalyzeClass(DataTable dt, FieldInfo info, int row, ref int column)
        {
            column++;
            ClassTypeInfo classType = info.BaseInfo as ClassTypeInfo;
            DataClass dataClass = new DataClass();
            //dataClass.Set(info.Name, info.Type, info.Check, info.Group);
            var fieldDict = classType.FieldDict;
            for (int i = 0; i < classType.Fields.Count; i++, column++)
            {
                string fieldName = dt.Rows[Values.DataSheetFieldIndex][column].ToString();
                FieldInfo fieldInfo = fieldDict[fieldName];
                Data dataField = AnalyzeField(dt, fieldInfo, row, ref column);
                if (dataField == null && (info.Name.Equals(Values.ELEMENT) || info.Name.Equals(Values.VALUE)))
                {
                    column += classType.Fields.Count - i;
                    dataClass = null;
                    break;
                }
                else if (dataField == null)
                    Util.LogErrorFormat("{0}.{1}数据为空,{2}", info.Type, info.Name, GetErrorSite(column + 1, row + 1));

                dataClass.Fields.Add(fieldName, dataField);
            }
            column--;
            return dataClass;
        }
        private Data AnalyzeList(DataTable dt, FieldInfo info, int row, ref int column)
        {
            ListTypeInfo listType = info.BaseInfo as ListTypeInfo;
            FieldInfo element = listType.ItemInfo;

            DataList dataList = new DataList();
            string flag = dt.Rows[Values.DataSheetFieldIndex][column].ToString();
            bool isEnd = false;
            column++;
            for (int i = 0; !flag.Equals(Values.DataSetEndFlag); i++)
            {
                if (isEnd == false)
                {
                    if (element.BaseInfo.EType == TypeType.Class)
                        column--;
                    Data dataField = AnalyzeField(dt, element, row, ref column);
                    if (dataField != null)
                        dataList.Elements.Add(dataField);
                    else
                        isEnd = true;
                }

                column++;
                flag = dt.Rows[Values.DataSheetFieldIndex][column].ToString();
            }
            return dataList;
        }
        private Data AnalyzeDict(DataTable dt, FieldInfo info, int row, ref int column)
        {
            DictTypeInfo dictType = info.BaseInfo as DictTypeInfo;
            FieldInfo keyInfo = dictType.KeyInfo;
            FieldInfo valueInfo = dictType.ValueInfo;

            DataDict dataDict = new DataDict();
            HashSet<object> hash = new HashSet<object>();
            string flag = dt.Rows[Values.DataSheetFieldIndex][column].ToString();
            bool isEnd = false;
            column++;
            for (int i = 0; !flag.Equals(Values.DataSetEndFlag); i++)
            {
                if (isEnd == false)
                {
                    DataBase dataKey = AnalyzeField(dt, keyInfo, row, ref column) as DataBase;
                    column++;

                    if (valueInfo.BaseInfo.EType == TypeType.Class)
                        column--;

                    Data dataValue = AnalyzeField(dt, valueInfo, row, ref column);
                    if (dataKey == null || dataValue == null)
                        isEnd = true;
                    else if (hash.Contains(dataKey.Data))
                        Util.LogErrorFormat("{0} {1} Key:{2}重复,{3}", info.Type, info.Name,
                            dataKey.Data.ToString(), GetErrorSite(column, row + 1));
                    else
                    {
                        hash.Add(dataKey.Data);
                        dataDict.Pairs.Add(new KeyValuePair<DataBase, Data>(dataKey, dataValue));
                    }
                }

                column++;
                flag = dt.Rows[Values.DataSheetFieldIndex][column].ToString();
            }
            return dataDict;
        }



        //private object GetValueOrEnum(DataTable dt, int r, int c, EnumTypeInfo enumType)
        //{
        //    object value = dt.Rows[r][c];
        //    if (enumType != null)
        //    {
        //        string key = value as string;
        //        if (enumType.EnumDict.ContainsKey(key))
        //            value = enumType.EnumDict[key];
        //        else
        //            Util.LogErrorFormat("{0}.{1}不存在,{2}", enumType.GetClassName(), key
        //                , GetErrorSite(c + 1, r + 1));
        //    }
        //    return value;
        //}
        //private int AnalyzeClassField(DataTable dt, DataClassInfo classFieldInfo, int index)
        //{
        //    int startColumn = index + 1;
        //    ClassTypeInfo classInfo = classFieldInfo.BaseInfo as ClassTypeInfo;
        //    classInfo.UpdateToDict();
        //    var classFieldDict = classInfo.GetFieldInfoDict();
        //    if (classInfo.HasSubClass)
        //        throw new Exception(string.Format("Excel中无法解析继承类型的数据,字段名:{0},{1}",
        //           classFieldInfo.Name, GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1)));
        //    for (int i = 0; i < classFieldDict.Count; i++, startColumn++)
        //    {
        //        string fieldName = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
        //        if (!classFieldDict.ContainsKey(fieldName))
        //        {
        //            throw new Exception(string.Format("在{0}类型的数据表中,{1}字段名与实际定义不一致,{2}",
        //                    ClassTypeInfo.Name, fieldName, GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1)));
        //        }
        //        if (string.IsNullOrWhiteSpace(fieldName))
        //        {
        //            throw new Exception(string.Format("数据表对应的类型字段名必须填写完整,{1}",
        //                    GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1)));
        //        }

        //        //解析数据类字段
        //        FieldInfo fieldInfo = classFieldDict[fieldName];
        //        string check = dt.Rows[Values.DataSheetCheckIndex][startColumn].ToString();
        //        check = string.IsNullOrWhiteSpace(check) ? fieldInfo.Check : check;

        //        BaseTypeInfo baseType = fieldInfo.BaseInfo;
        //        switch (baseType.TypeType)
        //        {
        //            case TypeType.Base:
        //            case TypeType.Enum:
        //                DataBaseInfo baseFieldInfo = new DataBaseInfo();
        //                baseFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, check, fieldInfo.Group);
        //                classFieldInfo.Fields.Add(baseFieldInfo.Name, baseFieldInfo);
        //                startColumn = AnalyzeBaseField(dt, baseFieldInfo, startColumn);
        //                break;
        //            case TypeType.Class://类的检查规则需要自定义
        //                DataClassInfo subClassFieldInfo = new DataClassInfo();
        //                subClassFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, check, fieldInfo.Group);
        //                classFieldInfo.Fields.Add(subClassFieldInfo.Name, subClassFieldInfo);
        //                startColumn = AnalyzeClassField(dt, subClassFieldInfo, startColumn);
        //                break;
        //            case TypeType.List://集合中元素的规则可手动逐个重写
        //                DataListInfo listFieldInfo = new DataListInfo();
        //                listFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, check, fieldInfo.Group);
        //                classFieldInfo.Fields.Add(listFieldInfo.Name, listFieldInfo);
        //                startColumn = AnalyzeListField(dt, listFieldInfo, startColumn);
        //                break;
        //            case TypeType.Dict://集合中元素的规则可手动逐个重写
        //                DataDictInfo dictFieldInfo = new DataDictInfo();
        //                dictFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, check, fieldInfo.Group);
        //                classFieldInfo.Fields.Add(dictFieldInfo.Name, dictFieldInfo);
        //                startColumn = AnalyzeDictField(dt, dictFieldInfo, startColumn);
        //                break;
        //            case TypeType.None:
        //            default:
        //                break;
        //        }
        //    }

        //    return startColumn - 1;
        //}
        //private int AnalyzeBaseField(DataTable dt, DataBaseInfo fieldInfo, int index)
        //{
        //    int column = index;
        //    EnumTypeInfo enumType = fieldInfo.BaseInfo as EnumTypeInfo;
        //    for (int i = Values.DataSheetDataStartIndex; i < dt.Rows.Count; i++)
        //    {
        //        object value = GetValueOrEnum(dt, i, column, enumType);
        //        fieldInfo.Data.Add(value);
        //    }
        //    return column;
        //}
        //private int AnalyzeListField(DataTable dt, DataListInfo listFieldInfo, int index)
        //{
        //    int startColumn = index + 1;
        //    ListTypeInfo listTypeInfo = listFieldInfo.BaseInfo as ListTypeInfo;
        //    BaseTypeInfo elemTypeInfo = TypeInfo.GetTypeInfo(listTypeInfo.ItemType);
        //    EnumTypeInfo enumType = elemTypeInfo as EnumTypeInfo;
        //    string check = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
        //    for (int j = Values.DataSheetDataStartIndex; j < dt.Rows.Count - Values.DataSheetDataStartIndex; j++)//行
        //    {
        //        DataElementInfo dataElement = new DataElementInfo();
        //        for (int i = 0; !check.StartsWith(Values.DataSetEndFlag); i++)//列
        //        {
        //            int column = startColumn + i;
        //            switch (elemTypeInfo.TypeType)
        //            {
        //                case TypeType.List:
        //                case TypeType.Dict:
        //                    Util.LogErrorFormat("数据表中{0}字段定义非法,List不允许直接嵌套集合,{1}",
        //                        listFieldInfo.Name, GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1));
        //                    break;
        //                case TypeType.Class:
        //                    DataClassInfo classFieldInfo = new DataClassInfo();
        //                    classFieldInfo.Set(i.ToString(), elemTypeInfo.GetClassName(), check, listFieldInfo.Group);

        //                    dataElement.Elements.Add(classFieldInfo);
        //                    break;
        //                case TypeType.Base:
        //                case TypeType.Enum:
        //                case TypeType.None:
        //                default:
        //                    object value = GetValueOrEnum(dt, j, column, enumType);
        //                    string flag = value as string;
        //                    if (flag == null || flag.Equals(Values.DataSetEndFlag))
        //                        break;

        //                    DataBaseInfo baseFieldInfo = new DataBaseInfo();
        //                    baseFieldInfo.Set(i.ToString(), elemTypeInfo.GetClassName(), check, listFieldInfo.Group);
        //                    baseFieldInfo.Data.Add(value);
        //                    dataElement.Elements.Add(baseFieldInfo);
        //                    break;
        //            }
        //        }
        //        listFieldInfo.DataSet.Add(dataElement);
        //        if (dataElement.Elements.Count > listFieldInfo.MaxIndex)
        //            listFieldInfo.MaxIndex = dataElement.Elements.Count;
        //    }
        //    startColumn = listFieldInfo.MaxIndex + 1;
        //    check = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
        //    return startColumn;
        //}
        //private int AnalyzeDictField(DataTable dt, DataDictInfo dictFieldInfo, int index)
        //{
        //    int startColumn = index + 1;
        //    DictTypeInfo dictTypeInfo = dictFieldInfo.BaseInfo as DictTypeInfo;
        //    BaseTypeInfo keyTypeInfo = TypeInfo.GetTypeInfo(dictTypeInfo.KeyType);
        //    BaseTypeInfo valueTypeInfo = TypeInfo.GetTypeInfo(dictTypeInfo.ValueType);
        //    string checkField = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
        //    for (int i = 0; !checkField.StartsWith(Values.DataSetEndFlag); i++)
        //    {
        //        DataBaseInfo keyInfo = new DataBaseInfo();
        //        string check = dt.Rows[Values.DataSheetCheckIndex][startColumn].ToString();
        //        keyInfo.Set(Values.KEY, keyTypeInfo.GetClassName(), check, dictTypeInfo.Group);
        //        startColumn = AnalyzeBaseField(dt, keyInfo, startColumn);

        //        startColumn = startColumn + 1;
        //        check = dt.Rows[Values.DataSheetCheckIndex][startColumn].ToString();
        //        switch (valueTypeInfo.TypeType)
        //        {
        //            case TypeType.List:
        //            case TypeType.Dict:
        //                Util.LogErrorFormat("数据表中{0}字段定义非法,Dict value不允许直接嵌套集合,{1}",
        //                    dictFieldInfo.Name, GetErrorSite(startColumn + 1, Values.DataSheetFieldIndex + 1));
        //                break;
        //            case TypeType.Class:
        //                {
        //                    DataClassInfo valueInfo = new DataClassInfo();
        //                    valueInfo.Set(Values.VALUE, valueTypeInfo.GetClassName(), check, dictTypeInfo.Group);
        //                    startColumn = AnalyzeClassField(dt, valueInfo, startColumn - 1);
        //                    dictFieldInfo.DataSet.Add(new KeyValuePair<DataBaseInfo, FieldInfo>(keyInfo, valueInfo));
        //                    break;
        //                }
        //            case TypeType.Base:
        //            case TypeType.Enum:
        //            case TypeType.None:
        //            default:
        //                {
        //                    DataBaseInfo valueInfo = new DataBaseInfo();
        //                    valueInfo.Set(Values.VALUE, valueTypeInfo.GetClassName(), check, dictTypeInfo.Group);
        //                    startColumn = AnalyzeBaseField(dt, valueInfo, startColumn);
        //                    dictFieldInfo.DataSet.Add(new KeyValuePair<DataBaseInfo, FieldInfo>(keyInfo, valueInfo));
        //                    break;
        //                }
        //        }
        //        startColumn = startColumn + 1;
        //        checkField = dt.Rows[Values.DataSheetFieldIndex][startColumn].ToString();
        //    }

        //    return startColumn;
        //}
    }

}
