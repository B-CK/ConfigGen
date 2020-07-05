using Desc.Wrap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desc
{
    public partial class ModuleInfoDock : Form
    {
        public static ModuleInfoDock Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new ModuleInfoDock();
                return _ins;
            }
        }
        private static ModuleInfoDock _ins;

        private ModuleInfoDock()
        {
            InitializeComponent();
        }

        ModuleWrap _wrap;
        public void Show(ModuleWrap wrap)
        {
            _wrap = wrap;
            _nameTextBox.Text = wrap.Name;
            ShowDialog();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (_wrap.Name != _nameTextBox.Text)
            {
                string path = $"{Util.ModuleDir}\\{_nameTextBox.Text}.xml";
                if (File.Exists(path))
                {
                    Util.MsgError("模块{0}已经存在!请重新定义名称.", _nameTextBox.Text);
                    return;
                }
            }
            _wrap.Name = _nameTextBox.Text;
            _wrap.Save(false);
            Util.LastRecord = _wrap.FullName;
            MainWindow.Ins.Text = Util.Format("结构描述 - {0}", _wrap.FullName);
            Close();
        }

        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
