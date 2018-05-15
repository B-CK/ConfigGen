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
            foreach (var item in LocalInfoManager.Instance.DataInfoDict)
            {
                TableDataInfo table = item.Value;
                List<TableFieldInfo> data = table.DataFields;
                for (int row = 0; row < table.DataLength; row++)//行
                {
                    for (int column = 0; column < data.Count; column++)//列
                    {
                        string v = AnalyzeField(data[column], row);
                        if (column + 1 == data.Count)
                            builder.AppendLine(v);
                        else
                            builder.AppendFormat("{0}{1}", v, Values.CsvSplitFlag);
                    }
                }

                string fileName = Path.GetFileNameWithoutExtension(table.RelPath);
                string filePath = string.Format("{0}\\{1}{2}", Values.ExportCsv, fileName, Values.CsvFileExt);
                Util.SaveFile(filePath, builder.ToString());
                builder.Clear();
            }
        }
        //isNesting - 是否为嵌套Class类型
        private static string AnalyzeField(TableFieldInfo fieldInfo, int row, bool isNesting = false)
        {
            string v = "";
            BaseTypeInfo typeInfo = fieldInfo.BaseInfo;
            switch (typeInfo.TypeType)
            {
                case TypeType.Base:
                case TypeType.Enum:
                    v = fieldInfo.Data[row].ToString().Trim();
                    if (fieldInfo.Type.Equals("bool"))
                        v = AnalyzeBool(v);
                    break;
                case TypeType.Class:
                    v = AnalyzeClass(fieldInfo, row, isNesting);
                    break;
                case TypeType.List:
                    v = AnalyzeList(fieldInfo, row);
                    break;
                case TypeType.Dict:
                    v = AnalyzeDict(fieldInfo, row);
                    break;
                case TypeType.None:
                default:
                    break;
            }
            return v;
        }
        private static string AnalyzeClass(TableFieldInfo fieldInfo, int row, bool isNesting = false)
        {
            StringBuilder builder = new StringBuilder();
            var fields = fieldInfo.ChildFields;
            for (int i = 0; i < fields.Count; i++)
            {
                string value = AnalyzeField(fields[i], row);
                if (isNesting && value.Equals(Values.DataSetEndFlag))
                    return Values.DataSetEndFlag;
                if (i + 1 == fields.Count)
                    builder.Append(value);
                else
                    builder.AppendFormat("{0}{1}", value, Values.CsvSplitFlag);
            }
            return builder.ToString();
        }
        private static string AnalyzeList(TableFieldInfo fieldInfo, int row)
        {
            StringBuilder builder = new StringBuilder();
            var fields = fieldInfo.ChildFields;
            int count = 0;
            for (int i = 0; i < fields.Count; i++)
            {
                string value = AnalyzeField(fields[i], row, true);
                if (!value.Equals(Values.DataSetEndFlag))
                {
                    if (i + 1 == fields.Count)
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
            builder.Insert(0,count == 0 ? count.ToString() : count + Values.CsvSplitFlag);
            return builder.ToString();
        }
        private static string AnalyzeDict(TableFieldInfo fieldInfo, int row)
        {
            StringBuilder builder = new StringBuilder();
            var fields = fieldInfo.ChildFields;
            int count = 0;
            for (int i = 0; i < fields.Count; i++)
            {
                string pair = AnalyzePair(fields[i], row);
                if (!pair.Equals(Values.DataSetEndFlag))
                {
                    if (i + 1 == fields.Count)
                        builder.Append(pair);
                    else
                        builder.AppendFormat("{0}{1}", pair, Values.CsvSplitFlag);
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
        private static string AnalyzePair(TableFieldInfo fieldInfo, int row)
        {
            TableFieldInfo pair = fieldInfo;
            TableFieldInfo key = pair.ChildFields[0];
            TableFieldInfo value = pair.ChildFields[1];
            string k = AnalyzeField(key, row);
            string v = AnalyzeField(value, row, true);
            if (k.Equals(Values.DataSetEndFlag)
                || v.Equals(Values.DataSetEndFlag))
                return Values.DataSetEndFlag;
            return string.Format("{0}{1}{2}", k, Values.CsvSplitFlag, v);
        }
        private static string AnalyzeBool(string str)
        {
            return str.ToLower().Equals("true") ? "1" : "0";
        }
    }
}
