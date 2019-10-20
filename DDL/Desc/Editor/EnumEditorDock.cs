using Desc.Wrap;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Desc.Editor
{
    public partial class EnumEditorDock : EditorDock
    {
        public static EnumEditorDock Create(EnumWrap wrap)
        {
            var dock = PoolManager.Ins.Pop<EnumEditorDock>();
            if (dock == null) dock = new EnumEditorDock();
            dock.Init(wrap);
            dock.Text = wrap.Name;
            dock._typeGroupBox.Text = "枚举[Enum]";
            return dock;
        }


        List<MemberEditor> _listBox = new List<MemberEditor>();
        MemberEditor _currentMember;
        private EnumEditorDock()
        {
            InitializeComponent();
            Show(MainWindow.Ins._dockPanel, DockState.Document);
        }

        protected override void OnInit(TypeWrap arg)
        {
            base.OnInit(arg);

            var wrap = GetWrap<EnumWrap>();
            _namespaceComboBox.Items.AddRange(NamespaceWrap.Array);

            _nameTextBox.Text = wrap.Name;
            _namespaceComboBox.SelectedItem = wrap.Namespace;
            _groupTextBox.Text = wrap.Group.IsEmpty() ? Util.DefaultGroup : wrap.Group;
            _descTextBox.Text = wrap.Desc;

            var enums = wrap.Items;
            var items = _memberListBox.Items;
            for (int i = 0; i < enums.Count; i++)
            {
                var itemEditor = EnumItemEditor.Create(this, enums[i]);
                items.Add(itemEditor);
                AddMember(itemEditor);
            }
        }
        protected override void OnSave()
        {
            base.OnSave();
            var enm = GetWrap<EnumWrap>();
            enm.Group = _groupTextBox.Text;
            if (enm.Name != _nameTextBox.Text || enm.Desc != _descTextBox.Text)
            {
                string fullName = enm.FullName;
                enm.Name = _nameTextBox.Text;
                enm.Desc = _descTextBox.Text;
                UpdateDock(fullName);
            }

            SwapNamespace(enm.Namespace, _namespaceComboBox.SelectedItem as NamespaceWrap);

            foreach (var item in _memberDict)
            {
                var editor = item.Value as EnumItemEditor;
                editor.Save();
                enm.OverrideField(editor);
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
            if (_memberListBox.SelectedItem == null) return;
            var member = _memberListBox.SelectedItem as MemberEditor;
            _memberListBox.Items.Remove(member);
            if (_currentMember == member)
                _currentMember = null;
            member.Clear();
            OnValueChange();
        }
        private void AddMenuItem_Click(object sender, System.EventArgs e)
        {
            var member = EnumItemEditor.Create(this, UnqueName);
            _memberListBox.Items.Add(member);
            _memberListBox.SelectedItem = member;
            if (_currentMember != null)
                _currentMember.Hide();
            _currentMember = member;
            AddMember(member);
            member.Show();
            OnValueChange();
        }
        private void OnValueChange(object sender, System.EventArgs e)
        {
            OnValueChange();
        }
        private void OnNameValueChange(object sender, EventArgs e)
        {
            OnValueChange();
            if (!_isInit) return;

            var nameBox = sender as TextBox;
            var enm = GetWrap<EnumWrap>();
            var nsw = enm.Namespace;
            if (!Util.CheckName(enm.Name))
                nameBox.Text = enm.Name;
            else if (nameBox.Text != enm.Name && nsw.Contains(nameBox.Text))
            {
                Util.MsgWarning("命名空间{0}中重复定义类型{1}!", nsw.FullName, nameBox.Text);
                nameBox.Text = enm.Name;
            }
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
