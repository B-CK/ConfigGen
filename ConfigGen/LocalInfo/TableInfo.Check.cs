using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace ConfigGen.LocalInfo
{
    public enum CheckRuleType
    {
        None,
        //-----默认检查规则


        /// <summary>
        /// 检查该字段引用数据是否在指定范围内,例:引用其他表中数据,或者枚举类中值
        /// 引用类字段数据
        /// 引用枚举
        /// </summary>
        Ref,
        /// <summary>
        /// 唯一性检查,包括键的唯一性检查
        /// </summary>
        Unique,
        /// <summary>
        /// 数据不可为空;
        /// trim参数,字符串不能为null,空白符;
        /// </summary>
        NoEmpty,
        /// <summary>
        /// 内容范围检查
        /// int,long,float:检查数值是否在指定范围内.
        /// </summary>
        Range,
        /// <summary>
        /// 文件存在性检查,file:path:ext;file:path
        /// </summary>
        FileExist,

    }

    class TableChecker
    {
        /// <summary>
        /// 检查类是否存在
        /// </summary>
        /// <param name="typeName">类型名,可全路径可不全路径</param>
        /// <param name="nameSpace">非空时会检查组合类型</param>
        /// <returns></returns>
        public static string CheckType(string typeName)
        {
            string error = null;
            if (!Local.Instance.TypeInfoLib.TypeInfoDict.ContainsKey(typeName))
                error = string.Format("类型{0}不存在", typeName);
            return error;
        }

        static readonly HashSet<string> DictKeyTypeLimit = new HashSet<string>() { "int", "long", "string" };
        public static string CheckDictKey(string type)
        {
            string error = null;
            if (!DictKeyTypeLimit.Contains(type))
            {
                TypeType typeType = TypeInfo.GetTypeType(type);
                if (typeType != TypeType.Enum)
                    error = "字段key类型错误" + type;
            }
            return error;
        }

        //---------------------------------------数据检查

        static string ClassFullName = "";
        static List<Data> DataKeys = new List<Data>();
        /// <summary>
        /// 仅检查基础类型数据,不对结构进行检查
        /// </summary>
        public static void CheckAllData()
        {
            foreach (var table in Local.Instance.DataInfoDict)
            {
                ClassFullName = table.Key;
                List<DataClass> datas = table.Value.Datas;
                ClassTypeInfo classType = table.Value.ClassTypeInfo;
                List<FieldInfo> fields = classType.Fields;
                for (int column = 0; column < fields.Count; column++)//列
                {
                    FieldInfo info = fields[column];
                    List<Data> dataColum = table.Value.GetDataColumn(info.Name);

                    //数据表键的唯一性检查
                    if (classType.IndexField.Name == info.Name)
                    {
                        info.Check += "|unique";
                        DataKeys.Clear();
                        DataKeys.AddRange(dataColum);
                    }

                    CheckField(info, dataColum);
                }
            }
            DataKeys.Clear();
            ClassFullName = "";
        }
        static long GetKey(int i)
        {
            long key = long.MinValue;
            if (DataKeys.Count > i)
                key = DataKeys[i] as DataBase;
            return key;
        }
        /// <summary>
        /// 检查类字段
        /// </summary>
        static void CheckField(FieldInfo info, List<Data> datas)
        {
            BaseTypeInfo baseType = info.BaseInfo;
            if (baseType.TypeType != TypeType.Class
                && string.IsNullOrWhiteSpace(info.Check))
                return;

            switch (baseType.TypeType)
            {
                case TypeType.Base:
                case TypeType.Enum:
                    CheckBase(info, datas);
                    break;
                case TypeType.Class:
                    CheckClass(info, datas);
                    break;
                case TypeType.List:
                    CheckList(info, datas);
                    break;
                case TypeType.Dict:
                    CheckDict(info, datas);
                    break;
                case TypeType.None:
                default:
                    break;
            }
        }
        static void CheckBase(FieldInfo info, List<Data> datas)
        {
            AnalyzeCheckRule(info);
            CheckFieldData(info, datas);
        }
        static void CheckClass(FieldInfo info, List<Data> datas)
        {
            ClassTypeInfo classType = info.BaseInfo as ClassTypeInfo;
            {
                List<FieldInfo> fields = classType.Fields;
                for (int column = 0; column < fields.Count; column++)//列
                {
                    FieldInfo field = fields[column];
                    List<Data> dataColum = new List<Data>();
                    for (int row = 0; row < datas.Count; row++)//行
                    {
                        DataClass dataClass = datas[row] as DataClass;
                        dataColum.Add(dataClass.Fields[field.Name]);
                    }

                    CheckField(field, dataColum);
                }
            }
            if (classType.HasSubClass)
            {
                foreach (var type in classType.SubClasses)
                {
                    ClassTypeInfo polyClass = TypeInfo.GetTypeInfo(type) as ClassTypeInfo;
                    List<FieldInfo> fields = polyClass.Fields;
                    for (int column = 0; column < fields.Count; column++)//列
                    {
                        FieldInfo field = fields[column];
                        List<Data> dataColum = new List<Data>();
                        for (int row = 0; row < datas.Count; row++)//行
                        {
                            DataClass dataClass = datas[row] as DataClass;
                            if (dataClass.Fields.ContainsKey(field.Name))
                                dataColum.Add(dataClass.Fields[field.Name]);
                        }

                        CheckField(field, dataColum);
                    }
                }
            }
        }
        static void CheckList(FieldInfo info, List<Data> datas)
        {
            ListTypeInfo listType = info.BaseInfo as ListTypeInfo;
            FieldInfo element = new FieldInfo();
            element.Set("List.Element", listType.GetClassName(), info.Check, info.Group);
            for (int i = 0; i < datas.Count; i++)
            {
                DataList dataList = datas[i] as DataList;
                CheckField(element, dataList.Elements);
            }
        }
        static void CheckDict(FieldInfo info, List<Data> datas)
        {
            DictTypeInfo dictInfo = info.BaseInfo as DictTypeInfo;
            FieldInfo keyInfo = new FieldInfo();
            keyInfo.Set("Dict.Key", dictInfo.KeyType, info.Check, info.Group);
            FieldInfo valueInfo = new FieldInfo();
            keyInfo.Set("Dict.Value", dictInfo.ValueType, info.Check, info.Group);
            for (int i = 0; i < datas.Count; i++)
            {
                DataDict dataDict = datas[i] as DataDict;
                List<Data> keys = new List<Data>();
                List<Data> values = new List<Data>();
                for (int j = 0; j < dataDict.Pairs.Count; j++)
                {
                    keys.Add(dataDict.Pairs[j].Key);
                    values.Add(dataDict.Pairs[j].Value);
                }
                CheckField(keyInfo, keys);
                CheckField(valueInfo, values);
            }
        }


        //一下均需要访问列数据..

        /// <summary>
        /// 解析检查规则
        /// </summary>
        static bool AnalyzeCheckRule(FieldInfo info)
        {
            string[] checks = info.Check.Split(Values.CheckRuleSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string refFlag = "ref";
            string[] rangeFlags = { "[", "]", "(", ")" };
            string noEmptyFlag = "noEmpty";
            string uniqueFlag = "unique";
            //string[] relOpFlags = { "<", ">", "<=", ">=", "==" };
            string fileExistFlags = "file";
            string dictKey = "key";
            string dictValue = "value";

            bool isOK = true;
            for (int i = 0; i < checks.Length; i++)
            {
                string check = checks[i];
                CheckRuleType ruleType = CheckRuleType.None;
                List<string> ruleArgs = new List<string>();
                bool isNullOrWhiteSpace = string.IsNullOrWhiteSpace(check);
                if (isNullOrWhiteSpace) continue;

                if (check.StartsWith(dictKey))
                    ;
                else if (check.StartsWith(dictValue))
                {

                }


                if (check.StartsWith(refFlag))
                {
                    ruleType = CheckRuleType.Ref;
                    ruleArgs.AddRange(check.Replace(refFlag, "").Split(Values.CheckRunleArgsSplitFlag.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries));
                }
                else if (check.StartsWith(noEmptyFlag))
                {
                    ruleType = CheckRuleType.NoEmpty;
                    ruleArgs.AddRange(check.Replace(noEmptyFlag, "").Split(Values.CheckRunleArgsSplitFlag.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries));
                }
                else if (check.StartsWith(uniqueFlag))
                {
                    ruleType = CheckRuleType.Unique;
                    ruleArgs.AddRange(check.Replace(uniqueFlag, "").Split(Values.CheckRunleArgsSplitFlag.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries));
                }
                else if (check.StartsWith(fileExistFlags))
                {
                    if (string.IsNullOrWhiteSpace(Values.AssetsDir))
                    {
                        Util.LogWarningFormat("未配置资源目录路径无法使用file检查功能.错误位置:{0} {1}", ClassFullName, info.Name);
                        continue;
                    }
                    ruleType = CheckRuleType.FileExist;
                    ruleArgs.AddRange(check.Replace(fileExistFlags, "").Split(Values.CheckRunleArgsSplitFlag.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries));
                }
                else
                {
                    for (int j = 0; j < rangeFlags.Length; j++)
                    {
                        if (check.StartsWith(rangeFlags[j]) || check.EndsWith(rangeFlags[j]))
                        {
                            ruleType = CheckRuleType.Range;
                            ruleArgs.AddRange(check.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                            break;
                        }
                    }
                    //for (int j = 0; j < relOpFlags.Length; j++)
                    //{
                    //    if (check.StartsWith(relOpFlags[j]) || check.EndsWith(relOpFlags[j]))
                    //    {
                    //        ruleType = CheckRuleType.RelationalOp;
                    //        ruleArgs.Add(check.Replace(relOpFlags[j], ""));
                    //        break;
                    //    }
                    //}
                }

                if (!info.RuleDict.ContainsKey(ruleType))
                    info.RuleDict.Add(ruleType, ruleArgs);

                if (!isNullOrWhiteSpace && ruleType == CheckRuleType.None)
                    Util.LogWarningFormat("类:{0} 异常:检查规则{1}不存在", ClassFullName, check);

            }
            return isOK;
        }
        /// <summary>
        /// 检查字段数据
        /// </summary>
        static void CheckFieldData(FieldInfo info, List<Data> datas)
        {
            string error = null;
            foreach (var checkRule in info.RuleDict)
            {
                List<string> ruleArgs = checkRule.Value;
                switch (checkRule.Key)
                {
                    case CheckRuleType.None:
                        break;
                    case CheckRuleType.Ref:
                        error = CheckRef(ruleArgs, datas);
                        break;
                    case CheckRuleType.Range:
                        error = CheckRange(ruleArgs, datas);
                        break;
                    case CheckRuleType.NoEmpty:
                        error = CheckNoEmpty(ruleArgs, datas);
                        break;
                    case CheckRuleType.Unique:
                        error = CheckUnique(ruleArgs, datas);
                        break;
                    case CheckRuleType.FileExist:
                        error = CheckFileExist(ruleArgs, datas);
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(error))
                Util.LogErrorFormat("Check:{0}.{1}\n{2}", ClassFullName, info.Name, error);
        }
        static string CheckRef(List<string> args, List<Data> datas)
        {
            if (args.Count == 0)
                return "[ref]引用检查规则未填写参数";

            StringBuilder error = new StringBuilder();
            int lastIndex = args[0].LastIndexOf('.');
            string className = args[0].Substring(0, lastIndex);
            string fieldName = args[0].Substring(lastIndex);

            var dataInfoDict = Local.Instance.DataInfoDict;
            if (dataInfoDict.ContainsKey(className))
            {
                List<Data> dataClass = Local.Instance.DataInfoDict[className].GetDataColumn(fieldName);
                if (dataClass != null)
                {
                    HashSet<object> hash = new HashSet<object>();
                    for (int i = 0; i < datas.Count; i++)
                    {
                        object data = (datas[i] as DataBase).Data;
                        if (!hash.Contains(data))
                            error.AppendFormat("[ref]key:{0} 引用{1}.{2}数据不存在\n", GetKey(i), className, fieldName);
                        else
                            hash.Add(data);
                    }
                }
                else
                    error.AppendFormat("[ref]引用{1}.{2}字段不存在", className, fieldName);
            }
            else
                error.AppendFormat("[ref]引用{0}类型不存在", className);
            return error.ToString();
        }
        static string CheckUnique(List<string> args, List<Data> datas)
        {
            StringBuilder error = new StringBuilder();
            HashSet<object> hash = new HashSet<object>();
            for (int i = 0; i < datas.Count; i++)
            {
                object data = (datas[i] as DataBase).Data;
                if (hash.Contains(data))
                    error.AppendFormat("[unique]key:{0} 集合中数据{1}重复\n", GetKey(i), data);
                else
                    hash.Add(data);
            }
            return error.ToString();
        }
        static string CheckNoEmpty(List<string> args, List<Data> datas)
        {
            StringBuilder error = new StringBuilder();
            string trim = args.Count > 0 ? args[0] : null;
            bool checkWhiteSpace = !string.IsNullOrWhiteSpace(trim);

            for (int i = 0; i < datas.Count; i++)
            {
                string v = datas[i] as DataBase;
                if (string.IsNullOrWhiteSpace(v))
                    error.AppendFormat("[notEmpty]key:{0} 第{1}行未填数据\n", GetKey(i), i + Values.DataSheetDataStartIndex + 1);
            }
            return error.ToString();
        }
        static string CheckFileExist(List<string> args, List<Data> datas)
        {
            if (args.Count == 0)
                return "[file]文件存在性检查规则未填写参数";

            StringBuilder error = new StringBuilder();

            int lastIndex = args[0].LastIndexOf('.');
            string dirPath = args[0].Substring(0, lastIndex);
            string ext = args[0].Substring(lastIndex);

            for (int i = 0; i < datas.Count; i++)
            {
                string file = datas[i] as DataBase;
                if (!string.IsNullOrWhiteSpace(ext))
                    file = string.Format("{0}\\{1}.{2}", Values.AssetsDir, file, ext);
                if (!File.Exists(file))
                    error.AppendFormat("[file]key:{0} 文件{1}不存在\n", GetKey(i), file);
            }

            return error.ToString();
        }
        static string CheckRange(List<string> args, List<Data> datas)
        {
            if (args.Count != 2)
                return "[range]数值范围性检查规则参数填写错误";

            StringBuilder error = new StringBuilder();
            bool leftOpen = args[0][0].Equals('(');
            int rightLenght = args[1].Length;
            bool rightOpen = args[1][rightLenght - 1].Equals(')');
            long start, end;
            start = long.Parse(args[0].Substring(1));
            end = long.Parse(args[1].Substring(0, rightLenght - 1));
            for (int i = 0; i < datas.Count; i++)
            {
                long v = datas[i] as DataBase;
                bool leftResult = leftOpen ? start < v : start <= v;
                bool rightResult = rightOpen ? v < end : v <= end;
                if (!leftResult || !rightResult)
                    error.AppendFormat("[range]key:{0} {1}数据不在范围内\n", GetKey(i), v);
            }

            return error.ToString();
        }

    }
}
