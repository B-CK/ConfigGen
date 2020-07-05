using Desc.Xml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Desc.Wrap
{
    public class NamespaceWrap : BaseWrap, IDisposable
    {
        public static Dictionary<string, NamespaceWrap> Dict { get { return _dict; } }
        static Dictionary<string, NamespaceWrap> _dict = new Dictionary<string, NamespaceWrap>();
        public static NamespaceWrap[] Array
        {
            get
            {
                if (array.Length != _dict.Count)
                {
                    var ls = new List<NamespaceWrap>(_dict.Values);
                    ls.Sort((a, b) => Comparer<string>.Default.Compare(a.DisplayName, b.DisplayName));
                    array = ls.ToArray();
                }
                return array;
            }
        }
        static NamespaceWrap[] array = new NamespaceWrap[0];
        public static void InitNamespaces()
        {
            string[] fs = Directory.GetFiles(Util.NamespaceDir);
            for (int i = 0; i < fs.Length; i++)
            {
                string path = fs[i];
                string name = Path.GetFileNameWithoutExtension(path);
                var value = Util.Deserialize<NamespaceXml>(path);
                Create(value);
            }

            //生成@命名空间(全局命名空间)
            string epath = Util.Format("{0}\\{1}.xml", Util.NamespaceDir, Util.EmptyNamespace);
            if (!File.Exists(epath))
            {
                var a = Create(Util.EmptyNamespace);
                a.SetDirty();
            }
        }
        public static void ClearAll()
        {
            foreach (var item in _dict)
                item.Value.Dispose();
            _dict.Clear();
        }
        /// <summary>
        /// 移除命名空间以及类型等
        /// </summary>
        public static void Remove(NamespaceWrap wrap)
        {
            if (_dict.ContainsKey(wrap.FullName))
            {
                _dict.Remove(wrap.FullName);
                var cls = wrap.Classes;
                for (int i = 0; i < cls.Count; i++)
                    ClassWrap.Dict.Remove(cls[i].FullName);
                var ens = wrap.Enums;
                for (int i = 0; i < ens.Count; i++)
                    EnumWrap.Dict.Remove(ens[i].FullName);
            }
        }
        public static NamespaceWrap GetNamespace(string name)
        {
            if (_dict.ContainsKey(name))
                return _dict[name];
            else
                return null;
        }
        public static NamespaceWrap Create(string name)
        {
            NamespaceXml xml = new NamespaceXml() { Name = name };
            NamespaceWrap wrap = Create(xml);
            return wrap;
        }
        public static NamespaceWrap Create(NamespaceXml xml)
        {
            var wrap = PoolManager.Ins.Pop<NamespaceWrap>();
            if (wrap == null)
                wrap = new NamespaceWrap(xml);
            else
                wrap.Init(xml);
            wrap.OnNameChange = OnNamespaceNameChange;
            return wrap;
        }
        static void OnNamespaceNameChange(BaseWrap wrap, string src)
        {
            if (_dict.ContainsKey(src))
            {
                var nsw = wrap as NamespaceWrap;
                _dict.Remove(src);
                _dict.Add(nsw.FullName, nsw);
                for (int i = 0; i < nsw._classes.Count; i++)
                {
                    var cls = nsw._classes[i];
                    string key = cls.FullName.Replace(wrap.FullName, src);
                    ClassWrap.Dict.Remove(key);
                    ClassWrap.Dict.Add(cls.FullName, cls);
                }
                for (int i = 0; i < nsw._enums.Count; i++)
                {
                    var enm = nsw._enums[i];
                    string key = enm.FullName.Replace(wrap.FullName, src);
                    EnumWrap.Dict.Remove(key);
                    EnumWrap.Dict.Add(enm.FullName, enm);
                }
            }
            else
            {
                Util.MsgError("{0}命名空间修改名称为{1}触发事件异常!", src, wrap.FullName);
            }
        }

        public Action<TypeWrap> AddTypeEvent;
        public Action<TypeWrap> RemoveTypeEvent;
        public Action<BaseWrap, string> OnDescChange;
        public Action<BaseWrap, string> OnTypeNameChange;

        /// <summary>
        /// 是否有被修改
        /// </summary>
        public bool IsDirty { get { return _isDirty; } }
        public string FilePath { get { return _path; } }
        public List<ClassWrap> Classes { get { return _classes; } }
        public List<EnumWrap> Enums { get { return _enums; } }
        public string Desc
        {
            get { return _xml.Desc; }
            set
            {
                if (_xml.Desc != value)
                {
                    string desc = _xml.Desc;
                    _xml.Desc = value;
                    OnDescChange?.Invoke(this, FullName);
                }
            }
        }

        public override string FullName => Name;
        public override string DisplayName
        {
            get
            {
                if (Desc.IsEmpty())
                    return FullName;
                else
                    return $"{FullName}:{Desc}";
            }
        }
        private List<ClassWrap> _classes;
        private List<EnumWrap> _enums;
        private NamespaceXml _xml;
        private bool _isDirty = false;
        private string _path;
        protected NamespaceWrap(NamespaceXml xml)
        {
            Init(xml);
        }
        private void Init(NamespaceXml xml)
        {
            base.Init(xml.Name);

            _xml = xml;
            _classes = new List<ClassWrap>();
            _enums = new List<EnumWrap>();
            _xml.Classes = _xml.Classes ?? new List<ClassXml>();
            _xml.Enums = _xml.Enums ?? new List<EnumXml>();
            _path = Util.GetNamespaceAbsPath(_name + ".xml");

            var xclasses = _xml.Classes;
            for (int i = 0; i < xclasses.Count; i++)
            {
                var cls = ClassWrap.Create(xclasses[i], this);
                cls.OnNameChange += TypeNameChange;
            }
            var xenums = _xml.Enums;
            for (int i = 0; i < xenums.Count; i++)
            {
                var enm = EnumWrap.Create(xenums[i], this);
                enm.OnNameChange += TypeNameChange;
            }

            _dict.Add(FullName, this);
            _isDirty = false;
        }
        public void AddTypeWrap(TypeWrap wrap, bool isDirty = true)
        {
            if (isDirty)
            {
                wrap.SetNodeState(NodeState | NodeState.Modify);
                SetDirty();
                wrap.OnNameChange += TypeNameChange;
            }

            wrap.Namespace = this;
            if (wrap is ClassWrap)
                _classes.Add(wrap as ClassWrap);
            else if (wrap is EnumWrap)
                _enums.Add(wrap as EnumWrap);
            Add(wrap.Name);
            AddTypeEvent?.Invoke(wrap);
        }
        public void RemoveTypeWrap(TypeWrap wrap)
        {
            SetDirty();
            Remove(wrap.Name);

            if (wrap is ClassWrap)
                _classes.Remove(wrap as ClassWrap);
            else if (wrap is EnumWrap)
                _enums.Remove(wrap as EnumWrap);
            RemoveTypeEvent?.Invoke(wrap);
            wrap.Dispose();
        }
        public void SetDirty()
        {
            _isDirty = true;
            SetNodeState(NodeState | NodeState.Modify);
            WrapManager.Ins.Current.NeedSaveNum++;
        }
        public void Save()
        {
            _xml.Name = _name;
            try
            {
                WrapManager.Ins.Current.NeedSaveNum--;
                _xml.Classes.Clear();
                _xml.Enums.Clear();
                for (int i = 0; i < _classes.Count; i++)
                    _xml.Classes.Add(_classes[i]);
                for (int i = 0; i < _enums.Count; i++)
                    _xml.Enums.Add(_enums[i]);
                string path = Util.GetNamespaceAbsPath(_name + ".xml");
                if (path != _path)
                {
                    File.Delete(_path);
                    _path = path;
                }
                Util.Serialize(_path, _xml);

                _isDirty = false;
                SetNodeState(NodeState & ~NodeState.Modify);
                for (int i = 0; i < _classes.Count; i++)
                    _classes[i].SetNodeState(_classes[i].NodeState & ~NodeState.Modify);
                for (int i = 0; i < _enums.Count; i++)
                    _enums[i].SetNodeState(_enums[i].NodeState & ~NodeState.Modify);
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("[Module]序列化命名空间{0}失败!\n{1}\n{2}\n",
                   _name, e.Message, e.StackTrace);
            }
        }
        public void SetStateWithType(NodeState state, bool isTrigger = true)
        {
            SetNodeState(state, isTrigger);
            foreach (var item in Classes)
                item.SetNodeState(state, isTrigger);
            foreach (var item in Enums)
                item.SetNodeState(state, isTrigger);
        }
        public void Cancle()
        {
            _name = "";
            _xml = null;
        }
        public override bool Check()
        {
            bool isOk = base.Check();
            List<string> repeat = new List<string>();
            HashSet<string> hash = new HashSet<string>();
            for (int i = 0; i < _classes.Count; i++)
            {
                var cls = _classes[i];
                bool c = cls.Check();
                isOk &= c;
                if (hash.Contains(cls.Name))
                    repeat.Add(cls.Name);
                else
                    hash.Add(cls.Name);
            }
            var xenums = _xml.Enums;
            for (int i = 0; i < _enums.Count; i++)
            {
                var enm = _enums[i];
                bool c = enm.Check();
                isOk &= c;
                if (hash.Contains(enm.Name))
                    repeat.Add(enm.Name);
                else
                    hash.Add(enm.Name);
            }
            if (repeat.Count != 0)
            {
                isOk &= false;
                Debug.LogErrorFormat("[Namespace]{0}空间中命名重复:{1}", FullName, string.Join(",", repeat));
            }
            if (isOk == false)
                SetNodeState(NodeState | NodeState.Error);
            else
                SetNodeState(NodeState & ~NodeState.Error);
            return isOk;
        }
        public override void Dispose()
        {
            base.Dispose();
            for (int i = 0; i < _classes.Count; i++)
                _classes[i].Dispose();
            for (int i = 0; i < _enums.Count; i++)
                _enums[i].Dispose();
            _classes.Clear();
            _enums.Clear();
            PoolManager.Ins.Push(this);
        }
        public override void ClearEvent()
        {
            base.ClearEvent();
            AddTypeEvent = null;
            RemoveTypeEvent = null;
            OnNameChange = null;
            OnTypeNameChange = null;
        }

        private void TypeNameChange(BaseWrap wrap, string src)
        {
            var typeWrap = wrap as TypeWrap;
            int length = typeWrap.Namespace.Name.Length + 1;
            Remove(src.Substring(length));
            Add(wrap.Name);
            OnTypeNameChange?.Invoke(wrap, src);
        }
    }
}
