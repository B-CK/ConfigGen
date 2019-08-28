using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Description
{
    public partial class CreatorDock : Form
    {
        public const int CLASS = 0;
        public const int ENUM = 1;


        private string[] _modules;
        private string[] _namespaces;
        public CreatorDock()
        {
            InitializeComponent();

            _modules = ConvertRelPath(Directory.GetFiles(Util.ModuleDir, "*.xml"));
            _namespaces = ConvertRelPath(Directory.GetFiles(Util.NamespaceDir, "*.xml"));
            _createListBox.SelectedIndex = 0;
        }
        private string[] ConvertRelPath(string[] paths)
        {
            for (int i = 0; i < paths.Length; i++)
                paths[i] = Path.GetFileNameWithoutExtension(paths[i]);
            return paths;
        }

        private void SelectType()
        {
            _2Label.Text = "命名空间:";
            _2ComboBox.Items.Clear();
            _2ComboBox.Items.AddRange(_namespaces);
        }
        private void SelectNamespace()
        {
            _2Label.Text = "模块名称:";
            _2ComboBox.Items.Clear();
            _2ComboBox.Items.AddRange(_modules);
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
            bool result = false;
            string name = _nameTextBox.Text;
            string txt2 = _2ComboBox.Text.IsEmpty() ? Util.EmptyNamespace : _2ComboBox.Text;
            switch (_createListBox.SelectedIndex)
            {
                case 0: result = MainWindow.Ins.CreateType(CLASS, name, txt2); break;
                case 1: result = MainWindow.Ins.CreateType(ENUM, name, txt2); break;
                case 2: result = MainWindow.Ins.CreateNamespace(name, txt2); break;
                default:
                    ConsoleDock.Ins.LogErrorFormat("创建内容选项中不存在项{0}", _createListBox.SelectedIndex);
                    break;
            }
            if (result)
                Close();
        }

        private void CancleButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
