using Tool.Config;
using Tool.Wrap;

namespace Tool.Check
{
    /// <summary>
    /// 禁止填写空白数据\t\b\n\r\f.
    /// </summary>
    public class NotEmptyChecker : Checker
    {
        public NotEmptyChecker(FieldWrap define) : base(define)
        {
        }
        public override bool VerifyRule()
        {
            bool isOk = base.VerifyRule();
            if (_isKey && _define.GetKeyDefine().FullName != Setting.STRING)
            {
                Warning($"notEmpty检查规则:dict.key数据类型仅支持string类型");
                isOk = false;
            }
            if (_isValue && _define.GetValueDefine().FullName != Setting.STRING)
            {
                Warning($"notEmpty检查规则:dict.value数据类型仅支持string类型");
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
                    // 检查集合中字符串是否为空
                    var list = (data as FList).Values;
                    for (int i = 0; i < list.Count; i++)
                        isOk &= Check(list[i]);
                    break;
                case Setting.DICT:
                    // 检查集合中字符串是否为空
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
                default:
                    break;
            }
            return isOk;
        }
        private bool Check(Data data)
        {
            return (data as FString).Value.IsEmpty();
        }
    }
}
