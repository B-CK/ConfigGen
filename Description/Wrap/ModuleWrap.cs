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

            ModuleWrap wrap = null;
            if (File.Exists(Util.LastRecord))
                wrap = Open(Util.LastRecord);
            else
                wrap = OpenDefault();

            var allNsw = NamespaceWrap.AllNamespaces;
            foreach (var item in allNsw)
                _default.AddImport(item.Value);
            var imps = _current.Imports;
            for (int i = 0; i < imps.Count; i++)
            {
                string ns = imps[i];
                if (allNsw.ContainsKey(ns))
                    allNsw[ns].IsVisible = true;
            }
        }
    
        public static ModuleWrap Create(string path)
        {
            ModuleXml xml = null;
            if (!File.Exists(path))
            {
                xml = new ModuleXml() { Name = Path.GetFileNameWithoutExtension(path) };
                Util.Serialize(path, xml);
            }
            ConsoleDock.Ins.LogFormat("创建模块{0}", path);
            ModuleWrap wrap = PoolManager.Ins.Pop<ModuleWrap>();
            if (wrap == null) wrap = new ModuleWrap(xml);
            Open(path);
            return wrap;
        }
        public static ModuleWrap OpenDefault()
        {
            if (_default == null)
                _default = Open(Util.DefaultModule);
            else
                _current = _default;
            return _default;
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

                if (_current != null) TrSave();
                _current = wrap;
                MainWindow.Ins.Text = Util.Format("结构描述 - {0}", wrap.FullName);
                ConsoleDock.Ins.LogFormat("打开模板{0}", path);
                return wrap;
            }
            else
            {
                ConsoleDock.Ins.LogWarningFormat("模块{0}不存在,开启默认模块.", path);
                return OpenDefault();
            }
        }
        public static DialogResult TrSave()
        {
            if (NamespaceWrap.HasModifyNamespace)
            {
                var result = MessageBox.Show("还有配置未保存,是否保存?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case DialogResult.Cancel:
                        EditorDock.CancleAll();
                        return DialogResult.Cancel;
                    case DialogResult.Yes:                      
                        NamespaceWrap.HasModifyNamespace = false;
                        EditorDock.SaveAll();
                        Default.Save();
                        Current.Save();
                        EditorDock.ClearAll();
                        return DialogResult.Yes;
                    case DialogResult.No:
                        EditorDock.CancleAll();
                        Default.Save(false);
                        Current.Save(false);
                        return DialogResult.No;
                }
            }
            else
            {
                Default.Save(false);
                Current.Save(false);
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
            if (Imports == null) return;

            for (int i = 0; i < Imports.Count; i++)
            {
                _imports.Add(Imports[i]);
                Add(Imports[i]);
            }
        }

        public void SaveAnother(string path)
        {
            Util.Serialize(path, _xml);
            ConsoleDock.Ins.LogFormat("另存模板{0}", path);
        }
        public void Close()
        {
            Dispose();
            if (this != _default)
                PoolManager.Ins.Push(this);
        }
        
        public void AddImport(NamespaceWrap wrap)
        {
            string name = wrap.FullName;
            if (Contains(name)) return;
            Imports.Add(name);
            Add(name);
        }
        public void RemoveImport(NamespaceWrap wrap)
        {
            string name = wrap.FullName;
            if (!Contains(name)) return;
            Imports.Remove(name);
            Remove(name);
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
    }
}
