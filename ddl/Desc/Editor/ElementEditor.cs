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
using WeifenLuo.WinFormsUI.Docking;

namespace Desc.Editor
{
    public partial class ElementEditor : DockContent
    {
        public static ElementEditor Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new ElementEditor();
                return _ins;
            }
        }
        static ElementEditor _ins;

        private ElementEditor()
        {
            InitializeComponent();
        }

        private DataGridViewCellCollection _cells;
        private int _index;
        Action _okEvt;

        public new void Show(DataGridViewCellCollection cells, int index, string type, Action ok)
        {
            _cells = cells;
            _index = index;
            _okEvt = ok;
            string types = cells[index].Value as string;
            types = types ?? "";
            if (type == Util.LIST)
            {//List
                _label1.Text = "元素类型:";
                _label2.Visible = false;
                _typeComboBox2.Visible = false;
                var items = _typeComboBox1.Items;
                items.Clear();
                items.AddRange(Util.GetAllTypes(true));
                var i = items.IndexOf(types.Trim());
                _typeComboBox1.SelectedIndex = i;
            }
            else if (type == Util.DICT)
            {//Dict
                _label1.Text = "Key类型:";
                _label2.Visible = true;
                _typeComboBox2.Visible = true;
                var items = _typeComboBox1.Items;
                items.Clear();
                items.AddRange(Util.GetKeyTypes());
                items = _typeComboBox2.Items;
                items.Clear();
                items.AddRange(Util.GetAllTypes(true));

                string[] nodes = types.Split(Util.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
                if (nodes != null && nodes.Length == 2)
                {
                    var key = _typeComboBox1.Items.IndexOf(nodes[0]);
                    _typeComboBox1.SelectedIndex = key;
                    var value = _typeComboBox2.Items.IndexOf(nodes[1]);
                    _typeComboBox2.SelectedIndex = value;
                }
            }

            ShowDialog();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (_label2.Visible == false)
                _cells[_index].Value = _typeComboBox1.Text;
            else
                _cells[_index].Value = $"{_typeComboBox1.Text}{Util.ArgsSplitFlag[0]}{_typeComboBox2.Text}";
            _okEvt?.Invoke();
            Close();
        }
        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
