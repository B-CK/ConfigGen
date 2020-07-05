using Desc.Wrap;
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
    public partial class NamespaceInfoDock : Form
    {
        public static NamespaceInfoDock Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new NamespaceInfoDock();
                return _ins;
            }
        }
        private static NamespaceInfoDock _ins;

        private NamespaceInfoDock()
        {
            InitializeComponent();
        }

        NamespaceWrap _wrap;

        public void Show(NamespaceWrap wrap)
        {
            _wrap = wrap;
            _nameTextBox.Text = wrap.Name;
            _descTextBox.Text = wrap.Desc;
            ShowDialog();
        }
       
        private void OkButton_Click(object sender, EventArgs e)
        {
            if (!Util.CheckName(_nameTextBox.Text)) return;
            if (_nameTextBox.Text != _wrap.Name && NamespaceWrap.Dict.ContainsKey(_nameTextBox.Text))
            {
                Util.MsgWarning("在当前模块/默认模块中已经存在{0}根节点!", _nameTextBox.Text);
                return;
            }
            _wrap.Name = _nameTextBox.Text;
            _wrap.Desc = _descTextBox.Text;
            _wrap.SetDirty();

            Close();
        }
        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
