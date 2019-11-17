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
            var namespaces = ModuleWrap.Current.GetNamespaces();
            var classes = ModuleWrap.Current.GetClasses();
            //命名空间
            if (ModuleWrap.Current.Namespaces.Count != _namespaceComboBox.Items.Count)
            {
                _namespaceComboBox.Items.Clear();
                _namespaceComboBox.Items.AddRange(namespaces);
            }
            //继承类型
            if (ModuleWrap.Current.Classes.Count != _inheritComboBox.Items.Count)
            {
                _inheritComboBox.Items.Clear();
                _inheritComboBox.Items.Add(string.Empty);
                _inheritComboBox.Items.AddRange(classes);
                _inheritComboBox.Items.Remove(GetWrap<ClassWrap>());
            }
            //设置字段
            _fieldTypeLib.Items.AddRange(Util.GetAllTypes());
            var rows = _memberList.Rows;
            if (!wrap.Inherit.IsEmpty() && ModuleWrap.Current.Classes.ContainsKey(wrap.Inherit))
            {
                _inherit = ModuleWrap.Current.Classes[wrap.Inherit];
                _inherit.OnWrapChange += OnParentChange;
                var pfields = _inherit.Fields;
                for (int i = 0; i < pfields.Count; i++)
                    rows.Add(InitField(pfields[i].Clone(), true));
            }
            var fields = wrap.Fields;
            for (int i = 0; i < fields.Count; i++)
                rows.Add(InitField(fields[i].Clone(), false));
            //设置数据路径
            int selectedIndex = -1;
            if (wrap.DataPath.IsEmpty())
                _indexComboBox.Enabled = false;
            else
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    if (rows[i].IsNewRow) continue;
                    var field = rows[i].Tag as FieldWrap;
                    if (SurportIndexKey(field.Type))
                    {
                        _indexComboBox.Items.Add(field);
                        if (wrap.Index == field.Name)
                            selectedIndex = _indexComboBox.Items.Count - 1;
                    }
                }
                _indexComboBox.Enabled = true;
            }

            //填充内容
            _nameTextBox.Text = wrap.Name;
            _indexComboBox.SelectedIndex = selectedIndex;
            _namespaceComboBox.SelectedItem = wrap.Namespace;
            if (_inherit != null)
                _inheritComboBox.SelectedItem = _inherit;
            else
                _inheritComboBox.SelectedText = string.Empty;
            _groupTextBox.Text = wrap.Group.IsEmpty() ? Util.DefaultGroup : wrap.Group;
            _descTextBox.Text = wrap.Desc;
            string absPath = Util.GetModuleAbsPath(wrap.DataPath);
            _dataPathTextBox.Text = Util.GetDataDirRelPath(absPath) ;

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
                _inherit.OnWrapChange -= OnParentChange;
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
            var absPath = Util.GetDataDirAbsPath(_dataPathTextBox.Text);
            cls.DataPath = Util.GetModuleRelPath(absPath);
            cls.Name = _nameTextBox.Text;
            cls.Desc = _descTextBox.Text;

            //切换命名空间
            SwapNamespace(cls.Namespace, _namespaceComboBox.SelectedItem as NamespaceWrap);

            var rows = _memberList.Rows;
            var fields = cls.Fields.ToArray();
            for (int i = 0; i < fields.Length; i++)
                cls.RemoveField(fields[i]);
            List<int> indexs = new List<int>();
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].IsNewRow || rows[i].ReadOnly) continue;
                if (rows[i].Cells[0].Value == null)
                {
                    indexs.Add(i);
                    return;
                }
                var field = rows[i].Tag as FieldWrap;
                if (field == null) continue;
                SaveField(field, rows[i].Cells);
                cls.AddField(field);
            }
            for (int i = 0; i < indexs.Count; i++)
                Debug.LogWarning($"[ClassDock]第{indexs[i]}行字段未命名,数据无效将无法保存!");
            cls.OnSave();
        }

        private void OnParentChange(TypeWrap inherit)
        {
            var removes = new List<DataGridViewRow>();
            var rows = _memberList.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].ReadOnly)
                    removes.Add(rows[i]);
            }
            _memberList.CellEndEdit -= MemberList_CellEndEdit;
            for (int i = 0; i < removes.Count; i++)
                rows.Remove(removes[i]);
            var pfields = _inherit.Fields;
            for (int i = 0; i < pfields.Count; i++)
                rows.Insert(i, InitField(pfields[i].Clone(), true));
            _memberList.CellEndEdit += MemberList_CellEndEdit;
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
            _inheritComboBox.Items.Add(wrap.FullName);
            _fieldTypeLib.Items.Add(wrap.FullName);
        }
        private void OnRemoveAnyType(TypeWrap wrap)
        {
            _inheritComboBox.Items.Remove(wrap.FullName);
            _fieldTypeLib.Items.Remove(wrap.FullName);
        }
        private void OnNamespaceNameChange(NamespaceWrap wrap, string src)
        {
            //修改命名空间列表
            var myItem = _namespaceComboBox.SelectedItem;
            var items = _namespaceComboBox.Items;
            items.Remove(wrap);
            items.Add(wrap);
            _namespaceComboBox.SelectedItem = myItem;

            //修改继承类列表|字段类型列表
            var classes = wrap.Classes;
            for (int i = 0; i < classes.Count; i++)
            {
                var cls = classes[i];
                string srcFullName = $"{src}.{cls.Name}";
                OnTypeNameChange(cls, srcFullName);
            }
        }
        private void OnTypeNameChange(BaseWrap wrap, string src)
        {
            //名称修改时排除触发自己
            if (wrap == _wrap)
            {
                UpdateDock(src);
                return;
            }

            if (wrap is ClassWrap)
            {
                //修改继承类列表
                var srcInherit = _inheritComboBox.SelectedItem;
                var inherits = _inheritComboBox.Items;
                inherits.Remove(wrap);
                inherits.Add(wrap);
                _inheritComboBox.SelectedItem = srcInherit;
            }

            //修改字段类型列表
            var fTypes = _fieldTypeLib.Items;
            fTypes.Add(wrap.FullName);
            var rows = _memberList.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].IsNewRow || rows[i].ReadOnly) continue;

                var cells = rows[i].Cells;
                var type = cells[_fieldTypeLib.DisplayIndex].Value as string;
                if (type == src)
                    cells[_fieldTypeLib.DisplayIndex].Value = wrap.FullName;
            }
            fTypes.Remove(src);
            OnValueChange();
        }
        /// <summary>
        /// 键所支持的类型
        /// </summary>
        private bool SurportIndexKey(string type)
        {
            return type == Util.INT || type == Util.LONG || type == Util.STRING || EnumWrap.Dict.ContainsKey(type);
        }

        #region 字段设置
        protected DataGridViewRow InitField(FieldWrap field, bool isHerit = false)
        {
            var row = PoolManager.Ins.Pop<DataGridViewRow>();
            if (row == null) row = new DataGridViewRow();
            string type = field.Type;
            string group = field.Group.IsEmpty() ? Util.DefaultGroup : field.Group;
            if (type.IndexOf(Util.LIST) != 0 && type.IndexOf(Util.DICT) != 0)
            {
                if (Util.HasType(type))
                    row.CreateCells(_memberList, field.Name, type, "##", field.Desc, group, field.Checker);
                else
                    row.CreateCells(_memberList, field.Name, null, "##", field.Desc, group, field.Checker);
            }
            else
            {
                string[] nodes = type.Split(Util.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
                switch (nodes.Length)
                {
                    case 2:
                        row.CreateCells(_memberList, field.Name, nodes[0], nodes[1], field.Desc, group, field.Checker);
                        break;
                    case 3:
                        row.CreateCells(_memberList, field.Name, nodes[0], $"{nodes[1]}:{nodes[2]}", field.Desc, group, field.Checker);
                        break;
                    default:
                        Debug.LogError($"[ClassDock]{GetWrap<ClassWrap>().FullName}的复合类型字段解析异常:{field}");
                        break;
                }
            }
            row.Tag = field;
            row.ReadOnly = isHerit;
            for (int i = 0; i < row.Cells.Count; i++)
            {
                var style = row.Cells[i].Style;
                style.ForeColor = isHerit ? Color.Yellow : Color.LightGray;
            }
            AddMemeber(field.Name);
            return row;
        }
        protected void SaveField(FieldWrap field, DataGridViewCellCollection cells)
        {
            var name = cells[_fieldNameLib.DisplayIndex].Value;
            var type = cells[_fieldTypeLib.DisplayIndex].Value;
            var value = cells[_elememtLib.DisplayIndex].Value;
            var desc = cells[_descLib.DisplayIndex].Value;
            var group = cells[_fieldGroupLib.DisplayIndex].Value;
            var checker = cells[_checkerLib.DisplayIndex].Value;

            field.Name = name == null ? string.Empty : name as string;
            field.Type = type == null ? Util.BOOL : type as string;
            if (field.Type.IndexOf(Util.LIST) == 0 || field.Type.IndexOf(Util.DICT) == 0)
                field.Type += $":{value}";
            field.Desc = desc == null ? string.Empty : desc as string;
            field.Group = group == null ? string.Empty : group as string;
            field.Checker = checker == null ? string.Empty : checker as string;
        }
        private void MemberList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isInit || e.RowIndex < 0) return;

            var cls = GetWrap<ClassWrap>();
            var list = sender as DataGridView;
            var row = list.Rows[e.RowIndex];
            var cell = row.Cells[e.ColumnIndex];
            if (row.Tag == null)
                row.Tag = FieldWrap.Create(UnqueName, cls);
            var field = row.Tag as FieldWrap;
            //字段名称/类型改变
            switch (e.ColumnIndex)
            {
                case 0:
                    string fieldName = cell.Value as string;
                    if (fieldName == field.Name) break;
                    if (ContainMember(fieldName))
                    {
                        Util.MsgWarning("[ClassDock]字段名重复:{0}", field.Name);
                        cell.Value = field.Name;
                        return;
                    }
                    field.Name = fieldName;
                    break;
                case 1:
                    string fieldType = cell.Value as string;
                    if (fieldType != Util.LIST && fieldType != Util.DICT)
                        row.Cells[_elememtLib.DisplayIndex].Value = "##";
                    else
                        row.Cells[_elememtLib.DisplayIndex].Value = "";
                    field.Type = fieldType;
                    break;
                default:
                    break;
            }

            var typeCell = row.Cells[_fieldNameLib.DisplayIndex];
            var type = typeCell.Value as string;
            if (type.IsEmpty())
                typeCell.Value = Util.BOOL;

            //添加类型支持的字段
            if (_indexComboBox.Enabled)
            {
                var items = _indexComboBox.Items;
                if (SurportIndexKey(field.Type))
                {
                    items.Remove(field);
                    items.Add(field);
                }
                else
                    items.Remove(field);
            }
            var groupCell = row.Cells[_fieldGroupLib.DisplayIndex];
            var group = groupCell.Value as string;
            if (group.IsEmpty())
                groupCell.Value = Util.DefaultGroup;

            OnValueChange();
        }
        //修改集合包含类型|字段组信息
        private void MemberList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = _memberList.Rows[e.RowIndex];
            if (row.IsNewRow && row.ReadOnly) return;

            var cells = row.Cells;
            if (e.ColumnIndex == _elememtLib.DisplayIndex)
            {
                var type = cells[_fieldTypeLib.DisplayIndex].Value as string;
                if (type == Util.LIST || type == Util.DICT)
                    ElementEditor.Ins.Show(cells, e.ColumnIndex, type, OnValueChange);
            }
            else if (e.ColumnIndex == _fieldGroupLib.DisplayIndex)
            {
                var groups = cells[_fieldGroupLib.DisplayIndex].Value as string;
                GroupDock.Ins.ShowGroups(groups, (gs) => cells[_fieldGroupLib.DisplayIndex].Value = gs);
            }
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
            if (_selectionIndex > -1 && _isInit)
            {
                _selectionIndex = e.RowIndex;
                if (!_memberList.Rows[_selectionIndex].IsNewRow)
                {
                    _memberList.Rows[_selectionIndex].Selected = true;
                    _memberList.CurrentCell = _memberList.Rows[_selectionIndex].Cells[0];
                }
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
                Util.MsgWarning("[ClassDock]命名空间{0}中重复定义类型{1}!", nsw.FullName, nameBox.Text);
                nameBox.Text = cls.Name;
            }
        }
        private void GroupButton_Click(object sender, EventArgs e)
        {
            GroupDock.Ins.ShowGroups(_groupTextBox.Text, (gs) => _groupTextBox.Text = gs);
        }
        private void OnInheritChange(object sender, EventArgs e)
        {
            if (!IsInit) return;
            var rows = _memberList.Rows;
            if (_inherit != null)
            {
                _inherit.OnWrapChange -= OnParentChange;
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
                _inherit.OnWrapChange += OnParentChange;
                OnParentChange(_inherit);
            }
            OnValueChange();
        }
        private void OnDataPathChange(object sender, EventArgs e)
        {
            if (_isInit == false) return;
            OnValueChange();
            var textBox = sender as TextBox;
            _indexComboBox.Enabled = !textBox.Text.IsEmpty();
            //if (!_indexComboBox.Enabled)
            //    _indexComboBox.Text = "";

            _indexComboBox.Items.Clear();
            var rows = _memberList.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].IsNewRow) continue;
                var field = rows[i].Tag as FieldWrap;
                if (SurportIndexKey(field.Type))
                    _indexComboBox.Items.Add(field);
            }
        }
        private void DataPathButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog.InitialDirectory = Util.DataDir;
                OpenFileDialog.Title = "引用数据文件";
                OpenFileDialog.Filter = "表|*.xls*";
                OpenFileDialog.RestoreDirectory = true;
                DialogResult result = OpenFileDialog.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    _dataPathTextBox.Text = "";
                    _indexComboBox.SelectedIndex = -1;
                    return;
                }

                _dataPathTextBox.Text = Util.GetDataDirRelPath(OpenFileDialog.FileName);
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("[ClassDock]获取数据文件路径失败!{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// 删除选择行
        /// </summary>
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_isInit) return;
            var cells = _memberList.SelectedCells;
            var removes = new HashSet<DataGridViewRow>();
            for (int i = 0; i < cells.Count; i++)
            {
                if (!removes.Contains(cells[i].OwningRow))
                    removes.Add(cells[i].OwningRow);
            }
            foreach (var row in removes)
            {
                var field = row.Tag as FieldWrap;
                if (row.IsNewRow) continue;
                if (field != null)
                    RemoveMember(field.Name);
                _memberList.Rows.Remove(row);
            }
            if (removes.Count > 0)
                OnValueChange();
        }

        #endregion
    }
}
