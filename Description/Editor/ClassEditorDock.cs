﻿using Description.Wrap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Description.Editor
{
    //名称:修改后需要更新其他类父类名称

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

        MemberEditor _currentMember;
        private ClassEditorDock()
        {
            InitializeComponent();
            Show(MainWindow.Ins._dockPanel, DockState.Document);
        }
        protected override void OnInit(BaseWrap arg)
        {
            base.OnInit(arg);

            var wrap = GetWrap<ClassWrap>();
            _namespaceComboBox.Items.AddRange(NamespaceWrap.Namespaces);
            _inhertComboBox.Items.Add("");
            _inhertComboBox.Items.AddRange(ClassWrap.Classes);
            _inhertComboBox.Items.Remove(wrap);

            _nameTextBox.Text = wrap.Name;
            _namespaceComboBox.Text = wrap.Namespace.FullName.IsEmpty() ? "@" : wrap.Namespace.FullName;
            _inhertComboBox.Text = wrap.Inherit.IsEmpty() ? "" : wrap.Inherit;
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
            if (cls.Name != _nameTextBox.Text)
            {
                string name = cls.Name;
                cls.Name = _nameTextBox.Text;
                UpdateTreeNode(name);
            }
            var index = _indexComboBox.SelectedItem as FieldEditor;
            cls.Index = index == null ? "" : index.Name;
            cls.Inherit = _inhertComboBox.Text;
            cls.Desc = _descTextBox.Text;
            cls.DataPath = _dataPathTextBox.Text;

            SetNamespace<ClassWrap>(cls.Namespace.Name, _namespaceComboBox.Text);

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
            if (_isDirty)
                cls.Namespace.SetDirty();
        }
        protected override void ValidateData()
        {
            base.ValidateData();
            string dataPath = Util.GetDataDirAbsPath(_dataPathTextBox.Text);
            bool hasDataPath = File.Exists(dataPath);
            StringBuilder builder = new StringBuilder();
            if (hasDataPath && _indexComboBox.Text.IsEmpty())
                builder.AppendFormat("数据表{0}未指定关键字!\n", ID);
            if (!_dataPathTextBox.Text.IsEmpty() && !hasDataPath)
                builder.AppendFormat("数据路径{0}不存在!\n", dataPath);
            if (!_inhertComboBox.Text.IsEmpty() &&
                !ClassWrap.ClassDict.ContainsKey(_inhertComboBox.Text))
                builder.AppendFormat("类型{0}的父类{1}不存在!\n", ID, _inhertComboBox.Text);
            if (builder.Length > 0)
                Util.MsgWarning(builder.ToString());
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

            //修改关键字CombBox界面
            var iItems = _indexComboBox.Items;
            index = iItems.IndexOf(member);
            int selectedIndex = _indexComboBox.SelectedIndex;
            iItems.Remove(member);
            iItems.Insert(index, member);
            _indexComboBox.SelectedIndex = selectedIndex;
        }

        private void OnValueChange(object sender, System.EventArgs e)
        {
            OnValueChange();
        }
        private void DataPathButton_Click(object sender, System.EventArgs e)
        {
            Util.Open(Util.DataDir, "引用数据文件", "数据文件|*.xml", (string file) =>
            {
                _dataPathTextBox.Text = Util.GetDataDirRelPath(file);
            }, "获取数据文件路径失败!");
        }
        private void MemberListBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var point = _memberListBox.PointToScreen(e.Location);
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
            if (nameBox.Text != cls.Name && nsw.Contains(nameBox.Text))
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
    }
}
