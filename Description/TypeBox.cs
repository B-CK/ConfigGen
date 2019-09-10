using System;
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

        ProertyType _proertyType = ProertyType.None;
        public Action OnCheckChange;

        public void InitBool(bool value = false)
        {
            _proertyType = ProertyType.Bool;
            _boolBox.Checked = value;
            EnableBox(_proertyType);
        }
        public void InitInt(int value = 0, int size = 32)
        {
            _proertyType = ProertyType.Int;
            _numericUpDown.Increment = 1;
            _numericUpDown.Maximum = int.MaxValue;
            _numericUpDown.Minimum = int.MinValue;
            _numericUpDown.Value = value;
            EnableBox(_proertyType);
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
            _proertyType = ProertyType.Float;
            _numericUpDown.Increment = (decimal)0.1f;
            _numericUpDown.Value = (decimal)value;
            _numericUpDown.Maximum = decimal.MaxValue;
            _numericUpDown.Minimum = decimal.MinValue;
            EnableBox(_proertyType);
        }
        public void InitEnum(string value, EnumItemWrap[] items)
        {
            _proertyType = ProertyType.Enum;
            EnableBox(_proertyType);
            _comboBox.Text = value;
            _comboBox.Items.AddRange(items);
        }
        public void InitString(string value = "")
        {
            _proertyType = ProertyType.String;
            EnableBox(_proertyType);
            _stringBox.Text = value;
        }
        public void InitList(object[] items)
        {
            _proertyType = ProertyType.List;
            EnableBox(_proertyType);
            _comboBox.Items.AddRange(items);
        }
        /// <summary>
        /// 键值对形式
        /// </summary>
        /// <param name="keys">仅支持少部分基础类型</param>
        /// <param name="values"></param>
        public void InitDict(object[] keys, object[] values)
        {
            _proertyType = ProertyType.Dict;
            EnableBox(_proertyType);
            _keyComboBox.Items.AddRange(keys);
            _valueComboBox.Items.AddRange(values);
        }

        public void GetDictType(out string key, out string value)
        {
            if (_proertyType == ProertyType.Dict)
            {
                key = _keyComboBox.Text;
                value = _valueComboBox.Text;
            }
            else
            {
                key = "";
                value = "";
            }
        }
        public string GetListType()
        {
            if (_proertyType == ProertyType.List)
                return _comboBox.Text;
            else
                return "";
        }
        public string GetValue()
        {
            switch (_proertyType)
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
