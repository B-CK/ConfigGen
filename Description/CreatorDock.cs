using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Description.Wrap;

namespace Description
{
    public partial class CreatorDock : Form
    {
        private string[] _modules;
        private string[] _namespaces;
        public CreatorDock()
        {
            InitializeComponent();

            _modules = new string[1] { ModuleWrap.Current.Name };
            _namespaces = new List<string>(NamespaceWrap.AllNamespaces.Keys).ToArray();

            _createListBox.SelectedIndex = 0;
        }

        private void SelectType()
        {
            _2Label.Text = "命名空间:";
            _2ComboBox.Items.Clear();
            _2ComboBox.Items.AddRange(_namespaces);
            _2ComboBox.Text = "";
            _2ComboBox.Enabled = true;
        }
        private void SelectNamespace()
        {
            _2Label.Text = "模块名称:";
            _2ComboBox.Items.Clear();
            _2ComboBox.Items.AddRange(_modules);
            _2ComboBox.Text = _modules[0];
            _2ComboBox.Enabled = false;
        }
        private void CreateListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (_createListBox.SelectedIndex)
            {
                case 0:
                case 1:
                    SelectType(); break;
                case 2: SelectNamespace(); break;
                default:
                    ConsoleDock.Ins.LogErrorFormat("创建内容选项中不存在项{0}", _createListBox.SelectedIndex);
                    break;
            }
        }
        private void OkButton_Click(object sender, EventArgs e)
        {
            Create();
        }
        private void Create()
        {
            string name = _nameTextBox.Text;
            if (!CheckName(name)) return;

            string namespace0 = _2ComboBox.Text.IsEmpty() ? Util.EmptyNamespace : _2ComboBox.Text;
            switch (_createListBox.SelectedIndex)
            {
                case 0:
                    {
                        if (!CheckNamespace(namespace0)) return;

                        NamespaceWrap nsw = NamespaceWrap.GetNamespace(namespace0);
                        if (!nsw.Contains(name))
                        {
                            var wrap = ClassWrap.Create(name, nsw);
                            FindNamespaceDock.Ins.AddSubNode(wrap, nsw);
                            TypeEditorDock.Create(wrap);
                            Close();
                        }
                        else
                            Util.MsgWarning("创建Class", "[Class]{0}已经存在!", name);
                        break;
                    }
                case 1:
                    {
                        if (!CheckNamespace(namespace0)) return;

                        NamespaceWrap nsw = NamespaceWrap.GetNamespace(namespace0);
                        if (!nsw.Contains(name))
                        {
                            var wrap = EnumWrap.Create(name, nsw);
                            FindNamespaceDock.Ins.AddSubNode(wrap, nsw);
                            TypeEditorDock.Create(wrap);
                            Close();
                        }
                        else
                            Util.MsgWarning("创建Enum", "[Class]{0}已经存在!", name);
                        break;
                    }
                case 2:
                    {
                        NamespaceWrap nsw = NamespaceWrap.GetNamespace(namespace0);
                        if (nsw == null)
                        {
                            FindNamespaceDock.Ins.AddRootNode(NamespaceWrap.Create(name));
                            Close();
                        }
                        else
                            Util.MsgWarning("创建Namespace", "命名空间{0}已经存在.", name);
                        break;
                    }
                default:
                    ConsoleDock.Ins.LogErrorFormat("创建内容选项中不存在项{0}", _createListBox.SelectedIndex);
                    break;
            }
        }
        private bool CheckName(string name)
        {
            if (name.IsEmpty())
            {
                Util.MsgError("错误", "名称不能为空!");
                return false;
            }
            else
                return true;
        }
        private bool CheckNamespace(string ns)
        {
            if (ns.IsEmpty())
            {
                Util.MsgError("错误", "命名空间名不能为空!");
                return false;
            }
            else
                return true;
        }
        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
