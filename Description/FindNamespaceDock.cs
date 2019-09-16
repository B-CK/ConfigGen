using System;
using System.IO;
using Description.Wrap;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;
using System.Collections.Generic;
using Description.Editor;

namespace Description
{
    [Flags]
    public enum NodeState
    {
        Include = 1,
        Exclude = 2,
        Modify = 4,
        Error = 8,//添加错误图标
    }

    public partial class FindNamespaceDock : DockContent
    {
        public static FindNamespaceDock Ins { get { return _ins; } }
        static FindNamespaceDock _ins;

        static readonly Color Include = Color.LightGray;
        static readonly Color Exclude = Color.Gray;
        static readonly Color Modify = Color.DodgerBlue;
        static readonly Color Error = Color.Red;

        enum ShowType
        {
            Include,//仅显示包含部分
            All,//包含部分亮灰色,其他暗灰色
            Error,//仅显示错误条目
        }

        ShowType _showType = ShowType.Include;
        TreeNode _node;
        public static void Inspect()
        {
            if (_ins == null)
            {
                _ins = new FindNamespaceDock();
                _ins.Show(MainWindow.Ins._dockPanel, DockState.DockLeft);
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
            UpdateTree();
            _nodeTreeView.Sort();
        }
        public void UpdateTree()
        {
            _nodeTreeView.BeginUpdate();
            ClearNodes(_nodeTreeView.Nodes);
            HashSet<string> hash = new HashSet<string>(ModuleWrap.Current.Imports);
            var all = NamespaceWrap.AllNamespaces;
            foreach (var item in all)
            {
                NamespaceWrap root = item.Value;
                if (ModuleWrap.Default == ModuleWrap.Current)
                    root.SetNodeState(NodeState.Include);
                else
                    root.SetNodeState(hash.Contains(item.Key) ? NodeState.Include : NodeState.Exclude);
                switch (_showType)
                {
                    case ShowType.Include:
                        if ((int)(root.NodeState & NodeState.Include) == 1)
                        {
                            AddRootNode(root);
                            for (int i = 0; i < root.Classes.Count; i++)
                            {
                                root.Classes[i].SetNodeState(root.NodeState);
                                AddSubNode(root.Classes[i], item.Value);
                            }
                            for (int i = 0; i < root.Enums.Count; i++)
                            {
                                root.Enums[i].SetNodeState(root.NodeState);
                                AddSubNode(root.Enums[i], item.Value);
                            }
                        }
                        break;
                    case ShowType.All:
                        AddRootNode(root);
                        for (int i = 0; i < root.Classes.Count; i++)
                        {
                            root.Classes[i].SetNodeState(root.NodeState);
                            AddSubNode(root.Classes[i], item.Value);
                        }
                        for (int i = 0; i < root.Enums.Count; i++)
                        {
                            root.Enums[i].SetNodeState(root.NodeState);
                            AddSubNode(root.Enums[i], item.Value);
                        }
                        break;
                    case ShowType.Error:
                        break;
                    default:
                        break;
                }

            }
            _nodeTreeView.EndUpdate();
        }
        public void AddRootNode(NamespaceWrap wrap)
        {
            TreeNode root = CreateNode(wrap);
            _nodeTreeView.Nodes.Add(root);
            SetNodeColor(root);
        }
        public void AddClass2Namespace(ClassWrap cWrap, NamespaceWrap nWrap)
        {
            AddSubNode(cWrap, nWrap);
            TreeNode rootNode = _nodeTreeView.Nodes[nWrap.FullName];
            SetNodeColor(rootNode);
        }
        public void AddEnum2Namespace(EnumWrap eWrap, NamespaceWrap nWrap)
        {
            AddSubNode(eWrap, nWrap);
            TreeNode rootNode = _nodeTreeView.Nodes[nWrap.FullName];
            SetNodeColor(rootNode);
        }
        public void SwapNamespace(TypeWrap wrap, string src, string dst)
        {
            TreeNode srcNode = _nodeTreeView.Nodes[src];
            TreeNode dstNode = _nodeTreeView.Nodes[dst];
            TreeNode node = srcNode.Nodes[wrap.FullName];
            srcNode.Nodes.Remove(node);
            dstNode.Nodes.Add(node);
            node.Name = node.FullPath;
        }

        private void AddSubNode(ClassWrap cWrap, NamespaceWrap nWrap)
        {
            TreeNode rootNode = _nodeTreeView.Nodes[nWrap.FullName];
            TreeNode sub = CreateNode(cWrap);
            rootNode.Nodes.Add(sub);
            SetNodeColor(sub);
        }
        private void AddSubNode(EnumWrap eWrap, NamespaceWrap nWrap)
        {
            TreeNode rootNode = _nodeTreeView.Nodes[nWrap.FullName];
            TreeNode sub = CreateNode(eWrap);
            rootNode.Nodes.Add(sub);
            SetNodeColor(sub);
        }
        private TreeNode CreateNode(BaseWrap wrap)
        {
            TreeNode node = PoolManager.Ins.Pop<TreeNode>();
            if (node == null)
                node = new TreeNode();
            node.Tag = wrap;
            node.Name = wrap.FullName;
            node.Text = wrap.Name;
            return node;
        }
        private void ClearNodes(TreeNodeCollection root)
        {
            for (int i = 0; i < root.Count; i++)
            {
                ClearNodes(root[i].Nodes);
                PoolManager.Ins.Push(root[i]);
            }
            root.Clear();
        }
        private void Set23MenuItem(bool state)
        {
            _includeMenuItem.Visible = state;
            _excludeMenuItem.Visible = state;
        }
        private void SetNodeColor(TreeNode node)
        {
            BaseWrap wrap = node.Tag as BaseWrap;
            if ((wrap.NodeState & NodeState.Include) != 0)
                node.ForeColor = Include;
            if ((wrap.NodeState & NodeState.Exclude) != 0)
                node.ForeColor = Exclude;
            if ((wrap.NodeState & NodeState.Modify) != 0)
                node.ForeColor = Modify;
            if ((wrap.NodeState & NodeState.Error) != 0)
                node.ForeColor = Error;
            //ConsoleDock.Ins.LogErrorFormat("未知节点状态类型{0}", wrap.NodeState);
        }
        /// <summary>
        /// 类型属性修改
        /// </summary>
        /// <param name="wrap"></param>
        private void OnWrapPropertiesModified(BaseWrap wrap)
        {
            TreeNode node = null;
            string key = wrap.FullName;
            var nodes = _nodeTreeView.Nodes;
            if (wrap is NamespaceWrap)
            {
                if (!nodes.ContainsKey(key))
                    Util.MsgError("TreeView中无法通过Key:{0}找到Namespace节点!", key);
                node = nodes[key];
            }
            else
            {
                TypeWrap tw = wrap as TypeWrap;
                key = tw.Namespace.FullName;
                if (!nodes.ContainsKey(key))
                    Util.MsgError("TreeView中无法通过Key:{0}找到Namespace节点!", key);
                else
                {
                    var root = _nodeTreeView.Nodes[key];
                    key = wrap.FullName;
                    if (!root.Nodes.ContainsKey(key))
                        Util.MsgError("TreeView中无法通过Key:{0}找到Class,Enum节点!", key);
                    node = root.Nodes[key];
                }
            }

            wrap.SetNodeState(NodeState.Modify);
            SetNodeColor(node);
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            base.WndProc(ref m);
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
            BaseWrap data = e.Node.Tag as BaseWrap;
            if ((data.NodeState & NodeState.Include) == 0) return;
            if (data is NamespaceWrap) return;

            if (EditorDock.IsContain(data.FullName))
            {
                EditorDock.FocusDock(data.FullName);
                return;
            }

            EditorDock dock = null;
            if (data is ClassWrap)
                dock = ClassEditorDock.Create(data as ClassWrap);
            else if (data is EnumWrap)
                dock = EnumEditorDock.Create(data as EnumWrap);
            dock.OnWrapPropertiesModified = OnWrapPropertiesModified;
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
            if (node == null) return;

            object wrap = node.Tag;
            if (wrap is NamespaceWrap)
            {
                var nsw = wrap as NamespaceWrap;
                ModuleWrap.Current.RemoveImport(nsw);
                if (nsw.FullName != Util.EmptyNamespace)
                    ModuleWrap.Default.RemoveImport(nsw);
                PoolManager.Ins.Push(nsw);
                _nodeTreeView.Nodes.Remove(node);
            }
            else if (wrap is ClassWrap)
            {
                var cls = wrap as ClassWrap;
                EditorDock.CloseDock(cls.FullName);
                var root = _nodeTreeView.Nodes[cls.Namespace.FullName];
                root.Nodes.RemoveByKey(cls.FullName);
                cls.Namespace.RemoveClassWrap(cls);
                cls.Dispose();
                PoolManager.Ins.Push(cls);
            }
            else if (wrap is EnumWrap)
            {
                var enm = wrap as EnumWrap;
                EditorDock.CloseDock(enm.FullName);
                var root = _nodeTreeView.Nodes[enm.Namespace.FullName];
                root.Nodes.RemoveByKey(enm.FullName);
                enm.Namespace.RemoveEnumWrap(enm);
                enm.Dispose();
                PoolManager.Ins.Push(enm);
            }
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
                nsw.SetNodeState(NodeState.Include);
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
                nsw.SetNodeState(NodeState.Exclude);
                SetNodeColor(node);
            }
        }
        private void CommitToLib(object sender, EventArgs e)
        {

        }
        private void UpdateToLib(object sender, EventArgs e)
        {

        }

        private void ShowAllBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = sender as CheckBox;
            _showType = state.Checked ? ShowType.All : ShowType.Include;
            UpdateTree();
        }
        private void ErrorBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
