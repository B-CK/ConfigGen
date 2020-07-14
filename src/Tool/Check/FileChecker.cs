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
            bool isOk = true;
            var define = data.Define;
            for (int i = 0; i < _ruleTable.Length; i++)
            {
                RuleInfo info = _ruleTable[i];
                switch (define.OriginalType)
                {
                    case Setting.STRING:
                        isOk |= Check(data, info);
                        break;
                    case Setting.LIST:
                        // 检查集合中文件路径是否存在
                        var list = (data as FList).Values;
                        for (int k = 0; k < list.Count; k++)
                            isOk |= Check(list[k], info);
                        break;
                    case Setting.DICT:
                        // 检查集合中文件路径是否存在
                        // key|Value均可作该检查
                        var dict = (data as FDict).Values;
                        foreach (var item in dict)
                        {
                            if (info._isKey)
                                isOk |= Check(item.Key, info);
                            else if (info._isValue)
                                isOk |= Check(item.Value, info);
                        }
                        break;
                }
            }
            return isOk;
        }
        private bool Check(Data data, RuleInfo file)
        {
            string relativePath = file._rule.Replace("*", data.ToString());
            Uri uri = new Uri($"{Setting.ApplicationDir}\\{relativePath}");
            string path = uri.AbsolutePath;
            if (File.Exists(path))
                return true;
            return false;
        }

        public override void OutputError(Data data)
        {
            DataError(data, $"File检查规则:{data}文件不存在!\n");
        }
    }
}
