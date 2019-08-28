using Description;
using Description.Properties;
using Description.Xml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Description
{
    public partial class MainWindow : Form
    {
        static MainWindow _ins;
        public static MainWindow Ins { get { return _ins; } }
        public DockPanel _dock { get { return _dockPanel; } }

        public MainWindow()
        {
            _ins = this;
            InitializeComponent();

            InitSettings();
            InitXml();
            ConsoleDock.Inspect();
            TypeEditorDock.Inspect();
            FindNamespaceDock.Inspect();

            ConsoleDock.Ins.Log("初始化成功~");
        }
        protected override void OnClosed(EventArgs e)
        {
            _module = null;
            _allNamespace.Clear();
            _ins = null;
            base.OnClosed(e);
        }

        private void InitSettings()
        {
            if (!Settings.Default.ModuleDir.IsEmpty())
                Util.ModuleDir = Settings.Default.ModuleDir;
            if (!Settings.Default.NamespaceDir.IsEmpty())
                Util.NamespaceDir = Settings.Default.NamespaceDir;
            Util.LastRecord = Settings.Default.LastModule;

            if (!Directory.Exists(Util.ModuleDir))
                Directory.CreateDirectory(Util.ModuleDir);
            if (!Directory.Exists(Util.NamespaceDir))
                Directory.CreateDirectory(Util.NamespaceDir);
        }

        #region Xml数据管理
        public string Path { get { return _path; } }
        public Dictionary<string, NamespaceXml> Namespaces { get { return _allNamespace; } }
        private Dictionary<string, NamespaceXml> _allNamespace;
        //private List<TypeEditorDock> _tempType;

        private string _path;
        private ModuleXml _module;
        private object cs;

        private void InitXml()
        {
            _allNamespace = new Dictionary<string, NamespaceXml>();
            string[] fs = Directory.GetFiles(Util.NamespaceDir);
            for (int i = 0; i < fs.Length; i++)
            {
                string path = fs[i];
                string name = System.IO.Path.GetFileNameWithoutExtension(path);
                var value = Util.Deserialize<NamespaceXml>(path);
                _allNamespace.Add(name, value);
            }

            string epath = Util.Format("{0}\\{1}.xml", Util.NamespaceDir, Util.EmptyNamespace);
            if (!File.Exists(epath))
            {
                var enamespace = new NamespaceXml();
                Util.Serialize(epath, enamespace);
                string ename = System.IO.Path.GetFileNameWithoutExtension(epath);
                _allNamespace.Add(ename, enamespace);
            }
        }
        public void CreateModule(string path)
        {
            ModuleXml module = new ModuleXml();
            Util.Serialize(path, module);
            ConsoleDock.Ins.LogFormat("创建模板{0}", path);
        }
        public void OpenDefault()
        {
            OpenModule(Util.DefaultModule);
        }
        public void OpenModule(string path)
        {
            this._path = path;

            if (File.Exists(path))
                _module = Util.Deserialize<ModuleXml>(path);
            else
                _module = new ModuleXml();
            MainWindow.Ins.Text = Util.Format("结构描述 - {0}", path);
            if (_module.Imports == null)
                return;

            HashSet<string> imports = new HashSet<string>(_module.Imports);
            foreach (var item in _allNamespace)
                item.Value.IsValide = !imports.Contains(item.Key);
            ConsoleDock.Ins.LogFormat("打开模板{0}", path);
        }
        public void CloseModule()
        {
            SaveModule();
            OpenDefault();
        }
        public void SaveModule()
        {
            Util.Serialize(_path, _module);
            ConsoleDock.Ins.LogFormat("保存模板{0}", _path);
        }
        public void SaveAnotherModule(string path)
        {
            Util.Serialize(path, _module);
            ConsoleDock.Ins.LogFormat("另存模板{0}", path);
        }

        #region 命名空间操作
        public bool CreateNamespace(string name, string module = Util.DefaultModuleName)
        {
            if (_allNamespace.ContainsKey(name))
            {
                Util.MsgWarning("创建命名空间", "命名空间{0}已经存在.", name);
                return false;
            }
            else
            {
                NamespaceXml namespaceXml = new NamespaceXml()
                {
                    Name = name,
                    Classes = new List<ClassXml>(),
                    Enums = new List<EnumXml>(),
                };
                string path = Util.Format("{0}\\{1}.xml", Util.NamespaceDir, name);
                try
                {
                    Util.Serialize(path, namespaceXml);
                }
                catch (Exception e)
                {
                    ConsoleDock.Ins.LogErrorFormat("创建命名空间{0}失败!\n{1}\n{2}\n",
                       name, e.Message, e.StackTrace);
                    return false;
                }
                _allNamespace.Add(namespaceXml.Name, namespaceXml);
                return true;
            }
        }
        public bool DeleteNamespace(string name)
        {
            if (_allNamespace.ContainsKey(name))
            {
                try
                {
                    string path = Util.Format("{0}\\{1}.xml", Util.NamespaceDir, name);
                    File.Delete(path);
                }
                catch (Exception e)
                {
                    ConsoleDock.Ins.LogErrorFormat("删除命名空间{0}失败!\n{1}\n{2}\n",
                       name, e.Message, e.StackTrace);
                    return false;
                }
                _allNamespace.Remove(name);
                return true;
            }
            return false;
        }
        #endregion

        #region 类型操作
        public bool CreateType(int type, string name, string namespace1)
        {
            if (_allNamespace.ContainsKey(namespace1))
            {
                bool isExist = false;
                string fullName = Util.Format("{0}.{1}", namespace1, name);
                NamespaceXml namespaceXml = _allNamespace[namespace1];
                if (type == CreatorDock.CLASS)
                {
                    List<ClassXml> cs = namespaceXml.Classes;
                    for (int i = 0; i < cs.Count; i++)
                        isExist = cs[i].FullName == fullName;
                    if (!isExist)
                    {
                        ClassXml classXml = new ClassXml()
                        {
                            Name = name,
                            Namespace = namespace1,
                            Fields = new List<FieldXml>(),
                        };
                        namespaceXml.Classes.Add(classXml);
                    }
                    else
                    {
                        Util.MsgWarning("创建Class", "{0}已经存在!", fullName);
                        return false;
                    }
                }
                else if (type == CreatorDock.ENUM)
                {
                    List<EnumXml> es = namespaceXml.Enums;
                    for (int i = 0; i < es.Count; i++)
                        isExist = es[i].FullName == fullName;
                    if (!isExist)
                    {
                        EnumXml enumXml = new EnumXml()
                        {
                            Name = name,
                            Namespace = namespace1,
                            Items = new List<EnumItemXml>(),
                        };
                        namespaceXml.Enums.Add(enumXml);
                    }
                    else
                    {
                        Util.MsgWarning("创建Enum", "{0}已经存在!", fullName);
                        return false;
                    }
                }
                return true;
            }
            else
            {
                Util.MsgWarning("创建类型", "命名空间{0}不存在,请先创建命名空间.", namespace1);
                return false;
            }
        }
        public bool DeleteType(int type, string name, string namespace1)
        {
            if (_allNamespace.ContainsKey(namespace1))
            {
                string fullName = Util.Format("{0}.{1}", namespace1, name);
                NamespaceXml namespaceXml = _allNamespace[namespace1];
                if (type == CreatorDock.CLASS)
                {
                    List<ClassXml> cs = namespaceXml.Classes;
                    for (int i = 0; i < cs.Count; i++)
                    {
                        if (cs[i].FullName == fullName)
                        {
                            namespaceXml.Classes.RemoveAt(i);
                            break;
                        }
                    }
                }
                else if (type == CreatorDock.ENUM)
                {
                    List<EnumXml> es = namespaceXml.Enums;
                    for (int i = 0; i < es.Count; i++)
                    {
                        if (es[i].FullName == fullName)
                        {
                            namespaceXml.Enums.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            return false;
        }
        public void OpenType()
        {

        }
        public void SaveType()
        {

        }
        public void CloseType()
        {

        }
        #endregion
        #endregion

        #region 文件
        private void CreateItem_Click(object sender, EventArgs e)
        {
            var creator = new CreatorDock();
            creator.ShowDialog();
        }
        private void CreateModuleItem_Click(object sender, EventArgs e)
        {
            _saveFileDialog.InitialDirectory = Util.ModuleDir;
            _saveFileDialog.Title = "创建模块";
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = _saveFileDialog.FileName;
                    CreateModule(fileName);
                }
                catch (Exception ex)
                {
                    ConsoleDock.Ins.LogErrorFormat("创建模板失败!\n{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }

        private void OpenModuleItem_Click(object sender, EventArgs e)
        {
            _openFileDialog.InitialDirectory = Util.ModuleDir;
            _openFileDialog.Title = "打开模块";
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = _openFileDialog.FileName;
                    OpenModule(fileName);
                }
                catch (Exception ex)
                {
                    ConsoleDock.Ins.LogErrorFormat("打开模板失败!\n{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }

        private void SaveModuleItem_Click(object sender, EventArgs e)
        {
            SaveModule();
        }

        private void SaveAnotherModuleItem_Click(object sender, EventArgs e)
        {
            _saveFileDialog.InitialDirectory = Util.ModuleDir;
            _saveFileDialog.Title = "另存模块";
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = _saveFileDialog.FileName;
                    SaveAnotherModule(fileName);
                }
                catch (Exception ex)
                {
                    ConsoleDock.Ins.LogErrorFormat("无法另存模块!\n{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }

        private void CloseModuleItem_Click(object sender, EventArgs e)
        {
            CloseModule();
        }
        #endregion

        #region 视图
        private void RefreshItem_Click(object sender, EventArgs e)
        {

        }
        private void OpenFindNamespaceItem_Click(object sender, EventArgs e)
        {
            FindNamespaceDock.Inspect();
        }

        private void OpenConsoleItem_Click(object sender, EventArgs e)
        {
            ConsoleDock.Inspect();
        }

        private void OpenTypeEditorItem_Click(object sender, EventArgs e)
        {
            TypeEditorDock.Inspect();
        }

        #endregion


    }
}
