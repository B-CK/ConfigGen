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
        /// <summary>
        /// 创建类型
        /// </summary>
        private void SelectType()
        {
            _2Label.Text = "命名空间:";
            _2ComboBox.Items.Clear();
            _2ComboBox.Items.AddRange(NamespaceWrap.Array);
            _2ComboBox.Text = Util.EmptyNamespace;
            _2ComboBox.Enabled = true;
        }
        /// <summary>
        /// 创建命名空间
        /// </summary>
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
                    Debug.LogErrorFormat("创建内容选项中不存在项{0}", _createListBox.SelectedIndex);
                    break;
            }
        }
        private void OkButton_Click(object sender, EventArgs e)
        {
            string name = _nameTextBox.Text;
            if (!Util.CheckName(name)) return;

            switch (_createListBox.SelectedIndex)
            {
                case 0:
                    {
                        NamespaceWrap nsw = NamespaceWrap.GetNamespace(Util.EmptyNamespace);
                        if (!_2ComboBox.Text.IsEmpty())
                            nsw = _2ComboBox.SelectedItem as NamespaceWrap;
                        string fullName = Util.Format("{0}.{1}", nsw.FullName, name);
                        if (ClassWrap.Dict.ContainsKey(fullName))
                        {
                            Util.MsgWarning("[Class]类型{0}已经存在!", fullName);
                            return;
                        }

                        var wrap = ClassWrap.Create(name, nsw);
                        wrap.AddNodeState(NodeState.Modify | NodeState.Include);
                        nsw.SetDirty();
                        NamespaceDock.Ins.AddType2Namespace(wrap, nsw);
                        ClassEditorDock.Create(wrap);
                        Close();
                        break;
                    }
                case 1:
                    {
                        NamespaceWrap nsw = NamespaceWrap.GetNamespace(Util.EmptyNamespace);
                        if (!_2ComboBox.Text.IsEmpty())
                            nsw = _2ComboBox.SelectedItem as NamespaceWrap;
                        string fullName = Util.Format("{0}.{1}", nsw.FullName, name);
                        if (EnumWrap.Dict.ContainsKey(fullName))
                        {
                            Util.MsgWarning("[Enum]枚举{0}已经存在!", fullName);
                            return;
                        }

                        var wrap = EnumWrap.Create(name, nsw);
                        wrap.AddNodeState(NodeState.Modify | NodeState.Include);
                        nsw.SetDirty();
                        NamespaceDock.Ins.AddType2Namespace(wrap, nsw);
                        EnumEditorDock.Create(wrap);
                        Close();
                        break;
                    }
                case 2:
                    {
                        NamespaceWrap nsw = NamespaceWrap.GetNamespace(name);
                        if (nsw == null)
                        {
                            NamespaceWrap wrap = NamespaceWrap.Create(name);
                            wrap.SetDirty();
                            ModuleWrap.Default.AddImport(wrap);
                            ModuleWrap.Current.AddImport(wrap);
                            NamespaceDock.Ins.AddRootNode(wrap);
                            Close();
                        }
                        else
                            Util.MsgWarning("命名空间{0}已经存在.", name);
                        break;
                    }
                default:
                    Debug.LogErrorFormat("创建内容选项中不存在项{0}", _createListBox.SelectedIndex);
                    break;
            }
        }
        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
