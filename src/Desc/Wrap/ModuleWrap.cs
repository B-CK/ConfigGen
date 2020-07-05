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
        public static ModuleWrap Create(string path)
        {
            ModuleXml xml = null;
            if (!File.Exists(path))
            {
                xml = new ModuleXml() { Name = Path.GetFileNameWithoutExtension(path) };
                Util.Serialize(path, xml);
                WrapManager.Ins.AddModuleXml(path, xml);
            }
            else
                xml = WrapManager.Ins.GetModuleXml(path);
            Debug.LogFormat("[Module]创建模块{0}", path);
            ModuleWrap wrap = PoolManager.Ins.Pop<ModuleWrap>();
            if (wrap == null) wrap = new ModuleWrap();
            wrap.Init(xml);
            return wrap;
        }

        /// <summary>
        /// 模块路径
        /// </summary>
        public override string FullName { get { return _path; } }
        /// <summary>
        /// 不要直接调组,而是调Util.Groups;
        /// </summary>
        public List<string> Imports { get { return _imports; } }
        public Dictionary<string, NamespaceWrap> Namespaces { get { return _namespaces; } }
        public Dictionary<string, ClassWrap> Classes { get { return _classes; } }
        public Dictionary<string, EnumWrap> Enums { get { return _enums; } }
        public int NeedSaveNum { get => _needSaveNum; set => _needSaveNum = value; }

        public override string DisplayName => FullName;
        public Action<NamespaceWrap> AddNamespaceEvent;
        public Action<NamespaceWrap> RemoveNamespaceEvent;
        public Action<NamespaceWrap, string> OnNamespaceNameChange;
        /// <summary>
        /// 模块中任意一个类型被修改
        /// </summary>
        public Action<TypeWrap> AddAnyTypeEvent;
        public Action<TypeWrap> RemoveAnyTypeEvent;
        public Action<BaseWrap, string> OnTypeNameChange;

        private Dictionary<string, NamespaceWrap> _namespaces;
        private Dictionary<string, ClassWrap> _classes;
        private Dictionary<string, EnumWrap> _enums;

        private HashSet<string> _checkTypes;
        private List<string> _imports;
        private List<string> _groups;
        private ModuleXml _xml;
        private string _path;
        private int _needSaveNum = 0;
        protected internal ModuleWrap() { }
        public void Init(ModuleXml xml)
        {
            base.Init(xml.Name);

            _needSaveNum = 0;

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
                    string key = Path.GetFileNameWithoutExtension(imps[i]);
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
            OnNameChange += OnModuleNameChange;
        }
        private void OnModuleNameChange(BaseWrap wrap, string src)
        {
            var srcPath = _path;
            var dirName = Path.GetDirectoryName(_path);
            _path = $"{dirName}\\{wrap.Name}.xml";
            File.Move(srcPath, _path);
        }
        public void SaveAnother(string path)
        {
            var another = new ModuleXml();
            another.Name = Path.GetFileNameWithoutExtension(path);
            another.Groups = _groups.ToString(Util.ArgsSplitFlag[0].ToString());
            another.Imports = _xml.Imports;
            Util.Serialize(path, another);
            Debug.LogFormat("[Module]另存模板{0}", path);
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
                _needSaveNum++;

            Add(name);
            _imports.Add(name);
            AddNamespace(wrap);
        }
        /// <summary>
        /// 断开树状结构上的每一个数据
        /// </summary>
        public void RemoveImport(NamespaceWrap wrap)
        {
            string name = wrap.FullName;
            _needSaveNum++;

            Remove(name);
            _imports.Remove(name);
            RemoveNamespace(wrap);
        }
        /// <summary>
        /// 仅移除名称,不触发事件
        /// </summary>
        public void RemoveImportNoEvent(NamespaceWrap wrap)
        {
            var fullName = wrap.FullName;
            Remove(fullName);
            Imports.Remove(fullName);
            _needSaveNum++;

            var state = NodeState.Exclude;
            wrap.SetNodeState(state);
            _namespaces.Remove(fullName);
            foreach (var item in wrap.Classes)
            {
                item.SetNodeState(state);
                _classes.Remove(item.FullName);
            }
            foreach (var item in wrap.Enums)
            {
                item.SetNodeState(state);
                _enums.Remove(item.FullName);
            }
        }
        public void AddImportNoEvent(NamespaceWrap wrap)
        {
            var fullName = wrap.FullName;
            if (Contains(fullName)) return;
            _needSaveNum++;
            Add(fullName);
            _imports.Add(fullName);

            var state = NodeState.Include;
            wrap.SetNodeState(state);
            _namespaces.Add(fullName, wrap);
            foreach (var item in wrap.Classes)
            {
                item.SetNodeState(state);
                _classes.Add(item.FullName, item);
            }
            foreach (var item in wrap.Enums)
            {
                item.SetNodeState(state);
                _enums.Add(item.FullName, item);
            }
        }


        public void Save(bool saveNsw = true)
        {
            _xml.Name = _name;
            var imps = new List<string>();
            for (int i = 0; i < _imports.Count; i++)
            {
                var key = _imports[i];
                if (NamespaceWrap.Dict.ContainsKey(key))
                {
                    var ns = NamespaceWrap.Dict[key];
                    var relative = Util.GetRelativePath(ns.FilePath, Util.ModuleDir);
                    imps.Add(relative);
                }
            }
            _xml.Imports = imps;
            _xml.Groups = string.Join(Util.ArgsSplitFlag[0].ToString(), Util.Groups);
            Util.Serialize(_path, _xml);
            Debug.LogFormat("[Module]保存模板{0}", _path);

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

        public void Close()
        {
            AddNamespaceEvent = null;
            RemoveNamespaceEvent = null;
            RemoveAnyTypeEvent = null;
            OnNamespaceNameChange = null;
            OnTypeNameChange = null;

            var state = NodeState.Exclude;
            foreach (var item in _namespaces)
            {
                item.Value.SetStateWithType(state, false);
                item.Value.ClearEvent();
            }
            foreach (var item in _classes)
                item.Value.ClearEvent();
            foreach (var item in _enums)
                item.Value.ClearEvent();
            _namespaces.Clear();
            _classes.Clear();
            _enums.Clear();
            Dispose();
            PoolManager.Ins.Push(this);
        }
        private void AddNamespace(NamespaceWrap wrap)
        {
            wrap.AddTypeEvent += AddTypeWrap;
            wrap.RemoveTypeEvent += RemoveTypeWrap;
            wrap.OnNameChange += NamespaceNameChange;
            wrap.OnTypeNameChange += TypeNameChange;
            _namespaces.Add(wrap.FullName, wrap);
            foreach (var item in wrap.Classes)
                AddTypeWrap(item);
            foreach (var item in wrap.Enums)
                AddTypeWrap(item);
            var state = NodeState.Include;
            wrap.SetNodeState(wrap.NodeState & ~NodeState.Exclude | state);
            AddNamespaceEvent?.Invoke(wrap);
        }
        private void RemoveNamespace(NamespaceWrap wrap)
        {
            _namespaces.Remove(wrap.FullName);
            var state = NodeState.Exclude;
            wrap.SetNodeState(state);
            var array = new List<TypeWrap>();
            array.AddRange(wrap.Classes);
            array.AddRange(wrap.Enums);
            foreach (var item in array)
                wrap.RemoveTypeWrap(item);
            RemoveNamespaceEvent?.Invoke(wrap);
            wrap.Dispose();
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
            AddAnyTypeEvent?.Invoke(wrap);
        }
        private void RemoveTypeWrap(TypeWrap wrap)
        {
            if (wrap is ClassWrap)
                _classes.Remove(wrap.FullName);
            else if (wrap is EnumWrap)
                _enums.Remove(wrap.FullName);
            RemoveAnyTypeEvent?.Invoke(wrap);
        }
        private void NamespaceNameChange(BaseWrap wrap, string src)
        {
            int index = _imports.IndexOf(src);
            _imports[index] = wrap.FullName;
            var nsw = wrap as NamespaceWrap;
            var classes = nsw.Classes;
            for (int i = 0; i < classes.Count; i++)
            {
                var cls = classes[i];
                var srcFullName = $"{src}.{cls.Name}";
                _classes.Remove(srcFullName);
                _classes.Add(cls.FullName, cls);
            }
            var enums = nsw.Enums;
            for (int i = 0; i < enums.Count; i++)
            {
                var enm = enums[i];
                var srcFullName = $"{src}.{enm.Name}";
                _enums.Remove(srcFullName);
                _enums.Add(enm.FullName, enm);
            }
            OnNamespaceNameChange?.Invoke(nsw, src);
        }
        private void TypeNameChange(BaseWrap wrap, string src)
        {
            switch (wrap)
            {
                case ClassWrap cls:
                    _classes.Remove(src);
                    _classes.Add(cls.FullName, cls);
                    break;
                case EnumWrap enm:
                    _enums.Remove(src);
                    _enums.Add(enm.FullName, enm);
                    break;
                default:
                    Debug.LogError($"[Module]TypeWrap无法解析!未知类型:{wrap.FullName}");
                    break;
            }
            OnTypeNameChange?.Invoke(wrap, src);
        }
        public override bool Check()
        {
            Debug.LogFormat("[Module]开始检查{0}模块!", _name);
            bool isOk = Util.CheckIdentifier(_name);
            if (isOk == false)
                Debug.LogErrorFormat("[Module]名称[{0}]不规范,请以'_',字母和数字命名且首字母只能为'_'和字母!", _name);
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
                    Debug.LogErrorFormat("[Module]{0}模块的{1}命名空间数据文件不存在!", _name, key);
                }
            }
            Debug.LogFormat("[Module]{0}模块检查完毕~", _name);
            return isOk;
        }

    }
}
//public static ModuleWrap Open(string path)
//{
//    if (_current != null && path == _current.FullName)
//        return _current;

//    Util.LastRecord = path;
//    if (File.Exists(path))
//    {
//        ModuleWrap wrap = PoolManager.Ins.Pop<ModuleWrap>();
//        if (path == Util.DefaultModule && _default != null)
//            wrap = _default;
//        else if (wrap == null)
//            wrap = new ModuleWrap();

//        if (_current != null)
//        {
//            TrSave();
//            _current.Close();
//        }
//        _current = wrap;
//    }
//    else
//    {
//        Debug.LogWarningFormat("[Module]模块{0}不存在,开启默认模块.", path);
//        _current = _default;
//    }

//    _noSaveNum = 0;
//    ResetAllState();
//    _current.Init(_dict[path]);
//    MainWindow.Ins.Text = Util.Format("结构描述 - {0}", _current.FullName);
//    Debug.LogFormat("[Module]打开模板{0}", path);
//    GC.Collect();
//    return _current;
//}
//public static void SilientSave(bool saveNamespace = false)
//{
//    Default.Save(saveNamespace);
//    Current.Save(saveNamespace);
//    EditorDock.CloseAll();
//}
//public static DialogResult TrSave()
//{
//    if (EditorDock.CheckOpenDock() || _noSaveNum != 0)
//    {
//        var result = MessageBox.Show("当前模块未保存,是否保存?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
//        switch (result)
//        {
//            case DialogResult.Cancel://取消操作,对数据不做任何处理
//                return DialogResult.Cancel;
//            case DialogResult.Yes://保存所有修改,且关闭模块
//                EditorDock.SaveAll();
//                Default.Save();
//                Current.Save();
//                EditorDock.CloseAll();
//                return DialogResult.Yes;
//            case DialogResult.No://放弃所有修改,且关闭模块
//                EditorDock.CloseAll();
//                return DialogResult.No;
//        }
//    }
//    else
//    {
//        SilientSave();
//    }
//    return DialogResult.None;
//}