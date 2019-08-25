using Description.Xml;
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
using WeifenLuo.WinFormsUI.Docking;

namespace Description
{
    public partial class FindNamespaceDock : DockContent
    {

        public static FindNamespaceDock Ins { get { return _ins; } }
        static FindNamespaceDock _ins;
        public static void Inspect()
        {
            if (_ins == null)
            {
                _ins = new FindNamespaceDock();
                _ins.Show(MainWindow.Ins._dock, DockState.DockLeft);
            }
            else
            {
                _ins.Show();
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            _ins = null;
            base.OnClosed(e);
        }
        public FindNamespaceDock()
        {
            InitializeComponent();

            string path = Util.LastRecord;
            if (File.Exists(path))
                Module.Ins.Open(path);
            else
                Module.Ins.OpenDefault();

            ///先完成命名空间的创建
            var namespaces = Module.Ins.Namespaces;
            for (int i = 0; i < namespaces.Count; i++)
            {
                NamespaceXml _namespace = namespaces[i];
                _typeTreeView.Nodes.Add(_namespace.Name);
                //string[] nodes = _namespace.Name.Split(Util.DotSplit, StringSplitOptions.RemoveEmptyEntries);
                //for (int k = 0; k < nodes.Length; k++)
                //{
                //    _typeTreeView.Nodes.Add(nodes)
                //}

                //string fmt = "{0}.{1}";
                //var classes = _namespace.Classes;
                //for (int k = 0; k < classes.Count; k++)
                //{
                //    string fullName = Util.Format(fmt, _namespace.Name, classes[k].Name);
                //}
                //var enums = _namespace.Enums;
                //for (int k = 0; k < enums.Count; k++)
                //{
                //    string fullName = Util.Format(fmt, _namespace.Name, enums[k].Name);
                //}
            }
        }

        private void TypeTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }
    }
}
