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

namespace Description.Editor
{
    public partial class FieldEditor : MemberEditor
    {
        public static FieldEditor Create(ClassEditorDock dock, FieldWrap wrap)
        {
            FieldEditor field = PoolManager.Ins.Pop<FieldEditor>();
            if (field == null) field = new FieldEditor();
            field._dock = dock;
            field.Init(wrap);
            return field;
        }
        public static FieldEditor Create(ClassEditorDock dock, string name)
        {
            FieldEditor field = PoolManager.Ins.Pop<FieldEditor>();
            if (field == null) field = new FieldEditor();
            FieldWrap wrap = PoolManager.Ins.Pop<FieldWrap>();
            if (wrap == null) wrap = FieldWrap.Create(name, dock.GetWrap<ClassWrap>());
            field._dock = dock;
            field.Init(wrap);
            return field;
        }
        public static implicit operator FieldWrap(FieldEditor editor)
        {
            return editor._wrap as FieldWrap;
        }

        public override string DisplayName
        {
            get
            {
                string name = _nameTextBox.Text ?? "Name?";
                string type = _typeComboBox.Text ?? "Type?";
                string value = _valueTypeBox.GetValue();
                string display = _valueTypeBox.Type == TypeBox.ProertyType.None ?
                    Util.Format("{0}:{1}", name, type)
                    : Util.Format("{0}:{1}={2}", name, type, value);
                return display;
            }
        }

        ClassEditorDock _dock;
        private FieldEditor()
        {
            InitializeComponent();
        }
        protected override void OnInit()
        {
            var field = _wrap as FieldWrap;
            Location = Point.Empty;
            var panel = _dock.MemberSplitContainer.Panel2;
            Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            Size = new Size(panel.Width - 95, panel.Height);
            _isNew = true;

            _nameTextBox.Text = field.Name;
            _typeComboBox.Items.AddRange(Util.GetAllTypes());
            _valueTypeBox.Clear();
            _defaultLabel.Text = "默认值:";
            if (field.Type.IsEmpty())
            {
                _typeComboBox.Text = "";
                _valueTypeBox.Visible = false;
            }
            else
            {
                SetMemberValue(field);
                _valueTypeBox.Visible = true;
                if (_typeComboBox.Text == Util.LIST || _typeComboBox.Text == Util.DICT)
                    _defaultLabel.Text = "元素类型:";
            }
            _isConstCheckBox.Checked = field.IsConst;
            _valueTypeBox.OnCheckChange = OnFieldValueChanged;
            _groupTextBox.Text = field.Group;
            _desTextBox.Text = field.Desc;
            _checkerComboBox.Text = field.Checker;
        }
        /// <summary>
        /// 不包含枚举与类类型
        /// </summary>
        private void SetMemberValue(FieldWrap field)
        {
            string[] nodes = field.Type.Split(Util.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
            switch (nodes.Length)
            {
                case 0:
                    _typeComboBox.Text = "";
                    break;
                case 1://基础类型
                    _typeComboBox.Text = nodes[0];
                    switch (nodes[0])
                    {
                        case Util.BOOL:
                            _valueTypeBox.InitBool(field.Value == "True");
                            break;
                        case Util.INT:
                            decimal int32;
                            if (decimal.TryParse(field.Value, out int32))
                                _valueTypeBox.InitInt(int32);
                            break;
                        case Util.LONG:
                            decimal int64;
                            if (decimal.TryParse(field.Value, out int64))
                                _valueTypeBox.InitInt(int64, 64);
                            break;
                        case Util.FLOAT:
                            decimal float32;
                            if (decimal.TryParse(field.Value, out float32))
                                _valueTypeBox.InitFloat(float32);
                            break;
                        case Util.STRING:
                            _valueTypeBox.InitString(field.Value);
                            break;
                        default:
                            if (EnumWrap.EnumDict.ContainsKey(field.Type))
                            {
                                EnumWrap wrap = EnumWrap.EnumDict[field.Type];
                                _valueTypeBox.InitEnum(field.Value, wrap.Items.ToArray());
                            }
                            else if (ClassWrap.ClassDict.ContainsKey(field.Type))
                            {
                                _valueTypeBox.Visible = false;
                            }
                            else
                            {
                                _valueTypeBox.EnableBox(TypeBox.ProertyType.None);
                                ConsoleDock.Ins.LogErrorFormat("未知字段类型{0}", field.Type);
                            }
                            break;
                    }
                    break;
                case 2://List
                    _typeComboBox.Text = nodes[0];
                    _valueTypeBox.InitList(nodes[1], Util.GetAllTypes());
                    break;
                case 3://Dict
                    _typeComboBox.Text = nodes[0];
                    _valueTypeBox.InitDict(nodes[1], nodes[2], Util.GetKeyTypes(), Util.GetAllTypes());
                    break;
                default:
                    break;
            }

        }
        public override void Save()
        {
            base.Save();
            var field = _wrap as FieldWrap;
            if (field.Name != _nameTextBox.Text)
                _dock.GetWrap<ClassWrap>().RemoveField(field);
            field.Name = _nameTextBox.Text;
            field.Type = _typeComboBox.Text;
            if (_typeComboBox.Text == Util.LIST || _typeComboBox.Text == Util.DICT)
            {
                field.Type += Util.ArgsSplitFlag[0] + _valueTypeBox.GetValue();
                field.Value = "";
            }
            else
                field.Value = _valueTypeBox.GetValue();
            field.IsConst = _isConstCheckBox.Checked;
            field.Group = _groupTextBox.Text;
            field.Desc = _desTextBox.Text;
            field.Checker = _checkerComboBox.Text;
        }
        public override void Hide()
        {
            base.Hide();
            _dock.MemberSplitContainer.Panel2.Controls.Remove(this);
        }
        public override void Show()
        {
            base.Show();
            _dock.MemberSplitContainer.Panel2.Controls.Add(this);
        }

        private void OnFieldTextChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;
            _dock.OnValueChange();
        }
        private void OnFieldValueChanged()
        {
            if (!_isInit) return;
            _dock.OnValueChange();
            _dock.RefreshMember(this);
        }
        private void OnFieldNameChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;
            _dock.OnValueChange();
            var nameBox = sender as TextBox;
            if (_dock.ContainMember(nameBox.Text))
            {
                Util.MsgWarning("类型{0}中重复定义字段{1}!", _dock.GetWrap<ClassWrap>().Name, nameBox.Text);
                nameBox.Text = _wrap.Name;
            }
            else
            {
                string oldName = Name;
                Name = nameBox.Text;
                _dock.RefreshMember(this, oldName);
            }
        }
        private void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;
            ComboBox combo = sender as ComboBox;
            _defaultLabel.Text = "默认值:";
            _valueTypeBox.Clear(false);
            _dock.OnValueChange();
            if (combo.Text.IsEmpty() || EnumWrap.EnumDict.ContainsKey(combo.Text)
                            || ClassWrap.ClassDict.ContainsKey(combo.Text))
                _valueTypeBox.Visible = false;
            else
            {
                _valueTypeBox.Visible = true;
                if (combo.Text == Util.LIST || combo.Text == Util.DICT)
                    _defaultLabel.Text = "元素类型:";
                switch (combo.Text)
                {
                    case Util.BOOL:
                        _valueTypeBox.InitBool(false);
                        break;
                    case Util.INT:
                        _valueTypeBox.InitInt(0);
                        break;
                    case Util.LONG:
                        _valueTypeBox.InitInt(0, 64);
                        break;
                    case Util.FLOAT:
                        _valueTypeBox.InitFloat(0);
                        break;
                    case Util.STRING:
                        _valueTypeBox.InitString("");
                        break;
                    case Util.LIST:
                        _valueTypeBox.InitList("", Util.GetAllTypes());
                        break;
                    case Util.DICT:
                        _valueTypeBox.InitDict("", "", Util.GetKeyTypes(), Util.GetAllTypes());
                        break;
                    default:
                        _valueTypeBox.EnableBox(TypeBox.ProertyType.None);
                        ConsoleDock.Ins.LogErrorFormat("未知字段类型{0}", combo.Text);
                        break;
                }
            }
            _dock.RefreshMember(this);
        }
    }
}
