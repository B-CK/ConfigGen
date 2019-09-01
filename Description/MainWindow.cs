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
            ModuleWrap.InitDefaultModule();
            NamespaceWrap.Init();
            ConsoleDock.Inspect();
            FindNamespaceDock.Inspect();

            ConsoleDock.Ins.Log("初始化成功~");
        }
        protected override void OnClosed(EventArgs e)
        {
            ModuleWrap.Default.Save();
            ModuleWrap.Current.Save();
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
                    ModuleWrap.Create(fileName);
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
                    ModuleWrap.Open(fileName);
                }
                catch (Exception ex)
                {
                    ConsoleDock.Ins.LogErrorFormat("打开模板失败!\n{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }

        private void SaveModuleItem_Click(object sender, EventArgs e)
        {
            ModuleWrap.Current.Save();
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
                    ModuleWrap.Current.SaveAnother(fileName);
                }
                catch (Exception ex)
                {
                    ConsoleDock.Ins.LogErrorFormat("无法另存模块!\n{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }

        private void CloseModuleItem_Click(object sender, EventArgs e)
        {
            ModuleWrap.Current.Close();
            ModuleWrap.OpenDefault();
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
