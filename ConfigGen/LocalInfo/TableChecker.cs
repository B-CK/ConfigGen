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
        /// 检查字段内容长度,仅限类字段,不包含子字段
        /// </summary>
        SameLenght,
        /// <summary>
        /// 数据不可为空
        /// </summary>
        NoEmpty,
        /// <summary>
        /// 字符串不能为null,空白符
        /// </summary>
        NoEmptyTrim,
        /// <summary>
        /// 内容是否非法检查
        /// </summary>
        Illegal,
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
        public static bool ParseCheckRule(TableFieldInfo tableFieldInfo)
        {
            string check = tableFieldInfo.Check;
            string[] rangeFlags = { "[", "]", "(", ")" };
            string[] relOpFlags = { "<", ">", "<=", ">=", "==" };

            return false;
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
                case CheckRuleType.SameLenght:
                    break;
                case CheckRuleType.NoEmpty:
                    break;
                case CheckRuleType.NoEmptyTrim:
                    break;
                case CheckRuleType.Illegal:
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
