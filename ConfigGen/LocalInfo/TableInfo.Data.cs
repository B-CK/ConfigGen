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
            for (int row = Values.DataSheetDataStartIndex; row < dt.Rows.Count; row++)
            {
                DataClass dataClass = new DataClass();
                string key = dt.Rows[row][0].ToString();
                if (string.IsNullOrWhiteSpace(key)) continue;

                int column = 0;
                for (int i = 0; i < ClassTypeInfo.Fields.Count; i++, column++)
                {
                    FieldInfo fieldInfo = ClassTypeInfo.Fields[i];
                    Data dataField = AnalyzeField(dt, fieldInfo, row, ref column);
                    dataClass.Fields.Add(fieldInfo.Name, dataField);
                }
                Datas.Add(dataClass);
            }
        }
        private string GetErrorSite(int c, int r)
        {
            return Util.GetErrorSite(AbsPath, c, r);
        }
        /// <summary>
        /// 用于多态类,集合类型等字段个数不确定的情况下
        /// </summary>
        private void RemoveEmpty(DataTable dt, int row, ref int column)
        {
            do
            {
                string value = dt.Rows[row][column].ToString();
                if (!string.IsNullOrWhiteSpace(value)) break;
                column++;
                if (column >= Values.SheetLineDataNum)
                    throw new Exception("数据表格(Excel)每行最大数据量1024!  " + column + ">=1024");
            } while (true);
        }
        /// <summary>
        /// 用于结束List,Dict类型
        /// </summary>
        private void DoSetEnd(DataTable dt, int row, ref int column)
        {
            bool isEnd = false;
            do
            {
                if (column >= dt.Rows[row].ItemArray.Length) return;
                string flag = dt.Rows[row][column].ToString();
                isEnd = flag.Equals(Values.DataSetEndFlag);
                column++;
            } while (!isEnd);
            column--;
        }

        private Data AnalyzeField(DataTable dt, FieldInfo info, int row, ref int column)
        {
            if (column >= dt.Rows[row].ItemArray.Length) return null;
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
            dataBase.Data = dt.Rows[row][column];
            string key = dataBase;
            if (key != null)
            {
                if (key.Equals(Values.DataSetEndFlag))//--集合类型结束
                    return null;
                else if (key.Equals(Values.Null))//--占位内容空,使用类型默认值
                    dataBase.Data = null;
                else if (string.IsNullOrWhiteSpace(key))//--表格DBNull类型,无内容!非集合情况下直接报错!
                    return null;
            }

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
        //--下列函数读取前,处理所有空格子
        //--集合读取完后,自动处理剩余空格子
        private Data AnalyzeClass(DataTable dt, FieldInfo info, int row, ref int column)
        {
            RemoveEmpty(dt, row, ref column);
            string flag = dt.Rows[row][column].ToString();
            if (flag.Equals(Values.DataSetEndFlag))//--集合结束标识符
                return null;

            ClassTypeInfo classType = info.BaseInfo as ClassTypeInfo;
            DataClass dataClass = new DataClass();
            ClassTypeInfo subClassType = null;
            if (classType.IsPolyClass)
            {
                string polyClass = dt.Rows[row][column].ToString();
                BaseTypeInfo bti = TypeInfo.GetBaseTypeInfo(classType.NamespaceName, polyClass);
                if (bti != null && classType.GetSubClass(bti.GetFullName()) != null || bti == classType)
                {
                    dataClass.Type = bti.GetFullName();
                    subClassType = bti as ClassTypeInfo;
                    column++;
                }
                else
                {
                    Util.LogErrorFormat("类型{0} 不存在或者未继承类型{1},{2}", polyClass, classType.GetFullName(), GetErrorSite(column + 1, row + 1));
                }
            }
            //--解析父类
            for (int i = 0; i < classType.Fields.Count; i++, column++)
            {
                FieldInfo fieldInfo = classType.Fields[i];
                Data dataField = AnalyzeField(dt, fieldInfo, row, ref column);
                if (dataField == null)
                    Util.LogErrorFormat("{0}.{1}数据为空,{2}", info.Type, info.Name, GetErrorSite(column + 1, row + 1));

                dataClass.Fields.Add(fieldInfo.Name, dataField);
            }
            //--解析子类 
            if (subClassType != null && classType != subClassType)
            {
                for (int i = 0; i < subClassType.Fields.Count; i++, column++)
                {
                    FieldInfo fieldInfo = subClassType.Fields[i];
                    Data dataField = AnalyzeField(dt, fieldInfo, row, ref column);
                    if (dataField == null)
                        Util.LogErrorFormat("{0}.{1}数据为空,{2}", info.Type, info.Name, GetErrorSite(column + 1, row + 1));

                    dataClass.Fields.Add(fieldInfo.Name, dataField);
                }
            }
            column--;
            return dataClass;
        }
        private Data AnalyzeList(DataTable dt, FieldInfo info, int row, ref int column)
        {
            RemoveEmpty(dt, row, ref column);
            ListTypeInfo listType = info.BaseInfo as ListTypeInfo;
            FieldInfo element = listType.ItemInfo;
            DataList dataList = new DataList();
            bool isEnd = false;
            while (!isEnd)
            {
                Data dataField = AnalyzeField(dt, element, row, ref column);
                if (dataField != null && !isEnd)
                {
                    dataList.Elements.Add(dataField);
                    column++;
                }
                else
                    break;
            }
            DoSetEnd(dt, row, ref column);
            return dataList;
        }
        private Data AnalyzeDict(DataTable dt, FieldInfo info, int row, ref int column)
        {
            RemoveEmpty(dt, row, ref column);
            DictTypeInfo dictType = info.BaseInfo as DictTypeInfo;
            FieldInfo keyInfo = dictType.KeyInfo;
            FieldInfo valueInfo = dictType.ValueInfo;

            DataDict dataDict = new DataDict();
            HashSet<object> hash = new HashSet<object>();
            bool isEnd = false;
            while (!isEnd)
            {
                DataBase dataKey = AnalyzeField(dt, keyInfo, row, ref column) as DataBase;
                if (dataKey == null)
                    break;
                column++;
                Data dataValue = AnalyzeField(dt, valueInfo, row, ref column);
                if (dataValue == null)
                    break;
                else if (hash.Contains(dataKey.Data))
                    Util.LogErrorFormat("{0} {1} Key:{2}重复,{3}", info.Type, info.Name,
                        dataKey.Data.ToString(), GetErrorSite(column, row + 1));
                else
                {
                    hash.Add(dataKey.Data);
                    dataDict.Pairs.Add(new KeyValuePair<DataBase, Data>(dataKey, dataValue));
                    column++;
                }
            }
            DoSetEnd(dt, row, ref column);
            return dataDict;
        }
    }

}
