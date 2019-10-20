using Desc.Wrap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Desc.Editor
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
        //当前选择对象所在行
        int _selectionIndex = 0;
        private ClassEditorDock()
        {
            InitializeComponent();
            Show(MainWindow.Ins._dockPanel, DockState.Document);
        }
        protected override void OnInit(TypeWrap arg)
        {
            base.OnInit(arg);
            var wrap = GetWrap<ClassWrap>();
            //命名空间
            if (ModuleWrap.Current.Namespaces.Count != _namespaceComboBox.Items.Count)
            {
                _namespaceComboBox.Items.Clear();
                _namespaceComboBox.Items.AddRange(ModuleWrap.Current.GetNamespaces());
            }
            //继承类型
            if (ModuleWrap.Current.Classes.Count != _inheritComboBox.Items.Count)
            {
                _inheritComboBox.Items.Clear();
                _inheritComboBox.Items.Add(string.Empty);
                _inheritComboBox.Items.AddRange(ModuleWrap.Current.GetClasses());
                _inheritComboBox.Items.Remove(GetWrap<ClassWrap>());
            }
            //设置字段
            _fieldTypeLib.Items.AddRange(Util.GetAllTypes());
            _fieldTypeLib.DisplayIndex = 0;
            _fieldTypeLib.DisplayMember = "DisplayFullName";
            var rows = _memberList.Rows;
            if (!wrap.Inherit.IsEmpty() && ClassWrap.Dict.ContainsKey(wrap.Inherit))
            {
                var parent = ClassWrap.Dict[wrap.Inherit];
                var pfields = parent.Fields;
                for (int i = 0; i < pfields.Count; i++)
                    rows.Add(InitField(pfields[i], true));
            }
            var fields = wrap.Fields;
            for (int i = 0; i < fields.Count; i++)
                rows.Add(InitField(fields[i], false));
            //设置数据路径
            if (_dataPathTextBox.Text.IsEmpty())
                _indexComboBox.Enabled = false;
            else
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    var cells = rows[i].Cells;
                    if (cells != null && cells.Count > 1)
                    {
                        var field = cells[0].Tag as FieldWrap;
                        if (SurportIndexKey(field.Type))
                            _indexComboBox.Items.Add(field);
                    }
                }
                _indexComboBox.Enabled = true;
            }

            //填充内容
            _nameTextBox.Text = wrap.Name;
            _indexComboBox.SelectedText = wrap.Index ?? "";
            _namespaceComboBox.SelectedItem = wrap.Namespace;
            if (!wrap.Inherit.IsEmpty() && ModuleWrap.Current.Classes.ContainsKey(wrap.FullName))
            {
                var inherit = ClassWrap.Dict[wrap.Inherit];
                _inheritComboBox.SelectedItem = inherit;
            }
            else
                _inheritComboBox.SelectedText = string.Empty;
            _groupTextBox.Text = wrap.Group.IsEmpty() ? Util.DefaultGroup : wrap.Group;
            _descTextBox.Text = wrap.Desc;
            _dataPathTextBox.Text = wrap.DataPath;

            ModuleWrap.Current.OnAddNamespace += OnAddNamespace;
            ModuleWrap.Current.OnRemoveNamespace += OnRemoveNamespace;
            ModuleWrap.Current.OnAddAnyType += OnAddAnyType;
            ModuleWrap.Current.OnRemoveAnyType += OnRemoveAnyType;
            ModuleWrap.Current.OnNamespaceNameChange += OnNamespaceNameChange;
            ModuleWrap.Current.OnTypeNameChange += OnTypeNameChange;
        }
        protected override void OnSave()
        {
            base.OnSave();
            var cls = GetWrap<ClassWrap>();
            var index = _indexComboBox.SelectedItem as FieldWrap;
            cls.Index = index == null ? "" : index.Name;
            var inherit = _inheritComboBox.SelectedItem as ClassWrap;
            cls.Inherit = inherit == null ? "" : inherit.FullName;
            if (cls.Parent != null)
                cls.Parent.AddChildClass(cls);
            cls.Group = _groupTextBox.Text;
            cls.DataPath = _dataPathTextBox.Text;
            cls.Name = _nameTextBox.Text;
            cls.Desc = _descTextBox.Text;

            //切换命名空间
            SwapNamespace(cls.Namespace, _namespaceComboBox.SelectedItem as NamespaceWrap);
 
            Dictionary<string, FieldWrap> dict = new Dictionary<string, FieldWrap>();
            var rows = _memberList.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].IsNewRow)
                    continue;

                var cells = rows[i].Cells;
                var field = cells[0].Tag as FieldWrap;
                if (field == null)
                    field = FieldWrap.Create(cells[0].Value as string, cls);
                else if (field != null)
                    field = field.Clone();
                else if (field.Host != cls)
                    continue;

                cells[0].Tag = field;
                SaveField(field, cells);
                dict.Add(field.Name, field);
            }
            HashSet<string> hash = new HashSet<string>();
            var fields = cls.Fields;
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                if (!dict.ContainsKey(field.Name))
                    cls.RemoveField(field);
                else
                {
                    var wrap = dict[field.Name];
                    if (wrap != field)
                    {
                        fields[i].Dispose();
                        fields[i] = wrap;
                    }
                    hash.Add(wrap.Name);
                }
            }
            List<FieldWrap> list = new List<FieldWrap>();
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].IsNewRow)
                    continue;

                var cells = rows[i].Cells;
                var field = cells[0].Tag as FieldWrap;
                if (field.Host != cls) continue;
                if (!hash.Contains(field.Name))
                    cls.AddField(field);
                list.Add(field);
            }
            cls.ResortField(list);
        }

        private void OnParentFieldChange()
        {

        }
        private void OnAddNamespace(NamespaceWrap wrap)
        {
            var items = _namespaceComboBox.Items;
            items.Add(wrap);
        }
        private void OnRemoveNamespace(NamespaceWrap wrap)
        {
            if (wrap == _wrap.Namespace)
                return;

            var items = _namespaceComboBox.Items;
            items.Remove(wrap);
        }
        private void OnAddAnyType(TypeWrap wrap)
        {
            _inheritComboBox.Items.Add(wrap);
            _fieldTypeLib.Items.Add(wrap);
        }
        private void OnRemoveAnyType(TypeWrap wrap)
        {
            if (wrap == _wrap)
                return;

            _inheritComboBox.Items.Remove(wrap);
            _fieldTypeLib.Items.Remove(wrap);
        }
        private void OnNamespaceNameChange(NamespaceWrap wrap, string src)
        {
            var items = _namespaceComboBox.Items;
            items.Remove(wrap);
            items.Add(wrap);
        }
        private void OnTypeNameChange(BaseWrap wrap, string src)
        {
            if (wrap == _wrap) return;

            var items = _inheritComboBox.Items;
            items.Remove(wrap);
            items.Add(wrap);
            string srcFullName = wrap.FullName.Replace(wrap.Name, src);
            UpdateDock(srcFullName);
        }
        /// <summary>
        /// 键所支持的类型
        /// </summary>
        private bool SurportIndexKey(string type)
        {
            return type == Util.INT || type == Util.LONG || type == Util.STRING || EnumWrap.Dict.ContainsKey(type);
        }

        #region 字段初始化
        protected DataGridViewRow InitField(FieldWrap field, bool isHerit = false)
        {
            DataGridViewRow row = new DataGridViewRow();
            string[] nodes = field.Type.Split(Util.ArgsSplitFlag);
            switch (nodes.Length)
            {
                case 1:
                    if (Util.BaseHash.Contains(field.Type) || EnumWrap.Dict.ContainsKey(field.Type))
                        row.CreateCells(_memberList, field.Name, field.IsConst, field.Type, field.Value, field.Desc, field.Checker);
                    else if (ClassWrap.Dict.ContainsKey(field.Type))
                        row.CreateCells(_memberList, field.Name, field.IsConst, field.Type, "", field.Desc, field.Checker);
                    break;
                case 2:
                    row.CreateCells(_memberList, field.Name, field.IsConst, nodes[0], nodes[1], field.Desc, field.Checker);
                    break;
                case 3:
                    row.CreateCells(_memberList, field.Name, field.IsConst, nodes[0], $"{nodes[1]}:{nodes[2]}", field.Desc, field.Checker);
                    break;
                default:
                    Debug.LogErrorFormat("未知类型:{0}", field.Type);
                    break;
            }
            row.Frozen = isHerit;
            row.Cells[0].ToolTipText = field.DisplayName;
            row.Cells[0].Tag = field;
            return row;
        }
        protected void SaveField(FieldWrap field, DataGridViewCellCollection cells)
        {
            field.Name = cells[0].Value as string;
            field.IsConst = (bool)cells[1].Value;
            field.Type = cells[2].Value as string;
            if (ClassWrap.Dict.ContainsKey(field.Type))
                field.Value = "";
            else
                field.Value = cells[3].Value as string;
            field.Desc = cells[4].Value as string;
            field.Group = cells[5].Value as string;
            field.Checker = cells[6].Value as string;
        }
        private void MemberList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //字段名称/类型改变
            if (e.ColumnIndex == 0 || e.ColumnIndex == 2)
            {
                var list = sender as DataGridView;
                var cell = list.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var field = cell.Tag as FieldWrap;
                var items = _indexComboBox.Items;
                if (SurportIndexKey(field.Type))
                {
                    items.Remove(field);
                    items.Add(field);
                }
                else
                {
                    items.Remove(field);
                }
            }
        }

        //--DataGridView拖拽功能
        private void MemberList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void MemberList_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))
            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))
                    _memberList.DoDragDrop(_memberList.Rows[e.RowIndex], DragDropEffects.Move);
            }
        }
        private void MemberList_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint(e.X, e.Y);
            if (idx < 0) return;

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
                if (row.IsNewRow) return;
                _memberList.Rows.Remove(row);
                _selectionIndex = idx;
                _memberList.Rows.Insert(idx, row);
            }
        }
        private int GetRowFromPoint(int x, int y)
        {
            for (int i = 0; i < _memberList.RowCount; i++)
            {
                Rectangle rec = _memberList.GetRowDisplayRectangle(i, false);
                if (_memberList.RectangleToScreen(rec).Contains(x, y))
                    return i;
            }
            return -1;
        }
        private void MemberList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (_selectionIndex > -1)
            {
                _memberList.Rows[_selectionIndex].Selected = true;
                _memberList.CurrentCell = _memberList.Rows[_selectionIndex].Cells[0];

            }
        }
        //--
        #endregion


        #region 待修改或废弃的代码
        /// <summary>
        /// 刷新成员列表,以及索引列表
        /// 父类修改/添加/删除字段均需刷新列表
        /// </summary>
        public void RefreshMember()
        {
            //_memberListBox.BeginUpdate();
            //_indexComboBox.BeginUpdate();

            //List<object> mbRemain = new List<object>();
            //var mbItems = _memberListBox.Items;
            //for (int i = 0; i < mbItems.Count; i++)
            //{
            //    if (!(mbItems[i] as FieldEditor).IsInherit)
            //        mbRemain.Add(mbItems[i]);
            //}
            //mbItems.Clear();

            //List<object> iRemain = new List<object>();
            //var iItems = _indexComboBox.Items;
            //for (int i = 0; i < iItems.Count; i++)
            //{
            //    if (!(iItems[i] as FieldEditor).IsInherit)
            //        iRemain.Add(iItems[i]);
            //}
            //iItems.Clear();

            //var cls = GetWrap<ClassWrap>();
            //var parent = cls.Parent;
            //if (parent != null)
            //{
            //    var pcurrent = parent.Fields.First;
            //    while (pcurrent != null)
            //    {
            //        MemberEditor item = PoolManager.Ins.Pop<MemberEditor>();
            //        if (_memberDict.ContainsKey(pcurrent.Value.Name))
            //            item = _memberDict[pcurrent.Value.Name];
            //        else
            //        {
            //            if (item == null)
            //                item = FieldEditor.Create(this, pcurrent.Value, true);
            //            AddMember(item);
            //        }

            //        pcurrent = pcurrent.Next;
            //        mbItems.Add(item);
            //        iItems.Add(item);
            //    }
            //}
            //for (int i = 0; i < mbRemain.Count; i++)
            //{
            //    if (_memberDict.ContainsKey((mbRemain[i] as FieldEditor).Name))
            //    {
            //        mbItems.Add(mbRemain[i]);
            //        iItems.Add(iRemain[i]);
            //    }
            //}

            //_indexComboBox.EndUpdate();
            //_memberListBox.EndUpdate();
        }
        /// <summary>
        /// 更新单个
        /// </summary>
        /// <param name="member"></param>
        /// <param name="oldFullName"></param>
        public void RefreshMember(MemberEditor member, string oldFullName = null)
        {
            ////修改成员列表数据
            //if (!oldFullName.IsEmpty())
            //{
            //    RemoveMember(oldFullName);
            //    AddMember(member);
            //}

            ////修改成员列表界面
            //var mItems = _memberListBox.Items;
            //int index = mItems.IndexOf(member);
            //mItems.Remove(member);
            //mItems.Insert(index, member);
            ////筛选出后,再修改名称时,是否过滤掉?
            ////if (!_memFindBox.Text.IsEmpty())
            ////    FindMember(_memFindBox.Text);

            ////修改关键字CombBox界面
            //var iItems = _indexComboBox.Items;
            //index = iItems.IndexOf(member);
            //int selectedIndex = _indexComboBox.SelectedIndex;
            //iItems.Remove(member);
            //iItems.Insert(index, member);
            //_indexComboBox.SelectedIndex = selectedIndex;
        }
        #endregion

        #region GUI事件
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
        /// <summary>
        /// 字段移除
        /// </summary>
        private void RemoveMenuItem_Click(object sender, EventArgs e)
        {
            //var member = _memberList.SelectedItem as MemberEditor;
            //_memberList.Items.Remove(member);
            //_indexComboBox.Items.Remove(member);
            //if (_currentMember == member)
            //    _currentMember = null;
            //member.Clear();
            //OnValueChange();
        }
        private void AddMenuItem_Click(object sender, EventArgs e)
        {
            //var member = FieldEditor.Create(this, UnqueName);
            //_memberListBox.Items.Add(member);
            //_indexComboBox.Items.Add(member);
            //_memberListBox.SelectedItem = member;
            //if (_currentMember != null)
            //    _currentMember.Hide();
            //_currentMember = member;
            //AddMember(member);
            //member.Show();
            //OnValueChange();
        }
        private void OnInheritChange(object sender, EventArgs e)
        {
            if (!IsInit) return;
            RefreshMember();
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
        private void GroupButton_Click(object sender, EventArgs e)
        {
            GroupDock.Ins.ShowGroups(_groupTextBox);
        }
        #endregion

    


    }
}
