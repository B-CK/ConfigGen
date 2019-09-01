using System;
using System.IO;
using Description.Wrap;
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

            if (File.Exists(Util.LastRecord))
                MainWindow.Ins.OpenModule(Util.LastRecord);
            else
                MainWindow.Ins.OpenDefault();

            var all = NamespaceWrap.AllNamespaces;
            foreach (var item in all)
            {
                NamespaceWrap root = item.Value;
                AddRootNode(root);
                for (int i = 0; i < root.Classes.Count; i++)
                    AddSubNode(root.Classes[i], item.Key);
                for (int i = 0; i < root.Enums.Count; i++)
                    AddSubNode(root.Enums[i], item.Key);
            }
            _nodeTreeView.Sort();
        }
        private void AddRootNode(NamespaceWrap wrap)
        {
            TreeNode root = new TreeNode() { Tag = wrap };
            _nodeTreeView.Nodes.Add(wrap.FullName, wrap.FullName);
        }
        private void AddSubNode(ClassWrap cWrap, string root)
        {
            TreeNode sub = new TreeNode() { Tag = cWrap };
            TreeNode rootNode = _nodeTreeView.Nodes[root];
            rootNode.Nodes.Add(cWrap.FullName, cWrap.FullName);
        }
        private void AddSubNode(EnumWrap eWrap, string root)
        {
            TreeNode sub = new TreeNode() { Tag = eWrap };
            TreeNode rootNode = _nodeTreeView.Nodes[root];
            rootNode.Nodes.Add(eWrap.FullName, eWrap.FullName);
        }


        /// <summary>
        /// 打开类型信息界面
        /// </summary>
        private void NodeTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            object data = e.Node.Tag;
            if (data is NamespaceWrap) return;

            if (data is ClassWrap)
                TypeEditorDock.Create(data as ClassWrap);
            else if (data is EnumWrap)
                TypeEditorDock.Create(data as EnumWrap);
        }

        private void NodeTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var point = _nodeTreeView.PointToScreen(e.Location);
            _nodeMenu.Show(point);
        }

        private void DeleteClass(object sender, EventArgs e)
        {

        }
        private void DeleteEnum(object sender, EventArgs e)
        {

        }
        private void DeleteNamespace(object sender, EventArgs e)
        {

        }
        private void CommitToLib(object sender, EventArgs e)
        {

        }
        private void UpdateToLib(object sender, EventArgs e)
        {

        }
    }
}
