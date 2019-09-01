using Description;
using Description.Properties;
using Description.Wrap;
using Description.Xml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using SPath = System.IO.Path;

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
            InitDefaultModule();
            NamespaceWrap.Init();
            ConsoleDock.Inspect();
            FindNamespaceDock.Inspect();

            ConsoleDock.Ins.Log("初始化成功~");
        }
        protected override void OnClosed(EventArgs e)
        {
            _module = null;
            _ins = null;
            PoolManager.Ins.Clear();
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

        #region Module:模板管理
        //private List<TypeEditorDock> _tempType;
        public ModuleXml CurrentModule { get { return _module; } }
        private string _path;
        private ModuleXml _module;

        private void InitDefaultModule()
        {
            if (!File.Exists(Util.DefaultModule))
                CreateModule(Util.DefaultModule);
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
            Text = Util.Format("结构描述 - {0}", path);
            if (_module.Imports == null)
                return;

            NamespaceWrap.UpdateImportedFlag(_module.Imports);
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

        public void CreateTypeDock()
        {

        }
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

        #region 编辑

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
             
        }

        #endregion


    }
}
