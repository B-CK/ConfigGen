using System;
using System.Text;
using System.Collections.Generic;

namespace ConfigGen.LocalInfo
{
    public enum CheckRuleType
    {
        None,
        /// <summary>
        /// 引用已声明类,检查引用类型匹配性
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
        public static bool CheckClass(string className)
        {
            return LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict.ContainsKey(className);
        }
        /// <summary>
        /// 检查字段类型,true:通过,false:不通过
        /// </summary>
        /// <param name="type">类型全名,即带命名空间</param>
        public static bool CheckFieldClassName(string type, out string errorString)
        {
            FieldTypeType fieldTypeType = TypeInfo.GetFieldTypeType(type);
            switch (fieldTypeType)
            {
                case FieldTypeType.Base:
                    break;
                case FieldTypeType.Class:
                case FieldTypeType.Enum:
                    int endIndex = type.LastIndexOf('.');
                    if (endIndex > -1 && !CheckClass(type))
                    {
                        errorString = string.Format("类型{0}不存在", type);
                        return false;
                    }
                    break;
                case FieldTypeType.List:
                    string element = type.Replace("list<", "").Replace(">", "");
                    FieldTypeType elementType = TypeInfo.GetFieldTypeType(element);
                    if (elementType == FieldTypeType.None)
                    {
                        errorString = string.Format("list中元素类型{0}不存在", element);
                        return false;
                    }
                    else if (elementType == FieldTypeType.Dict || elementType == FieldTypeType.List)
                    {
                        errorString = string.Format("list元素类型{0}不能再为集合", element);
                        return false;
                    }
                    break;
                case FieldTypeType.Dict:
                    string keyValue = type.Replace("dict<", "").Replace(">", "");
                    string[] nodes = keyValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (nodes.Length != 2)
                    {
                        errorString = string.Format("dict中只能填写key类型和value类型,错误{0}", type);
                        return false;
                    }
                    FieldTypeType keyType = TypeInfo.GetFieldTypeType(nodes[0]);
                    if (keyType != FieldTypeType.Base && keyType != FieldTypeType.Enum)
                    {
                        errorString = string.Format("dict中key类型只能为基础类型或者枚举,错误{0}", type);
                        return false;
                    }
                    FieldTypeType valueType = TypeInfo.GetFieldTypeType(nodes[1]);
                    if (valueType == FieldTypeType.None)
                    {
                        errorString = string.Format("dict中value类型{0}不存在", nodes[1]);
                        return false;
                    }
                    else if (valueType == FieldTypeType.Dict || valueType == FieldTypeType.List)
                    {
                        errorString = string.Format("dict中value类型{0}不能再为集合", nodes[1]);
                        return false;
                    }
                    break;
                case FieldTypeType.None:
                default:
                    errorString = string.Format("类型{0}不存在", type);
                    return false;
            }

            errorString = null;
            return true;
        }


        //---------------------------------------数据检查
        public static void ParseCheckRule(TableFieldInfo tableFieldInfo)
        {
            string[] checks = tableFieldInfo.Check.Split(Values.CheckRuleSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string refFlag = "ref";
            string defineFlag = "define";
            string[] rangeFlags = { "[", "]", "(", ")" };
            string noEmptyFlag = "noEmpty";
            string uniqueFlag = "unique";
            string[] relOpFlags = { "<", ">", "<=", ">=", "==" };
            string fileExistFlags = "file";

            for (int i = 0; i < checks.Length; i++)
            {
                string check = checks[i];
                CheckRuleType ruleType = CheckRuleType.None;
                List<string> ruleArgs = new List<string>();

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
                if (!tableFieldInfo.RuleDict.ContainsKey(ruleType))
                    tableFieldInfo.RuleDict.Add(ruleType, ruleArgs);
            }
        }
        public static bool CheckFieldData(TableFieldInfo tableFieldInfo, object data)
        {
            CheckRuleType ruleType = CheckRuleType.None;
            switch (ruleType)
            {
                case CheckRuleType.None:
                    break;
                case CheckRuleType.Ref:
                    break;
                case CheckRuleType.Define:
                    break;
                case CheckRuleType.Range:
                    break;
                case CheckRuleType.NoEmpty:
                    break;
                case CheckRuleType.Validity:
                    break;
                case CheckRuleType.Unique:
                    break;
                case CheckRuleType.RelationalOp:
                    break;
                case CheckRuleType.FileExist:
                    break;
                default:
                    break;
            }
            return false;
        }
        static bool CheckNoEmpty(string type, object data, CheckRuleType ruleType)
        {
            return false;
        }


    }
}
