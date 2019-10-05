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
            if (!Util.CheckName(_nameTextBox.Text)) return;
            if (NamespaceWrap.AllNamespaces.ContainsKey(_nameTextBox.Text))
            {
                Util.MsgWarning("在当前模块/默认模块中已经存在{0}根节点!", _nameTextBox.Text);
                return;
            }
            ModuleWrap.Default.RemoveImport(_wrap);
            if (ModuleWrap.Default != ModuleWrap.Current)
                ModuleWrap.Current.RemoveImport(_wrap);
            NamespaceWrap.AllNamespaces.Remove(_wrap.FullName);
            var classes = _wrap.Classes;
            var enums = _wrap.Enums;
            for (int i = 0; i < classes.Count; i++)
                ClassWrap.Dict.Remove(classes[i].FullName);
            for (int i = 0; i < enums.Count; i++)
                EnumWrap.Dict.Remove(enums[i].FullName);

            string src = _wrap.FullName;
            _wrap.Name = _nameTextBox.Text;
            _wrap.Desc = _descTextBox.Text;
            _wrap.SetDirty();
            ModuleWrap.Default.AddImport(_wrap);
            if (ModuleWrap.Default != ModuleWrap.Current)
                ModuleWrap.Current.AddImport(_wrap);
            NamespaceWrap.AllNamespaces.Add(_wrap.FullName, _wrap);
            for (int i = 0; i < classes.Count; i++)
                ClassWrap.Dict.Add(classes[i].FullName, classes[i]);
            for (int i = 0; i < enums.Count; i++)
                EnumWrap.Dict.Add(enums[i].FullName, enums[i]);

            //更新显示界面
            NamespaceDock.Ins.UpdateNodeName(src, _wrap);
            NamespaceDock.Ins.UpdateModule();
            Close();
        }
        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
