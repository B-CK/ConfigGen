using System;
using System.IO;
using Desc.Wrap;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;
using System.Collections.Generic;
using Desc.Editor;

namespace Desc
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
                _ins.InitTree();
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
            _showAllBox.Checked = false;
            _nodeFilterBox.Text = "";

            _nodeTreeView.BeginUpdate();
            ClearNodes(_nodeTreeView.Nodes);
            var namespaces = WrapManager.Ins.Current.Namespaces;
            foreach (var item in namespaces)
            {
                var root = item.Value;
                if ((root.NodeState & NodeState.Include) != 0)
                    AddRootNode(root);
            }
            _nodeTreeView.EndUpdate();
            WrapManager.Ins.Current.AddNamespaceEvent += AddRootNode;
            WrapManager.Ins.Current.RemoveNamespaceEvent += RemoveRootNode;
            WrapManager.Ins.Current.AddAnyTypeEvent += AddChildNode;
            WrapManager.Ins.Current.RemoveAnyTypeEvent += RemoveChildNode;
            WrapManager.Ins.Current.Check();
        }

        private void AddRootNode(NamespaceWrap wrap)
        {
            TreeNode root = CreateNode(wrap);
            _nodeTreeView.Nodes.Add(root);
            wrap.OnNameChange += OnNamespaceNameChange;
            wrap.OnDescChange += OnNamespaceNameChange;
            wrap.OnNodeStateChange += OnNodeStateChange;

            for (int i = 0; i < wrap.Classes.Count; i++)
                AddChildNode(wrap.Classes[i]);
            for (int i = 0; i < wrap.Enums.Count; i++)
                AddChildNode(wrap.Enums[i]);
        }
        private void RemoveRootNode(NamespaceWrap wrap)
        {
            for (int i = 0; i < wrap.Classes.Count; i++)
                RemoveChildNode(wrap.Classes[i]);
            for (int i = 0; i < wrap.Enums.Count; i++)
                RemoveChildNode(wrap.Enums[i]);

            TreeNode root = _nodeTreeView.Nodes[wrap.FullName];
            _nodeTreeView.Nodes.Remove(root);
            wrap.OnNameChange -= OnNamespaceNameChange;
            wrap.OnNodeStateChange -= OnNodeStateChange;
            PoolManager.Ins.Push(root);
        }
        private void AddChildNode(TypeWrap wrap)
        {
            NamespaceWrap namespaceWrap = wrap.Namespace;
            TreeNode rootNode = _nodeTreeView.Nodes[namespaceWrap.FullName];
            TreeNode sub = CreateNode(wrap);
            rootNode.Nodes.Add(sub);
            wrap.OnNameChange += OnNodeNameChange;
            wrap.OnDescChange += OnNodeNameChange;
            wrap.OnNodeStateChange += OnNodeStateChange;
        }
        private void RemoveChildNode(TypeWrap wrap)
        {
            NamespaceWrap namespaceWrap = wrap.Namespace;
            TreeNode root = _nodeTreeView.Nodes[namespaceWrap.FullName];
            var childNode = root.Nodes[wrap.FullName];
            root.Nodes.Remove(childNode);
            wrap.OnNameChange -= OnNodeNameChange;
            wrap.OnNodeStateChange -= OnNodeStateChange;
            PoolManager.Ins.Push(childNode);
        }
        private void OnNamespaceNameChange(BaseWrap wrap, string src)
        {
            var namespaceWrap = wrap as NamespaceWrap;
            TreeNode root = _nodeTreeView.Nodes[src];
            root.Text = namespaceWrap.DisplayName;
            root.Name = namespaceWrap.FullName;
            var classes = namespaceWrap.Classes;
            for (int i = 0; i < classes.Count; i++)
            {
                var cls = classes[i];
                string key = cls.FullName.Replace(wrap.FullName, src);
                var node = root.Nodes[key];
                node.Text = cls.DisplayName;
                node.Name = cls.FullName;
            }
            var enums = namespaceWrap.Enums;
            for (int i = 0; i < enums.Count; i++)
            {
                var enm = enums[i];
                string key = enm.FullName.Replace(wrap.FullName, src);
                var node = root.Nodes[key];
                node.Text = enm.DisplayName;
                node.Name = enm.FullName;
            }
        }
        private void OnNodeNameChange(BaseWrap wrap, string src)
        {
            var typeWrap = wrap as TypeWrap;
            string ns = typeWrap.Namespace.FullName;
            TreeNode root = _nodeTreeView.Nodes[ns];
            TreeNode node = root.Nodes[src];
            node.Text = wrap.DisplayName;
            node.Name = wrap.FullName;
        }
        private void OnNodeStateChange(BaseWrap wrap, NodeState state)
        {
            TreeNode node = null;
            string key = wrap.FullName;
            var nodes = _nodeTreeView.Nodes;
            if (wrap is NamespaceWrap)
            {
                if (!nodes.ContainsKey(key))
                    Util.MsgError("[NamespaceDock]TreeView中无法通过Key:{0}找到Namespace节点!", key);
                node = nodes[key];
            }
            else
            {
                TypeWrap tw = wrap as TypeWrap;
                key = tw.Namespace.FullName;
                if (!nodes.ContainsKey(key))
                    Util.MsgError("[NamespaceDock]TreeView中无法通过Key:{0}找到Namespace节点!", key);
                else
                {
                    var root = _nodeTreeView.Nodes[key];
                    key = wrap.FullName;
                    if (!root.Nodes.ContainsKey(key))
                        Util.MsgError("[NamespaceDock]TreeView中无法通过Key:{0}找到Class,Enum节点!", key);
                    node = root.Nodes[key];
                }
            }

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
                default:
                    node.ForeColor = Color.Gray;
                    break;
            }
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
        public void SaveNamespaceWrap(NamespaceWrap wrap)
        {
            wrap.Save();
        }

        #region GUI事件
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
        /// <summary>
        /// 鼠标右键菜单 
        /// </summary>
        private void NodeTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            TreeNode node = _nodeTreeView.GetNodeAt(e.X, e.Y);
            if (node != null)
                _nodeTreeView.SelectedNode = node;
            var selected = _nodeTreeView.SelectedNode;
            var nsw = selected.Tag as NamespaceWrap;
            bool isNsw = selected != null && nsw != null;                  //是命名空间节点

            _modifyRootMenuItem.Visible = isNsw && !selected.Name.Equals(Util.EmptyNamespace);
            _saveRootMenuItem.Visible = isNsw;
            _includeMenuItem.Visible = isNsw;
            _excludeMenuItem.Visible = isNsw;
            //在排除情况下的命名空间,直接删除,会影响其他模块
            if (isNsw)
                _deleteMenuItem.Visible = (nsw.NodeState & NodeState.Exclude) != 0;
            else
            {//类/枚举等删除
                var typewrap = selected.Tag as TypeWrap;
                _deleteMenuItem.Visible = (typewrap.NodeState & NodeState.Include) != 0;
            }
            _rootSeparator.Visible = isNsw;

            var point = _nodeTreeView.PointToScreen(e.Location);
            _nodeMenu.Show(point);
        }
        /// <summary>
        /// 删除命名空间
        /// </summary>
        private void NodeTreeView_DeleteNode(object sender, EventArgs e)
        {
            var node = _nodeTreeView.SelectedNode;
            if (node == null) return;

            object wrap = node.Tag;
            switch (wrap)
            {
                case NamespaceWrap nsw:
                    if ((nsw.NodeState & NodeState.Exclude) == 0)
                        return;

                    var result = MessageBox.Show("删除文件可能造成其他丢失数据!请保证数据仅在该模块使用.", "删除命名空间", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.OK)
                    {
                        WrapManager.Ins.Current.RemoveImport(nsw);
                        PoolManager.Ins.Push(nsw);

                        //移除命名空间相关数据
                        NamespaceWrap.Remove(nsw);
                        Util.Delete(nsw.FilePath);
                        WrapManager.Ins.Current.Check();
                    }
                    break;
                case TypeWrap tw:
                    tw.Namespace.RemoveTypeWrap(tw);
                    break;
                default:
                    break;
            }
            PoolManager.Ins.Push(node);
            WrapManager.Ins.Current.Check();
        }
        private void NodeTreeView_Modify(object sender, EventArgs e)
        {
            var nsw = (_nodeTreeView.SelectedNode.Tag as NamespaceWrap);
            NamespaceInfoDock.Ins.Show(nsw);
        }
        private void NodeTreeView_Save(object sender, EventArgs e)
        {
            var wrap = (_nodeTreeView.SelectedNode.Tag as NamespaceWrap);
            SaveNamespaceWrap(wrap);
            Debug.LogFormat("[NamespaceDock]成功手动保存命名空间{0}!", wrap.FullName);
        }
        private void NodeTreeView_Include(object sender, EventArgs e)
        {
            var node = _nodeTreeView.SelectedNode;
            object wrap = node.Tag;
            if (wrap is NamespaceWrap)
            {
                var nsw = wrap as NamespaceWrap;
                WrapManager.Ins.Current.AddImportNoEvent(nsw);
                WrapManager.Ins.Current.Check();
            }
        }
        private void NodeTreeView_Exclude(object sender, EventArgs e)
        {
            var node = _nodeTreeView.SelectedNode;
            object wrap = node.Tag;
            if (wrap is NamespaceWrap)
            {
                var nsw = wrap as NamespaceWrap;
                WrapManager.Ins.Current.RemoveImportNoEvent(nsw);
                RemoveRootNode(nsw);
                WrapManager.Ins.Current.Check();
            }
        }
        private void CommitToLib(object sender, EventArgs e)
        {

        }
        private void UpdateToLib(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 显示所有节点
        /// </summary>
        private void ShowAllBox_CheckedChanged(object sender, EventArgs e)
        {
            var state = sender as CheckBox;
            var nodes = _nodeTreeView.Nodes;
            if (state.Checked)
            {
                foreach (var item in NamespaceWrap.Dict)
                {
                    var root = item.Value;
                    if ((root.NodeState & NodeState.Exclude) == 0) continue;

                    if (root.FullName.IndexOf(_nodeFilterBox.Text, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        AddRootNode(root);
                        root.SetStateWithType(NodeState.Exclude);
                        //OnNodeStateChange(root, NodeState.Exclude);
                        //foreach (var cls in root.Classes)
                        //    OnNodeStateChange(cls, NodeState.Exclude);
                        //foreach (var enm in root.Enums)
                        //    OnNodeStateChange(enm, NodeState.Exclude);
                    }
                    else
                    {
                        Debug.LogErrorFormat("[NamespaceDock]{0}模块中的命名空间{1}居然会不在字典中!?\n改了命名空间库,却没修改模块?",
                            WrapManager.Ins.Current.Name, item.Key);
                    }
                }
            }
            else
            {
                foreach (var item in NamespaceWrap.Dict)
                {
                    var root = item.Value;
                    if ((root.NodeState & NodeState.Exclude) == 0) continue;

                    RemoveRootNode(root);
                }

            }
        }
        /// <summary>
        /// 命名空间节点查询
        /// </summary>
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
                if (!nodes[i].Name.StartsWith(find.Text, StringComparison.OrdinalIgnoreCase))
                    _removes.Add(nodes[i]);
            }
            _nodeTreeView.BeginUpdate();
            for (int i = 0; i < _removes.Count; i++)
                nodes.Remove(_removes[i]);
            _nodeTreeView.EndUpdate();
        }
        #endregion
    }
}
