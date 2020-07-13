using Tool.Config;
using Tool.Wrap;
using System;

namespace Tool.Check
{
    public abstract class Checker
    {
        protected FieldWrap _define;
        protected string _rule;
        protected bool IsDict => _isKey || _isValue;
        protected bool _isKey;
        protected bool _isValue;
        protected static readonly string KEY = Setting.KEY;
        protected static readonly string VALUE = Setting.VALUE;
        public Checker(FieldWrap define, string rule)
        {
            _define = define;
            _rule = rule;
        }
        /// <summary>
        /// 部分检查规则需要整列数据
        /// </summary>
        public virtual bool CheckColumn()
        {
            _define.Checkers.Remove(this);
            return true;
        }
        public virtual bool VerifyRule()
        {
            if (_define.IsClass)
            {
                Warning("基本规则:不支持Class类型检查,但支持检查Class类型中定义的字段.");
                return false;
            }

            bool isOk = true;
            if (!_rule.IsEmpty())
            {
                _isKey = _rule.StartsWith(KEY, StringComparison.OrdinalIgnoreCase);
                _isValue = _rule.StartsWith(VALUE, StringComparison.OrdinalIgnoreCase);
                bool isDict = _define.IsContainer && _define.OriginalType == Setting.DICT;
                if (!isDict && IsDict)
                {
                    isOk = _isKey = _isValue = false;
                    Warning("基本规则:非dict类型数据,使用key|value无法正常检查");
                }
            }
            return isOk;
        }
        public abstract bool VerifyData(Data data);
        /// <summary>
        /// 输出不符合规则的字段数据
        /// </summary>
        public abstract void OutputError(Data data);

        /// <summary>
        /// 移除dict类修饰符
        /// </summary>
        /// <returns></returns>
        protected string GetRuleNoModify()
        {
            string rule = _rule;
            if (_isKey)
                rule = rule.Substring(KEY.Length);
            if (_isValue)
                rule = rule.Substring(VALUE.Length);
            if (rule.StartsWith(":"))
                rule = rule.Substring(1);
            return rule;
        }
        protected void Warning(string msg)
        {
            Util.LogWarning($"{_define.Host.FullName}Field:{_define.Name}({_define.FullName}) {msg}");
        }
        protected void Error(string msg)
        {
            Util.LogError($"{_define.Host.FullName} Field:{_define.Name}({_define.FullName}) {msg}");
        }
        protected void DataError(Data data, string error)
        {
            FClass root = data.Host;
            while (!root.IsRoot)
                root = root.Host;
            string fullName = root.Define.FullName;
            var config = ConfigWrap.Get(fullName);
            var cls = ClassWrap.Get(fullName);
            int index = cls.Fields.IndexOf(config.Index);
            Util.LogError($"【{root.Values[index]}】{_define.Host.FullName} Field:{_define.Name}({_define.FullName}) {error}");
        }
    }
}
