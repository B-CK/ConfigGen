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
        /// 为数据表格指定类型,检查类型信息是否匹配
        /// </summary>
        Define,

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
        public static string CheckClass(string className)
        {
            string error = null;
            if (!LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict.ContainsKey(className))
                error = string.Format("类型{0}不存在", className);
            return error;
        }
        /// <summary>
        /// 检查字段类型,基于单个格子
        /// true:通过,false:不通过
        /// </summary>
        /// <param name="type">类型全名,即带命名空间</param>
        public static string CheckFieldClassName(string type)
        {
            string errorString = null;
            FieldTypeType fieldTypeType = TypeInfo.GetFieldTypeType(type);
            switch (fieldTypeType)
            {
                case FieldTypeType.Base:
                    break;
                case FieldTypeType.Class:
                case FieldTypeType.Enum:
                    int endIndex = type.LastIndexOf('.');
                    errorString = CheckClass(type);
                    if (endIndex > -1 && !string.IsNullOrWhiteSpace(errorString))
                    {
                        return errorString;
                    }
                    break;
                case FieldTypeType.List:
                    string element = type.Replace("list<", "").Replace(">", "");
                    FieldTypeType elementType = TypeInfo.GetFieldTypeType(element);
                    if (elementType == FieldTypeType.None)
                    {
                        return string.Format("list中元素类型{0}不存在", element);
                    }
                    else if (elementType == FieldTypeType.Dict || elementType == FieldTypeType.List)
                    {
                        return string.Format("list元素类型{0}不能再为集合", element);
                    }
                    break;
                case FieldTypeType.Dict:
                    string keyValue = type.Replace("dict<", "").Replace(">", "");
                    string[] nodes = keyValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (nodes.Length != 2)
                    {
                        return string.Format("dict中只能填写key类型和value类型,错误类型{0}", type);
                    }
                    FieldTypeType keyType = TypeInfo.GetFieldTypeType(nodes[0]);
                    if (keyType != FieldTypeType.Base && keyType != FieldTypeType.Enum)
                    {
                        return string.Format("dict中key类型只能为基础类型或者枚举,错误类型{0}", type);
                    }
                    FieldTypeType valueType = TypeInfo.GetFieldTypeType(nodes[1]);
                    if (valueType == FieldTypeType.None)
                    {
                        return string.Format("dict中value类型{0}不存在", nodes[1]);
                    }
                    else if (valueType == FieldTypeType.Dict || valueType == FieldTypeType.List)
                    {
                        return string.Format("dict中value类型{0}不能再为集合", nodes[1]);
                    }
                    break;
                case FieldTypeType.None:
                default:
                    return string.Format("类型种类{0}不存在", type);
            }

            return errorString;
        }


        //---------------------------------------数据检查
        public static string ParseCheckRule(TableFieldInfo tableFieldInfo)
        {
            string[] checks = tableFieldInfo.Check.Split(Values.CheckRuleSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string refFlag = "ref";
            string defineFlag = "define";
            string[] rangeFlags = { "[", "]", "(", ")" };
            string noEmptyFlag = "noEmpty";
            string uniqueFlag = "unique";
            string[] relOpFlags = { "<", ">", "<=", ">=", "==" };
            string fileExistFlags = "file";

            string error = null;
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
                else if (check.StartsWith(defineFlag))
                {
                    ruleType = CheckRuleType.Define;
                    ruleArgs.AddRange(check.Replace(defineFlag, "").Split(Values.CheckRunleArgsSplitFlag.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries));
                }
                else if (check.StartsWith(defineFlag))
                {
                    ruleType = CheckRuleType.Range;
                    ruleArgs.AddRange(check.Replace(defineFlag, "").Split(Values.CheckRunleArgsSplitFlag.ToCharArray(),
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

                if (isNullOrWhiteSpace == false)
                {
                    error = string.Format("{0}字段检查规则{1}不存在", tableFieldInfo.Name, check);
                    continue;
                }
                if (!tableFieldInfo.RuleDict.ContainsKey(ruleType))
                    tableFieldInfo.RuleDict.Add(ruleType, ruleArgs);
            }
            return error;
        }
        public static string CheckFieldData(TableFieldInfo tableFieldInfo, object data)
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
                    case CheckRuleType.Define:
                        errorString = CheckDefine(ruleArgs, data);
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
