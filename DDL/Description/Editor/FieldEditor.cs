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
        const string TYPE = "Type?";
        const string NAME = "Name?";

        public static FieldEditor Create(ClassEditorDock dock, FieldWrap wrap, bool isInherit = false)
        {
            FieldEditor editor = PoolManager.Ins.Pop<FieldEditor>();
            if (editor == null) editor = new FieldEditor();
            editor.Init(dock, wrap);
            editor._isInherit = isInherit;
            return editor;
        }
        public static FieldEditor Create(ClassEditorDock dock, string name)
        {
            FieldEditor editor = PoolManager.Ins.Pop<FieldEditor>();
            if (editor == null) editor = new FieldEditor();
            FieldWrap wrap = PoolManager.Ins.Pop<FieldWrap>();
            if (wrap == null) wrap = FieldWrap.Create(name, dock.GetWrap<ClassWrap>());
            editor.Init(dock, wrap);
            return editor;
        }
        public static implicit operator FieldWrap(FieldEditor editor)
        {
            return editor._wrap as FieldWrap;
        }

        public override string DisplayName
        {
            get
            {
                if (_isInherit)
                {
                    var field = _wrap as FieldWrap;
                    string name = field.Name ?? NAME;
                    string type = field.Type ?? TYPE;
                    string value = field.Value;
                    if (IsConst)
                        return Util.Format("# {0}({1})={2}", name, type, value);
                    else if (Util.LIST == type || Util.DICT == type)
                        return Util.Format("# {0}({1}:{2})", name, type, value);
                    else
                        return Util.Format("# {0}({1})", name, type);
                }
                else
                {
                    string name = _nameTextBox.Text ?? NAME;
                    var item = _typeComboBox.SelectedItem;
                    string type = _typeComboBox.Text ?? TYPE;
                    if (item is string)
                        type = (string)item;
                    else if (item is TypeWrap)
                        type = (item as TypeWrap).FullName;
                    string value = _valueTypeBox.GetValue();
                    if (IsConst)
                        return Util.Format("  {0}({1})={2}", name, type, value);
                    else if (Util.LIST == type || Util.DICT == type)
                        return Util.Format("  {0}({1}:{2})", name, type, value);
                    else
                        return Util.Format("  {0}({1})", name, type);
                }
            }
        }
        public string FieldName => (_wrap as FieldWrap).Name;
        /// <summary>
        /// 继承字段,禁止修改
        /// </summary>
        public bool IsInherit => _isInherit;
        protected bool _isInherit;        
        private FieldEditor()
        {
            InitializeComponent();
        }
        protected override void OnInit()
        {
            base.OnInit();
            var field = _wrap as FieldWrap;
            Location = Point.Empty;
            var panel = GetDock<ClassEditorDock>().MemberSplitContainer.Panel2;
            Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            Size = new Size(panel.Width - 95, panel.Height);
            _isInherit = false;   

            _nameTextBox.Text = field.Name;
            _typeComboBox.Items.AddRange(Util.GetAllTypes());
            _valueTypeBox.Clear();
            _defaultLabel.Text = "默认值:";
            if (field.Type.IsEmpty())
            {
                _typeComboBox.Text = Util.BOOL;
                _valueTypeBox.InitBool();
            }
            else
            {
                SetMemberValue(field);
                if (_typeComboBox.Text == Util.LIST || _typeComboBox.Text == Util.DICT)
                    _defaultLabel.Text = "元素类型:";
            }
            _isConstCheckBox.Checked = field.IsConst;
            _valueTypeBox.OnCheckChange = OnFieldValueChanged;
            _groupTextBox.Text = field.Group.IsEmpty() ? Util.DefaultGroup : field.Group;
            _desTextBox.Text = field.Desc;
            _checkerComboBox.Text = field.Checker;
        }
        /// <summary>
        /// 不包含枚举与类类型
        /// </summary>
        private void SetMemberValue(FieldWrap field)
        {
            string[] nodes = field.Type.Split(Util.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
            var types = new object[_typeComboBox.Items.Count];
            _typeComboBox.Items.CopyTo(types, 0);
            _typeComboBox.SelectedItem = Util.FindTypeItem(nodes[0], types);
            _isConstCheckBox.Visible = true;
            switch (nodes.Length)
            {
                case 1://基础类型
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
                            if (EnumWrap.Dict.ContainsKey(field.Type))
                            {
                                EnumWrap wrap = EnumWrap.Dict[field.Type];
                                _valueTypeBox.InitEnum(field.Value, wrap.Items.ToArray());
                                _valueTypeBox.Visible = true;
                            }
                            else if (ClassWrap.Dict.ContainsKey(field.Type))
                            {
                                ClassWrap wrap = ClassWrap.Dict[field.Type];
                                _valueTypeBox.Visible = false;
                                _isConstCheckBox.Visible = false;
                            }
                            else
                            {
                                _valueTypeBox.Visible = false;
                                _isConstCheckBox.Visible = false;
                                _valueTypeBox.EnableBox(TypeBox.ProertyType.None);
                                Debug.LogErrorFormat("字段{0}的类型{1}无法解析", _wrap.Name, field.Type);
                            }
                            break;
                    }
                    break;
                case 2://List
                    _isConstCheckBox.Visible = false;
                    _valueTypeBox.InitList(nodes[1], Util.GetCombTypes());
                    break;
                case 3://Dict
                    _isConstCheckBox.Visible = false;
                    _valueTypeBox.InitDict(nodes[1], nodes[2], Util.GetKeyTypes(), Util.GetCombTypes());
                    break;
                default:
                    _isConstCheckBox.Visible = false;
                    Util.MsgError("{0}类型的{1}字段类型/值设置异常", ParentName, field.Name);
                    break;
            }

        }
        public override void Save()
        {
            base.Save();
            var field = _wrap as FieldWrap;         
            field.Name = _nameTextBox.Text;
            var typeItem = _typeComboBox.SelectedItem;
            field.Type = typeItem == null ? "" : typeItem.ToString();
            if (_typeComboBox.Text == Util.LIST || _typeComboBox.Text == Util.DICT)
            {
                field.Type += Util.ArgsSplitFlag[0] + _valueTypeBox.GetValue();
                field.Value = "";
            }
            else
                field.Value = _valueTypeBox.GetValue();
            field.IsConst = IsConst;
            field.Group = _groupTextBox.Text;
            field.Desc = _desTextBox.Text;
            field.Checker = _checkerComboBox.Text;
        }
        public override void Hide()
        {
            base.Hide();
            GetDock<ClassEditorDock>().MemberSplitContainer.Panel2.Controls.Remove(this);
        }
        public override void Show()
        {
            base.Show();
            GetDock<ClassEditorDock>().MemberSplitContainer.Panel2.Controls.Add(this);
        }
        private bool IsConst
        {
            get
            {
                return _isConstCheckBox.Checked
                    && (EnumWrap.Dict.ContainsKey(_typeComboBox.Text)
                    || _typeComboBox.Text == Util.BOOL || _typeComboBox.Text == Util.INT
                    || _typeComboBox.Text == Util.LONG || _typeComboBox.Text == Util.FLOAT
                    || _typeComboBox.Text == Util.STRING);
            }
        }
        private void OnFieldTextChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;
            GetDock<ClassEditorDock>().OnValueChange();
        }
        private void OnFieldValueChanged()
        {
            if (!_isInit) return;
            GetDock<ClassEditorDock>().OnValueChange();
            GetDock<ClassEditorDock>().RefreshMember(this);
        }
        private void OnFieldNameChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;
            var dock = GetDock<ClassEditorDock>();
            dock.OnValueChange();
            var nameBox = sender as TextBox;
            if (!Util.CheckName(_wrap.Name))
                nameBox.Text = _wrap.Name;
            else if (dock.ContainMember(nameBox.Text))
            {
                Util.MsgWarning("类型{0}中重复定义字段{1}!", dock.GetWrap<ClassWrap>().Name, nameBox.Text);
                nameBox.Text = _wrap.Name;
            }
            else
            {
                string oldName = Name;
                Name = nameBox.Text;
                dock.RefreshMember(this, oldName);
            }
        }
        private void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;
            ComboBox combo = sender as ComboBox;
            _defaultLabel.Text = "默认值:";
            _valueTypeBox.Clear(false);
            var dock = GetDock<ClassEditorDock>();
            dock.OnValueChange();
            _isConstCheckBox.Visible = true;
            string selectedType = combo.SelectedItem.ToString();
            if (combo.Text.IsEmpty())
            {
                _valueTypeBox.Visible = false;
                _isConstCheckBox.Visible = false;
            }
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
                        _isConstCheckBox.Visible = false;
                        _valueTypeBox.InitList("", Util.GetCombTypes());
                        break;
                    case Util.DICT:
                        _isConstCheckBox.Visible = false;
                        _valueTypeBox.InitDict("", "", Util.GetKeyTypes(), Util.GetCombTypes());
                        break;
                    default:
                        if (EnumWrap.Dict.ContainsKey(selectedType))
                        {
                            EnumWrap wrap = EnumWrap.Dict[selectedType];
                            var array = wrap.Items.ToArray();
                            string initValue = "";
                            if (array.Length > 0)
                                initValue = array[0].FullName;
                            _valueTypeBox.InitEnum(initValue, array);
                        }
                        else if (ClassWrap.Dict.ContainsKey(selectedType))
                        {
                            _isConstCheckBox.Visible = false;
                            _valueTypeBox.EnableBox(TypeBox.ProertyType.None);
                        }
                        else
                        {
                            _isConstCheckBox.Visible = false;
                            Debug.LogErrorFormat("字段{0}选择的类型{1}无法解析", _wrap.Name, combo.Text);
                        }
                        break;
                }
            }
            dock.RefreshMember(this);
        }
        private void GroupButton_Click(object sender, EventArgs e)
        {
            GroupDock.Ins.ShowGroups(_groupTextBox);
        }
    }
}
