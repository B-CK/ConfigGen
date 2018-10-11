using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using ConfigGen.LocalInfo;

namespace ConfigGen.Export
{
    class ExportCsv
    {
        public static void Export()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in TableInfo.DataInfoDict)
            {
                ClassTypeInfo classType = item.Value.ClassTypeInfo;
                List<DataClass> datas = item.Value.Datas;
                for (int row = 0; row < datas.Count; row++)//行
                {
                    List<FieldInfo> fields = classType.Fields;
                    for (int column = 0; column < fields.Count; column++)//列
                    {
                        string fieldName = fields[column].Name;
                        string v = AnalyzeField(fields[column], datas[row].Fields[fieldName]);
                        if (column + 1 == fields.Count)
                            builder.AppendLine(v);
                        else
                            builder.AppendFormat("{0}{1}", v, Values.CsvSplitFlag);
                    }
                }


                string fileName = item.Value.ClassTypeInfo.GetFullName().Replace(".", "/");
                string filePath = string.Format("{0}\\{1}{2}", Values.ExportCsv, fileName, Values.CsvFileExt);
                Util.SaveFile(filePath, builder.ToString());
                builder.Clear();
            }
        }

        private static string AnalyzeField(FieldInfo info, Data data, bool isNesting = false)
        {
            string v = "";
            BaseTypeInfo typeInfo = info.BaseInfo;
            switch (typeInfo.EType)
            {
                case TypeType.Base:
                    DataBase dataBase = data as DataBase;
                    switch (info.Type)
                    {
                        case TypeInfo.INT:
                            v = ((int)dataBase).ToString();
                            break;
                        case TypeInfo.LONG:
                            v = ((long)dataBase).ToString();
                            break;
                        case TypeInfo.FLOAT:
                            v = ((float)dataBase).ToString();
                            break;
                        case TypeInfo.BOOL:
                            v = dataBase ? "1" : "0";
                            break;
                        case TypeInfo.STRING:
                            v = dataBase;
                            break;
                        default:
                            Util.LogErrorFormat("导出Csv基础类型数据异常" + info.Type);
                            break;
                    }
                    break;
                case TypeType.Enum:
                    DataBase enumData = data as DataBase;
                    v = enumData.Data as string;
                    break;
                case TypeType.Class:
                    v = AnalyzeClass(info, data, isNesting);
                    break;
                case TypeType.List:
                    v = AnalyzeList(info, data);
                    break;
                case TypeType.Dict:
                    v = AnalyzeDict(info, data);
                    break;
                case TypeType.None:
                default:
                    break;
            }
            return v;
        }
        private static string AnalyzeClass(FieldInfo info, Data data, bool isNesting = false)
        {
            StringBuilder builder = new StringBuilder();
            DataClass dataClass = data as DataClass;
            ClassTypeInfo classType = info.BaseInfo as ClassTypeInfo;
            if (classType.IsPolyClass)
            {
                string polyType = dataClass.Type;
                builder.AppendFormat("{0}.{1}{2}", Values.ConfigRootNode, polyType, Values.CsvSplitFlag);
                //--父类字段
                for (int j = 0; j < classType.Fields.Count; j++)
                {
                    FieldInfo fieldInfo = classType.Fields[j];
                    Data dataBase = dataClass.Fields[fieldInfo.Name];
                    string value = AnalyzeField(fieldInfo, dataBase);
                    if (isNesting && value.Equals(Values.DataSetEndFlag))
                        return Values.DataSetEndFlag;

                    if (j == 0)
                        builder.AppendFormat("{0}", value);
                    else
                        builder.AppendFormat("{0}{1}", Values.CsvSplitFlag, value);
                }
                var polyClassType = classType.SubClassDict[polyType];
                if (polyClassType != null)
                {   //--子类字段
                    for (int j = 0; j < polyClassType.Fields.Count; j++)
                    {
                        FieldInfo fieldInfo = polyClassType.Fields[j];
                        Data dataBase = dataClass.Fields[fieldInfo.Name];
                        string value = AnalyzeField(fieldInfo, dataBase);
                        if (isNesting && value.Equals(Values.DataSetEndFlag))
                            return Values.DataSetEndFlag;

                        builder.AppendFormat("{0}{1}", Values.CsvSplitFlag, value);
                    }
                }
                else
                {
                    Util.LogErrorFormat("类型{0} 不存在或者未继承类型{1}", polyType, classType.GetFullName());
                }
            }
            else
            {
                for (int j = 0; j < classType.Fields.Count; j++)
                {
                    FieldInfo fieldInfo = classType.Fields[j];
                    Data dataBase = dataClass.Fields[fieldInfo.Name];
                    string value = AnalyzeField(fieldInfo, dataBase);
                    if (isNesting && value.Equals(Values.DataSetEndFlag))
                        return Values.DataSetEndFlag;
                    if (j + 1 == classType.Fields.Count)
                        builder.AppendFormat(value);
                    else
                        builder.AppendFormat("{0}{1}", value, Values.CsvSplitFlag);
                }
            }
            return builder.ToString();
        }
        private static string AnalyzeList(FieldInfo info, Data data)
        {
            StringBuilder builder = new StringBuilder();
            ListTypeInfo listInfo = info.BaseInfo as ListTypeInfo;
            FieldInfo elementInfo = listInfo.ItemInfo;
            DataList dataList = data as DataList;
            int count = 0;
            var elements = dataList.Elements;
            for (int i = 0; i < elements.Count; i++)
            {
                string value = AnalyzeField(elementInfo, elements[i], true);
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
        private static string AnalyzeDict(FieldInfo info, Data data)
        {
            StringBuilder builder = new StringBuilder();
            DictTypeInfo dictInfo = info.BaseInfo as DictTypeInfo;
            FieldInfo keyInfo = dictInfo.KeyInfo;
            FieldInfo valueInfo = dictInfo.ValueInfo;
            DataDict dataDict = data as DataDict;
            int count = 0;
            var pairs = dataDict.Pairs;
            for (int i = 0; i < pairs.Count; i++)
            {
                string key = AnalyzeField(keyInfo, pairs[i].Key);
                string value = AnalyzeField(valueInfo, pairs[i].Value);
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
