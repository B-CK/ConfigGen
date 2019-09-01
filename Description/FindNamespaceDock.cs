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


        TreeNode _node;
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
                ModuleWrap.Open(Util.LastRecord);
            else
                ModuleWrap.OpenDefault();

            var all = NamespaceWrap.AllNamespaces;
            foreach (var item in all)
            {
                NamespaceWrap root = item.Value;
                AddRootNode(root);
                for (int i = 0; i < root.Classes.Count; i++)
                    AddSubNode(root.Classes[i], item.Value);
                for (int i = 0; i < root.Enums.Count; i++)
                    AddSubNode(root.Enums[i], item.Value);
            }
            _nodeTreeView.Sort();
        }
        public void AddRootNode(NamespaceWrap wrap)
        {
            TreeNode root = GetNode(wrap);
            _nodeTreeView.Nodes.Add(root);
            ModuleWrap.Default.AddImport(wrap);
            ModuleWrap.Current.AddImport(wrap);
        }
        public void AddSubNode(ClassWrap cWrap, NamespaceWrap nWrap)
        {
            TreeNode rootNode = _nodeTreeView.Nodes[nWrap.FullName];
            TreeNode sub = GetNode(cWrap);
            rootNode.Nodes.Add(sub);
            nWrap.AddClass(cWrap);
        }
        public void AddSubNode(EnumWrap eWrap, NamespaceWrap nWrap)
        {
            TreeNode rootNode = _nodeTreeView.Nodes[nWrap.FullName];
            TreeNode sub = GetNode(eWrap);
            rootNode.Nodes.Add(sub);
            nWrap.AddEnum(eWrap);
        }
        private TreeNode GetNode(BaseWrap wrap)
        {
            TreeNode node = PoolManager.Ins.Pop<TreeNode>();
            if (node == null)
                node = new TreeNode()
                {
                    Tag = wrap,
                    Name = wrap.FullName,
                    Text = wrap.FullName
                };
            return node;
        }
        private void Set23MenuItem(bool state)
        {
            _includeMenuItem.Enabled = state;
            _excludeMenuItem.Enabled = state;
        }


        private void NodeTreeView_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            _node = e.Node;
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
            _nodeTreeView.SelectedNode = _node;
            Set23MenuItem(ModuleWrap.Default != ModuleWrap.Current
                && _node.Tag is NamespaceWrap);
            _nodeMenu.Show(point);
        }
        private void NodeTreeView_DeleteNode(object sender, EventArgs e)
        {
            var node = _nodeTreeView.SelectedNode;
            object wrap = node.Tag;
            if (wrap is NamespaceWrap)
            {
                var nsw = wrap as NamespaceWrap;
                ModuleWrap.Current.RemoveImport(nsw);
                ModuleWrap.Default.RemoveImport(nsw);
            }
            else if (wrap is ClassWrap)
            {
                var cls = wrap as ClassWrap;
                cls.Namespace.RemoveClass(cls);
            }
            else if (wrap is EnumWrap)
            {
                var enm = wrap as EnumWrap;
                enm.Namespace.RemoveEnum(enm);
            }
            _nodeTreeView.Nodes.Remove(node);
            PoolManager.Ins.Push(node);
        }
        private void NodeTreeView_Include(object sender, EventArgs e)
        {
            var node = _nodeTreeView.SelectedNode;
            object wrap = node.Tag;
            if (wrap is NamespaceWrap)
            {
                var nsw = wrap as NamespaceWrap;
                ModuleWrap.Current.RemoveImport(nsw);
            }
        }
        private void NodeTreeView_Exclude(object sender, EventArgs e)
        {
            var node = _nodeTreeView.SelectedNode;
            object wrap = node.Tag;
            if (wrap is NamespaceWrap)
            {
                var nsw = wrap as NamespaceWrap;
                ModuleWrap.Current.AddImport(nsw);
            }
        }
        private void CommitToLib(object sender, EventArgs e)
        {

        }
        private void UpdateToLib(object sender, EventArgs e)
        {

        }
        
    }
}
