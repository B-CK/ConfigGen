using Description.Wrap;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Description.Editor
{
    public partial class ClassEditorDock : Description.Editor.EditorDock
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
            Show(MainWindow.Ins._dock, DockState.Document);
        }
        protected override void Init(BaseWrap arg)
        {
            base.Init(arg);

            var wrap = GetWrap<ClassWrap>();
            _namespaceComboBox.Items.AddRange(NamespaceWrap.Namespaces);
            _inhertComboBox.Items.AddRange(ClassWrap.Classes);
            _inhertComboBox.Items.Remove(wrap.DisplayName);

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
            _namespaceComboBox.Text = wrap.Namespace.FullName.IsEmpty() ? NamespaceWrap.Namespaces[0].FullName : wrap.Namespace.FullName;
            _inhertComboBox.Text = wrap.Inherit.IsEmpty() ? ClassWrap.Classes[0].FullName : wrap.Inherit;
            _descTextBox.Text = wrap.Desc;
            _dataPathTextBox.Text = wrap.DataPath;
        }
        protected override void Save()
        {
            base.Save();
            var wrap = GetWrap<ClassWrap>();
            wrap.Name = _nameTextBox.Text;
            wrap.Index = _indexComboBox.Text;
            wrap.Namespace.Name = _namespaceComboBox.Text;
            wrap.Inherit = _inhertComboBox.Text;
            wrap.Desc = _descTextBox.Text;
            wrap.DataPath = _dataPathTextBox.Text;

            var nsw = NamespaceWrap.GetNamespace(_namespaceComboBox.Text);
            if (nsw.FullName != wrap.Namespace.FullName)
            {
                nsw.AddClass(wrap);
                wrap.Namespace.RemoveClass(wrap);
                wrap.Namespace = nsw;
            }
            nsw.SetDirty();
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
                AddMember(fields[i]);
            _memValueTextBox.OnCheckChange = () => _isDirty = true;
        }
        protected override void ShowMember(BaseWrap member)
        {
            base.ShowMember(member);
            if (member == null)
            {
                _memNameTextBox.Text = "";
                _memTypeComboBox.Text = "";
                _memValueTextBox.Text = "";
                _memGroupTextBox.Text = "";
                _memDescTextBox.Text = "";
                _checkerComboBox.Text = "";
            }
            else
            {
                FieldWrap field = member as FieldWrap;
                _memNameTextBox.Text = field.Name;
                _memTypeComboBox.Text = field.Type;
                _memValueTextBox.Text = field.Value;
                _memGroupTextBox.Text = field.Group;
                _memDescTextBox.Text = field.Desc;
                _checkerComboBox.Text = field.Checker;
            }
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
            RemoveMember(index);
            BaseWrap wrap = _memberListBox.Items[0] as BaseWrap;
            ShowMember(wrap);
        }
        private void AddMenuItem_Click(object sender, System.EventArgs e)
        {
            var member = FieldWrap.Create(UnqueName, GetWrap<ClassWrap>());
            AddMember(member);
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
