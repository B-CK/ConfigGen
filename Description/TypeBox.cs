﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Description.Wrap;

namespace Description
{
    public partial class TypeBox : UserControl
    {
        public enum ProertyType
        {
            None,
            Bool,
            Int,
            Float,
            Enum,
            String,
            List,
            Dict,
        }

        ProertyType _propertyType = ProertyType.None;
        public Action OnCheckChange;

        public void InitBool(bool value = false)
        {
            _propertyType = ProertyType.Bool;
            _boolBox.Checked = value;
            EnableBox(_propertyType);
        }
        public void InitInt(int value = 0, int size = 32)
        {
            _propertyType = ProertyType.Int;
            _numericUpDown.Increment = 1;
            _numericUpDown.Maximum = int.MaxValue;
            _numericUpDown.Minimum = int.MinValue;
            _numericUpDown.Value = value;
            EnableBox(_propertyType);
            switch (size)
            {
                case 16:
                    _numericUpDown.Maximum = short.MaxValue;
                    break;
                case 32:
                    _numericUpDown.Maximum = int.MaxValue;
                    break;
                case 64:
                    _numericUpDown.Maximum = long.MaxValue;
                    break;
                default:
                    ConsoleDock.Ins.LogErrorFormat("不存在Int{0}大小!", size);
                    break;
            }
        }
        public void InitFloat(float value = 0)
        {
            _propertyType = ProertyType.Float;
            _numericUpDown.Increment = (decimal)0.1f;
            _numericUpDown.Value = (decimal)value;
            _numericUpDown.Maximum = decimal.MaxValue;
            _numericUpDown.Minimum = decimal.MinValue;
            EnableBox(_propertyType);
        }
        public void InitEnum(string value, EnumItemWrap[] items)
        {
            _propertyType = ProertyType.Enum;
            EnableBox(_propertyType);
            _comboBox.Text = value;
            _comboBox.Items.AddRange(items);
        }
        public void InitString(string value = "")
        {
            _propertyType = ProertyType.String;
            EnableBox(_propertyType);
            _stringBox.Text = value;
        }
        public void InitList(object[] items)
        {
            _propertyType = ProertyType.List;
            EnableBox(_propertyType);
            _comboBox.Items.AddRange(items);
        }
        /// <summary>
        /// 键值对形式
        /// </summary>
        /// <param name="keys">仅支持少部分基础类型</param>
        /// <param name="values"></param>
        public void InitDict(object[] keys, object[] values)
        {
            _propertyType = ProertyType.Dict;
            EnableBox(_propertyType);
            _keyComboBox.Items.AddRange(keys);
            _valueComboBox.Items.AddRange(values);
        }

        public string GetSetType()
        {
            if (_propertyType == ProertyType.List)
                return _comboBox.Text;
            else if (_propertyType == ProertyType.Dict)
                return Util.Format("{0}{1}{2}", _keyComboBox.Text, Util.ArgsSplitFlag[0], _valueComboBox.Text);
            else
            {
                Util.MsgError("字段类型({0})不匹配,错误调用泛型类型", _propertyType);
                return "?";
            }
        }
        public string GetValue()
        {
            switch (_propertyType)
            {
                case ProertyType.Bool:
                    return _boolBox.Checked.ToString();
                case ProertyType.Int:
                    return _numericUpDown.Value.ToString();
                case ProertyType.Float:
                    return _numericUpDown.Value.ToString();
                case ProertyType.Enum:
                    return _comboBox.Text;
                case ProertyType.String:
                    return _stringBox.Text;
                case ProertyType.None:
                default:
                    return "";
            }
        }

        public void EnableBox(ProertyType type)
        {
            _boolBox.Visible = false;
            _comboBox.Visible = false;
            _numericUpDown.Visible = false;
            _stringBox.Visible = false;
            _keyComboBox.Visible = false;
            _valueComboBox.Visible = false;
            switch (type)
            {
                case ProertyType.None:
                    break;
                case ProertyType.Bool:
                    _boolBox.Visible = true;
                    break;
                case ProertyType.Int:
                case ProertyType.Float:
                    _numericUpDown.Visible = true;
                    break;
                case ProertyType.Enum:
                    _comboBox.Visible = true;
                    break;
                case ProertyType.String:
                    _stringBox.Visible = true;
                    break;
                case ProertyType.List:
                    _comboBox.Visible = true;
                    break;
                case ProertyType.Dict:
                    _keyComboBox.Visible = true;
                    _valueComboBox.Visible = true;
                    break;
                default:
                    break;
            }
        }

        public TypeBox()
        {
            InitializeComponent();
        }

        private void MemTypeComboBox_TextChanged(object sender, EventArgs e)
        {
            if (OnCheckChange != null)
                OnCheckChange();
        }

        private void BoolBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OnCheckChange != null)
                OnCheckChange();
        }

        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (OnCheckChange != null)
                OnCheckChange();
        }

        private void _keyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void _valueComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void _comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

//public void SetBool(bool value)
//{
//    if (_proertyType == ProertyType.Bool)
//        _boolBox.Checked = value;
//}
//public void SetInt(int value)
//{
//    if (_proertyType == ProertyType.Int)
//        _numericUpDown.Value = value;
//}
//public void SetFloat(float value)
//{
//    if (_proertyType == ProertyType.Float)
//        _numericUpDown.Value = (decimal)value;
//}
//public void SetEnum(string value)
//{
//    if (_proertyType == ProertyType.Enum)
//        _comboBox.Text = value;
//}
//public void SetString(string value)
//{
//    if (_proertyType == ProertyType.String)
//        _comboBox.Text = value;
//}
//public void SetList(string item)
//{
//    if (_proertyType == ProertyType.List)
//        _comboBox.Text = item;
//}
//public void SetDict(string key, string value)
//{
//    if (_proertyType == ProertyType.Dict)
//    {
//        _keyComboBox.Text = key;
//        _valueComboBox.Text = value;
//    }
//}