using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using ConfigGen.LocalInfo;

namespace ConfigGen.Export
{
    class ExportCsv
    {
        /// <summary>
        /// Class,List,Dict多列型数据在解析时,固定读取数据长度
        /// </summary>
        public static void Export()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in Local.Instance.DataInfoDict)
            {
                DataClassInfo data = item.Value.Datas;
                for (int row = 0; row < item.Value.DataLength; row++)//行
                {
                    List<FieldInfo> fields = (data.BaseInfo as ClassTypeInfo).Fields;
                    for (int column = 0; column < fields.Count; column++)//列
                    {
                        string fieldName = fields[column].Name;
                        string v = AnalyzeField(data.Fields[fieldName], row);
                        if (column + 1 == fields.Count)
                            builder.AppendLine(v);
                        else
                            builder.AppendFormat("{0}{1}", v, Values.CsvSplitFlag);
                    }
                }

                string fileName = data.BaseInfo.GetClassName().Replace(".", "/");
                string filePath = string.Format("{0}\\{1}{2}", Values.ExportCsv, fileName, Values.CsvFileExt);
                Util.SaveFile(filePath, builder.ToString());
                builder.Clear();
            }
        }
        //isNesting - 是否为嵌套Class类型
        private static string AnalyzeField(FieldInfo fieldInfo, int row, bool isNesting = false)
        {
            string v = "";
            BaseTypeInfo typeInfo = fieldInfo.BaseInfo;
            switch (typeInfo.TypeType)
            {
                case TypeType.Base:
                case TypeType.Enum:
                    DataBaseInfo dataBase = fieldInfo as DataBaseInfo;
                    object temp = dataBase.Data[row];
                    v = temp.ToString().Trim();
                    if (fieldInfo.Type.Equals("bool"))
                        v = v.ToLower().Equals("true") ? "1" : "0";
                    break;
                case TypeType.Class:
                    DataClassInfo dataClass = fieldInfo as DataClassInfo;
                    v = AnalyzeClass(dataClass, row, isNesting);
                    break;
                case TypeType.List:
                    DataListInfo dataList = fieldInfo as DataListInfo;
                    v = AnalyzeList(dataList, row);
                    break;
                case TypeType.Dict:
                    DataDictInfo dataDict = fieldInfo as DataDictInfo;
                    v = AnalyzeDict(dataDict, row);
                    break;
                case TypeType.None:
                default:
                    break;
            }
            return v;
        }
        private static string AnalyzeClass(DataClassInfo dataClass, int row, bool isNesting = false)
        {
            StringBuilder builder = new StringBuilder();
            ClassTypeInfo baseClassType = dataClass.BaseInfo as ClassTypeInfo;
            if (baseClassType.HasSubClass)
            {
                string polyType = dataClass.Types[row];
                builder.Append(polyType);
                for (int j = 0; j < baseClassType.Fields.Count; j++)
                {
                    FieldInfo fieldInfo = baseClassType.Fields[j];
                    FieldInfo dataBase = dataClass.Fields[fieldInfo.Name];
                    string value = AnalyzeField(dataBase, row);
                    if (isNesting && value.Equals(Values.DataSetEndFlag))
                        return Values.DataSetEndFlag;

                    builder.AppendFormat("{0}{1}", Values.CsvSplitFlag, value);
                }
                if (baseClassType.SubClasses.Contains(polyType))
                {
                    ClassTypeInfo polyClassType = TypeInfo.GetTypeInfo(polyType) as ClassTypeInfo;
                    for (int j = 0; j < polyClassType.Fields.Count; j++)
                    {
                        FieldInfo fieldInfo = polyClassType.Fields[j];
                        FieldInfo dataBase = dataClass.Fields[fieldInfo.Name];
                        string value = AnalyzeField(dataBase, row);
                        if (isNesting && value.Equals(Values.DataSetEndFlag))
                            return Values.DataSetEndFlag;

                        builder.AppendFormat("{0}{1}", Values.CsvSplitFlag, value);
                    }
                }
            }
            else
            {
                for (int j = 0; j < baseClassType.Fields.Count; j++)
                {
                    FieldInfo fieldInfo = baseClassType.Fields[j];
                    FieldInfo dataBase = dataClass.Fields[fieldInfo.Name];
                    string value = AnalyzeField(dataBase, row);
                    if (isNesting && value.Equals(Values.DataSetEndFlag))
                        return Values.DataSetEndFlag;
                    if (j + 1 == baseClassType.Fields.Count)
                        builder.AppendFormat(value);
                    else
                        builder.AppendFormat("{0}{1}", value, Values.CsvSplitFlag);
                }
            }
            return builder.ToString();
        }
        private static string AnalyzeList(DataListInfo dataList, int row)
        {
            StringBuilder builder = new StringBuilder();
            int count = 0;
            var elements = dataList.DataSet;
            for (int i = 0; i < elements.Count; i++)
            {
                string value = AnalyzeField(elements[i], row, true);
                if (!value.Equals(Values.DataSetEndFlag))
                {
                    if (i + 1 == elements.Count)
                        builder.Append(value);
                    else
                        builder.AppendFormat("{0}{1}", value, Values.CsvSplitFlag);
                }
                else
                {
                    if (builder.Length > 0)
                        builder.Remove(builder.Length - 1, 1);
                    break;
                }

                count++;
            }
            builder.Insert(0, count == 0 ? count.ToString() : count + Values.CsvSplitFlag);
            return builder.ToString();
        }
        private static string AnalyzeDict(DataDictInfo dataDict, int row)
        {
            StringBuilder builder = new StringBuilder();
            int count = 0;
            var pairs = dataDict.DataSet;
            for (int i = 0; i < pairs.Count; i++)
            {
                string key = AnalyzeField(pairs[i].Key, row);
                string value = AnalyzeField(pairs[i].Value, row);
                if (key.Equals(Values.DataSetEndFlag)
                    || value.Equals(Values.DataSetEndFlag))
                    break;

                builder.AppendFormat("{0}{1}{2}{3}", Values.CsvSplitFlag, key, Values.CsvSplitFlag, value);
                count++;
            }
            builder.Insert(0, count.ToString());
            return builder.ToString();
        }
    }
}
