using Tool.Config;
using Tool.Wrap;
using System;

namespace Tool.Check
{
    public struct RuleInfo
    {
        public string _rule;
        public bool _isKey;
        public bool _isValue;
        public bool IsDict => _isKey || _isValue;
    }
    public abstract class Checker
    {
        protected FieldWrap _define;
        protected RuleInfo[] _ruleTable;
        protected string _rules;
        protected static readonly string KEY = Setting.KEY;
        protected static readonly string VALUE = Setting.VALUE;
        public Checker(FieldWrap define, string rules)
        {
            _define = define;
            _rules = rules;
            string[] nodes = rules.Split(Setting.CheckSplit, System.StringSplitOptions.RemoveEmptyEntries);
            _ruleTable = new RuleInfo[nodes.Length];
            for (int i = 0; i < nodes.Length; i++)
                _ruleTable[i] = new RuleInfo() { _rule = nodes[i] };
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
            for (int i = 0; i < _rules.Length; i++)
                isOk &= CheckIsDictRule(ref _ruleTable[i]);
            return isOk;
        }
        private bool CheckIsDictRule(ref RuleInfo info)
        {
            bool isOk = true;
            string rule = info._rule;
            info._isKey = rule.StartsWith(KEY, StringComparison.OrdinalIgnoreCase);
            info._isValue = rule.StartsWith(VALUE, StringComparison.OrdinalIgnoreCase);
            bool isDict = _define.IsContainer && _define.OriginalType == Setting.DICT;
            if (!isDict && info.IsDict)
            {
                isOk = info._isKey = info._isValue = false;
                Warning("基本规则:非dict类型数据,使用key|value无法正常检查");
            }
            else
            {
                if (info._isKey)
                    rule = rule.Substring(KEY.Length);
                if (info._isValue)
                    rule = rule.Substring(VALUE.Length);
                if (rule.StartsWith(":"))
                    rule = rule.Substring(1);
                info._rule = rule;
            }
            return isOk;
        }
        public abstract bool VerifyData(Data data);
        /// <summary>
        /// 输出不符合规则的字段数据
        /// </summary>
        public abstract void OutputError(Data data);
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
