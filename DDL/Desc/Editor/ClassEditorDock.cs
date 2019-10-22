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
        //当前选择对象所在行
        int _selectionIndex = 0;
        ClassWrap _inherit;
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
            if (!wrap.Inherit.IsEmpty() && ModuleWrap.Current.Classes.ContainsKey(wrap.Inherit))
            {
                _inherit = ModuleWrap.Current.Classes[wrap.Inherit];
                _inherit.OnDataChange += OnParentChange;
                var pfields = _inherit.Fields;
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
                    if (rows[i].IsNewRow) continue;
                    var field = rows[i].Tag as FieldWrap;
                    if (SurportIndexKey(field.Type))
                        _indexComboBox.Items.Add(field);
                }
                _indexComboBox.Enabled = true;
            }

            //填充内容
            _nameTextBox.Text = wrap.Name;
            _indexComboBox.SelectedText = wrap.Index ?? "";
            _namespaceComboBox.SelectedItem = wrap.Namespace;
            if (_inherit != null)
                _inheritComboBox.SelectedItem = _inherit;
            else
                _inheritComboBox.SelectedText = string.Empty;
            _groupTextBox.Text = wrap.Group.IsEmpty() ? Util.DefaultGroup : wrap.Group;
            _descTextBox.Text = wrap.Desc;
            _dataPathTextBox.Text = wrap.DataPath;

            _fieldNameLib.DisplayIndex = 0;
            _fieldTypeLib.DisplayIndex = 1;
            _elememtLib.DisplayIndex = 2;
            _descLib.DisplayIndex = 3;
            _fieldGroup.DisplayIndex = 4;
            _checkerLib.DisplayIndex = 5;

            ModuleWrap.Current.OnAddNamespace += OnAddNamespace;
            ModuleWrap.Current.OnRemoveNamespace += OnRemoveNamespace;
            ModuleWrap.Current.OnAddAnyType += OnAddAnyType;
            ModuleWrap.Current.OnRemoveAnyType += OnRemoveAnyType;
            ModuleWrap.Current.OnNamespaceNameChange += OnNamespaceNameChange;
            ModuleWrap.Current.OnTypeNameChange += OnTypeNameChange;
        }
        protected override void Clear()
        {
            base.Clear();

            ModuleWrap.Current.OnAddNamespace -= OnAddNamespace;
            ModuleWrap.Current.OnRemoveNamespace -= OnRemoveNamespace;
            ModuleWrap.Current.OnAddAnyType -= OnAddAnyType;
            ModuleWrap.Current.OnRemoveAnyType -= OnRemoveAnyType;
            ModuleWrap.Current.OnNamespaceNameChange -= OnNamespaceNameChange;
            ModuleWrap.Current.OnTypeNameChange -= OnTypeNameChange;
            if (_inherit != null)
                _inherit.OnDataChange -= OnParentChange;
            _inherit = null;
            _selectionIndex = 0;
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
                if (rows[i].IsNewRow || rows[i].ReadOnly)
                    continue;

                var field = rows[i].Tag as FieldWrap;
                rows[i].Tag = field;
                SaveField(field, rows[i].Cells);
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
                    fields[i] = dict[field.Name];
                    hash.Add(fields[i].Name);
                }
            }
            List<FieldWrap> list = new List<FieldWrap>();
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].IsNewRow || rows[i].ReadOnly)
                    continue;

                var field = rows[i].Tag as FieldWrap;
                if (!hash.Contains(field.Name))
                    cls.AddField(field);
                list.Add(field);
            }
            cls.ResortField(list);
        }

        private void OnParentChange(ClassWrap inherit)
        {
            var rows = _memberList.Rows;
            var pfields = _inherit.Fields;
            for (int i = 0; i < pfields.Count; i++)
                rows.Insert(i, InitField(pfields[i], true));
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
            var row = PoolManager.Ins.Pop<DataGridViewRow>();
            if (row == null) row = new DataGridViewRow();
            string[] nodes = field.Type.Split(Util.ArgsSplitFlag);
            switch (nodes.Length)
            {
                case 1:
                    if (Util.BaseHash.Contains(field.Type) || EnumWrap.Dict.ContainsKey(field.Type))
                        row.CreateCells(_memberList, field.Name, field.Type, field.Value, field.Desc, field.Checker);
                    else if (ClassWrap.Dict.ContainsKey(field.Type))
                        row.CreateCells(_memberList, field.Name, field.Type, "", field.Desc, field.Checker);
                    break;
                case 2:
                    row.CreateCells(_memberList, field.Name, nodes[0], nodes[1], field.Desc, field.Checker);
                    break;
                case 3:
                    row.CreateCells(_memberList, field.Name, nodes[0], $"{nodes[1]}:{nodes[2]}", field.Desc, field.Checker);
                    break;
                default:
                    Debug.LogErrorFormat("未知类型:{0}", field.Type);
                    break;
            }
            row.Tag = field;
            row.ReadOnly = isHerit;
            for (int i = 0; i < row.Cells.Count; i++)
            {
                var style = row.Cells[i].Style;
                style.ForeColor = isHerit ? Color.Yellow : Color.LightGray;
            }
            return row;
        }
        protected void SaveField(FieldWrap field, DataGridViewCellCollection cells)
        {
            field.Name = cells[0].Value == null ? string.Empty : cells[0].Value as string;
            field.Type = cells[1].Value == null ? Util.BOOL : cells[1].Value as string;
            if (ClassWrap.Dict.ContainsKey(field.Type))
                field.Value = string.Empty;
            else
                field.Value = cells[2].Value == null ? string.Empty : cells[2].Value as string;
            field.Desc = cells[3].Value == null ? string.Empty : cells[3].Value as string;
            field.Group = cells[4].Value == null ? string.Empty : cells[4].Value as string;
            field.Checker = cells[5].Value == null ? string.Empty : cells[5].Value as string;
        }
        private void MemberList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isInit) return;
            //字段名称/类型改变
            if (e.ColumnIndex == 0 || e.ColumnIndex == 2)
            {
                var cls = GetWrap<ClassWrap>();
                var list = sender as DataGridView;
                var row = list.Rows[e.RowIndex];
                var cell = row.Cells[e.ColumnIndex];
                if (row.Tag == null)
                    row.Tag = FieldWrap.Create($"_{row.Cells.Count}", GetWrap<ClassWrap>());
                var field = row.Tag as FieldWrap;
                switch (e.ColumnIndex)
                {
                    case 0:
                        string fieldName = cell.Value as string;
                        if (cls.Contains(fieldName))
                            cell.Value = field.Name;
                        else
                            field.Name = fieldName;
                        var typeCell = row.Cells[_fieldTypeLib.DisplayIndex];
                        var type = typeCell.Value as string;
                        if (type.IsEmpty())
                            typeCell.Value = Util.BOOL;
                        break;
                    case 2:
                        field.Type = cell.Value as string;
                        break;
                }
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
            OnValueChange();
        }
        private void MemberList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Debug.Log($"[DoubleClick]:Row:{e.RowIndex}\tCol:{e.ColumnIndex}");
        }
        private void MemberList_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            var row = _memberList.Rows[e.RowIndex];
            if (row.IsNewRow) return;
            Debug.LogError($"Delete:{e.RowIndex}-{e.RowCount}");
            OnValueChange();
        }
        //--DataGridView拖拽功能
        private void MemberList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void MemberList_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left) && e.RowIndex > -1)
            {
                var row = _memberList.Rows[e.RowIndex];
                if (row.ReadOnly || row.IsNewRow) return;
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
                var toRow = _memberList.Rows[idx];
                var fromRow = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
                if (fromRow.IsNewRow || toRow.ReadOnly) return;
                _memberList.Rows.Remove(fromRow);
                if (idx >= _memberList.Rows.Count)
                    idx = _memberList.Rows.Count - 1;
                _selectionIndex = idx;
                _memberList.Rows.Insert(idx, fromRow);
                OnValueChange();
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
        //--------
        #endregion

        #region GUI事件
        private void OnValueChange(object sender, EventArgs e)
        {
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
        private void OnInheritChange(object sender, EventArgs e)
        {
            if (!IsInit) return;
            var rows = _memberList.Rows;
            if (_inherit != null)
            {
                _inherit.OnDataChange -= OnParentChange;
                var pfields = _inherit.Fields;
                for (int i = 0; i < pfields.Count; i++)
                {
                    PoolManager.Ins.Push(rows[0]);
                    rows.RemoveAt(0);
                }
            }
            _inherit = _inheritComboBox.SelectedItem as ClassWrap;
            if (_inherit != null)
            {
                _inherit.OnDataChange += OnParentChange;
                OnParentChange(_inherit);
            }
            OnValueChange();
        }
        private void GroupButton_Click(object sender, EventArgs e)
        {
            GroupDock.Ins.ShowGroups(_groupTextBox);
        }
        private void OnDataPathChange(object sender, EventArgs e)
        {
            OnValueChange();
            var textBox = sender as TextBox;
            _indexComboBox.Enabled = !textBox.Text.IsEmpty();
            if (!_indexComboBox.Enabled)
                _indexComboBox.Text = "";
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
        #endregion

    }
}
