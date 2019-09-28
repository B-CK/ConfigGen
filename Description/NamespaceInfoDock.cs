using Description.Wrap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Description
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
            if (NamespaceWrap.AllNamespaces.ContainsKey(_nameTextBox.Text))
            {
                Util.MsgWarning("在当前模块/默认模块中已经存在{0}根节点!", _nameTextBox.Text);
                return;
            }
            string srcDisplayName = _wrap.DisplayName;
            ModuleWrap.Default.RemoveImport(_wrap);
            if (ModuleWrap.Default != ModuleWrap.Current)
                ModuleWrap.Current.RemoveImport(_wrap);
            NamespaceWrap.AllNamespaces.Remove(_wrap.FullName);
        
            _wrap.Name = _nameTextBox.Text;
            _wrap.Desc = _descTextBox.Text;
            _wrap.SetDirty();
            ModuleWrap.Default.AddImport(_wrap);
            NamespaceWrap.AllNamespaces.Add(_wrap.FullName, _wrap);
            NamespaceDock.Ins.UpdateNodeName(srcDisplayName, _wrap);
            NamespaceDock.Ins.UpdateNodeColorState(_wrap);
            Close();
        }
        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
