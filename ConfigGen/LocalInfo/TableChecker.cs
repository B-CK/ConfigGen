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
        /// 枚举值存在性检查
        /// </summary>
        EnumValue,


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

        /// <summary>
        /// 仅检查基础类型数据,不对结构进行检查
        /// </summary>
        public static void CheckAllData()
        {
            Dictionary<string, TableDataInfo> datas = Local.Instance.DataInfoDict;
            foreach (var table in datas)
            {
                DataClassInfo dataClass = table.Value.DataClassInfo;
                ClassTypeInfo classType = dataClass.BaseInfo as ClassTypeInfo;
                classType.IndexField.RuleDict.Add(CheckRuleType.Unique, null);
                CheckField(classType.IndexField);//数据表键的唯一性检查
                CheckField(dataClass);
            }
        }
        /// <summary>
        /// 检查类字段
        /// </summary>
        static void CheckField(FieldInfo info)
        {
            BaseTypeInfo baseType = info.BaseInfo;
            switch (baseType.TypeType)
            {
                case TypeType.Base:
                case TypeType.Enum:
                    CheckBase(info);
                    break;
                case TypeType.Class:
                    CheckClass(info);
                    break;
                case TypeType.List:
                    CheckList(info);
                    break;
                case TypeType.Dict:
                    CheckDict(info);
                    break;
                case TypeType.None:
                default:
                    break;
            }
        }
        static void CheckBase(FieldInfo info)
        {
            DataBaseInfo dataBase = info as DataBaseInfo;
            AnalyzeCheckRule(dataBase);
            CheckFieldData(dataBase);
        }
        static void CheckClass(FieldInfo info)
        {
            DataClassInfo dataClass = info as DataClassInfo;
            foreach (var field in dataClass.Fields)
                CheckField(field.Value);
        }
        static void CheckList(FieldInfo info)
        {
            DataListInfo dataList = info as DataListInfo;
            for (int i = 0; i < dataList.Elements.Count; i++)
                CheckField(dataList.Elements[i]);
        }
        static void CheckDict(FieldInfo info)
        {
            DataDictInfo dataDict = info as DataDictInfo;
            foreach (var pair in dataDict.Pairs)
            {
                pair.Key.RuleDict.Add(CheckRuleType.Unique, null);
                CheckField(pair.Key);
                CheckField(pair.Value);
            }
        }

        /// <summary>
        /// 解析检查规则
        /// </summary>
        static bool AnalyzeCheckRule(DataBaseInfo tableFieldInfo)
        {
            if (string.IsNullOrWhiteSpace(tableFieldInfo.Check))
                return false;

            string[] checks = tableFieldInfo.Check.Split(Values.CheckRuleSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string refFlag = "ref";
            string[] rangeFlags = { "[", "]", "(", ")" };
            string noEmptyFlag = "noEmpty";
            string uniqueFlag = "unique";
            //string[] relOpFlags = { "<", ">", "<=", ">=", "==" };
            string fileExistFlags = "file";

            bool isOK = true;
            for (int i = 0; i < checks.Length; i++)
            {
                string check = checks[i];
                CheckRuleType ruleType = CheckRuleType.None;
                List<string> ruleArgs = new List<string>();
                bool isNullOrWhiteSpace = string.IsNullOrWhiteSpace(check);
                if (isNullOrWhiteSpace) continue;


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
                if (!tableFieldInfo.RuleDict.ContainsKey(ruleType))
                    tableFieldInfo.RuleDict.Add(ruleType, ruleArgs);
            }
            return isOK;
        }
        /// <summary>
        /// 检查字段数据
        /// </summary>
        static string CheckFieldData(DataBaseInfo dataField)
        {
            string errorString = null;
            foreach (var checkRule in dataField.RuleDict)
            {
                List<string> ruleArgs = checkRule.Value;
                switch (checkRule.Key)
                {
                    case CheckRuleType.None:
                        break;
                    case CheckRuleType.Ref:
                        errorString = CheckRef(ruleArgs, dataField);
                        break;
                    case CheckRuleType.Range:
                        errorString = CheckRange(ruleArgs, dataField);
                        break;
                    case CheckRuleType.NoEmpty:
                        errorString = CheckNoEmpty(ruleArgs, dataField);
                        break;
                    case CheckRuleType.Unique:
                        errorString = CheckUnique(ruleArgs, dataField);
                        break;
                    case CheckRuleType.FileExist:
                        errorString = CheckFileExist(ruleArgs, dataField);
                        break;
                    default:
                        break;
                }
            }

            return errorString;
        }
        //hash效率是否比list块
        //object类型数据能否比较内容
        static string CheckRef(List<string> args, DataBaseInfo dataField)
        {
            if (args.Count == 0)
                return "[ref]引用检查规则未填写参数";

            List<object> datas = dataField.Data;
            StringBuilder error = new StringBuilder();
            int lastIndex = args[0].LastIndexOf('.');
            string className = args[0].Substring(0, lastIndex);
            string fieldName = args[0].Substring(lastIndex);

            var dataInfoDict = Local.Instance.DataInfoDict;
            if (dataInfoDict.ContainsKey(className))
            {
                DataClassInfo dataClass = Local.Instance.DataInfoDict[className].DataClassInfo;
                if (dataClass.Fields.ContainsKey(fieldName))
                {
                    DataBaseInfo dataBase = dataClass.Fields[fieldName] as DataBaseInfo;
                    HashSet<object> hash = new HashSet<object>(dataBase.Data);
                    for (int i = 0; i < datas.Count; i++)
                    {
                        if (!hash.Contains(datas[i]))
                            error.AppendFormat("[ref]引用{0}类型{1}字段的第{2}行数据不存在\n",
                                className, fieldName, i + Values.DataSheetDataStartIndex);
                    }
                }
                else
                    error.AppendFormat("[ref]引类型{0}中无{1}字段", className, fieldName);
            }
            else
                error.AppendFormat("[ref]引类型{0}不存在", className);

            return error.ToString();
        }
        static string CheckUnique(List<string> args, DataBaseInfo dataField)
        {
            StringBuilder error = new StringBuilder();
            List<object> datas = dataField.Data;
            HashSet<object> hash = new HashSet<object>(datas);
            for (int i = 0; i < datas.Count; i++)
            {
                if (!hash.Contains(datas[i]))
                    error.AppendFormat("[unique]数据列中{0}数据重复\n", datas[i]);
            }
            return error.ToString();
        }
        static string CheckNoEmpty(List<string> args, DataBaseInfo dataField)
        {
            StringBuilder error = new StringBuilder();
            List<object> datas = dataField.Data;
            string trim = args.Count > 0 ? args[0] : null;
            bool isString = dataField.Type.Equals("string");
            bool checkWhiteSpace = !string.IsNullOrWhiteSpace(trim);
            for (int i = 0; i < datas.Count; i++)
            {
                string v = datas[i] as string;
                if (isString)
                {
                    if (checkWhiteSpace && string.IsNullOrWhiteSpace(v))
                        error.AppendFormat("[notEmpty:trim]第{0}行为空白字符\n", i + Values.DataSheetDataStartIndex);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(v))
                        error.AppendFormat("[notEmpty]第{0}行未填数据\n", i + Values.DataSheetDataStartIndex);
                }
            }
            return error.ToString();
        }
        static string CheckFileExist(List<string> args, DataBaseInfo dataField)
        {
            if (args.Count == 0)
                return "[file]文件存在性检查规则未填写参数";

            List<object> datas = dataField.Data;
            StringBuilder error = new StringBuilder();
            int lastIndex = args[0].LastIndexOf('.');
            string dirPath = args[0].Substring(0, lastIndex);
            string ext = args[0].Substring(lastIndex);

            for (int i = 0; i < datas.Count; i++)
            {
                string file = datas[i] as string;
                if (!string.IsNullOrWhiteSpace(ext))
                    file = string.Format("{0}.{1}", file, ext);
                if (!File.Exists(file))
                    error.AppendFormat("[file]文件{0}不存在\n", file);
            }

            return error.ToString();
        }
        static string CheckRange(List<string> args, DataBaseInfo dataField)
        {
            if (args.Count != 2)
                return "[range]数值范围性检查规则参数填写错误";

            StringBuilder error = new StringBuilder();
            List<object> datas = dataField.Data;
            bool leftOpen = args[0][0].Equals('(');
            bool rightOpen = args[1][1].Equals(')');
            long start = Convert.ToInt64(args[0][1]);
            long end = Convert.ToInt64(args[1][0]);
            for (int i = 0; i < datas.Count; i++)
            {
                long v = Convert.ToInt64(datas[i]);
                bool leftResult = leftOpen ? start < v : start <= v;
                bool rightResult = rightOpen ? v < end : v <= end;
                if (!leftResult || !rightResult)
                    error.AppendFormat("[range]第{0}行数据不在范围内\n", i + Values.DataSheetDataStartIndex);
            }

            return error.ToString();
        }

    }
}
