﻿using System.Collections.Generic;
using Tool.Config;
using Tool.Wrap;

namespace Tool.Check
{
    /// <summary>
    /// 检查涉及表结构,多态性字段均不支持.
    /// 检查数据唯一性,仅支持最外层基础类型字段检查以及集合中数据检查.
    /// 注:string类型中可能出现不同的空字符串[\t\b\n\r\f]等,均按不同内容处理.表格主键默认带有该功能
    /// 由于多态过于复杂,会导致表结构不整齐,各条数据之间结构不一致,难以解析数据.所以不支持Class类型
    /// </summary>
    public class UniqueChecker : Checker
    {
        //_ruleTable.Length == 0表示不需要检查dict.value
        //dict检查需要在规则中添加value
        public UniqueChecker(FieldWrap define, string rule) : base(define, rule)
        {
        }

        public override bool VerifyRule()
        {
            bool isOk = base.VerifyRule();
            if (_define.IsRaw || _define.IsEnum)
            {//.是一个最外层的基础类型字段,检查整列数据.
                if (!_define.Host.IsConfig())
                {
                    Warning($"Unique检查规则:仅支持目标表结构最外层基础类型字段数据!");
                    isOk = false;
                }
            }
            else if (_define.IsContainer)
            { //.是一个集合结构,直接检查结构内部数据
                if (_define.OriginalType == Setting.LIST
                    && _define.GetItemDefine().IsClass)
                {
                    Warning($"Unique检查规则:由于多态的复杂性,不支持list中数据类型为Class!仅支持基础类型.");
                    isOk = false;
                }
                else if (_define.OriginalType == Setting.DICT && _ruleTable.Length != 0
                    && _define.GetValueDefine().IsClass)//_ruleTable.Length != 0 表示需要检查dict.value
                {
                    for (int i = 0; i < _ruleTable.Length; i++)
                    {
                        RuleInfo info = _ruleTable[i];
                        if (info._isValue)
                            Warning($"Unique检查规则:由于多态的复杂性,不支持dict.value数据类型为Class!仅支持基础类型.");
                    }
                    isOk = false;
                }
            }
            return isOk;
        }
        public override bool VerifyData(Data data)
        {
            bool isOk = false;
            var define = data.Define;
            switch (define.OriginalType)
            {
                case Setting.LIST:
                    isOk |= Check(data as FList);
                    break;
                case Setting.DICT:
                    isOk |= Check(data as FDict);
                    break;
                default:
                    Error("程序BUG:基础/Enum类型仅做一次整表检查!");
                    isOk = false;
                    break;
            }
            return isOk;
        }
        public override void OutputError(Data data)
        {
            DataError(data, $"Unique检查规则:{data}数据不唯一!\n");
        }

        public override bool CheckColumn(bool remove)
        {
            if (_define.IsRaw)
            {
                base.CheckColumn(true);
                var config = ConfigWrap.Get(_define.Host.FullName);
                var cls = _define.Host;
                int index = cls.Fields.IndexOf(_define);
                var hash = new HashSet<Data>(config.Data.Values.MapTo<Data, Data>((data) =>
                {
                    FClass fcls = data as FClass;
                    data = fcls.Values[index];
                    return data;
                }));

                bool isOk = hash.Count == config.Data.Values.Count;
                if (!isOk)
                    Error($"Unique检查规则:数据重复!");
                return isOk;
            }
            return true;
        }
        /// <summary>
        /// 集合内部数据是否唯一
        /// </summary>
        private bool Check(FList data)
        {
            var hash = new HashSet<Data>(data.Values);
            return hash.Count == data.Values.Count;
        }
        /// <summary>
        /// 集合内部数据是否唯一
        /// key默认已检查,该功能仅检查value
        /// </summary>
        private bool Check(FDict data)
        {
            var hashValue = new HashSet<Data>(data.Values.Values);
            return _ruleTable.Length == 0 || hashValue.Count == data.Values.Count;
        }
    }
}
