using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace ConfigGen.Description
{
    public enum CheckRuleType
    {
        None,
        //-----默认检查规则


        /// <summary>
        /// 检查该字段引用数据是否为指定数据表中主键,例:引用其他表中主键数据
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

    partial class TableChecker
    {
        static ClassTypeInfo ClassInfo = null;
        static List<Data> DataKeys = new List<Data>();
        /// <summary>
        /// 仅检查基础类型数据,不对结构进行检查
        /// </summary>
        public static void CheckAllData()
        {
            foreach (var table in TableInfo.DataInfoDict)
            {
                ClassInfo = table.Value.ClassTypeInfo;
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
                        string unique = CheckRuleType.Unique.ToString().ToLower();
                        classType.IndexField.AddCheckRule(unique);
                        DataKeys.Clear();
                        DataKeys.AddRange(dataColum);
                    }

                    CheckField(info, dataColum);
                }
            }
            DataKeys.Clear();
            ClassInfo = null;
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
            if (baseType.EType != TypeType.Class
                && string.IsNullOrWhiteSpace(info.Check))
                return;

            switch (baseType.EType)
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
            bool isOk = AnalyzeCheckRule(info);
            if (isOk) CheckFieldData(info, datas);
        }
        static void CheckClass(FieldInfo info, List<Data> datas)
        {
            //--检查父类
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
            //--检查子类
            if (classType.IsPolyClass)
            {
                var enumerator = classType.GetSubClassEnumerator();
                while (enumerator.MoveNext())
                {
                    var polyType = (KeyValuePair<string, ClassTypeInfo>)enumerator.Current;
                    List<FieldInfo> fields = polyType.Value.Fields;
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
            FieldInfo element = listType.ItemInfo;
            element.Set(Values.ELEMENT, listType.ItemType, info.Check, info.Group);
            for (int i = 0; i < datas.Count; i++)
            {
                DataList dataList = datas[i] as DataList;
                CheckField(element, dataList.Elements);
            }
        }
        static void CheckDict(FieldInfo info, List<Data> datas)
        {
            DictTypeInfo dictInfo = info.BaseInfo as DictTypeInfo;
            string keyCheck, valueCheck;
            keyCheck = valueCheck = "";
            string[] checks = info.Check.Split(Values.ItemSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < checks.Length; i++)
            {
                int index = checks[i].IndexOf(Values.ArgsSplitFlag);
                string checkTarget = checks[i].Substring(0, index).ToLower();
                if (checkTarget == Values.KEY.ToLower())
                    keyCheck = string.Format("{0}{1}{2}", keyCheck, Values.ItemSplitFlag, checks[i].Substring(index + 1));
                else if (checkTarget == Values.VALUE.ToLower())
                    valueCheck = string.Format("{0}{1}{2}", valueCheck, Values.ItemSplitFlag, checks[i].Substring(index + 1));
                else
                {
                    Util.LogWarningFormat("Type:{0} CheckRule:{1} 格式错误", info.Type, info.Check);
                    return;
                }
            }
            FieldInfo keyInfo = dictInfo.KeyInfo;
            keyInfo.Set(Values.KEY, dictInfo.KeyType, keyCheck, info.Group);
            FieldInfo valueInfo = dictInfo.ValueInfo;
            valueInfo.Set(Values.VALUE, dictInfo.ValueType, valueCheck, info.Group);
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


        /// <summary>
        /// 解析检查规则
        /// </summary>
        static bool AnalyzeCheckRule(FieldInfo info)
        {
            string[] checks = info.Check.Split(Values.ItemSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (checks.Length == 0) return false;

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
                    ruleArgs.AddRange(check.Replace(refFlag, "").Split(Values.ArgsSplitFlag.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries));
                }
                else if (check.StartsWith(noEmptyFlag))
                {
                    ruleType = CheckRuleType.NoEmpty;
                    ruleArgs.AddRange(check.Replace(noEmptyFlag, "").Split(Values.ArgsSplitFlag.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries));
                }
                else if (check.StartsWith(uniqueFlag))
                {
                    ruleType = CheckRuleType.Unique;
                    ruleArgs.AddRange(check.Replace(uniqueFlag, "").Split(Values.ArgsSplitFlag.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries));
                }
                else if (check.StartsWith(fileExistFlags))
                {
                    ruleType = CheckRuleType.FileExist;
                    ruleArgs.AddRange(check.Replace(fileExistFlags, "").Split(Values.ArgsSplitFlag.ToCharArray(),
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
                    Util.LogWarningFormat("类:{0} 异常:检查规则{1}不存在", ClassInfo.GetFullName(), check);

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
                if (!string.IsNullOrWhiteSpace(error))
                    Util.LogErrorFormat("Check:{0}.{1} File:{2}\n{3}", ClassInfo.GetFullName(), info.Name, ClassInfo.DataPath, error);
            }
        }
        static string CheckRef(List<string> args, List<Data> datas)
        {
            if (args.Count == 0)
                return "[ref]引用检查规则未填写参数";

            //检查引用主键
            StringBuilder error = new StringBuilder();
            string className = args[0];

            var dataInfoDict = TableInfo.DataInfoDict;
            if (dataInfoDict.ContainsKey(className))
            {
                TableDataInfo table = dataInfoDict[className] as TableDataInfo;
                ClassTypeInfo classType = table.ClassTypeInfo;
                List<Data> keys = table.GetDataColumn(classType.IndexField.Name);
                HashSet<string> hash = new HashSet<string>();
                keys.ForEach(k => hash.Add(k as DataBase));
                for (int i = 0; i < datas.Count; i++)
                {
                    string data = datas[i] as DataBase;
                    if (!hash.Contains(data))
                        error.AppendFormat("[ref]key:{0} {1}.{2}中不包含{3}\n", GetKey(i),
                            classType.GetFullName(), classType.IndexField.Name, data);
                    else
                        hash.Add(data);
                }
            }
            else
                error.AppendFormat("[ref]引用{0}类型不存在", className);
            return error.ToString();
        }
        static string CheckUnique(List<string> args, List<Data> datas)
        {
            StringBuilder error = new StringBuilder();
            HashSet<string> hash = new HashSet<string>();
            for (int i = 0; i < datas.Count; i++)
            {
                string data = datas[i] as DataBase;
                if (hash.Contains(data))
                    error.AppendFormat("[unique]key:{0} value:{1}重复\n", GetKey(i), data);
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

            string dirPath = args[0];
            string ext = args.Count == 2 ? args[1].Replace(".", "") : "";

            for (int i = 0; i < datas.Count; i++)
            {
                string file = (datas[i] as DataBase);
                if (!string.IsNullOrWhiteSpace(ext))
                    file = string.Format("{0}/{1}.{2}", dirPath, file, ext);
                else
                    file = string.Format("{0}/{1}", dirPath, file);
                if (!File.Exists(file))
                    error.AppendFormat("[file]key:{0} value:{1}不存在\n", GetKey(i), file);
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
            double start, end;
            start = double.Parse(args[0].Substring(1));
            end = double.Parse(args[1].Substring(0, rightLenght - 1));
            for (int i = 0; i < datas.Count; i++)
            {
                double v = datas[i] as DataBase;
                bool leftResult = leftOpen ? start < v : start <= v;
                bool rightResult = rightOpen ? v < end : v <= end;
                if (!leftResult || !rightResult)
                    error.AppendFormat("[range]key:{0} value:{1}不在范围内\n", GetKey(i), v);
            }

            return error.ToString();
        }

    }
}
