﻿using Desc.Xml;
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
            ClassWrap.ClearAll();
            EnumWrap.ClearAll();
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
                _dict.Remove(src);
                _dict.Add(wrap.FullName, wrap as NamespaceWrap);
            }
            else
            {
                Util.MsgError("{0}命名空间修改名称为{1}触发事件异常!", src, wrap.FullName);
            }
        }

        public Action<TypeWrap> OnAddType;
        public Action<TypeWrap> OnRemoveType;
        public Action<BaseWrap, string> OnDescChange;

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
                    OnDescChange?.Invoke(this, desc);
                }
            }
        }

        public override string DisplayName
        {
            get
            {
                if (Desc.IsEmpty())
                    return FullName;
                else
                    return Util.Format("{0}:{1}", FullName, Desc);
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
                ClassWrap.Create(xclasses[i], this);
            var xenums = _xml.Enums;
            for (int i = 0; i < xenums.Count; i++)
                EnumWrap.Create(xenums[i], this);

            _dict.Add(FullName, this);
            _isDirty = false;
        }
        public void AddTypeWrap(TypeWrap wrap, bool isDirty = true)
        {
            if (isDirty)
            {
                wrap.SetNodeState(NodeState | NodeState.Modify);
                SetDirty();
            }

            wrap.Namespace = this;
            if (wrap is ClassWrap)
                _classes.Add(wrap as ClassWrap);
            else if (wrap is EnumWrap)
                _enums.Add(wrap as EnumWrap);
            Add(wrap.Name);
            OnAddType?.Invoke(wrap);
        }
        public void RemoveTypeWrap(TypeWrap wrap)
        {
            SetDirty();
            Remove(wrap.Name);

            if (wrap is ClassWrap)
                _classes.Remove(wrap as ClassWrap);
            else if (wrap is EnumWrap)
                _enums.Remove(wrap as EnumWrap);
            OnRemoveType?.Invoke(wrap);
            wrap.Namespace = null;
            wrap.Dispose();
        }
        public void SetDirty()
        {
            _isDirty = true;
            SetNodeState(NodeState | NodeState.Modify);
            ModuleWrap.AddDirty();
        }
        public void Save()
        {
            _xml.Name = _name;
            try
            {
                ModuleWrap.RemoveDirty();
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
                Debug.LogErrorFormat("序列化命名空间{0}失败!\n{1}\n{2}\n",
                   _name, e.Message, e.StackTrace);
            }
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
            OnAddType = null;
            OnRemoveType = null;
            if (ModuleWrap.Default == ModuleWrap.Current)
            {
                try
                {
                    string path = Util.GetNamespaceAbsPath(FullName + ".xml");
                    File.Delete(path);
                    _dict.Remove(FullName);
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("删除命名空间{0}失败!\n{1}\n{2}\n",
                       FullName, e.Message, e.StackTrace);
                }
            }
            for (int i = 0; i < _classes.Count; i++)
                _classes[i].Dispose();
            for (int i = 0; i < _enums.Count; i++)
                _enums[i].Dispose();
            _classes.Clear();
            _enums.Clear();
            PoolManager.Ins.Push(this);
        }
    }
}
