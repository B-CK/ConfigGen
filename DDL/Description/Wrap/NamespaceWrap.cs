using Description.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Description.Wrap
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
            return wrap;
        }



        /// <summary>
        /// 是否有被修改
        /// </summary>
        public bool IsDirty { get { return _isDirty; } }
        public string FilePath { get { return _path; } }

        public List<ClassWrap> Classes { get { return _classes; } }
        public List<EnumWrap> Enums { get { return _enums; } }
        public string Desc { get { return _xml.Desc; } set { _xml.Desc = value; } }

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
                wrap.AddNodeState(NodeState.Modify);
                SetDirty();
            }

            if (wrap is ClassWrap)
                _classes.Add(wrap as ClassWrap);
            else if (wrap is EnumWrap)
                _enums.Add(wrap as EnumWrap);
            Add(wrap.Name);
        }
        public void RemoveTypeWrap(TypeWrap wrap)
        {
            if (!Contains(wrap.Name)) return;

            SetDirty();
            Remove(wrap.Name);

            if (wrap is ClassWrap)
                _classes.Remove(wrap as ClassWrap);
            else if (wrap is EnumWrap)
                _enums.Remove(wrap as EnumWrap);
        }
        public void UpdateStateWithChild(NodeState state)
        {
            AddNodeState(state);
            var classes = Classes;
            for (int i = 0; i < classes.Count; i++)
                classes[i].AddNodeState(state);
            var enums = Enums;
            for (int i = 0; i < enums.Count; i++)
                enums[i].AddNodeState(state);
        }
        public void ResetStateWithChild(NodeState state)
        {
            RemoveNodeState(state);
            var classes = Classes;
            for (int i = 0; i < classes.Count; i++)
                classes[i].RemoveNodeState(state);
            var enums = Enums;
            for (int i = 0; i < enums.Count; i++)
                enums[i].RemoveNodeState(state);
        }
        public void SetDirty()
        {
            _isDirty = true;
            AddNodeState(NodeState.Modify);
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
                if ((NodeState & NodeState.Error) == 0)
                    RemoveNodeState(~NodeState.Include);
                else
                    RemoveNodeState(~(NodeState.Include | NodeState.Error));
                for (int i = 0; i < _classes.Count; i++)
                {
                    if ((_classes[i].NodeState & NodeState.Error) == 0)
                        _classes[i].RemoveNodeState(~NodeState.Include);
                    else
                        _classes[i].RemoveNodeState(~(NodeState.Include | NodeState.Error));
                }
                for (int i = 0; i < _enums.Count; i++)
                {
                    if ((_enums[i].NodeState & NodeState.Error) == 0)
                        _enums[i].RemoveNodeState(~NodeState.Include);
                    else
                        _enums[i].RemoveNodeState(~(NodeState.Include | NodeState.Error));
                }
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
            for (int i = 0; i < _classes.Count; i++)
            {
                bool c = _classes[i].Check();
                isOk &= c;
            }
            var xenums = _xml.Enums;
            for (int i = 0; i < _enums.Count; i++)
            {
                bool c = _enums[i].Check();
                isOk &= c;
            }
            if (isOk == false)
                AddNodeState(NodeState.Error);
            else
                RemoveNodeState(NodeState.Error);
            return isOk;
        }
        public override void Dispose()
        {
            base.Dispose();
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
