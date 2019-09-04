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
            string[] fs = new string[wrap.Fields.Count];
            for (int i = 0; i < wrap.Fields.Count; i++)
                fs[i] = wrap.Fields[i].ToString();
            dock._keyComboBox.Items.AddRange(fs);
            dock._keyComboBox.Text = wrap.Index;
            string[] ns = NamespaceWrap.AllNamespaces.Keys.ToArray();
            dock._namespaceComboBox.Items.AddRange(ns);
            dock._namespaceComboBox.Text = wrap.Namespace.FullName;
            HashSet<string> hash = new HashSet<string>(ClassWrap.ClassHash);
            hash.Remove(wrap.FullName);
            dock._inhertComboBox.Items.AddRange(hash.ToArray());
            dock._inhertComboBox.Text = wrap.Inherit;
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
            Save();
            _wrap.Dispose();
            PoolManager.Ins.Push(this);
        }
    }
}
