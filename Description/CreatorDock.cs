using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Description.Wrap;
using Description.Editor;

namespace Description
{
    public partial class CreatorDock : Form
    {
        private string[] _modules;
        public CreatorDock()
        {
            InitializeComponent();

            _modules = new string[1] { ModuleWrap.Current.Name };
            _createListBox.SelectedIndex = 0;
        }
        private void SelectType()
        {
            _2Label.Text = "命名空间:";
            _2ComboBox.Items.Clear();
            _2ComboBox.Items.AddRange(NamespaceWrap.Namespaces);
            _2ComboBox.Text = NamespaceWrap.Namespaces.Length > 0 ? NamespaceWrap.Namespaces[0].FullName : "";
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
        private bool CheckName(string name, string ns)
        {
            string fullName = Util.Format("{0}.{1}", name, ns);
            if (name.IsEmpty())
            {
                Util.MsgWarning("名称不能为空!");
                return false;
            }
            else if (!Util.CheckIdentifier(name))
            {
                return false;
            }
            else if (ClassWrap.ClassDict.ContainsKey(fullName))
            {
                Util.MsgWarning("类型{0}已经存在!", fullName);
                return false;
            }
            return true;
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
            string name = _nameTextBox.Text;
            string namespace0 = _2ComboBox.Text.IsEmpty() ? Util.EmptyNamespace : _2ComboBox.Text;
            if (!CheckName(name, namespace0)) return;

            switch (_createListBox.SelectedIndex)
            {
                case 0:
                    {
                        NamespaceWrap nsw = NamespaceWrap.GetNamespace(namespace0);
                        if (!nsw.Contains(name))
                        {
                            var wrap = ClassWrap.Create(name, nsw);                       
                            FindNamespaceDock.Ins.AddClass2Namespace(wrap, nsw);
                            ClassEditorDock.Create(wrap);
                            Close();
                        }
                        else
                            Util.MsgWarning("[Class]{0}已经存在!", name);
                        break;
                    }
                case 1:
                    {
                        NamespaceWrap nsw = NamespaceWrap.GetNamespace(namespace0);
                        if (!nsw.Contains(name))
                        {
                            var wrap = EnumWrap.Create(name, nsw);                   
                            FindNamespaceDock.Ins.AddEnum2Namespace(wrap, nsw);
                            EnumEditorDock.Create(wrap);
                            Close();
                        }
                        else
                            Util.MsgWarning("[Class]{0}已经存在!", name);
                        break;
                    }
                case 2:
                    {
                        NamespaceWrap nsw = NamespaceWrap.GetNamespace(namespace0);
                        if (nsw == null)
                        {                            
                            NamespaceWrap wrap = NamespaceWrap.Create(name);
                            wrap.SetDirty();
                            wrap.SetNodeState(NodeState.Modify);
                            ModuleWrap.Default.AddImport(wrap);
                            ModuleWrap.Current.AddImport(wrap);
                            FindNamespaceDock.Ins.AddRootNode(wrap);
                            Close();
                        }
                        else
                            Util.MsgWarning("命名空间{0}已经存在.", name);
                        break;
                    }
                default:
                    ConsoleDock.Ins.LogErrorFormat("创建内容选项中不存在项{0}", _createListBox.SelectedIndex);
                    break;
            }
        }
        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
