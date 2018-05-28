using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConfigGen.LocalInfo
{
    //---类型定义表
    public class TableDefineInfo : TableInfo
    {
        public TableDefineInfo(string relPath, DataTable data)
            : base(relPath, data)
        {
            ClassInfoDict = new Dictionary<string, ClassTypeInfo>();
            EnumInfoDict = new Dictionary<string, EnumTypeInfo>();
        }
        public Dictionary<string, ClassTypeInfo> ClassInfoDict { get; private set; }
        public Dictionary<string, EnumTypeInfo> EnumInfoDict { get; private set; }


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
        public override void Analyze()
        {
            DataTable dt = TableDataSet;
            string defineTypeStr = "";
            string name = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string flag = dt.Rows[i][0].ToString();
                if (!flag.StartsWith(Values.DefineTypeFlag)) continue;

                //开始定义类型
                defineTypeStr = flag.TrimStart(Values.DefineTypeFlag.ToCharArray());
                string nameSpace = TypeInfo.GetNamespaceName(RelPath);
                name = dt.Rows[i][1].ToString();
                if (defineTypeStr.Equals("class"))
                {
                    string inherit = dt.Rows[i][2].ToString();
                    string dataTable = dt.Rows[i][3].ToString();
                    string group = dt.Rows[i][4].ToString();
                    ClassTypeInfo classInfo = new ClassTypeInfo();
                    classInfo.RelPath = RelPath;
                    classInfo.Name = name;
                    classInfo.NamespaceName = FilterEmptyOrNull(nameSpace);
                    classInfo.Inherit = FilterEmptyOrNull(inherit);
                    classInfo.DataTable = FilterEmptyOrNull(dataTable);
                    classInfo.Group = FilterEmptyOrNull(group);
                    classInfo.TypeType = TypeType.Class;
                    classInfo.Fields = new List<FieldInfo>();
                    classInfo.IsExist = true;



                    int j = i += 2;
                    for (; j < dt.Rows.Count; j++)
                    {
                        flag = dt.Rows[j][0].ToString();
                        if (string.IsNullOrWhiteSpace(flag) || flag.StartsWith(Values.DefineTypeFlag))
                            break;

                        FieldInfo fieldInfo = new FieldInfo();
                        fieldInfo.Name = dt.Rows[j][0].ToString();
                        string fieldType = dt.Rows[j][1].ToString();
                        fieldInfo.Type = fieldType;
                        fieldInfo.Des = FilterEmptyOrNull(dt.Rows[j][2].ToString());
                        fieldInfo.Check = FilterEmptyOrNull(dt.Rows[j][3].ToString());
                        fieldInfo.Group = FilterEmptyOrNull(dt.Rows[j][4].ToString());
                        classInfo.Fields.Add(fieldInfo);

                        //数据索引-字段信息
                        if (j == i)
                        {
                            string error = TableChecker.CheckDictKey(fieldInfo.Type);
                            if (!string.IsNullOrWhiteSpace(error))
                                Util.LogErrorFormat("表索引键类型只能为int,long,string,enum,错误位置{0}", classInfo.RelPath);
                            classInfo.IndexField = fieldInfo;
                        }
                    }
                    i = j - 1;
                    ClassInfoDict.Add(name, classInfo);
                    Local.Instance.TypeInfoLib.Add(classInfo);
                }
                else if (defineTypeStr.Equals("enum"))
                {
                    EnumTypeInfo enumInfo = new EnumTypeInfo();
                    enumInfo.RelPath = RelPath;
                    enumInfo.Name = name;
                    enumInfo.NamespaceName = FilterEmptyOrNull(TypeInfo.GetNamespaceName(RelPath));
                    enumInfo.Group = FilterEmptyOrNull(dt.Rows[i][2].ToString());
                    enumInfo.TypeType = TypeType.Enum;
                    enumInfo.KeyValuePair = new List<EnumKeyValue>();
                    enumInfo.IsExist = true;

                    int j = i += 2;
                    for (; j < dt.Rows.Count; j++)
                    {
                        string flag_ = dt.Rows[j][0].ToString();
                        if (flag_.StartsWith(Values.DefineTypeFlag))
                            break;

                        EnumKeyValue kv = new EnumKeyValue();
                        kv.Key = dt.Rows[j][0].ToString();
                        kv.Value = dt.Rows[j][1].ToString();
                        kv.Des = dt.Rows[j][2].ToString();
                        enumInfo.KeyValuePair.Add(kv);
                    }
                    enumInfo.UpdateToDict();
                    i = j - 1;
                    EnumInfoDict.Add(name, enumInfo);
                    Local.Instance.TypeInfoLib.Add(enumInfo);
                }
                else
                {
                    throw new Exception(string.Format("只能是##class,##enum等类型,Type:{0},错误位置:{1}",
                        defineTypeStr, Util.GetErrorSite(RelPath, 1, i + 1)));
                }
            }
        }


        string FilterEmptyOrNull(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? null : str;
        }
    }
}
