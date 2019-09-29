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

    public partial class NamespaceDock : DockContent
    {
        public static NamespaceDock Ins { get { return _ins; } }
        static NamespaceDock _ins;

        List<TreeNode> _removes = new List<TreeNode>();
        public static void Inspect()
        {
            if (_ins == null)
            {
                _ins = new NamespaceDock();
                _ins.Show(MainWindow.Ins._dockPanel, DockState.DockLeft);
            }
            else
            {
                _ins.Show();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            base.WndProc(ref m);
        }
        protected override void OnClosed(EventArgs e)
        {
            _ins = null;
            base.OnClosed(e);
        }
        private NamespaceDock()
        {
            InitializeComponent();
            InitTree();
            _nodeTreeView.Sort();
        }

        //开启模块的时候调用
        public void InitTree()
        {
            ModuleWrap.Current.Check();
            _nodeFilterBox.Text = "";
            _nodeTreeView.BeginUpdate();
            ClearNodes(_nodeTreeView.Nodes);
            var all = NamespaceWrap.AllNamespaces;
            var current = ModuleWrap.Current.Imports;
            for (int k = 0; k < current.Count; k++)
            {
                var root = NamespaceWrap.AllNamespaces[current[k]];
                if ((root.NodeState & NodeState.Include) != 0)
                {
                    AddRootNode(root);
                    for (int i = 0; i < root.Classes.Count; i++)
                        AddSubNode(root.Classes[i], root);
                    for (int i = 0; i < root.Enums.Count; i++)
                        AddSubNode(root.Enums[i], root);
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
        public void AddType2Namespace(TypeWrap wrap, NamespaceWrap nWrap)
        {
            AddSubNode(wrap, nWrap);
            TreeNode root = _nodeTreeView.Nodes[nWrap.FullName];
            SetNodeColor(root);
        }
        /// <summary>
        /// 更新NamespaceWrap类型节点
        /// </summary>
        public void UpdateNodeName(string srcFullName, NamespaceWrap wrap)
        {
            TreeNode root = _nodeTreeView.Nodes[srcFullName];
            root.Text = wrap.DisplayName;
            root.Name = wrap.FullName;
        }
        /// <summary>
        /// 更新TypeWrap类型节点
        /// </summary>
        public void UpdateNodeName(string srcFullName, TypeWrap wrap)
        {
            string ns = wrap.Namespace.FullName;
            TreeNode root = _nodeTreeView.Nodes[ns];
            TreeNode node = root.Nodes[srcFullName];
            node.Text = wrap.DisplayName;
            node.Name = wrap.FullName;
        }
        public void SwapNamespace(string srcFullName, TypeWrap wrap, string src, string dst)
        {
            TreeNode srcNode = _nodeTreeView.Nodes[src];
            TreeNode dstNode = _nodeTreeView.Nodes[dst];
            TreeNode node = srcNode.Nodes[srcFullName];
            srcNode.Nodes.Remove(node);
            node.Text = wrap.DisplayName;
            node.Name = wrap.FullName;
            dstNode.Nodes.Add(node);
        }
        /// <summary>
        /// 仅更新当前节点,不包含子节点
        /// </summary>
        public TreeNode UpdateNodeColorState(BaseWrap wrap)
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
            SetNodeColor(node);
            return node;
        }
        public void UpdateNamespaceNode(NamespaceWrap wrap)
        {
            UpdateNodeColorState(wrap);
            var classes = wrap.Classes;
            for (int k = 0; k < classes.Count; k++)
                UpdateNodeColorState(classes[k]);
            var enums = wrap.Enums;
            for (int k = 0; k < enums.Count; k++)
                UpdateNodeColorState(enums[k]);
        }
        //仅保存单个命名空间且更新显示状态
        public void SaveNamespaceWrap(NamespaceWrap wrap)
        {
            wrap.Save();
            UpdateNodeColorState(wrap);
            var classes = wrap.Classes;
            var enums = wrap.Enums;
            for (int i = 0; i < classes.Count; i++)
                UpdateNodeColorState(classes[i]);
            for (int i = 0; i < enums.Count; i++)
                UpdateNodeColorState(enums[i]);
        }

        private void AddSubNode(TypeWrap cWrap, NamespaceWrap nWrap)
        {
            TreeNode rootNode = _nodeTreeView.Nodes[nWrap.FullName];
            TreeNode sub = CreateNode(cWrap);
            rootNode.Nodes.Add(sub);
            SetNodeColor(sub);
        }
        private TreeNode CreateNode(BaseWrap wrap)
        {
            TreeNode node = PoolManager.Ins.Pop<TreeNode>();
            if (node == null)
                node = new TreeNode();
            node.Tag = wrap;
            node.Text = wrap.DisplayName;
            node.Name = wrap.FullName;
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
        private void SetNodeColor(TreeNode node)
        {
            BaseWrap wrap = node.Tag as BaseWrap;
            switch (wrap.NodeState)
            {
                case NodeState.Modify | NodeState.Include:
                    node.ForeColor = Color.DodgerBlue;
                    break;
                case NodeState.Modify | NodeState.Exclude:
                    node.ForeColor = Color.CornflowerBlue;
                    break;
                case NodeState.Error | NodeState.Modify | NodeState.Include:
                case NodeState.Error | NodeState.Include:
                    node.ForeColor = Color.Red;
                    break;
                case NodeState.Error | NodeState.Modify | NodeState.Exclude:
                case NodeState.Error | NodeState.Exclude:
                    node.ForeColor = Color.IndianRed;
                    break;
                case NodeState.Include:
                    node.ForeColor = Color.LightGray;
                    break;
                case NodeState.Exclude:
                    node.ForeColor = Color.Gray;
                    break;
                default:
                    break;
            }
            //ConsoleDock.Ins.LogErrorFormat("未知节点状态类型{0}", wrap.NodeState);
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
        }
        private void NodeTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            TreeNode node = _nodeTreeView.GetNodeAt(e.X, e.Y);
            if (node != null)
                _nodeTreeView.SelectedNode = node;
            var selected = _nodeTreeView.SelectedNode;
            bool isNsw = selected != null && selected.Tag is NamespaceWrap;
            bool isDefault = ModuleWrap.Default == ModuleWrap.Current;
            _modifyRootMenuItem.Visible = isNsw && !selected.Name.Equals(Util.EmptyNamespace);
            _saveRootMenuItem.Visible = isNsw;
            _includeMenuItem.Visible = isNsw && !isDefault;
            _excludeMenuItem.Visible = isNsw && !isDefault;
            _rootSeparator.Visible = isNsw;

            var point = _nodeTreeView.PointToScreen(e.Location);
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
                PoolManager.Ins.Push(nsw);
                _nodeTreeView.Nodes.Remove(node);
            }
            else if (wrap is TypeWrap)
            {
                var tw = wrap as TypeWrap;
                EditorDock.CloseDock(tw.FullName);
                var root = _nodeTreeView.Nodes[tw.Namespace.FullName];
                root.Nodes.RemoveByKey(tw.FullName);
                tw.BreakParent();
                tw.Dispose();
                PoolManager.Ins.Push(tw);
                UpdateNodeColorState(root.Tag as BaseWrap);
            }
            PoolManager.Ins.Push(node);
            MainWindow.Ins.CheckError();
        }
        private void NodeTreeView_Modify(object sender, EventArgs e)
        {
            var nsw = (_nodeTreeView.SelectedNode.Tag as NamespaceWrap);
            if (nsw == null)
                Util.MsgError("选择的节点不是根节点!", _nodeTreeView.SelectedNode.Name);
            else
                NamespaceInfoDock.Ins.Show(nsw);
        }
        private void NodeTreeView_Save(object sender, EventArgs e)
        {
            var wrap = (_nodeTreeView.SelectedNode.Tag as NamespaceWrap);
            if (wrap == null)
                Util.MsgError("选择的节点不是根节点!", _nodeTreeView.SelectedNode.Name);
            else
            {
                SaveNamespaceWrap(wrap);
                ConsoleDock.Ins.LogFormat("成功手动保存命名空间{0}!", wrap.FullName);
            }
        }
        private void NodeTreeView_Include(object sender, EventArgs e)
        {
            var node = _nodeTreeView.SelectedNode;
            object wrap = node.Tag;
            if (wrap is NamespaceWrap)
            {
                var nsw = wrap as NamespaceWrap;
                ModuleWrap.Current.AddImport(nsw);
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
            if (ModuleWrap.Default == ModuleWrap.Current) return;

            var state = sender as CheckBox;
            var nodes = _nodeTreeView.Nodes;
            var module = state.Checked ? ModuleWrap.Default : ModuleWrap.Current;
            var imps = module.Imports;
            for (int i = 0; i < imps.Count; i++)
            {
                string key = imps[i];
                if (NamespaceWrap.AllNamespaces.ContainsKey(key))
                {
                    if (nodes.ContainsKey(key)) continue;

                    var item = NamespaceWrap.AllNamespaces[key];
                    if (item.FullName.IndexOf(_nodeFilterBox.Text, StringComparison.OrdinalIgnoreCase) != -1)
                        AddRootNode(item);
                }
                else
                {
                    Util.MsgError("{0}模块中的命名空间{1}居然会不在字典中!?\n改了命名空间库,却没修改模块?", module.Name, key);
                }
            }
        }
        private void ErrorBox_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void NodeFilterBox_TextChanged(object sender, EventArgs e)
        {
            TextBox find = sender as TextBox;
            _nodeTreeView.BeginUpdate();
            _nodeTreeView.Nodes.AddRange(_removes.ToArray());
            _removes.Clear();
            _nodeTreeView.EndUpdate();
            _nodeTreeView.Sort();

            if (find.Text.IsEmpty()) return;
            var nodes = _nodeTreeView.Nodes;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Name.IndexOf(find.Text, StringComparison.OrdinalIgnoreCase) == -1)
                    _removes.Add(nodes[i]);
            }
            _nodeTreeView.BeginUpdate();
            for (int i = 0; i < _removes.Count; i++)
                nodes.Remove(_removes[i]);
            _nodeTreeView.EndUpdate();
        }
    }
}
