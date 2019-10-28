using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Desc.Editor
{
    public partial class GroupDock : Form
    {
        public static GroupDock Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new GroupDock();
                return _ins;
            }
        }
        static GroupDock _ins;

        Action<string> _okEvt;
        private GroupDock()
        {
            _ins = this;
            InitializeComponent();
        }

        public void ShowGroups(string gs, Action<string> okEvt)
        {
            gs = gs ?? Util.DefaultGroup;
            _okEvt = okEvt;
            HashSet<string> hash = new HashSet<string>(Util.Groups);
            for (int i = 0; i < Util.Groups.Length; i++)
            {
                var check = PoolManager.Ins.Pop<CheckBox>();
                if (check == null)
                    check = new CheckBox();
                check.Text = Util.Groups[i];
                check.Name = Util.Groups[i];
                check.Checked = false;
                _flowLayoutPanel.Controls.Add(check);
            }
            string[] nodes = gs.Split(Util.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < nodes.Length; i++)
            {
                if (_flowLayoutPanel.Controls.Count == 0)
                    break;
                string key = nodes[i];
                var check = _flowLayoutPanel.Controls[key] as CheckBox;
                check.Text = key;
                check.Checked = hash.Contains(key);
            }
            ShowDialog();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            var ctrls = _flowLayoutPanel.Controls;
            for (int i = 0; i < ctrls.Count; i++)
            {
                var check = ctrls[i] as CheckBox;
                PoolManager.Ins.Push(check);
            }
            ctrls.Clear();
        }
        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void OKButton_Click(object sender, EventArgs e)
        {
            var builder = new StringBuilder();
            var ctrls = _flowLayoutPanel.Controls;
            for (int i = 0; i < ctrls.Count; i++)
            {
                var check = ctrls[i] as CheckBox;
                if (check.Checked)
                    builder.AppendFormat("{0}{1}", check.Text, Util.ArgsSplitFlag[0]);
            }
            string result = builder.ToString();
            if (builder.Length > 0)
                result = result.Substring(0, result.Length - 1);
            _okEvt?.Invoke(result);
            Close();
        }
    }
}
