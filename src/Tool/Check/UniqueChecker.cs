using System.Collections.Generic;
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
        public UniqueChecker(FieldWrap define) : base(define)
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
                else if (_define.OriginalType == Setting.DICT
                    && _define.GetValueDefine().IsClass)
                {
                    Warning($"Unique检查规则:由于多态的复杂性,不支持dict.value数据类型为Class!仅支持基础类型.");
                    isOk = false;
                }
            }
            return isOk;
        }
        public override bool VerifyData(Data data)
        {
            bool isOk = true;
            var define = data.Define;
            switch (define.OriginalType)
            {
                case Setting.LIST:
                    isOk = Check(data as FList);
                    break;
                case Setting.DICT:
                    isOk = Check(data as FDict);
                    break;
                default:
                    Error("程序BUG:基础/Enum类型仅做一次整表检查!");
                    isOk = false;
                    break;
            }
            return isOk;
        }
        
        public override bool CheckColumn()
        {
            base.CheckColumn();
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
            var hash = new HashSet<Data>(data.Values.Values);
            return hash.Count == data.Values.Count;
        }


    }
}
