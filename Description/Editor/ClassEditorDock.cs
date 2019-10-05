using Description.Wrap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        static OpenFileDialog OpenFileDialog = new OpenFileDialog();

        List<MemberEditor> _listBox = new List<MemberEditor>();
        MemberEditor _currentMember;
        private ClassEditorDock()
        {
            InitializeComponent();
            Show(MainWindow.Ins._dockPanel, DockState.Document);
        }
        protected override void OnInit(TypeWrap arg)
        {
            base.OnInit(arg);

            var wrap = GetWrap<ClassWrap>();
            _namespaceComboBox.Items.AddRange(NamespaceWrap.Namespaces);
            _inhertComboBox.Items.Add(string.Empty);
            _inhertComboBox.Items.AddRange(ClassWrap.Array);
            _inhertComboBox.Items.Remove(wrap);

            _nameTextBox.Text = wrap.Name;
            _namespaceComboBox.SelectedItem = wrap.Namespace;
            if (!wrap.Inherit.IsEmpty())
            {
                if (!ClassWrap.Dict.ContainsKey(wrap.Inherit))
                    Debug.LogErrorFormat("[Class]类型{0}的父类{1}类型不存在!", wrap.FullName, wrap.Inherit);
                else
                {
                    var inherit = ClassWrap.Dict[wrap.Inherit];
                    _inhertComboBox.SelectedItem = inherit;
                }
            }
            else
                _inhertComboBox.SelectedText = string.Empty;
            _groupTextBox.Text = wrap.Group.IsEmpty() ? Util.Groups[0] : wrap.Group;
            _descTextBox.Text = wrap.Desc;
            _dataPathTextBox.Text = wrap.DataPath;

            string fieldName = wrap.Index ?? "";
            var fields = wrap.Fields;
            var items = _memberListBox.Items;
            for (int i = 0; i < fields.Count; i++)
            {
                var fieldEditor = FieldEditor.Create(this, fields[i]);
                items.Add(fieldEditor);
                AddMember(fieldEditor);
                _indexComboBox.Items.Add(fieldEditor);
                if (fieldName == fieldEditor.Name)
                    _indexComboBox.SelectedItem = fieldEditor;
            }
            if (_dataPathTextBox.Text.IsEmpty())
                _indexComboBox.Enabled = false;
            else
                _indexComboBox.Enabled = true;
        }
        protected override void OnSave()
        {
            base.OnSave();
            var cls = GetWrap<ClassWrap>();
            var index = _indexComboBox.SelectedItem as FieldEditor;
            cls.Index = index == null ? "" : index.Name;
            var inherit = _inhertComboBox.SelectedItem as ClassWrap;
            cls.Inherit = inherit == null ? "" : inherit.FullName;
            cls.Group = _groupTextBox.Text;
            cls.DataPath = _dataPathTextBox.Text;
            if (cls.Name != _nameTextBox.Text || cls.Desc != _descTextBox.Text)
            {
                string fullName = cls.FullName;
                cls.Name = _nameTextBox.Text;
                cls.Desc = _descTextBox.Text;
                UpdateTreeNode(fullName);
            }


            SetNamespace<ClassWrap>(cls.Namespace, _namespaceComboBox.SelectedItem as NamespaceWrap);

            foreach (var item in _memberDict)
            {
                var editor = item.Value as MemberEditor;
                editor.Save();
                if (editor.IsNew)
                {
                    if (editor is FieldEditor)
                        cls.OverrideField(editor as FieldEditor);
                }
                else
                {
                    if (editor is FieldEditor)
                        cls.RemoveField(editor as FieldEditor);
                }
            }
        }
        protected override void Clear()
        {
            base.Clear();
            _memberListBox.Items.Clear();
        }
        public void RefreshMember(MemberEditor member, string oldFullName = null)
        {
            //修改成员列表数据
            if (!oldFullName.IsEmpty())
            {
                RemoveMember(oldFullName);
                AddMember(member);
            }

            //修改成员列表界面
            var mItems = _memberListBox.Items;
            int index = mItems.IndexOf(member);
            mItems.Remove(member);
            mItems.Insert(index, member);
            //筛选出后,再修改名称时,是否过滤掉?
            //if (!_memFindBox.Text.IsEmpty())
            //    FindMember(_memFindBox.Text);

            //修改关键字CombBox界面
            var iItems = _indexComboBox.Items;
            index = iItems.IndexOf(member);
            int selectedIndex = _indexComboBox.SelectedIndex;
            iItems.Remove(member);
            iItems.Insert(index, member);
            _indexComboBox.SelectedIndex = selectedIndex;
        }
        private void FindMember(string name)
        {
            if (name.IsEmpty()) return;
            _memberListBox.Items.AddRange(_listBox.ToArray());
            _listBox.Clear();
            foreach (var item in _memberDict)
            {
                var mem = item.Value as MemberEditor;
                if (mem.Name.IndexOf(name) == -1)
                    _listBox.Add(mem);
            }
            var items = _memberListBox.Items;
            for (int i = 0; i < _listBox.Count; i++)
                items.Remove(_listBox[i]);
        }

        private void OnValueChange(object sender, System.EventArgs e)
        {
            OnValueChange();
        }
        private void DataPathButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                OpenFileDialog.InitialDirectory = Util.DataDir;
                OpenFileDialog.Title = "引用数据文件";
                OpenFileDialog.Filter = "表|*.xls|表|*.xlsx";
                DialogResult result = OpenFileDialog.ShowDialog();
                _dataPathTextBox.Text = Util.GetDataDirRelPath(OpenFileDialog.FileName);
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("获取数据文件路径失败!{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        private void MemberListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var point = _memberListBox.PointToScreen(e.Location);
                _memberListBox.SelectedIndex = _memberListBox.IndexFromPoint(e.Location);
                _memberMenu.Show(point);
            }

            var list = sender as ListBox;
            var member = list.SelectedItem as MemberEditor;
            if (member == null || _currentMember == member) return;
            if (_currentMember != null)
                _currentMember.Hide();
            _currentMember = member;
            member.Show();
        }
        private void RemoveMenuItem_Click(object sender, System.EventArgs e)
        {
            var member = _memberListBox.SelectedItem as MemberEditor;
            _memberListBox.Items.Remove(member);
            _indexComboBox.Items.Remove(member);
            if (_currentMember == member)
                _currentMember = null;
            member.Clear();
            OnValueChange();
        }
        private void AddMenuItem_Click(object sender, System.EventArgs e)
        {
            var member = FieldEditor.Create(this, UnqueName);
            _memberListBox.Items.Add(member);
            _indexComboBox.Items.Add(member);
            _memberListBox.SelectedItem = member;
            if (_currentMember != null)
                _currentMember.Hide();
            _currentMember = member;
            AddMember(member);
            member.IsNew = true;
            member.Show();
            OnValueChange();
        }
        private void OnNameValueChange(object sender, EventArgs e)
        {
            OnValueChange();
            if (!_isInit) return;

            var nameBox = sender as TextBox;
            var cls = GetWrap<ClassWrap>();
            var nsw = cls.Namespace;
            if (!Util.CheckName(cls.Name))
                nameBox.Text = cls.Name;
            else if (nameBox.Text != cls.Name && nsw.Contains(nameBox.Text))
            {
                Util.MsgWarning("命名空间{0}中重复定义类型{1}!", nsw.FullName, nameBox.Text);
                nameBox.Text = cls.Name;
            }
        }
        private void OnDataPathChange(object sender, EventArgs e)
        {
            OnValueChange();
            var textBox = sender as TextBox;
            _indexComboBox.Enabled = !textBox.Text.IsEmpty();
            if (!_indexComboBox.Enabled)
                _indexComboBox.Text = "";
        }
        private void MemFilterBox_TextChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;
            var find = sender as TextBox;
            FindMember(find.Text);
        }
        private void GroupButton_Click(object sender, EventArgs e)
        {
            GroupDock.Ins.ShowGroups(_groupTextBox);
        }
    }
}
