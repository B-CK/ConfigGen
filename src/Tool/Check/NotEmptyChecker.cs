using Tool.Config;
using Tool.Wrap;

namespace Tool.Check
{
    /// <summary>
    /// 禁止填写空白数据\t\b\n\r\f.
    /// </summary>
    public class NotEmptyChecker : Checker
    {
        public NotEmptyChecker(FieldWrap define, string rule) : base(define, rule)
        {
        }
        public override bool VerifyRule()
        {
            bool isOk = base.VerifyRule();
            for (int i = 0; i < _ruleTable.Length; i++)
            {
                var info = _ruleTable[i];
                switch (_define.OriginalType)
                {
                    case Setting.LIST:
                        if (_define.GetItemDefine().FullName != Setting.STRING)
                        {
                            Warning($"NotEmpty检查规则:list中数据类型仅支持string类型");
                            isOk = false;
                        }
                        break;
                    case Setting.DICT:
                        if (info._isKey && _define.GetKeyDefine().FullName != Setting.STRING)
                        {
                            Warning($"NotEmpty检查规则:dict.key数据类型仅支持string类型");
                            isOk = false;
                        }
                        else if (info._isValue && _define.GetValueDefine().FullName != Setting.STRING)
                        {
                            Warning($"NotEmpty检查规则:dict.value数据类型仅支持string类型");
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
                var info = _ruleTable[i];
                switch (define.OriginalType)
                {
                    case Setting.STRING:
                        isOk &= Check(data);
                        break;
                    case Setting.LIST:
                        // 检查集合中字符串是否为空
                        var list = (data as FList).Values;
                        for (int k = 0; k < list.Count; k++)
                            isOk &= Check(list[k]);
                        break;
                    case Setting.DICT:
                        // 检查集合中字符串是否为空
                        // key|Value均可作该检查
                        var dict = (data as FDict).Values;
                        foreach (var item in dict)
                        {
                            if (info._isKey)
                                isOk &= Check(item.Key);
                            else if (info._isValue)
                                isOk &= Check(item.Value);
                        }
                        break;
                    default:
                        break;
                }
            }
           
            return isOk;
        }
        private bool Check(Data data)
        {
            return (data as FString).Value.IsEmpty();
        }

        public override void OutputError(Data data)
        {
            DataError(data, $"NotEmpty检查规则:字符串为空白字符!\n");
        }
    }
}
