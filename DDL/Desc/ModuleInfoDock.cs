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

        bool _hasChange = false;
        ModuleWrap _wrap;
        public void Show(ModuleWrap wrap)
        {
            _wrap = wrap;
            _nameTextBox.Text = wrap.Name;
            _groupGridView.Rows.Clear();
            var rows = _groupGridView.Rows;
            for (int i = 0; i < wrap.Groups.Count; i++)
                rows.Add(wrap.Groups[i]);
            ShowDialog();
            _hasChange = false;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (_hasChange)
            {
                string path = $"{Util.ModuleDir}\\{_nameTextBox.Text}.xml";
                if (File.Exists(path))
                {
                    Util.MsgError("模块{0}已经存在!请重新定义名称.", _nameTextBox.Text);
                    return;
                }
            }
            _wrap.Name = _nameTextBox.Text;
            _wrap.Groups.Clear();
            var rows = _groupGridView.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var value = rows[i].Cells[0].Value as string;
                if (value != null)
                    _wrap.Groups.Add(value);
            }
            _wrap.Save(false);
            Close();
        }

        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ModuleWrap.Default == ModuleWrap.Current)
                _nameTextBox.Text = ModuleWrap.Default.Name;
            else
                _hasChange = true;
        }
    }
}
