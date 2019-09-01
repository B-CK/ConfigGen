using Description.Xml;
using System.Collections.Generic;
using System.IO;

namespace Description.Wrap
{
    public class ModuleWrap : BaseWrap
    {
        public static ModuleWrap Default { get { return _default; } }
        public static ModuleWrap Current { get { return _current; } }
        static ModuleWrap _default;
        static ModuleWrap _current;

        public static void InitDefaultModule()
        {
            var xml = new ModuleXml() { Name = Util.DefaultModuleName };
            if (!File.Exists(Util.DefaultModule))
                Util.Serialize(Util.DefaultModule, xml);
        }
        public static ModuleWrap Create(string path)
        {
            ModuleXml xml = null;
            if (File.Exists(path))
                xml = Util.Deserialize<ModuleXml>(path);
            else
                xml = new ModuleXml() { Name = Path.GetFileNameWithoutExtension(path) };
            ConsoleDock.Ins.LogFormat("创建模块{0}", path);
            ModuleWrap wrap = PoolManager.Ins.Pop<ModuleWrap>();
            if (wrap == null) wrap = new ModuleWrap(xml);
            return wrap;
        }
        public static ModuleWrap OpenDefault()
        {
            if (_default == null)
                _default = Open(Util.DefaultModule);
            return _default;
        }
        public static ModuleWrap Open(string path)
        {
            if (File.Exists(path))
            {
                var xml = Util.Deserialize<ModuleXml>(path);
                ModuleWrap wrap = PoolManager.Ins.Pop<ModuleWrap>();
                if (wrap == null) wrap = new ModuleWrap(xml);
                
                _current = wrap;               
                MainWindow.Ins.Text = Util.Format("结构描述 - {0}", wrap.FullName);
                NamespaceWrap.UpdateImportedFlag(wrap.Imports);
                ConsoleDock.Ins.LogFormat("打开模板{0}", path);
                return wrap;
            }
            else
            {
                ConsoleDock.Ins.LogWarningFormat("模块{0}不存在,开启默认模块.", path);
                return OpenDefault();
            }
        }

        public override string FullName { get { return _path; } }
        public List<string> Imports { get { return _xml.Imports; } }
        private ModuleXml _xml;
        private string _path;
        protected ModuleWrap(ModuleXml xml) : base(xml.Name)
        {
            _xml = xml;
            _path = Util.GetModuleAbsPath(_name + ".xml");
            if (Imports == null) return;

            for (int i = 0; i < Imports.Count; i++)
                Add(Imports[i]);
        }

        public void Save()
        {
            Util.Serialize(_path, _xml);
            ConsoleDock.Ins.LogFormat("保存模板{0}", _path);
        }
        public void SaveAnother(string path)
        {
            Util.Serialize(path, _xml);
            ConsoleDock.Ins.LogFormat("另存模板{0}", path);
        }
        public void Close()
        {
            Save();
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
    }
}
