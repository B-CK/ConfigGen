using System.Collections.Generic;
using Tool.Config;
using Tool.Wrap;

namespace Tool.Check
{
    /// <summary>
    /// 检查涉及表结构,多态性字段均不支持.
    /// 检查数据存在性.enum默认已检查.及当前表中填写的数据必须在引用表中指定字段存在该数据
    /// </summary>
    public class RefChecker : Checker
    {
        private ConfigWrap _target;
        private string _targetField;
        private HashSet<Data> _hash;
        public RefChecker(FieldWrap define, string rule) : base(define, rule)
        {
        }

        public override bool VerifyRule()
        {
            bool isOk = base.VerifyRule();
            string rule = GetRuleNoModify();
            if (rule.IsEmpty())
            {
                Warning($"Ref检查规则:表达式为空");
                isOk = false;
            }
            else
            {
                string[] nodes = rule.Split(Setting.DotSplit, System.StringSplitOptions.RemoveEmptyEntries);
                if (nodes.Length == 3)
                {
                    string targetName = $"{nodes[0]}.{nodes[1]}";
                    if (!ConfigWrap.IsConfig(targetName))
                    {
                        Warning($"Ref检查规则:目标{targetName}配置表无法获取!");
                        isOk = false;
                    }
                    _target = ConfigWrap.Get(targetName);
                    _targetField = nodes[2];
                    if (_target == null)
                    {
                        Warning("Ref检查规则:仅支持引用表结构最外层基础类型字段数据");
                        isOk = false;
                    }
                    else
                    {
                        var targetCls = ClassWrap.Get(targetName);
                        var define = targetCls.Fields.Find(field => field.Name == _targetField);
                        if (define.IsContainer)
                        {
                            if (define.OriginalType == Setting.LIST
                                && define.GetItemDefine().IsClass)
                            {
                                Warning($"Ref检查规则:由于多态的复杂性,不支持list中数据类型为Class!仅支持基础类型.");
                                isOk = false;
                            }
                            else if (define.OriginalType == Setting.DICT
                                && define.GetValueDefine().IsClass)
                            {
                                Warning($"Ref检查规则:由于多态的复杂性,不支持dict.value数据类型为Class!仅支持基础类型.");
                                isOk = false;
                            }
                        }
                        else if (define.IsEnum || define.IsRaw)
                        {

                        }
                        else
                        {
                            Warning($"Ref检查规则:由于多态的复杂性,不支持数据类型为Class!仅支持基础类型.");
                            isOk = false;
                        }
                    }
                }
                else
                {
                    Warning($"Ref检查规则:格式错误,正确格式[Namespace.Class.Field]");
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
                case Setting.INT:
                case Setting.LONG:
                case Setting.FLOAT:
                case Setting.STRING:
                    isOk = Check(data);
                    break;
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
            //引用表字段数据列表
            var targetCls = ClassWrap.Get(_target.FullName);
            int index = targetCls.Fields.FindIndex(field => field.Name == _targetField);
            _hash = new HashSet<Data>(_target.Data.Values.MapTo<Data, Data>((d) =>
            {
                var line = d as FClass;
                return line.Values[index];
            }));
            return true;
        }

        private bool Check(Data data)
        {
            return _hash.Contains(data);
        }
        /// <summary>
        /// 检查列表中数据是否在引用表中存在
        /// </summary>
        private bool Check(FList data)
        {
            bool isOk = true;
            var list = data.Values;
            for (int i = 0; i < list.Count; i++)
            {
                isOk &= Check(list[i]);
            }
            return isOk;
        }
        private bool Check(FDict data)
        {
            bool isOk = true;
            var dict = data.Values;
            foreach (var item in dict)
            {
                if (_isKey)
                    isOk &= Check(item.Key);
                if (_isValue)
                    isOk &= Check(item.Value);
            }
            return isOk;
        }

        public override void OutputError()
        {
            Error($"Ref检查规则:当前数据在引用字段数据列不存在!\n最后一条数据:\n{Program.LastData.ExportData()}\n");
        }    
    }
}
