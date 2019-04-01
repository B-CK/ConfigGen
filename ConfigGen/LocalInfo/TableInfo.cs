using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace ConfigGen.Description
{
    /// <summary>
    /// Sheet数据内容
    /// </summary>
    public abstract class TableInfo
    {
        /// <summary>
        /// 文件相对路径
        /// </summary>
        public string AbsPath { get; private set; }
        public ClassTypeInfo ClassTypeInfo { get; private set; }
        public List<DataClass> Datas { get; protected set; }

        public TableInfo(string absPath, ClassTypeInfo classType)
        {
            AbsPath = absPath;
            if (classType != null)
                ClassTypeInfo = classType;
            else
                Util.LogErrorFormat("数据表结构没有指明类型,数据路径:{0}", AbsPath);
        }
        public abstract void Analyze();

        /// <summary>
        /// 字段列数据字典
        /// <para>key:字段名 value:数据列</para>
        /// </summary>
        Dictionary<string, List<Data>> DataColumnDict = new Dictionary<string, List<Config>>();

        /// <summary>
        /// 表主键列字段数据
        /// </summary>
        public List<Data> GetDataColumn(string fieldName)
        {
            List<Config> dataColum = new List<Config>();
            var fieldDict = ClassTypeInfo.FieldDict;
            if (!fieldDict.ContainsKey(fieldName))
                return dataColum;

            if (DataColumnDict.ContainsKey(fieldName))
            {
                dataColum = DataColumnDict[fieldName];
            }
            else
            {
                for (int row = 0; row < Datas.Count; row++)//行
                    dataColum.Add(Datas[row].Fields[fieldName]);
                DataColumnDict.Add(fieldName, dataColum);
            }
            return dataColum;
        }
        public void RemoveField(FieldInfo field)
        {
            for (int k = 0; k < Datas.Count; k++)
                Datas[k].Fields.Remove(field.Name);

            DataColumnDict.Remove(field.Name);
        }

        public static Dictionary<string, TableInfo> DataInfoDict { get; private set; }
        public static void Init()
        {
            Util.Start();
            HashSet<string> hash = new HashSet<string>();
            DataInfoDict = new Dictionary<string, TableInfo>();
            var classList = TypeInfo.Instance.ClassInfos;
            for (int i = 0; i < classList.Count; i++)
            {
                var c = classList[i];
                if (string.IsNullOrWhiteSpace(c.DataPath)) continue;
                if (!hash.Contains(c.DataPath))
                    hash.Add(c.DataPath);
                else
                {
                    Util.LogErrorFormat("数据类{0}指定数据文件与其他类相同", c.GetFullName());
                    continue;
                }
                if (Consts.IsOptPart && !DataFileInfo.HasChangeFile(c.DataPath)) continue;

                //FileInfo info = new FileInfo(c.DataPath);
                //if (info.LastWriteTime == info.CreationTime) continue;
                //info.CreationTime = info.LastWriteTime;

                TableInfo data = null;
                if (File.Exists(c.DataPath))
                {
                    Util.Start();
                    var ds = Util.ReadXlsxFile(c.DataPath, out string error);
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Util.LogErrorFormat("Excel文件解析错误:{0}", error);
                        continue;
                    }

                    //--合并Sheet
                    DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
                    for (int j = 1; j < ds.Tables.Count; j++)
                    {
                        DataTable dt1 = ds.Tables[j].Copy();
                        if (dt1.Rows.Count < (Consts.DataSheetDataStartIndex + 1) || dt1 == null)
                        {
                            Util.LogErrorFormat("{0}文件中{1}表定义异常", c.DataPath, dt1.TableName);
                            continue;
                        }
                        else
                        {
                            for (int k = 0; k < Consts.DataSheetDataStartIndex; k++)
                                dt1.Rows.RemoveAt(0);
                            dt.Merge(dt1);
                        }
                    }
                    if (dt != null)
                        data = new TableDataInfo(c.DataPath, dt, c);
                    else
                    {
                        Util.LogErrorFormat(c.DataPath + " 中无数据或者无法读取不存在!");
                        continue;
                    }
                }
                else if (Directory.Exists(c.DataPath))
                {
                    Util.Start();
                    data = new TableXmlInfo(c.DataPath, c);
                }
                else
                {
                    Util.LogErrorFormat("数据路径 {0} 不存在!", c.DataPath);
                }

                if (data != null)
                {
                    data.Analyze();

                    if (!DataInfoDict.ContainsKey(c.GetFullName()))
                        DataInfoDict.Add(c.GetFullName(), data);
                    Util.Stop(string.Format("解析数据:{0}", c.DataPath));
                }
            }
            DataFileInfo.Save();
            Util.Stop("==>> 解析数据文件完毕!");
        }
    }






    //public abstract class Data { }

    //public class DataBase : Data
    //{
    //    public object Data;

    //    public static implicit operator int(DataBase value)
    //    {
    //        if (value.Data == null)
    //            return -1;
    //        return Convert.ToInt32(value.Data);
    //    }
    //    public static implicit operator long(DataBase value)
    //    {
    //        if (value.Data == null)
    //            return -1;
    //        return Convert.ToInt64(value.Data);
    //    }
    //    public static implicit operator float(DataBase value)
    //    {
    //        if (value.Data == null)
    //            return -1;
    //        return Convert.ToSingle(value.Data);
    //    }
    //    public static implicit operator double(DataBase value)
    //    {
    //        if (value.Data == null)
    //            return -1;
    //        return Convert.ToDouble(value.Data);
    //    }
    //    public static implicit operator string(DataBase value)
    //    {
    //        if (value.Data == null)
    //            return "";
    //        return Convert.ToString(value.Data);
    //    }
    //    public static implicit operator bool(DataBase value)
    //    {
    //        if (value.Data == null)
    //            return false;
    //        string v = value.Data as string;
    //        return v.ToLower().Equals("true") ? true : false;
    //    }
    //}
    //public class DataClass : Data
    //{
    //    public string Type { get; set; }
    //    public Dictionary<string, Data> Fields;
    //    public DataClass()
    //    {
    //        Fields = new Dictionary<string, Data>();
    //    }
    //}
    //public class DataList : Data
    //{
    //    public List<Data> Elements;
    //    public DataList()
    //    {
    //        Elements = new List<Data>();
    //    }
    //}
    //public class DataDict : Data
    //{
    //    public List<KeyValuePair<DataBase, Data>> Pairs { get; set; }
    //    public DataDict()
    //    {
    //        Pairs = new List<KeyValuePair<DataBase, Data>>();
    //    }
    //}
}
