using Description.Wrap;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Description.Editor
{
    public partial class ClassEditorDock : EditorDock
    {
        public static ClassEditorDock Create(ClassWrap wrap)
        {
            var dock = PoolManager.Ins.Pop<ClassEditorDock>();
            if (dock == null) dock = new ClassEditorDock();
            dock.Init(wrap);
            dock.Text = wrap.Name;
            dock._typeGroupBox.Text = "类型[Class]";
            return dock;
        }

        private ClassEditorDock()
        {
            InitializeComponent();
            Show(MainWindow.Ins._dockPanel, DockState.Document);
        }
        protected override void Init(BaseWrap arg)
        {
            base.Init(arg);

            var wrap = GetWrap<ClassWrap>();
            _indexComboBox.Items.AddRange(wrap.Fields.ToArray());
            _namespaceComboBox.Items.AddRange(NamespaceWrap.Namespaces);
            _inhertComboBox.Items.Add("");
            _inhertComboBox.Items.AddRange(ClassWrap.Classes);
            _inhertComboBox.Items.Remove(wrap);


            _nameTextBox.Text = wrap.Name;
            if (wrap.Index.IsEmpty())
            {
                if (wrap.Indexes.Length > 1)
                    _indexComboBox.Text = wrap.Indexes[0];
                else
                    _indexComboBox.Text = "";
            }
            else
            {
                _indexComboBox.Text = wrap.Index;
            }
            _namespaceComboBox.Text = wrap.Namespace.FullName.IsEmpty() ? "@" : wrap.Namespace.FullName;
            _inhertComboBox.Text = wrap.Inherit.IsEmpty() ? "" : wrap.Inherit;
            _descTextBox.Text = wrap.Desc;
            _dataPathTextBox.Text = wrap.DataPath;

            _isInit = true;
        }
        protected override void Save()
        {
            base.Save();
            var wrap = GetWrap<ClassWrap>();
            wrap.Name = _nameTextBox.Text;
            wrap.Index = _indexComboBox.Text;
            wrap.Inherit = _inhertComboBox.Text;
            wrap.Desc = _descTextBox.Text;
            wrap.DataPath = _dataPathTextBox.Text;

            SetNamespace<ClassWrap>(wrap.Namespace.Name, _namespaceComboBox.Text);
        }
        protected override void Clear()
        {
            base.Clear();
            _nameTextBox.Text = "";
            _namespaceComboBox.Text = "";
            _indexComboBox.Text = "";
            _inhertComboBox.Text = "";
            _descTextBox.Text = "";
            _dataPathTextBox.Text = "";
        }
        protected override void InitMember()
        {
            base.InitMember();

            _memTypeComboBox.Items.AddRange(ClassWrap.Classes);
            _memTypeComboBox.Items.AddRange(Util.BaseTypes);

            var wrap = GetWrap<ClassWrap>();
            var fields = wrap.Fields;
            for (int i = 0; i < fields.Count; i++)
            {
                AddMember(fields[i]);
                _memberListBox.Items.Add(fields[i]);
            }
            _memValueTextBox.OnCheckChange = OnValueChange;
            SetPropsPanelVisible(false);
        }
        protected override void ShowMember(BaseWrap member)
        {
            base.ShowMember(member);

            SetPropsPanelVisible(true);
            FieldWrap field = member as FieldWrap;
            _memNameTextBox.Text = field.Name;
            _memTypeComboBox.Text = field.Type;
            _memValueTextBox.Text = field.Value;
            _memGroupTextBox.Text = field.Group;
            _memDescTextBox.Text = field.Desc;
            _checkerComboBox.Text = field.Checker;

            _defaultLabel.Text = "默认值:";
            if (_memTypeComboBox.Text.IsEmpty())
                _memValueTextBox.Visible = false;
            else
            {
                _memValueTextBox.Visible = true;
                if (_memTypeComboBox.Text == Util.LIST && _memTypeComboBox.Text == Util.DICT)
                    _defaultLabel.Text = "元素类型:";
            }
        }
        protected override void SaveMember(BaseWrap member)
        {
            FieldWrap field = member as FieldWrap;
            field.Name = _memNameTextBox.Text;
            field.Type = _memTypeComboBox.Text;
            if (_memTypeComboBox.Text == Util.LIST && _memTypeComboBox.Text == Util.DICT)
                field.Type += Util.ArgsSplitFlag[0] + _memValueTextBox.GetSetType();
            else
                field.Value = _memValueTextBox.GetValue();
            field.Group = _memGroupTextBox.Text;
            field.Desc = _memDescTextBox.Text;
            field.Checker = _checkerComboBox.Text;
        }
        protected override void HideMember(BaseWrap member)
        {
            base.HideMember(member);
            SetPropsPanelVisible(false);
        }
        protected void SetPropsPanelVisible(bool enable)
        {
            _nameLabel.Visible = enable;
            _typeLabel.Visible = enable;
            _defaultLabel.Visible = enable;
            _groupLabel.Visible = enable;
            _descLabel.Visible = enable;
            _checkLabel.Visible = enable;

            _memNameTextBox.Visible = enable;
            _memTypeComboBox.Visible = enable;
            _memValueTextBox.Visible = enable;
            _memGroupTextBox.Visible = enable;
            _memDescTextBox.Visible = enable;
            _checkerComboBox.Visible = enable;
            _readOnlyCheckBox.Visible = enable;
        }

        private void UpdateIndexes()
        {
            _indexComboBox.Items.Clear();
            var items = _memberListBox.Items;
            for (int i = 0; i < items.Count; i++)
                _indexComboBox.Items.Add(items[i]);
        }

        private void OnValueChange(object sender, System.EventArgs e)
        {
            OnValueChange();
        }
        private void DataPathButton_Click(object sender, System.EventArgs e)
        {
            Util.Open(Util.DataDir, "引用数据文件", (string file) =>
            {
                _dataPathTextBox.Text = Util.GetDataDirRelPath(file);
            }, "获取数据文件路径失败!");
        }
        private void MemberListBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var point = _memberListBox.PointToScreen(e.Location);
            _memberMenu.Show(point);
        }
        private void RemoveMenuItem_Click(object sender, System.EventArgs e)
        {
            int index = _memberListBox.SelectedIndex;
            if (CheckMemeberIndex(index)) return;

            var member = _memberListBox.Items[index];
            ClassWrap wrap = GetWrap<ClassWrap>();
            wrap.RemoveField(member as FieldWrap);

            RemoveMember(index);
            _memberListBox.Items.RemoveAt(index);
            UpdateIndexes();
        }
        private void AddMenuItem_Click(object sender, System.EventArgs e)
        {
            var member = FieldWrap.Create(UnqueName, GetWrap<ClassWrap>());
            ClassWrap wrap = GetWrap<ClassWrap>();
            wrap.AddField(member as FieldWrap);

            AddMember(member);
            _memberListBox.Items.Add(member);           
            UpdateIndexes();
        }

        private void MemTypeComboBox_TextChanged(object sender, System.EventArgs e)
        {
            var combobox = sender as ComboBox;
            switch (combobox.Text)
            {
                case Util.BOOL:
                    _memValueTextBox.InitBool();
                    break;
                case Util.INT:
                    _memValueTextBox.InitInt();
                    break;
                case Util.LONG:
                    _memValueTextBox.InitInt(0, 64);
                    break;
                case Util.FLOAT:
                    _memValueTextBox.InitFloat();
                    break;
                case Util.STRING:
                    _memValueTextBox.InitString();
                    break;
                case Util.LIST:
                    _memValueTextBox.InitList(Util.GetAllTypes());
                    break;
                case Util.DICT:
                    _memValueTextBox.InitDict(Util.GetKeyTypes(), Util.GetAllTypes());
                    break;
                default:
                    if (EnumWrap.EnumDict.ContainsKey(combobox.Text))
                    {
                        EnumWrap wrap = EnumWrap.EnumDict[combobox.Text];
                        _memValueTextBox.InitEnum(combobox.Text, wrap.Items.ToArray());
                    }
                    else
                    {
                        _memValueTextBox.EnableBox(TypeBox.ProertyType.None);
                        ConsoleDock.Ins.LogErrorFormat("未知类型{0}", combobox.Text);
                    }
                    break;
            }
        }
    }
}
