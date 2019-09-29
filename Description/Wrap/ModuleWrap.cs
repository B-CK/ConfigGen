using Description.Editor;
using Description.Xml;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Description.Wrap
{
    internal class ModuleWrap : BaseWrap
    {
        public static ModuleWrap Default { get { return _default; } }
        public static ModuleWrap Current { get { return _current; } }
        static ModuleWrap _default;
        static ModuleWrap _current;
        static int _noSaveNum = 0;
        public static void InitModule()
        {
            ModuleXml xml = null;
            if (!File.Exists(Util.DefaultModule))
            {
                var module = new ModuleXml() { Name = Util.DefaultModuleName };
                Util.Serialize(Util.DefaultModule, module);
            }
            else
                xml = Util.Deserialize<ModuleXml>(Util.DefaultModule);
            _default = new ModuleWrap(xml);
            var allNsw = NamespaceWrap.AllNamespaces;
            foreach (var item in allNsw)
                _default.AddImport(item.Value, true);
            _default.Save(false);

            //打开模块,即当前模块
            Open(Util.LastRecord);
        }

        public static ModuleWrap Create(string path)
        {
            ModuleXml xml = null;
            if (!File.Exists(path))
            {
                xml = new ModuleXml() { Name = Path.GetFileNameWithoutExtension(path) };
                Util.Serialize(path, xml);
            }
            else
                xml = Util.Deserialize<ModuleXml>(path);
            ConsoleDock.Ins.LogFormat("创建模块{0}", path);
            ModuleWrap wrap = PoolManager.Ins.Pop<ModuleWrap>();
            if (wrap == null) wrap = new ModuleWrap(xml);
            return wrap;
        }
        public static ModuleWrap Open(string path)
        {
            if (_current != null && path == _current.FullName)
                return _current;

            Util.LastRecord = path;
            if (File.Exists(path))
            {
                ModuleWrap wrap = PoolManager.Ins.Pop<ModuleWrap>();
                if (path == Util.DefaultModule && _default != null)
                    wrap = _default;
                else
                {
                    var xml = Util.Deserialize<ModuleXml>(path);
                    if (wrap == null) wrap = new ModuleWrap(xml);
                }

                if (_current != null)
                {
                    TrSave();
                    if (_default != _current)
                        _current.Close();
                }
                _current = wrap;
            }
            else
            {
                ConsoleDock.Ins.LogWarningFormat("模块{0}不存在,开启默认模块.", path);
                _current = _default;
            }

            _noSaveNum = 0;
            _current.InitState();
            MainWindow.Ins.Text = Util.Format("结构描述 - {0}", _current.FullName);
            ConsoleDock.Ins.LogFormat("打开模板{0}", path);
            return _current;
        }
        public static void AddDirty() { ++_noSaveNum; }
        public static void RemoveDirty() { --_noSaveNum; }
        public static void SilientSave(bool saveNamespace = false)
        {
            Default.Save(saveNamespace);
            Current.Save(saveNamespace);
            EditorDock.CloseAll();
        }
        public static DialogResult TrSave()
        {
            if (EditorDock.CheckOpenDock() || _noSaveNum != 0)
            {
                var result = MessageBox.Show("当前模块未保存,是否保存?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return DialogResult.Cancel;
                    case DialogResult.Yes:
                        EditorDock.SaveAll();
                        Default.Save();
                        Current.Save();
                        EditorDock.CloseAll();
                        return DialogResult.Yes;
                    case DialogResult.No:
                        Default.Save(false);
                        Current.Save(false);
                        EditorDock.CloseAll();
                        return DialogResult.No;
                }
            }
            else
            {
                SilientSave();
            }
            return DialogResult.None;
        }
        /// <summary>
        /// 清理无效命名空间
        /// </summary>
        public static void ClearDump()
        {

        }
        public override string FullName { get { return _path; } }

        public List<string> Imports { get { return _imports; } }

        public override string DisplayName => FullName;


        private List<string> _imports;
        private ModuleXml _xml;
        private string _path;
        protected ModuleWrap(ModuleXml xml) : base(xml.Name)
        {
            _xml = xml;
            _path = Util.GetModuleAbsPath(_name + ".xml");
            _imports = new List<string>();
            if (_xml.Imports == null) return;

            var imps = _xml.Imports;
            for (int i = 0; i < imps.Count; i++)
            {
                _imports.Add(imps[i]);
                Add(imps[i]);
            }
        }
        protected void InitState()
        {
            var allNsw = NamespaceWrap.AllNamespaces;
            var imps = Imports;
            for (int k = 0; k < imps.Count; k++)
            {
                string ns = imps[k];
                if (allNsw.ContainsKey(ns))
                {
                    var nsw = allNsw[ns];
                    nsw.AddNodeState(NodeState.Include);
                    var classes = nsw.Classes;
                    for (int i = 0; i < classes.Count; i++)
                        classes[i].AddNodeState(NodeState.Include);
                    var enums = nsw.Enums;
                    for (int i = 0; i < enums.Count; i++)
                        enums[i].AddNodeState(NodeState.Include);
                }
            }
        }
        public void SaveAnother(string path)
        {
            var another = new ModuleXml();
            another.Name = Path.GetFileNameWithoutExtension(path);
            another.Imports = _xml.Imports;
            Util.Serialize(path, another);
            ConsoleDock.Ins.LogFormat("另存模板{0}", path);
        }
        public void Close()
        {
            Dispose();
            if (this != _default)
                PoolManager.Ins.Push(this);
        }
        public void AddImport(NamespaceWrap wrap, bool isInit = false)
        {
            string name = wrap.FullName;
            if (Contains(name)) return;
            Add(name);
            _imports.Add(name);
            wrap.AddNodeState(NodeState.Include);
            if (!isInit)
                AddDirty();
        }
        public void RemoveImport(NamespaceWrap wrap)
        {
            string name = wrap.FullName;
            if (!Contains(name)) return;
            Remove(name);
            Imports.Remove(name);
            wrap.AddNodeState(NodeState.Exclude);
            AddDirty();
        }
        public void Save(bool saveNsw = true)
        {
            //_xml.Name = _name;
            _xml.Imports = _imports;
            Util.Serialize(_path, _xml);
            ConsoleDock.Ins.LogFormat("保存模板{0}", _path);

            if (!saveNsw) return;
            for (int i = 0; i < _imports.Count; i++)
            {
                string key = _imports[i];
                var nsw = NamespaceWrap.AllNamespaces[key];
                if (nsw != null && nsw.IsDirty)
                    nsw.Save();
            }
        }
        public void Cancle()
        {
            _name = "";
            _imports.Clear();
            for (int i = 0; i < _imports.Count; i++)
            {
                string key = _imports[i];
                var nsw = NamespaceWrap.AllNamespaces[key];
                if (nsw != null && nsw.IsDirty)
                    nsw.Cancle();
            }
        }

        public override bool Check()
        {
            ConsoleDock.Ins.LogFormat("开始检查{0}模块!", _name);
            bool isOk = Util.CheckIdentifier(_name);
            if (isOk == false)
                ConsoleDock.Ins.LogErrorFormat("名称[{0}]不规范,请以'_',字母和数字命名且首字母只能为'_'和字母!", _name);
            for (int i = 0; i < _imports.Count; i++)
            {
                string key = _imports[i];
                var nsw = NamespaceWrap.AllNamespaces[key];
                nsw.Check();
            }
            ConsoleDock.Ins.LogFormat("{0}模块检查完毕~", _name);
            return isOk;
        }
    }
}
