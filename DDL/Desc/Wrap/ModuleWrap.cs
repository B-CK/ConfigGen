using Desc.Editor;
using Desc.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Desc.Wrap
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
            MainWindow.Ins.Text = Util.Format("结构描述 - {0}", _default.FullName);
            var allNsw = NamespaceWrap.Dict;
            foreach (var item in allNsw)
                _default.AddImport(item.Value, true);
            _default.Save(false);
            ResetAllState();

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
                _dict.Add(path, xml);
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
            ResetAllState();
            _current.Init(_dict[path]);
            MainWindow.Ins.Text = Util.Format("结构描述 - {0}", _current.FullName);
            Debug.LogFormat("打开模板{0}", path);
            GC.Collect();
            return _current;
        }
        /// <summary>
        /// 重置模块中各节点的状态
        /// </summary>
        private static void ResetAllState()
        {
            var state = NodeState.Exclude;
            foreach (var item in NamespaceWrap.Dict)
            {
                var wrap = item.Value;
                wrap.SetNodeState(state, false);
                foreach (var cls in wrap.Classes)
                    cls.SetNodeState(state, false);
                foreach (var enm in wrap.Enums)
                    enm.SetNodeState(state, false);
            }
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
        public Dictionary<string, NamespaceWrap> Namespaces { get { return _namespaces; } }
        public Dictionary<string, ClassWrap> Classes { get { return _classes; } }
        public Dictionary<string, EnumWrap> Enums { get { return _enums; } }

        public override string DisplayName => FullName;
        public Action<NamespaceWrap> OnAddNamespace;
        public Action<NamespaceWrap> OnRemoveNamespace;
        /// <summary>
        /// 类型添加移除事件与NamespaceWrap中的添加移除注册一个即可
        /// </summary>
        public Action<TypeWrap> OnAddAnyType;
        public Action<TypeWrap> OnRemoveAnyType;
        public Action<NamespaceWrap, string> OnNamespaceNameChange;
        public Action<BaseWrap, string> OnTypeNameChange;

        private Dictionary<string, NamespaceWrap> _namespaces;
        private Dictionary<string, ClassWrap> _classes;
        private Dictionary<string, EnumWrap> _enums;

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

            _namespaces = new Dictionary<string, NamespaceWrap>();
            _classes = new Dictionary<string, ClassWrap>();
            _enums = new Dictionary<string, EnumWrap>();
            _imports = new List<string>();
            _groups = new List<string>();
            if (_xml.Imports != null)
            {
                var imps = _xml.Imports;
                for (int i = 0; i < imps.Count; i++)
                {
                    string key = imps[i];
                    AddNamespace(NamespaceWrap.Dict[key]);
                    _imports.Add(key);
                    Add(key);
                }
            }
            if (!_xml.Groups.IsEmpty())
            {
                var groups = _xml.Groups.Split(Util.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
                _groups.AddRange(groups);
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
        public NamespaceWrap[] GetNamespaces()
        {
            var wraps = new List<NamespaceWrap>(_namespaces.Values);
            return wraps.ToArray();
        }
        public ClassWrap[] GetClasses()
        {
            var wraps = new List<ClassWrap>(_classes.Values);
            return wraps.ToArray();
        }
        public EnumWrap[] GetEnums()
        {
            var wraps = new List<EnumWrap>(_enums.Values);
            return wraps.ToArray();
        }
        public void AddImport(NamespaceWrap wrap, bool isInit = false)
        {
            string name = wrap.FullName;
            if (Contains(name)) return;
            if (!isInit)
                AddDirty();

            Add(name);
            _imports.Add(name);
            AddNamespace(wrap);
        }
        public void RemoveImport(NamespaceWrap wrap)
        {
            string name = wrap.FullName;
            if (!Contains(name)) return;
            AddDirty();

            Remove(name);
            _imports.Remove(name);
            RemoveNamespace(wrap);
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

        private void Close()
        {
            foreach (var item in _namespaces)
            {
                item.Value.OnAddType = null;
                item.Value.OnRemoveType = null;
            }
            _namespaces.Clear();
            Dispose();
            if (this != _default)
                PoolManager.Ins.Push(this);
        }
        private void AddNamespace(NamespaceWrap wrap)
        {
            wrap.OnAddType += AddTypeWrap;
            wrap.OnRemoveType += RemoveTypeWrap;
            wrap.OnNameChange += NamespaceNameChange;
            _namespaces.Add(wrap.FullName, wrap);
            foreach (var item in wrap.Classes)
                AddTypeWrap(item);
            foreach (var item in wrap.Enums)
                AddTypeWrap(item);
            var state = NodeState.Include;
            wrap.SetNodeState(wrap.NodeState & ~NodeState.Exclude | state);
            OnAddNamespace?.Invoke(wrap);
        }
        private void RemoveNamespace(NamespaceWrap wrap)
        {
            var state = NodeState.Exclude;
            wrap.SetNodeState(state);
            _namespaces.Remove(wrap.FullName);
            foreach (var item in wrap.Classes)
                wrap.RemoveTypeWrap(item);
            foreach (var item in wrap.Enums)
                wrap.RemoveTypeWrap(item);
            wrap.OnAddType = null;
            wrap.OnRemoveType = null;
            OnRemoveNamespace?.Invoke(wrap);
        }
        private void AddTypeWrap(TypeWrap wrap)
        {
            var state = NodeState.Include;
            wrap.SetNodeState(wrap.NodeState & ~NodeState.Exclude | state);
            wrap.OnNameChange += OnTypeNameChange;
            if (wrap is ClassWrap)
                _classes.Add(wrap.FullName, wrap as ClassWrap);
            else if (wrap is EnumWrap)
                _enums.Add(wrap.FullName, wrap as EnumWrap);
            OnAddAnyType?.Invoke(wrap);
        }
        private void RemoveTypeWrap(TypeWrap wrap)
        {
            if (wrap is ClassWrap)
                _classes.Remove(wrap.FullName);
            else if (wrap is EnumWrap)
                _enums.Remove(wrap.FullName);

            var state = NodeState.Exclude;
            wrap.SetNodeState(state);
            OnRemoveAnyType?.Invoke(wrap);
        }
        private void NamespaceNameChange(BaseWrap wrap, string src)
        {
            int index = _imports.IndexOf(src);
            _imports[index] = wrap.FullName;
            OnNamespaceNameChange?.Invoke(wrap as NamespaceWrap, src);
        }
        public override bool Check()
        {
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
