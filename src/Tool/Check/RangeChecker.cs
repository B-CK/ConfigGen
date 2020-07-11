﻿using System.Collections.Generic;
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
        static readonly char[] array = new char[] { LEFT_FULL, LEFT_HALF, RIGHT_FULL, RIGHT_HALF, POSITIVE, NEGATIVE };
        static readonly HashSet<string> hash = new HashSet<string>() { Setting.INT, Setting.LONG, Setting.FLOAT };


        /// <summary>
        /// ]=true
        /// )=false
        /// </summary>
        private bool _rightState;
        /// <summary>
        /// [=true
        /// (=false
        /// </summary>
        private bool _leftState;
        private float _rightFloat;
        private float _leftFloat;
        private long _rightInt;
        private long _leftInt;

        public RangeChecker(FieldWrap define) : base(define)
        {
        }
        public override bool VerifyRule()
        {
            bool isOk = base.VerifyRule();
            string rule = GetRuleNoModify();
            if (rule.IsEmpty())
            {
                Warning($"Range检查规则:表达式为空");
                isOk = false;
            }
            if (_isKey && !hash.Contains(_define.GetKeyDefine().FullName))
            {
                Warning($"Range检查规则:dict.key数据类型仅支持int,long,float类型");
                isOk = false;
            }
            if (_isValue && !hash.Contains(_define.GetValueDefine().FullName))
            {
                Warning($"Range检查规则:dict.value数据类型仅支持int,long,float类型");
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

            switch (right)
            {
                case RIGHT_FULL: _rightState = true; break;
                case RIGHT_HALF: _rightState = false; break;
            }
            switch (left)
            {
                case LEFT_FULL: _leftState = true; break;
                case LEFT_HALF: _leftState = false; break;
            }

            string content = rule.Trim(array);
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
                    if (nodes[0].Length == 1 && nodes[0] == NEGATIVE.ToString())
                        _leftInt = _define.OriginalType == Setting.INT ? int.MinValue : long.MinValue;
                    else if (!long.TryParse(nodes[0], out _leftInt))
                        isOk = false;

                    if (nodes[1].Length == 1 && nodes[1] == POSITIVE.ToString())
                        _rightInt = _define.OriginalType == Setting.INT ? int.MaxValue : long.MaxValue;
                    else if (!long.TryParse(nodes[1], out _rightInt))
                        isOk = false;
                    break;
                case Setting.FLOAT:
                    if (nodes[0].Length == 1 && nodes[0] == NEGATIVE.ToString())
                        _leftFloat = float.MinValue;
                    else if (!float.TryParse(nodes[0], out _leftFloat))
                        isOk = false;

                    if (nodes[1].Length == 1 && nodes[1] == POSITIVE.ToString())
                        _rightFloat = float.MaxValue;
                    else if (!float.TryParse(nodes[1], out _rightFloat))
                        isOk = false;
                    break;
                default:
                    Warning("Range检查规则:仅支持int,long,float类型!");
                    isOk = false;
                    break;
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
                    isOk &= Check((data as FInt).Value);
                    break;
                case Setting.LONG:
                    isOk &= Check((data as FLong).Value);
                    break;
                case Setting.FLOAT:
                    isOk &= Check((data as FFloat).Value);
                    break;
                case Setting.LIST:
                    isOk &= Check(data as FList);
                    break;
                case Setting.DICT:
                    isOk &= Check(data as FDict);
                    break;
            }
            return isOk;
        }

        private bool Check(float data)
        {
            if (_leftState && data >= _leftFloat || !_leftState && data > _leftFloat)
                return true;

            if (_rightState && data <= _rightFloat || !_rightState && data < _rightFloat)
                return true;

            return false;
        }
        private bool Check(long data)
        {
            if (_leftState && data >= _leftInt || !_leftState && data > _leftInt)
                return true;

            if (_rightState && data <= _rightInt || !_rightState && data < _rightInt)
                return true;

            return false;
        }
        private bool Check(FList data)
        {
            bool isOk = true;
            var list = data.Values;
            for (int i = 0; i < list.Count; i++)
                isOk &= SwitchData(list[i], _define.GetItemDefine().OriginalType);
            return isOk;
        }
        private bool Check(FDict data)
        {
            bool isOk = true;
            var dict = data.Values;
            foreach (var item in dict)
            {
                if (_isKey)
                    isOk &= SwitchData(item.Key, item.Key.Define.OriginalType);
                if (_isValue)
                    isOk &= SwitchData(item.Value, item.Value.Define.OriginalType);
            }
            return isOk;
        }
        private bool SwitchData(Data data, string type)
        {
            bool isOk = true;
            switch (type)
            {
                case Setting.INT:
                    var dint = data as FInt;
                    isOk &= Check(dint.Value);
                    break;
                case Setting.LONG:
                    var dlong = data as FLong;
                    isOk &= Check(dlong.Value);
                    break;
                case Setting.FLOAT:
                    var dfloat = data as FFloat;
                    isOk &= Check(dfloat.Value);
                    break;
            }
            return isOk;
        }
    }
}