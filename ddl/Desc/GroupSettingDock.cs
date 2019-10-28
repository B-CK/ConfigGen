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
    public partial class GroupSettingDock : Form
    {
        public static GroupSettingDock Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new GroupSettingDock();
                return _ins;
            }
        }
        private static GroupSettingDock _ins;


        private GroupSettingDock()
        {
            InitializeComponent();
        }

        public new void Show()
        {
            _groupGridView.Rows.Clear();
            var rows = _groupGridView.Rows;
            var groups = Util.Groups;
            for (int i = 0; i < groups.Length; i++)
                rows.Add(groups[i]);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            var rows = _groupGridView.Rows;
            string[] gs = new string[rows.Count];
            for (int i = 0; i < rows.Count; i++)
            {
                var value = rows[i].Cells[0].Value as string;
                if (!value.IsEmpty())
                    gs[i] = value;
            }
            Util.Groups = gs;
            Close();
        }
        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
