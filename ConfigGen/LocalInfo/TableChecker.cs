using System;
using System.Text;
using System.Collections.Generic;

namespace ConfigGen.LocalInfo
{
    public enum CheckRuleType
    {
        None,
        /// <summary>
        /// 检查该字段引用数据是否在指定范围内,例:引用其他表中数据,或者枚举类中值
        /// 引用类字段数据
        /// 引用枚举
        /// </summary>
        Ref,

        /// <summary>
        /// 内容范围检查
        /// int,long,float:检查数值是否在指定范围内.
        /// string,list,dict:检查长度是否在指定范围内.
        /// </summary>
        Range,
        /// <summary>
        /// 数据不可为空;
        /// trim参数,字符串不能为null,空白符;
        /// </summary>
        NoEmpty,
        /// <summary>
        /// 有效性检查
        /// </summary>
        Validity,
        /// <summary>
        /// 唯一性检查
        /// </summary>
        Unique,
        /// <summary>
        /// 关系运算
        /// </summary>
        RelationalOp,
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
            if (!LocalInfoManager.Instance.TypeInfoLib.TypeInfoDict.ContainsKey(typeName))
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
        public static bool ParseCheckRule(FieldInfo tableFieldInfo)
        {
            if (string.IsNullOrWhiteSpace(tableFieldInfo.Check))
                return false;

            string[] checks = tableFieldInfo.Check.Split(Values.CheckRuleSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string refFlag = "ref";
            string[] rangeFlags = { "[", "]", "(", ")" };
            string noEmptyFlag = "noEmpty";
            string uniqueFlag = "unique";
            string[] relOpFlags = { "<", ">", "<=", ">=", "==" };
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
                    for (int j = 0; j < relOpFlags.Length; j++)
                    {
                        if (check.StartsWith(relOpFlags[j]) || check.EndsWith(relOpFlags[j]))
                        {
                            ruleType = CheckRuleType.RelationalOp;
                            ruleArgs.Add(check.Replace(relOpFlags[j], ""));
                            break;
                        }
                    }
                }
                if (!tableFieldInfo.RuleDict.ContainsKey(ruleType))
                    tableFieldInfo.RuleDict.Add(ruleType, ruleArgs);
            }
            return isOK;
        }
        public static string CheckFieldData(FieldInfo tableFieldInfo, object data)
        {
            string errorString = null;
            foreach (var checkRule in tableFieldInfo.RuleDict)
            {
                List<string> ruleArgs = checkRule.Value;
                switch (checkRule.Key)
                {
                    case CheckRuleType.None:
                        break;
                    case CheckRuleType.Ref:
                        errorString = CheckRef(ruleArgs, data);
                        break;
                    case CheckRuleType.Range:
                        errorString = CheckRange(ruleArgs, data);
                        break;
                    case CheckRuleType.NoEmpty:
                        errorString = CheckNoEmpty(ruleArgs, data);
                        break;
                    case CheckRuleType.Validity:
                        errorString = CheckValidity(ruleArgs, data);
                        break;
                    case CheckRuleType.Unique:
                        errorString = CheckUnique(ruleArgs, data);
                        break;
                    case CheckRuleType.RelationalOp:
                        errorString = CheckRelationalOp(ruleArgs, data);
                        break;
                    case CheckRuleType.FileExist:
                        errorString = CheckFileExist(ruleArgs, data);
                        break;
                    default:
                        break;
                }
            }

            return errorString;
        }
        static string CheckRef(List<string> args, object data)
        {
            if (args.Count == 0)
                return "[ref]引用检查规则未填写参数";

            int lastIndex = args[0].LastIndexOf('.');
            string className = args[0].Substring(0, lastIndex);
            string fieldName = args[0].Substring(lastIndex);


            return null;
        }
        static string CheckDefine(List<string> args, object data)
        {
            string error = null;
            return error;
        }
        static string CheckRange(List<string> args, object data)
        {
            string error = null;
            return error;
        }
        static string CheckNoEmpty(List<string> args, object data)
        {
            string error = null;
            return error;
        }
        static string CheckValidity(List<string> args, object data)
        {
            string error = null;
            return error;
        }
        static string CheckUnique(List<string> args, object data)
        {
            string error = null;
            return error;
        }
        static string CheckRelationalOp(List<string> args, object data)
        {
            string error = null;
            return error;
        }
        static string CheckFileExist(List<string> args, object data)
        {
            string error = null;
            return error;
        }
    }
}
