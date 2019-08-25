using Description;
using Description.Properties;
using Description.Xml;
using System;
using System.Collections.Generic;
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
            ConsoleDock.Inspect();
            TypeEditorDock.Inspect();
            FindNamespaceDock.Inspect();

            ConsoleDock.Ins.Log("初始化成功~");
        }
        protected override void OnClosed(EventArgs e)
        {
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
        private void DescriptorWindow_Load(object sender, EventArgs e)
        {

            return;




            //解析类型定义
            Dictionary<string, NamespaceXml> pairs = new Dictionary<string, NamespaceXml>();
            string path = "无法解析Xml.NamespaceDes";

            //try
            //{
            //    var configXml = Util.Deserialize(Setting.ConfigXml, typeof(ConfigXml)) as ConfigXml;
            //    if (configXml.Root.IsEmpty())
            //        throw new Exception("数据结构导出时必须指定命名空间根节点<Config Root=\"**\">");
            //    Setting.ConfigRootNode = configXml.Root;
            //    List<string> include = configXml.Import;
            //    for (int i = 0; i < include.Count; i++)
            //    {
            //        path = Util.GetAbsPath(include[i]);
            //        var des = Util.Deserialize(path, typeof(NamespaceXml)) as NamespaceXml;
            //        if (pairs.ContainsKey(des.Name))
            //        {
            //            pairs[des.Name].Classes.AddRange(des.Classes);
            //            pairs[des.Name].Enums.AddRange(des.Enums);
            //        }
            //        else
            //        {
            //            des.XmlDir = Path.GetDirectoryName(path);
            //            pairs.Add(des.Name, des);
            //        }
            //    }
            //    GroupInfo.LoadGroup(configXml.Group);
            //}
            //catch (Exception e)
            //{
            //    Util.LogErrorFormat("路径:{0} Error:{1}\n{2}", path, e.Message, e.StackTrace);
            //    return;
            //}

            //HashSet<string> fullHash = new HashSet<string>();
            //var nit = pairs.GetEnumerator();
            //while (nit.MoveNext())
            //{
            //    var item = nit.Current;
            //    string namespace0 = item.Key;
            //    for (int i = 0; i < item.Value.Classes.Count; i++)
            //    {
            //        ClassXml classDes = item.Value.Classes[i];
            //        var cls = new ClassInfo(classDes, namespace0);
            //        if (cls.IsConfig())
            //            new ConfigInfo(classDes, namespace0, item.Value.XmlDir);
            //    }
            //    for (int i = 0; i < item.Value.Enums.Count; i++)
            //        new EnumInfo(item.Value.Enums[i], namespace0);
            //}
        }

        #region 文件
        private void CreateModuleItem_Click(object sender, EventArgs e)
        {
            _saveFileDialog.InitialDirectory = Util.ModuleDir;
            _saveFileDialog.Title = "创建模块";
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = _saveFileDialog.FileName;
                    Module.Ins.Create(fileName);
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
                    Module.Ins.Open(fileName);
                }
                catch (Exception ex)
                {
                    ConsoleDock.Ins.LogErrorFormat("打开模板失败!\n{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }

        private void SaveModuleItem_Click(object sender, EventArgs e)
        {
            Module.Ins.Save();
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
                    Module.Ins.SaveAnother(fileName);
                }
                catch (Exception ex)
                {
                    ConsoleDock.Ins.LogErrorFormat("无法另存模块!\n{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }

        private void CloseModuleItem_Click(object sender, EventArgs e)
        {
            Module.Ins.Close();
        }
        #endregion

        #region 视图
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
