using System;
using System.Collections.Generic;
using System.IO;
using Tool.Config;
using Tool.Wrap;

namespace Tool.Check
{
    /// <summary>
    /// 支持多条检查规则,以或的方式检查
    /// </summary>
    public class FileChecker : Checker
    {
        public FileChecker(FieldWrap define, string rule) : base(define, rule)
        {
        }

        public override bool VerifyRule()
        {
            bool isOk = base.VerifyRule();
            if (_rules.IsEmpty() || _ruleTable.Length == 0)
            {
                Warning($"File检查规则:未填写内容!");
                isOk = false;
            }
            if (_define.IsRaw && _define.OriginalType != Setting.STRING)
            {
                Warning($"File检查规则:基础类型数据类型仅支持string类型");
                isOk = false;
            }
            for (int i = 0; i < _ruleTable.Length; i++)
            {
                RuleInfo info = _ruleTable[i];
                string rule = info._rule;
                if (rule.IsEmpty())
                {
                    Warning($"File检查规则:表达式为空");
                    isOk = false;
                }
                switch (_define.OriginalType)
                {
                    case Setting.LIST:
                        if (_define.GetItemDefine().FullName != Setting.STRING)
                        {
                            Warning($"File检查规则:list中数据类型仅支持string类型");
                            isOk = false;
                        }
                        break;
                    case Setting.DICT:
                        if (info._isKey && _define.GetKeyDefine().FullName != Setting.STRING)
                        {
                            Warning($"File检查规则:dict.key数据类型仅支持string类型");
                            isOk = false;
                        }
                        else if (info._isValue && _define.GetValueDefine().FullName != Setting.STRING)
                        {
                            Warning($"File检查规则:dict.value数据类型仅支持string类型");
                            isOk = false;
                        }
                        break;
                }
            }
            return isOk;
        }
        public override bool VerifyData(Data data)
        {
            bool isOk = false;
            var define = data.Define;
            if (define.OriginalType == Setting.DICT)
                isOk |= Check(data as FDict);
            else if (define.OriginalType == Setting.LIST)
                isOk |= Check(data as FList);
            else
            {
                for (int i = 0; i < _ruleTable.Length; i++)
                {
                    RuleInfo info = _ruleTable[i];
                    isOk |= Check(data, info);
                }
            }
            return isOk;
        }

        private bool Check(Data data, RuleInfo file)
        {
            string relativePath = file._rule.Replace("*", data.ToString());
            Uri uri = new Uri($"{Setting.ApplicationDir}\\{relativePath}");
            string path = uri.LocalPath;
            if (File.Exists(path))
                return true;
            return false;
        }
        // 检查集合中文件路径是否存在
        private bool Check(FList data)
        {
            bool isOk = true;
            var list = (data as FList).Values;
            for (int k = 0; k < list.Count; k++)
            {
                bool flag = false;
                for (int i = 0; i < _ruleTable.Length; i++)
                {
                    RuleInfo info = _ruleTable[i];
                    flag |= Check(list[k], info);
                }
                isOk &= flag;
            }
            return isOk;
        }
        // 检查集合中文件路径是否存在
        // key|Value均可作该检查
        private bool Check(FDict data)
        {
            bool isOk = true;
            var dict = (data as FDict).Values;
            foreach (var item in dict)
            {
                bool flagk = false;
                bool flagv = false;
                bool hasKey = false;
                bool hasValue = false;
                for (int i = 0; i < _ruleTable.Length; i++)
                {
                    RuleInfo info = _ruleTable[i];
                    if (info._isKey)
                    {
                        flagk |= Check(item.Key, info);
                        hasKey |= true;
                    }
                    else if (info._isValue)
                    {
                        flagv |= Check(item.Value, info);
                        hasValue |= true;
                    }
                }
                if (hasKey && hasValue)
                    isOk &= flagk && flagv;
                else if (hasKey && !hasValue)
                    isOk &= flagk;
                else if (!hasKey && hasValue)
                    isOk &= flagv;
                else
                    Error("File检查规则:程序Bug,未检查出规则配置错误!");
            }
            return isOk;
        }

        public override void OutputError(Data data)
        {
            DataError(data, $"File检查规则:{data}文件不存在!\n");
        }
    }
}
