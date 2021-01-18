using System;
using System.Collections.Generic;
using Tool.Config;
using Tool.Wrap;

namespace Tool.Check
{
    public class RangeChecker : Checker
    {
        const char LEFT_FULL = '[';
        const char LEFT_HALF = '(';
        const char RIGHT_FULL = ']';
        const char RIGHT_HALF = ')';
        const char POSITIVE = '+';
        const char NEGATIVE = '-';
        static readonly char[] ARRAY = new char[] { LEFT_FULL, LEFT_HALF, RIGHT_FULL, RIGHT_HALF };
        static readonly HashSet<string> HASH = new HashSet<string>() { Setting.INT, Setting.LONG, Setting.FLOAT };

        struct RangeRule
        {
            /// <summary>
            /// ]=true
            /// )=false
            /// </summary>
            public bool _rightState;
            /// <summary>
            /// [=true
            /// (=false
            /// </summary>
            public bool _leftState;
            public float _rightFloat;
            public float _leftFloat;
            public long _rightInt;
            public long _leftInt;
        }

        RangeRule[] _ranges;

        public RangeChecker(FieldWrap define, string rule) : base(define, rule)
        {
        }
        public override bool VerifyRule()
        {
            bool isOk = base.VerifyRule();
            if (_define.IsRaw && !HASH.Contains(_define.OriginalType))
            {
                Warning($"Range检查规则:基础类型数据类型仅支持int,long,float类型!当前类型:{_define.OriginalType}");
                isOk = false;
            }
            _ranges = new RangeRule[_ruleTable.Length];
            for (int i = 0; i < _ruleTable.Length; i++)
            {
                RuleInfo info = _ruleTable[i];
                string rule = info._rule;
                if (rule.IsEmpty())
                {
                    Warning($"Range检查规则:表达式为空");
                    isOk = false;
                }


                if (rule.Length < 5 || rule.IsEmpty())
                {
                    Warning($"Range检查规则:表达式长度必定大于等于5,而当前长度为{rule.Length}!");
                    isOk = false;
                }

                char left = rule[0];
                char right = rule[rule.Length - 1];
                if (!(left == LEFT_FULL || left == LEFT_HALF || left == NEGATIVE)
                   && (right == RIGHT_FULL || right == RIGHT_HALF || right == POSITIVE))
                {
                    Warning("Range检查规则:支持区间标识符错误(),[],(],[)!");
                    isOk = false;
                }

                RangeRule range = new RangeRule();
                switch (right)
                {
                    case RIGHT_FULL: range._rightState = true; break;
                    case RIGHT_HALF: range._rightState = false; break;
                }
                switch (left)
                {
                    case LEFT_FULL: range._leftState = true; break;
                    case LEFT_HALF: range._leftState = false; break;
                }

                string content = rule.Trim(ARRAY);
                string[] nodes = content.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (nodes.Length != 2)
                {
                    Warning("Range检查规则:范围参数漏填!");
                    isOk = false;
                }

                switch (_define.OriginalType)
                {
                    case Setting.INT:
                    case Setting.LONG:
                    case Setting.FLOAT:
                        isOk = PaserLeftRight(nodes, _define.OriginalType, ref range);
                        break;
                    case Setting.LIST:
                        var item = _define.GetItemDefine();
                        if (!HASH.Contains(item.OriginalType))
                        {
                            Warning($"Range检查规则:list中数据类型仅支持int,long,float类型");
                            isOk = false;
                        }
                        else
                            isOk = PaserLeftRight(nodes, item.OriginalType, ref range);
                        break;
                    case Setting.DICT:
                        if (info._isKey)
                        {
                            var key = _define.GetKeyDefine();
                            if (!HASH.Contains(key.FullName))
                            {
                                Warning($"Range检查规则:dict.key数据类型仅支持int,long,float类型");
                                isOk = false;
                            }
                            else
                                isOk = PaserLeftRight(nodes, key.FullName, ref range);
                        }
                        else if (info._isValue)
                        {
                            var value = _define.GetValueDefine();
                            if (!HASH.Contains(value.FullName))
                            {
                                Warning($"Range检查规则:dict.value数据类型仅支持int,long,float类型");
                                isOk = false;
                            }
                            else
                                isOk = PaserLeftRight(nodes, value.FullName, ref range);
                        }
                        break;
                }
                _ranges[i] = range;
            }
            return isOk;
        }
        private bool PaserLeftRight(string[] nodes, string type, ref RangeRule range)
        {
            bool isOk = true;
            string leftv = nodes[0].Trim();
            string rightv = nodes[1].Trim();
            switch (type)
            {
                case Setting.INT:
                case Setting.LONG:
                    if (leftv.Length == 1 && leftv[0] == NEGATIVE)
                        range._leftInt = _define.OriginalType == Setting.INT ? int.MinValue : long.MinValue;
                    else if (!long.TryParse(leftv, out range._leftInt))
                        isOk = false;

                    if (rightv.Length == 1 && rightv[0] == POSITIVE)
                        range._rightInt = _define.OriginalType == Setting.INT ? int.MaxValue : long.MaxValue;
                    else if (!long.TryParse(rightv, out range._rightInt))
                        isOk = false;
                    break;
                case Setting.FLOAT:
                    if (leftv.Length == 1 && leftv[0] == NEGATIVE)
                        range._leftFloat = float.MinValue;
                    else if (!float.TryParse(leftv, out range._leftFloat))
                        isOk = false;

                    if (rightv.Length == 1 && rightv[0] == POSITIVE)
                        range._rightFloat = float.MaxValue;
                    else if (!float.TryParse(rightv, out range._rightFloat))
                        isOk = false;
                    break;
                default:
                    Warning($"Range检查规则:仅支持int,long,float类型!当前类型:{type}.");
                    isOk = false;
                    break;
            }

            if (type == Setting.INT || type == Setting.LONG)
            {
                if (range._leftInt >= range._rightInt)
                {
                    Warning($"Range检查规则:左值必须小于右值!规则:{_rules}.");
                    isOk = false;
                }
            }
            else if (type == Setting.FLOAT)
            {
                if (range._leftFloat >= range._rightFloat)
                {
                    Warning($"Range检查规则:左值必须小于右值!规则:{_rules}.");
                    isOk = false;
                }
            }
            return isOk;
        }

        public override bool VerifyData(Data data)
        {
            bool isOk = false;

            if (_define.OriginalType == Setting.DICT)
                isOk |= Check(data as FDict);
            else if (_define.OriginalType == Setting.LIST)
                isOk |= Check(data as FList);
            else
            {
                for (int i = 0; i < _ranges.Length; i++)
                {
                    var info = _ruleTable[i];
                    var range = _ranges[i];
                    switch (_define.OriginalType)
                    {
                        case Setting.INT:
                            isOk |= Check((data as FInt).Value, range);
                            break;
                        case Setting.LONG:
                            isOk |= Check((data as FLong).Value, range);
                            break;
                        case Setting.FLOAT:
                            isOk |= Check((data as FFloat).Value, range);
                            break;
                    }
                }
            }
            return isOk;
        }
        public override void OutputError(Data data)
        {
            DataError(data, $"Range检查规则:{data}数据超出{_rules}范围!\n");
        }
        private bool Check(float data, RangeRule range)
        {
            bool isOk = false;
            isOk |= ((range._leftState && data >= range._leftFloat || !range._leftState && data > range._leftFloat)
            && (range._rightState && data <= range._rightFloat || !range._rightState && data < range._rightFloat));
            return isOk;
        }
        private bool Check(long data, RangeRule range)
        {
            bool isOk = false;
            isOk |= ((range._leftState && data >= range._leftInt || !range._leftState && data > range._leftInt)
            && (range._rightState && data <= range._rightInt || !range._rightState && data < range._rightInt));
            return isOk;
        }
        private bool Check(FList data)
        {
            bool isOk = true;
            var list = data.Values;
            for (int i = 0; i < list.Count; i++)
            {
                bool flag = false;
                for (int k = 0; k < _ranges.Length; k++)
                {
                    var info = _ruleTable[k];
                    var range = _ranges[k];
                    flag |= SwitchData(list[i], _define.GetItemDefine().OriginalType, range, info);
                }
                isOk &= flag;
            }
            return isOk;
        }
        private bool Check(FDict data)
        {
            bool isOk = true;
            var dict = data.Values;
            foreach (var item in dict)
            {
                bool flagk = false;
                bool flagv = false;
                bool hasKey = false;
                bool hasValue = false;
                for (int i = 0; i < _ranges.Length; i++)
                {
                    var info = _ruleTable[i];
                    var range = _ranges[i];
                    if (info._isKey)
                    {
                        flagk |= SwitchData(item.Key, item.Key.Define.OriginalType, range, info);
                        hasKey |= true;
                    }
                    else if (info._isValue)
                    {
                        flagv |= SwitchData(item.Value, item.Value.Define.OriginalType, range, info);
                        hasValue |= true;
                    }
                }
                if (hasKey && hasValue)
                    isOk &= flagk && flagv;
                else if (hasKey && !hasValue)
                    isOk &= flagk;
                else if (!hasKey && hasValue)
                    isOk &= flagv;
                else
                    Error("Range检查规则:程序Bug,未检查出规则配置错误!");
            }
            return isOk;
        }
        private bool SwitchData(Data data, string type, RangeRule range, RuleInfo info)
        {
            bool isOk = true;
            switch (type)
            {
                case Setting.INT:
                    isOk &= Check((data as FInt).Value, range);
                    break;
                case Setting.LONG:
                    isOk &= Check((data as FLong).Value, range);
                    break;
                case Setting.FLOAT:
                    isOk &= Check((data as FFloat).Value, range);
                    break;
            }
            return isOk;
        }
    }
}
