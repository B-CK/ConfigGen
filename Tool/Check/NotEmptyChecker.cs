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
        public override void OutputError(Data data)
        {
            throw new System.NotImplementedException("暂时不支持该功能");
        }
        public override bool VerifyData(Data data)
        {
            throw new System.NotImplementedException("暂时不支持该功能");
        }
        public override bool VerifyRule()
        {
            throw new System.NotImplementedException("暂时不支持该功能");
        }

        //public override bool VerifyRule()
        //{
        //    bool isOk = base.VerifyRule();
        //    switch (_define.OriginalType)
        //    {
        //        case Setting.STRING:
        //            break;
        //        case Setting.LIST:
        //            if (_define.GetItemDefine().FullName != Setting.STRING)
        //            {
        //                Warning($"NotEmpty检查规则:list中数据类型仅支持string类型");
        //                isOk = false;
        //            }
        //            break;
        //        case Setting.DICT:
        //            for (int i = 0; i < _ruleTable.Length; i++)
        //            {
        //                var info = _ruleTable[i];
        //                if (info._isKey && _define.GetKeyDefine().FullName != Setting.STRING)
        //                {
        //                    Warning($"NotEmpty检查规则:dict.key数据类型仅支持string类型");
        //                    isOk = false;
        //                }
        //                else if (info._isValue && _define.GetValueDefine().FullName != Setting.STRING)
        //                {
        //                    Warning($"NotEmpty检查规则:dict.value数据类型仅支持string类型");
        //                    isOk = false;
        //                }
        //            }
        //            break;
        //        default:
        //            Warning($"NotEmpty检查规则:基础类型数据类型仅支持string类型");
        //            isOk = false;
        //            break;
        //    }

        //    return isOk;
        //}
        //public override bool VerifyData(Data data)
        //{
        //    bool isOk = false;
        //    var define = data.Define;
        //    if (define.OriginalType == Setting.DICT)
        //        isOk |= Check(data as FDict);
        //    else if (define.OriginalType == Setting.LIST)
        //        isOk |= Check(data as FList);
        //    else
        //        isOk |= Check(data);

        //    return isOk;
        //}
        //private bool Check(Data data)
        //{
        //    return !(data as FString).Value.IsEmpty();
        //}
        //private bool Check(FList data)
        //{
        //    bool isOk = true;
        //    var list = (data as FList).Values;
        //    for (int k = 0; k < list.Count; k++)
        //        isOk &= Check(list[k]);
        //    return isOk;
        //}
        //private bool Check(FDict data)
        //{
        //    bool isOk = true;
        //    var dict = (data as FDict).Values;
        //    foreach (var item in dict)
        //    {
        //        bool flagk = false;
        //        bool flagv = false;
        //        bool hasKey = false;
        //        bool hasValue = false;
        //        for (int i = 0; i < _ruleTable.Length; i++)
        //        {
        //            RuleInfo info = _ruleTable[i];
        //            if (info._isKey)
        //            {
        //                flagk |= Check(item.Key);
        //                hasKey |= true;
        //            }
        //            else if (info._isValue)
        //            {
        //                flagv |= Check(item.Value);
        //                hasValue |= true;
        //            }
        //        }
        //        if (hasKey && hasValue)
        //            isOk &= flagk && flagv;
        //        else if (hasKey && !hasValue)
        //            isOk &= flagk;
        //        else if (!hasKey && hasValue)
        //            isOk &= flagv;
        //        else
        //            Error("NotEmpty检查规则:程序Bug,未检查出规则配置错误!");
        //    }
        //    return isOk;
        //}
        //public override void OutputError(Data data)
        //{
        //    DataError(data, $"NotEmpty检查规则:字符串为空白字符!\n");
        //}
    }
}
