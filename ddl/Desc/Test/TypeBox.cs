using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Desc.Wrap;

namespace Desc
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

        public Action OnCheckChange;
        public ProertyType Type => _propertyType;
        ProertyType _propertyType = ProertyType.None;

        Point hidePoint = new Point(0, -999);

        public void InitBool(bool value = false)
        {
            _propertyType = ProertyType.Bool;
            _boolBox.Checked = value;
            EnableBox(_propertyType);
        }
        public void InitInt(decimal value = 0, int size = 32)
        {
            _propertyType = ProertyType.Int;
            _numericUpDown.DecimalPlaces = 0;
            _numericUpDown.Maximum = int.MaxValue;
            _numericUpDown.Minimum = int.MinValue;
            _numericUpDown.Increment = 1;
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
                    Debug.LogErrorFormat("[NamespaceDock]不存在Int{0}大小!", size);
                    break;
            }
        }
        public void InitFloat(decimal value = 0)
        {
            _propertyType = ProertyType.Float;
            _numericUpDown.DecimalPlaces = 3;
            _numericUpDown.Maximum = decimal.MaxValue;
            _numericUpDown.Minimum = decimal.MinValue;
            _numericUpDown.Increment = (decimal)0.01f;
            _numericUpDown.Value = value;
            EnableBox(_propertyType);
        }
        public void InitEnum(string value, EnumItemWrap[] items)
        {
            _propertyType = ProertyType.Enum;
            EnableBox(_propertyType);
            _comboBox.Items.Clear();
            _comboBox.Items.AddRange(items);
            _comboBox.SelectedItem = Util.FindTypeItem(value, items);
        }
        public void InitString(string value = "")
        {
            _propertyType = ProertyType.String;
            EnableBox(_propertyType);
            _stringBox.Text = value;
        }
        public void InitList(string value, object[] items)
        {
            _propertyType = ProertyType.List;
            EnableBox(_propertyType);
            _comboBox.Items.Clear();
            _comboBox.Items.AddRange(items);
            _comboBox.SelectedItem = Util.FindTypeItem(value, items);
        }
        /// <summary>
        /// 键值对形式
        /// </summary>
        /// <param name="keys">仅支持少部分基础类型</param>
        /// <param name="values"></param>
        public void InitDict(string key, string value, object[] keys, object[] values)
        {
            _propertyType = ProertyType.Dict;
            EnableBox(_propertyType);

            _keyComboBox.Items.Clear();
            _keyComboBox.Items.AddRange(keys);
            _keyComboBox.SelectedItem = Util.FindTypeItem(key, values);

            _valueComboBox.Items.Clear();
            _valueComboBox.Items.AddRange(values);
            _valueComboBox.SelectedItem = Util.FindTypeItem(value, values);
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
                case ProertyType.String:
                    return _stringBox.Text;
                case ProertyType.Enum:
                    return _comboBox.SelectedItem == null ? "" : _comboBox.SelectedItem.ToString();
                case ProertyType.List:
                    return _comboBox.SelectedItem == null ? "" : _comboBox.SelectedItem.ToString();
                case ProertyType.Dict:
                    var a = Util.Format("{0}{1}{2}", _keyComboBox.Text, Util.ArgsSplitFlag[0], _valueComboBox.SelectedItem);
                    return a;
                case ProertyType.None:
                default:
                    return "";
            }
        }
        public void EnableBox(ProertyType type)
        {
            _boolBox.Location = hidePoint;
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
                    _boolBox.Location = new Point(5, 6);
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
        public void Clear(bool clearEvt = true)
        {
            if (clearEvt)
                OnCheckChange = null;

            _propertyType = ProertyType.None;
            _boolBox.Checked = false;
            _stringBox.Text = "";
            _numericUpDown.Value = 0;
            _comboBox.Text = "";
            _keyComboBox.Text = "";
            _valueComboBox.Text = "";
        }
        public TypeBox()
        {
            InitializeComponent();
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (OnCheckChange != null)
                OnCheckChange();
        }
    }
}
