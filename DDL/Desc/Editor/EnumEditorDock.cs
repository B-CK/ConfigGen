using Desc.Wrap;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        int _selectionIndex = 0;
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
            var rows = _memberList.Rows;
            for (int i = 0; i < enums.Count; i++)
            {
                var itemRow = InitEnumItem(enums[i].Clone());
                rows.Add(itemRow);
            }

            WrapManager.Ins.Current.AddNamespaceEvent += OnAddNamespace;
            WrapManager.Ins.Current.RemoveNamespaceEvent += OnRemoveNamespace;
            WrapManager.Ins.Current.OnNamespaceNameChange += OnNamespaceNameChange;

            wrap.OnNameChange += OnTypeNameChange;
        }
        protected override void OnSave()
        {
            base.OnSave();
            var enm = GetWrap<EnumWrap>();
            enm.Group = _groupTextBox.Text;
            enm.Name = _nameTextBox.Text;
            enm.Desc = _descTextBox.Text;

            SwapNamespace(enm.Namespace, _namespaceComboBox.SelectedItem as NamespaceWrap);

            var rows = _memberList.Rows;
            var items = enm.Items.ToArray();
            for (int i = 0; i < items.Length; i++)
                enm.RemoveItem(items[i]);
            List<int> indexs = new List<int>();
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].IsNewRow || rows[i].ReadOnly) continue;
                if (rows[i].Cells[0].Value == null)
                {
                    indexs.Add(i);
                    return;
                }
                var item = rows[i].Tag as EnumItemWrap;
                if (item == null) continue;
                SaveField(item, rows[i].Cells);
                enm.AddItem(item);
            }
            for (int i = 0; i < indexs.Count; i++)
                Debug.LogWarning($"[EnumDock]第{indexs[i]}行枚举项未命名,数据无效将无法保存!");
            enm.OnSave();
        }
        protected override void Clear()
        {
            base.Clear();
            _memberList.Rows.Clear();

            WrapManager.Ins.Current.AddNamespaceEvent -= OnAddNamespace;
            WrapManager.Ins.Current.RemoveNamespaceEvent -= OnRemoveNamespace;
            WrapManager.Ins.Current.OnNamespaceNameChange -= OnNamespaceNameChange;
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
        private void OnNamespaceNameChange(NamespaceWrap wrap, string src)
        {
            var myItem = _namespaceComboBox.SelectedItem;
            var items = _namespaceComboBox.Items;
            items.Remove(wrap);
            items.Add(wrap);
            _namespaceComboBox.SelectedItem = myItem;

            var enm = GetWrap<EnumWrap>();
            if (enm.Namespace == wrap)
            {
                string srcFullName = $"{src}.{enm.Name}";
                UpdateDock(srcFullName);
            }
        }
        private void OnTypeNameChange(BaseWrap wrap, string src)
        {
            if (wrap == _wrap)
                UpdateDock(src);
        }

        #region 字段设置
        protected DataGridViewRow InitEnumItem(EnumItemWrap item)
        {
            var row = PoolManager.Ins.Pop<DataGridViewRow>();
            if (row == null) row = new DataGridViewRow();
            row.CreateCells(_memberList, item.Name, item.Alias, item.Value, item.Desc, item.Group);
            row.Tag = item;
            AddMemeber(item.Name);
            return row;
        }
        protected void SaveField(EnumItemWrap item, DataGridViewCellCollection cells)
        {
            var name = cells[_fieldNameLib.DisplayIndex].Value;
            var alias = cells[_fieldAliasLib.DisplayIndex].Value;
            var value = cells[_fieldValueLib.DisplayIndex].Value;
            var desc = cells[_fieldDescLib.DisplayIndex].Value;
            var group = cells[_fieldGroupLib.DisplayIndex].Value;

            item.Name = name == null ? string.Empty : name as string;
            item.Alias = alias == null ? string.Empty : alias as string;
            if (value != null && int.TryParse(value as string, out int v))
                item.Value = v;
            else
                item.Value = 0;
            item.Desc = desc == null ? string.Empty : desc as string;
            item.Group = group == null ? string.Empty : group as string;
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
        private void MemberList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var enm = GetWrap<EnumWrap>();
            var list = sender as DataGridView;
            var row = list.Rows[e.RowIndex];
            var cell = row.Cells[e.ColumnIndex];
            if (row.Tag == null)
                row.Tag = EnumItemWrap.Create(UnqueName, enm);
            var item = row.Tag as EnumItemWrap;
            if (e.ColumnIndex == _fieldNameLib.DisplayIndex)
            {
                string itemName = cell.Value as string;
                if (ContainMember(itemName))
                {
                    Util.MsgWarning("[EnumDock]字段名重复:{0}", item.Name);
                    cell.Value = item.Name;
                    return;
                }
            }
            else if (e.ColumnIndex == _fieldValueLib.DisplayIndex)
            {
                string value = cell.Value as string;
                if (!int.TryParse(value, out int v))
                {
                    cell.Value = cell.RowIndex.ToString();
                    Util.MsgWarning($"[EnumDock]\"{value}\"非整型!");
                }
            }
            if (item.Group.IsEmpty())
            {
                var groupCell = row.Cells[_fieldGroupLib.DisplayIndex];
                var group = groupCell.Value as string;
                if (group.IsEmpty())
                    groupCell.Value = Util.DefaultGroup;
            }
            OnValueChange();
        }
        private void MemberList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var cells = _memberList.Rows[e.RowIndex].Cells;
            if (e.ColumnIndex == _fieldGroupLib.DisplayIndex)
            {
                var groups = cells[_fieldGroupLib.DisplayIndex].Value as string;
                GroupDock.Ins.ShowGroups(groups, (gs) => cells[_fieldGroupLib.DisplayIndex].Value = gs);
            }
        }
        #endregion

        #region GUI事件
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
                Util.MsgWarning("[EnumDock]命名空间{0}中重复定义类型{1}!", nsw.FullName, nameBox.Text);
                nameBox.Text = enm.Name;
            }
        }
        private void GroupButton_Click(object sender, EventArgs e)
        {
            GroupDock.Ins.ShowGroups(_groupTextBox.Text, (gs) => _groupTextBox.Text = gs);
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
