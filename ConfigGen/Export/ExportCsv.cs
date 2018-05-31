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
                List<DataClass> datas = item.Value.Datas;
                for (int row = 0; row < datas.Count; row++)//行
                {
                    List<FieldInfo> fields = (datas[row].BaseInfo as ClassTypeInfo).Fields;
                    for (int column = 0; column < fields.Count; column++)//列
                    {
                        string fieldName = fields[column].Name;
                        string v = AnalyzeField(datas[row].Fields[fieldName]);
                        if (column + 1 == fields.Count)
                            builder.AppendLine(v);
                        else
                            builder.AppendFormat("{0}{1}", v, Values.CsvSplitFlag);
                    }
                }


                string fileName = item.Value.ClassTypeInfo.GetClassName().Replace(".", "/");
                string filePath = string.Format("{0}\\{1}{2}", Values.ExportCsv, fileName, Values.CsvFileExt);
                Util.SaveFile(filePath, builder.ToString());
                builder.Clear();
            }
        }
        //isNesting - 是否为嵌套Class类型
        private static string AnalyzeField(FieldInfo info, bool isNesting = false)
        {
            string v = "";
            BaseTypeInfo typeInfo = info.BaseInfo;
            switch (typeInfo.TypeType)
            {
                case TypeType.Base:
                case TypeType.Enum:
                    DataBase dataBase = info as DataBase;
                    object temp = dataBase.Data;
                    v = temp.ToString().Trim();
                    if (info.Type.Equals("bool"))
                        v = v.ToLower().Equals("true") ? "1" : "0";
                    break;
                case TypeType.Class:
                    v = AnalyzeClass(info, isNesting);
                    break;
                case TypeType.List:
                    v = AnalyzeList(info);
                    break;
                case TypeType.Dict:
                    v = AnalyzeDict(info);
                    break;
                case TypeType.None:
                default:
                    break;
            }
            return v;
        }
        private static string AnalyzeClass(FieldInfo info, bool isNesting = false)
        {
            StringBuilder builder = new StringBuilder();
            DataClass dataClass = info as DataClass;
            ClassTypeInfo baseClassType = dataClass.BaseInfo as ClassTypeInfo;
            if (baseClassType.HasSubClass)
            {
                string polyType = dataClass.Type;
                builder.Append(polyType);
                for (int j = 0; j < baseClassType.Fields.Count; j++)
                {
                    FieldInfo fieldInfo = baseClassType.Fields[j];
                    FieldInfo dataBase = dataClass.Fields[fieldInfo.Name];
                    string value = AnalyzeField(dataBase);
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
                        string value = AnalyzeField(dataBase);
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
                    string value = AnalyzeField(dataBase);
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
        private static string AnalyzeList(FieldInfo info)
        {
            StringBuilder builder = new StringBuilder();
            DataList dataList = info as DataList;
            int count = 0;
            var elements = dataList.Elements;
            for (int i = 0; i < elements.Count; i++)
            {
                string value = AnalyzeField(elements[i], true);
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
        private static string AnalyzeDict(FieldInfo info)
        {
            StringBuilder builder = new StringBuilder();
            DataDict dataDict = info as DataDict;
            int count = 0;
            var pairs = dataDict.Pairs;
            for (int i = 0; i < pairs.Count; i++)
            {
                string key = AnalyzeField(pairs[i].Key);
                string value = AnalyzeField(pairs[i].Value);
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
