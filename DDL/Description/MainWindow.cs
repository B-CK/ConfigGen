using Description;
using Description.Editor;
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

        public MainWindow()
        {
            _ins = this;
            Debug.Init();
            InitializeComponent();
            ConsoleDock.Inspect();

            MainSetup();
        }
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = ModuleWrap.TrSave();
            if (result == DialogResult.Cancel)
                e.Cancel = true;
        }
        protected override void OnClosed(EventArgs e)
        {
            Debug.Dispose();
            Settings.Default.Save();
            PoolManager.Ins.Clear();
            base.OnClosed(e);
            _ins = null;
        }
        private void MainSetup()
        {
            if (!Settings.Default.ModuleDir.IsEmpty())
                Util.ModuleDir = Settings.Default.ModuleDir;
            if (!Settings.Default.NamespaceDir.IsEmpty())
                Util.NamespaceDir = Settings.Default.NamespaceDir;
            if (!Directory.Exists(Util.ModuleDir))
                Directory.CreateDirectory(Util.ModuleDir);
            if (!Directory.Exists(Util.NamespaceDir))
                Directory.CreateDirectory(Util.NamespaceDir);
            Util.Groups = new string[Settings.Default.Groups.Count];
            Settings.Default.Groups.CopyTo(Util.Groups, 0);

            NamespaceWrap.InitNamespaces();
            ModuleWrap.InitModule();
            NamespaceDock.Inspect();
            ClassWrap.RecordChildren();

            Debug.Log("初始化成功~");
        }


        #region 文件
        /// <summary>
        /// 新建
        /// </summary>
        private void CreateItem_Click(object sender, EventArgs e)
        {
            var creator = new CreatorDock();
            creator.ShowDialog();
        }
        /// <summary>
        /// 保存类型
        /// </summary>
        private void SaveTypeMenuItem_Click(object sender, EventArgs e)
        {
            var dock = _dockPanel.ActiveDocument as EditorDock;
            if (dock == null) return;
            EditorDock.SaveDock(dock);
        }

        /// <summary>
        /// 新建模块
        /// </summary>
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
                    ModuleWrap.Open(fileName);
                    NamespaceDock.Inspect();
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("创建模板失败!\n{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }
        /// <summary>
        /// 打开模块
        /// </summary>
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
                    NamespaceDock.Inspect();
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("打开模板失败!\n{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }
        /// <summary>
        /// 保存模块
        /// </summary>
        private void SaveModuleItem_Click(object sender, EventArgs e)
        {
            ModuleWrap.SilientSave();
            var imps = ModuleWrap.Current.Imports;
            for (int i = 0; i < imps.Count; i++)
            {
                var wrap = NamespaceWrap.AllNamespaces[imps[i]];
                if ((wrap.NodeState & NodeState.Modify & NodeState.Modify) != 0)
                    NamespaceDock.Ins.SaveNamespaceWrap(wrap);
            }
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
                    Debug.LogErrorFormat("无法另存模块!\n{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }
        #endregion

        #region 编辑
        private void RefreshItem_Click(object sender, EventArgs e)
        {
            var result = ModuleWrap.TrSave();
            if (result != DialogResult.Cancel)
            {
                ModuleWrap.CloseCurrent();
                NamespaceWrap.ClearAll();
                EditorDock.CloseAll();

                MainSetup();
            }
        }
        private void CheckError_Click(object sender, EventArgs e)
        {
            NamespaceDock.Ins.UpdateModule();
        }
        #endregion

        #region 视图
        private void OpenFindNamespaceItem_Click(object sender, EventArgs e)
        {
            NamespaceDock.Inspect();
        }
        private void OpenConsoleItem_Click(object sender, EventArgs e)
        {
            ConsoleDock.Inspect();
        }
        #endregion
    }
}
