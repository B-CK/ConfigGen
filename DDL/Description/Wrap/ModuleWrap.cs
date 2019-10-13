using Description.Editor;
using Description.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Description.Wrap
{
    public class ModuleWrap : BaseWrap
    {
        public static ModuleWrap Default { get { return _default; } }
        public static ModuleWrap Current { get { return _current; } }
        static ModuleWrap _default;
        static ModuleWrap _current;
        static int _noSaveNum = 0;

        static Dictionary<string, ModuleXml> _dict = new Dictionary<string, ModuleXml>();
        public static void InitModule()
        {
            _dict.Clear();
            var ms = Directory.GetFiles(Util.ModuleDir, "*.xml");
            for (int i = 0; i < ms.Length; i++)
            {
                string path = ms[i];
                if (!_dict.ContainsKey(path))
                {
                    var module = Util.Deserialize<ModuleXml>(path);
                    _dict.Add(path, module);
                }
            }

            ModuleXml xml = _dict[Util.DefaultModule];
            _default = new ModuleWrap();
            _default.Init(xml);
            var allNsw = NamespaceWrap.Dict;
            foreach (var item in allNsw)
                _default.AddImport(item.Value, true);
            _default.Save(false);

            if (Util.LastRecord.IsEmpty())
                Open(Util.DefaultModule);
            else
                Open(Util.LastRecord);
        }

        public static void CloseCurrent()
        {
            _current = null;
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
                xml = _dict[path];
            Debug.LogFormat("创建模块{0}", path);
            ModuleWrap wrap = PoolManager.Ins.Pop<ModuleWrap>();
            if (wrap == null) wrap = new ModuleWrap();
            wrap.Init(xml);
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
                else if (wrap == null)
                    wrap = new ModuleWrap();

                if (_current != null)
                {
                    TrSave();
                    _current.Close();
                }
                _current = wrap;
            }
            else
            {
                Debug.LogWarningFormat("模块{0}不存在,开启默认模块.", path);
                _current = _default;
            }

            _noSaveNum = 0;
            _current.Init(_dict[path]);
            _current.InitState();
            MainWindow.Ins.Text = Util.Format("结构描述 - {0}", _current.FullName);
            Debug.LogFormat("打开模板{0}", path);
            GC.Collect();
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
                    case DialogResult.Cancel://取消操作,对数据不做任何处理
                        return DialogResult.Cancel;
                    case DialogResult.Yes://保存所有修改,且关闭模块
                        EditorDock.SaveAll();
                        Default.Save();
                        Current.Save();
                        EditorDock.CloseAll();
                        return DialogResult.Yes;
                    case DialogResult.No://放弃所有修改,且关闭模块
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
        public List<string> Groups { get { return _groups; } }
        public List<string> Imports { get { return _imports; } }

        public override string DisplayName => FullName;

        private HashSet<string> _checkTypes;
        private List<string> _imports;
        private List<string> _groups;
        private ModuleXml _xml;
        private string _path;
        protected ModuleWrap() { }
        protected void Init(ModuleXml xml)
        {
            base.Init(xml.Name);

            _xml = xml;
            _path = Util.GetModuleAbsPath(_name + ".xml");
            _checkTypes = new HashSet<string>();
            _imports = new List<string>();
            _groups = new List<string>();
            if (_xml.Imports != null)
            {
                var imps = _xml.Imports;
                for (int i = 0; i < imps.Count; i++)
                {
                    _imports.Add(imps[i]);
                    Add(imps[i]);
                }
            }
            if (!_xml.Groups.IsEmpty())
            {
                var groups = _xml.Groups.Split(Util.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
                _groups.AddRange(groups);
            }
        }
        protected void InitState()
        {
            var allNsw = NamespaceWrap.Dict;
            foreach (var item in allNsw)
            {
                var nsw = item.Value;
                var state = Contains(nsw.FullName) ? NodeState.Include : NodeState.Exclude;
                nsw.UpdateStateWithChild(state);
            }
        }
        public void SaveAnother(string path)
        {
            var another = new ModuleXml();
            another.Name = Path.GetFileNameWithoutExtension(path);
            another.Groups = _groups.ToString(Util.ArgsSplitFlag[0].ToString());
            another.Imports = _xml.Imports;
            Util.Serialize(path, another);
            Debug.LogFormat("另存模板{0}", path);
        }
        private void Close()
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
            wrap.UpdateStateWithChild(NodeState.Include);
            if (!isInit)
                AddDirty();
        }
        public void RemoveImport(NamespaceWrap wrap)
        {
            string name = wrap.FullName;
            if (!Contains(name)) return;
            Remove(name);
            Imports.Remove(name);
            wrap.ResetStateWithChild(~NodeState.Exclude);
            AddDirty();
        }
        public void RemoveImport(string fullName)
        {
            Remove(fullName);
            Imports.Remove(fullName);
            AddDirty();
        }
        public void Save(bool saveNsw = true)
        {
            _xml.Name = _name;
            _xml.Imports = _imports;
            _xml.Groups = _groups.ToString(Util.ArgsSplitFlag[0].ToString());
            Util.Serialize(_path, _xml);
            Debug.LogFormat("保存模板{0}", _path);

            if (!saveNsw) return;
            for (int i = 0; i < _imports.Count; i++)
            {
                string key = _imports[i];
                var nsw = NamespaceWrap.Dict[key];
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
                var nsw = NamespaceWrap.Dict[key];
                if (nsw != null && nsw.IsDirty)
                    nsw.Cancle();
            }
        }

        /// <summary>
        /// 仅供当前模块使用
        /// </summary>
        private void UpdateCheckTypes()
        {
            //重置检查类型
            _checkTypes.Clear();
            for (int i = 0; i < _imports.Count; i++)
            {
                string key = _imports[i];
                if (NamespaceWrap.Dict.ContainsKey(key))
                {
                    var nsw = NamespaceWrap.Dict[key];
                    foreach (var item in nsw.Hash)
                        _checkTypes.Add($"{nsw.FullName}.{item}");
                }
            }
        }
        public bool CheckType(string fullName)
        {
            return _checkTypes.Contains(fullName);
        }
        public override bool Check()
        {
            _current.UpdateCheckTypes();
            Debug.LogFormat("开始检查{0}模块!", _name);
            bool isOk = Util.CheckIdentifier(_name);
            if (isOk == false)
                Debug.LogErrorFormat("名称[{0}]不规范,请以'_',字母和数字命名且首字母只能为'_'和字母!", _name);
            for (int i = 0; i < _imports.Count; i++)
            {
                string key = _imports[i];
                if (NamespaceWrap.Dict.ContainsKey(key))
                {
                    var nsw = NamespaceWrap.Dict[key];
                    nsw.Check();
                }
                else
                {
                    isOk &= false;
                    Debug.LogErrorFormat("{0}模块的{1}命名空间数据文件不存在!", _name, key);
                }
            }
            Debug.LogFormat("{0}模块检查完毕~", _name);
            return isOk;
        }
    }
}
