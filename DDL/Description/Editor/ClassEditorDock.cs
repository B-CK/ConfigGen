using Description.Wrap;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            _namespaceComboBox.Items.AddRange(NamespaceWrap.Array);
            _inheritComboBox.Items.Add(string.Empty);
            _inheritComboBox.Items.AddRange(ClassWrap.Array);
            _inheritComboBox.Items.Remove(wrap);

            _nameTextBox.Text = wrap.Name;
            _namespaceComboBox.SelectedItem = wrap.Namespace;
            if (!wrap.Inherit.IsEmpty())
            {
                if (!ClassWrap.Dict.ContainsKey(wrap.Inherit))
                    Debug.LogErrorFormat("[Class]类型{0}的父类{1}类型不存在!", wrap.FullName, wrap.Inherit);
                else
                {
                    var inherit = ClassWrap.Dict[wrap.Inherit];
                    _inheritComboBox.SelectedItem = inherit;
                }
            }
            else
                _inheritComboBox.SelectedText = string.Empty;
            _groupTextBox.Text = wrap.Group.IsEmpty() ? Util.DefaultGroup : wrap.Group;
            _descTextBox.Text = wrap.Desc;
            _dataPathTextBox.Text = wrap.DataPath;

            var items = _memberListBox.Items;
            if (ClassWrap.Dict.ContainsKey(wrap.Inherit))
            {
                var parent = ClassWrap.Dict[wrap.Inherit];
                var pcurrent = parent.Fields.First;
                while (pcurrent != null)
                {
                    var fieldEditor = FieldEditor.Create(this, pcurrent.Value, true);
                    items.Add(fieldEditor);
                    AddMember(fieldEditor);
                    _indexComboBox.Items.Add(fieldEditor);
                    pcurrent = pcurrent.Next;
                }
            }
            string fieldName = wrap.Index ?? "";
            var current = wrap.Fields.First;
            while (current != null)
            {
                var fieldEditor = FieldEditor.Create(this, current.Value);
                items.Add(fieldEditor);
                AddMember(fieldEditor);
                _indexComboBox.Items.Add(fieldEditor);
                if (fieldName == fieldEditor.Name)
                    _indexComboBox.SelectedItem = fieldEditor;
                current = current.Next;
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
            var inherit = _inheritComboBox.SelectedItem as ClassWrap;
            cls.Inherit = inherit == null ? "" : inherit.FullName;
            if (ClassWrap.Dict.ContainsKey(cls.Inherit))
                cls.AddChildClass(ClassWrap.Dict[cls.Inherit]);
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
                var editor = item.Value as FieldEditor;
                if (editor.IsInherit) continue;
                if (editor.IsDelete)
                    cls.RemoveField(editor);
                else
                {
                    editor.Save();
                    cls.OverrideField(editor);
                }
            }
            cls.ResortField();
        }
        protected override void OnDiscard()
        {
            base.OnDiscard();
            if (!_isDirty) return;

            var cls = GetWrap<ClassWrap>();
            cls.RecoverField();
        }
        protected override void Clear()
        {
            base.Clear();
            _memberListBox.Items.Clear();
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (!IsInit) return;
            RefreshMember();
        }
        /// <summary>
        /// 刷新成员列表,以及索引列表
        /// 父类修改/添加/删除字段均需刷新列表
        /// </summary>
        public void RefreshMember()
        {
            _memberListBox.BeginUpdate();
            _indexComboBox.BeginUpdate();

            List<object> mbRemain = new List<object>();
            var mbItems = _memberListBox.Items;
            for (int i = 0; i < mbItems.Count; i++)
            {
                if (!(mbItems[i] as FieldEditor).IsInherit)
                    mbRemain.Add(mbItems[i]);
            }
            mbItems.Clear();

            List<object> iRemain = new List<object>();
            var iItems = _indexComboBox.Items;
            for (int i = 0; i < iItems.Count; i++)
            {
                if (!(iItems[i] as FieldEditor).IsInherit)
                    iRemain.Add(iItems[i]);
            }
            iItems.Clear();

            var cls = GetWrap<ClassWrap>();
            var parent = cls.Parent;
            if (parent != null)
            {
                var pcurrent = parent.Fields.First;
                while (pcurrent != null)
                {
                    var item = _memberDict[pcurrent.Value.Name];
                    pcurrent = pcurrent.Next;
                    mbItems.Add(item);
                    iItems.Add(item);
                }
            }
            mbItems.AddRange(mbRemain.ToArray());
            iItems.AddRange(iRemain.ToArray());

            _indexComboBox.EndUpdate();
            _memberListBox.EndUpdate();
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
        /// <summary>
        /// 查找字段
        /// </summary>
        private void FindMember(string name)
        {
            _memberListBox.Items.Clear();
            var cls = GetWrap<ClassWrap>();
            var parent = cls.Parent;
            if (parent != null)
            {
                var pcurrent = parent.Fields.First;
                while (pcurrent != null)
                {
                    var item = _memberDict[pcurrent.Value.Name];
                    pcurrent = pcurrent.Next;
                    if (item.IsDelete || item.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) == -1) continue;
                    _memberListBox.Items.Add(item);
                }
            }
            var current = cls.Fields.First;
            while (current != null)
            {
                var item = _memberDict[current.Value.Name];
                current = current.Next;
                if (item.IsDelete || item.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) == -1) continue;
                _memberListBox.Items.Add(item);
            }
        }

        private void OnValueChange(object sender, EventArgs e)
        {
            OnValueChange();
        }
        private void DataPathButton_Click(object sender, EventArgs e)
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
            var fieldEditor = member as FieldEditor;
            if (fieldEditor != null && fieldEditor.IsInherit)
                return;

            member.Show();
        }
        private void RemoveMenuItem_Click(object sender, EventArgs e)
        {
            var member = _memberListBox.SelectedItem as MemberEditor;
            _memberListBox.Items.Remove(member);
            _indexComboBox.Items.Remove(member);
            if (_currentMember == member)
                _currentMember = null;
            member.Clear();
            OnValueChange();
        }
        private void AddMenuItem_Click(object sender, EventArgs e)
        {
            var member = FieldEditor.Create(this, UnqueName);
            _memberListBox.Items.Add(member);
            _indexComboBox.Items.Add(member);
            _memberListBox.SelectedItem = member;
            if (_currentMember != null)
                _currentMember.Hide();
            _currentMember = member;
            AddMember(member);
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

        #region 字段移动按钮
        private void Down_Click(object sender, EventArgs e)
        {
            if (_currentMember == null) return;
            var from = _currentMember as FieldEditor;
            if (from.IsInherit) return;

            var cls = GetWrap<ClassWrap>();
            cls.DownField(from.FieldName);

            int index = _memberListBox.Items.IndexOf(from);
            if (index == _memberDict.Count - 1) return;
            var to = _memberListBox.Items[index + 1] as FieldEditor;
            _memberListBox.Items[index] = to;
            _memberListBox.Items[index + 1] = from;
            OnValueChange();
        }
        private void Up_Click(object sender, EventArgs e)
        {
            if (_currentMember == null) return;
            var from = _currentMember as FieldEditor;
            if (from.IsInherit) return;

            var cls = GetWrap<ClassWrap>();
            cls.UpField(from.FieldName);

            int index = _memberListBox.Items.IndexOf(from);
            if (index == 0) return;//非继承时
            if (cls.Parent != null && index == cls.Parent.Fields.Count)
                return;//继承时
            var to = _memberListBox.Items[index - 1] as FieldEditor;
            _memberListBox.Items[index] = to;
            _memberListBox.Items[index - 1] = from;
            OnValueChange();
        }
        private void Picture_MouseDown(object sender, MouseEventArgs e)
        {
            var picture = sender as PictureBox;
            picture.BackColor = Color.CornflowerBlue;
        }
        private void Picture_MouseUp(object sender, MouseEventArgs e)
        {
            var picture = sender as PictureBox;
            picture.BackColor = Color.FromArgb(56, 56, 56);
        }
        #endregion
    }
}
