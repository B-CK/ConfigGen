using System;
using System.Collections.Generic;
using System.IO;
using Tool.Config;
using Tool.Wrap;

namespace Tool.Check
{
    /// <summary>
    /// 支持多条检查规则
    /// </summary>
    public class FileChecker : Checker
    {
        /// <summary>
        /// 目录路径相对于工具调用环境
        /// </summary>
        private string _dir;
        private string _ext;

        public FileChecker(FieldWrap define, string rule) : base(define, rule)
        {
        }

        public override bool VerifyRule()
        {
            bool isOk = base.VerifyRule();
            string rule = GetRuleNoModify();
            if (rule.IsEmpty())
            {
                Warning($"File检查规则:表达式为空");
                isOk = false;
            }
            if (_isKey && _define.GetKeyDefine().FullName != Setting.STRING)
            {
                Warning($"File检查规则:dict.key数据类型仅支持string类型");
                isOk = false;
            }
            if (_isValue && _define.GetValueDefine().FullName != Setting.STRING)
            {
                Warning($"File检查规则:dict.value数据类型仅支持string类型");
                isOk = false;
            }
            string[] nodes = rule.Split(Setting.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
            Uri uri = new Uri($"{Setting.ApplicationDir}\\{nodes[0]}");
            _dir = uri.AbsolutePath;
            if (nodes.Length >= 2)
                _ext = nodes[1];
            if (!Directory.Exists(_dir))
            {
                Warning($"File检查规则:{_dir}目录不存在");
                isOk = false;
            }

            return isOk;
        }
        public override bool VerifyData(Data data)
        {
            bool isOk = true;
            var define = data.Define;
            switch (define.OriginalType)
            {
                case Setting.STRING:
                    isOk &= Check(data);
                    break;
                case Setting.LIST:
                    // 检查集合中文件路径是否存在
                    var list = (data as FList).Values;
                    for (int i = 0; i < list.Count; i++)
                        isOk &= Check(list[i]);
                    break;
                case Setting.DICT:
                    // 检查集合中文件路径是否存在
                    // key|Value均可作该检查
                    var dict = (data as FDict).Values;
                    foreach (var item in dict)
                    {
                        if (_isKey)
                            isOk &= Check(item.Key);
                        if (_isValue)
                            isOk &= Check(item.Value);
                    }
                    break;
            }
            return isOk;
        }
        private bool Check(Data data)
        {
            string relPath = (data as FString).Value;
            string path = $"{_dir}{relPath}";
            if (!_ext.IsEmpty())
            {
                _ext.TrimStart('.');
                path = $"{path}.{_ext}";
            }
            if (File.Exists(path))
                return true;

            Warning($"File检查规则:{path} 文件不存在");
            return false;
        }

        public override void OutputError()
        {
            Error($"File检查规则:文件不存在!\n最后一条数据:\n{Program.LastData.ExportData()}\n");
        }
    }
}
