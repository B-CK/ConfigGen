using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Description.Wrap;

namespace Description
{
    public partial class ClassInfoDock : UserControl, IUserOperation
    {
        public static ClassInfoDock Create(ClassWrap wrap)
        {
            ClassInfoDock dock = PoolManager.Ins.Pop<ClassInfoDock>();
            if (dock == null) dock = new ClassInfoDock(wrap);

            dock._nameTextBox.Text = wrap.Name;
            int index = dock._keyComboBox.FindStringExact(wrap.Index);
            dock._keyComboBox.Select(index, 1);
            index = dock._namespaceComboBox.FindStringExact(wrap.Namespace.Name);
            dock._namespaceComboBox.Select(index, 1);
            index = dock._inhertComboBox.FindStringExact(wrap.Inherit);
            dock._inhertComboBox.Select(index, 1);
            dock._desTextBox.Text = wrap.Desc;
            dock.DataPathLabel.Text = Util.Format("数据路径: {0}", wrap.DataPath);
            return dock;
        }

        private ClassWrap _wrap;

        private ClassInfoDock(ClassWrap wrap)
        {
            InitializeComponent();
            _wrap = wrap;
        }
        public void Save()
        {
            _wrap.Name = _nameTextBox.Text;
            _wrap.Index = _keyComboBox.Text;
            _wrap.Namespace = NamespaceWrap.GetNamespace(_namespaceComboBox.Text);
            _wrap.Inherit = _inhertComboBox.Text;
            _wrap.Desc = _desTextBox.Text;
            _wrap.DataPath = _dataPathTextBox.Text;
        }
        public void Close()
        {
            _wrap.Dispose();
            PoolManager.Ins.Push(this);
        }
    }
}
