using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desc
{
    public partial class GridWindow : Form
    {
        public GridWindow()
        {
            InitializeComponent();
        }

        #region DataGridView拖拽功能
        int selectionIdx = 0;
        private void MemberList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void MemberList_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))
            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))
                    MemberList.DoDragDrop(MemberList.Rows[e.RowIndex], DragDropEffects.Move);
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
                MemberList.Rows.Remove(row);
                selectionIdx = idx;
                MemberList.Rows.Insert(idx, row);
            }
        }
        private int GetRowFromPoint(int x, int y)
        {
            for (int i = 0; i < MemberList.RowCount; i++)
            {
                Rectangle rec = MemberList.GetRowDisplayRectangle(i, false);
                if (MemberList.RectangleToScreen(rec).Contains(x, y))
                    return i;
            }
            return -1;
        }
        private void MemberList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (selectionIdx > -1)
            {
                MemberList.Rows[selectionIdx].Selected = true;
                MemberList.CurrentCell = MemberList.Rows[selectionIdx].Cells[0];
            }
        }
        #endregion

        private void MemberList_Click(object sender, EventArgs e)
        {
            var rows = MemberList.Rows;
        }
    }
}
