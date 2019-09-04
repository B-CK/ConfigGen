using System;
using System.IO;
using Description.Wrap;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;
using System.Collections.Generic;

namespace Description
{
    public enum NodeState
    {
        Include,
        Exclude,
        Modify,
        Error,//添加错误图标
    }

    public partial class FindNamespaceDock : DockContent
    {
        public static FindNamespaceDock Ins { get { return _ins; } }
        static FindNamespaceDock _ins;

        static readonly Color Include = Color.LightGray;
        static readonly Color Exclude = Color.Gray;
        static readonly Color Modify = Color.DodgerBlue;
        static readonly Color Error = Color.Red;

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

            ModuleWrap wrap = null;
            if (File.Exists(Util.LastRecord))
                wrap = ModuleWrap.Open(Util.LastRecord);
            else
                wrap = ModuleWrap.OpenDefault();

            UpdateTree();
        }
        public void UpdateTree()
        {
            ClearNodes();
            HashSet<string> hash = new HashSet<string>(ModuleWrap.Current.Imports);
            var all = NamespaceWrap.AllNamespaces;
            foreach (var item in all)
            {
                NamespaceWrap root = item.Value;
                if (ModuleWrap.Default == ModuleWrap.Current)
                    root.NodeState = NodeState.Include;
                else
                    root.NodeState = hash.Contains(item.Key) ? NodeState.Include : NodeState.Exclude;
                AddRootNode(root);
                for (int i = 0; i < root.Classes.Count; i++)
                    AddSubNode(root.Classes[i], item.Value);
                for (int i = 0; i < root.Enums.Count; i++)
                    AddSubNode(root.Enums[i], item.Value);
            }
            //检查各节点自身状态
            //---TODO
            _nodeTreeView.Sort();
        }
        public void AddRootNode(NamespaceWrap wrap)
        {
            TreeNode root = GetNode(wrap);
            _nodeTreeView.Nodes.Add(root);
            SetNodeColor(root);
        }
        public void AddSubNode(ClassWrap cWrap, NamespaceWrap nWrap)
        {
            TreeNode rootNode = _nodeTreeView.Nodes[nWrap.FullName];
            TreeNode sub = GetNode(cWrap);
            rootNode.Nodes.Add(sub);
            nWrap.AddClass(cWrap);
            cWrap.NodeState = nWrap.NodeState;
            SetNodeColor(sub);
        }
        public void AddSubNode(EnumWrap eWrap, NamespaceWrap nWrap)
        {
            TreeNode rootNode = _nodeTreeView.Nodes[nWrap.FullName];
            TreeNode sub = GetNode(eWrap);
            rootNode.Nodes.Add(sub);
            nWrap.AddEnum(eWrap);
            eWrap.NodeState = nWrap.NodeState;
            SetNodeColor(sub);
        }
        private TreeNode GetNode(BaseWrap wrap)
        {
            TreeNode node = PoolManager.Ins.Pop<TreeNode>();
            if (node == null)
                node = new TreeNode();
            node.Tag = wrap;
            node.Name = wrap.FullName;
            node.Text = wrap.Name;
            return node;
        }
        private void ClearNodes()
        {
            var nodes = _nodeTreeView.Nodes;
            for (int i = 0; i < nodes.Count; i++)
                PoolManager.Ins.Push(nodes[i]);
            _nodeTreeView.Nodes.Clear();
        }
        private void Set23MenuItem(bool state)
        {
            _includeMenuItem.Enabled = state;
            _excludeMenuItem.Enabled = state;
        }
        private void SetNodeColor(TreeNode node)
        {
            BaseWrap wrap = node.Tag as BaseWrap;
            switch (wrap.NodeState)
            {
                case NodeState.Include:
                    node.ForeColor = Include;
                    break;
                case NodeState.Exclude:
                    node.ForeColor = Exclude;
                    break;
                case NodeState.Modify:
                    node.ForeColor = Modify;
                    break;
                case NodeState.Error:
                    node.ForeColor = Error;
                    break;
                default:
                    ConsoleDock.Ins.LogErrorFormat("未知节点状态类型{0}", wrap.NodeState);
                    break;
            }
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
                if (nsw.FullName != Util.EmptyNamespace)
                    ModuleWrap.Default.RemoveImport(nsw);
                PoolManager.Ins.Push(nsw);
            }
            else if (wrap is ClassWrap)
            {
                var cls = wrap as ClassWrap;
                cls.Namespace.RemoveClass(cls);
                PoolManager.Ins.Push(cls);
            }
            else if (wrap is EnumWrap)
            {
                var enm = wrap as EnumWrap;
                enm.Namespace.RemoveEnum(enm);
                PoolManager.Ins.Push(enm);
            }
            _nodeTreeView.Nodes.Remove(node);
            for (int i = 0; node.Nodes != null && i < node.Nodes.Count; i++)
                PoolManager.Ins.Push(node.Nodes[i]);
            PoolManager.Ins.Push(node);
        }
        private void NodeTreeView_Include(object sender, EventArgs e)
        {
            var node = _nodeTreeView.SelectedNode;
            object wrap = node.Tag;
            if (wrap is NamespaceWrap)
            {
                var nsw = wrap as NamespaceWrap;
                ModuleWrap.Current.AddImport(nsw);
                nsw.NodeState = NodeState.Include;
                SetNodeColor(node);
            }
        }
        private void NodeTreeView_Exclude(object sender, EventArgs e)
        {
            var node = _nodeTreeView.SelectedNode;
            object wrap = node.Tag;
            if (wrap is NamespaceWrap)
            {
                var nsw = wrap as NamespaceWrap;
                ModuleWrap.Current.RemoveImport(nsw);
                nsw.NodeState = NodeState.Exclude;
                SetNodeColor(node);
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
