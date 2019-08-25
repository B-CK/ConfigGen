using Description.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Description
{
    public class Module
    {
        public static Module Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new Module();
                return _ins;
            }
        }
        static Module _ins;

        public string Path { get { return _path; } }
        public List<NamespaceXml> Namespaces { get { return _namespaces; } }

        private Dictionary<string, NamespaceXml> _allNamespace;

        private string _path;
        private ModuleXml _xml;
        private List<NamespaceXml> _namespaces;
        private List<TypeEditorDock> _tempType;

        private Module()
        {
            _allNamespace = new Dictionary<string, NamespaceXml>();
            string[] fs = Directory.GetFiles(Util.NamespaceDir);
            for (int i = 0; i < fs.Length; i++)
            {
                string path = fs[i];
                var value = Util.Deserialize<NamespaceXml>(path);
                _allNamespace.Add(path, value);
            }
        }
        public void Create(string path)
        {
            ModuleXml module = new ModuleXml();
            Util.Serialize(path, module);
            ConsoleDock.Ins.LogFormat("创建模板{0}", path);
        }
        public void OpenDefault()
        {
            Open(Util.DefaultModule);
        }
        public void Open(string path)
        {
            this._path = path;

            if (File.Exists(path))
                _xml = Util.Deserialize<ModuleXml>(path);
            else
                _xml = new ModuleXml();
            _namespaces = new List<NamespaceXml>();
            MainWindow.Ins.Text = Util.Format("结构描述 - {0}", path);
            if (_xml.Imports == null)
                return;

            List<string> imports = _xml.Imports;
            for (int i = 0; i < imports.Count; i++)
            {
                string key = imports[i];
                if (_allNamespace.ContainsKey(key))
                    _namespaces.Add(_allNamespace[key]);
                else
                    ConsoleDock.Ins.LogWarningFormat("本地无法找到{0}空间.", key);
            }
            ConsoleDock.Ins.LogFormat("打开模板{0}", path);
        }
        public void Close()
        {
            Save();
            OpenDefault();
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
        public void Dispose()
        {
            _path = null;
            _xml = null;
            _namespaces.Clear();
            _allNamespace.Clear();
        }

        #region 类型操作
        public void CreateType()
        {

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
    }
}
