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
        public static bool HasModifyNamespace = false;
        public static Dictionary<string, NamespaceWrap> AllNamespaces { get { return _allNamespaces; } }
        static Dictionary<string, NamespaceWrap> _allNamespaces = new Dictionary<string, NamespaceWrap>();
        public static NamespaceWrap[] Namespaces
        {
            get
            {
                if (_namespaces.Length != _allNamespaces.Count)
                {
                    var ls = new List<NamespaceWrap>(_allNamespaces.Values);
                    ls.Sort((a, b) => Comparer<string>.Default.Compare(a.DisplayName, b.DisplayName));
                    _namespaces = ls.ToArray();
                }
                return _namespaces;
            }
        }
        static NamespaceWrap[] _namespaces = new NamespaceWrap[0];
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
                HasModifyNamespace = true;
                var a = Create(Util.EmptyNamespace);
                a.SetDirty();
                a.SetNodeState(NodeState.Modify);
            }
        }

        public static NamespaceWrap GetNamespace(string name)
        {
            if (_allNamespaces.ContainsKey(name))
                return _allNamespaces[name];
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
        private bool _isDirty = false;
        public bool IsVisible { get { return _isVisible; } set { _isVisible = value; } }
        private bool _isVisible = false;

        public List<ClassWrap> Classes { get { return _classes; } }
        public List<EnumWrap> Enums { get { return _enums; } }
        public string Desc { get { return _xml.Desc; } set { _xml.Desc = value; } }

        public override string DisplayName
        {
            get
            {
                if (Desc.IsEmpty())
                    return Name;
                else
                    return Util.Format("{0}:{1}", Name, Desc);
            }
        }

        private List<ClassWrap> _classes;
        private List<EnumWrap> _enums;
        private NamespaceXml _xml;

        private string _path;
        protected NamespaceWrap(NamespaceXml xml) : base(xml.Name)
        {
            Init(xml);
        }
        private void Init(NamespaceXml xml)
        {
            _isDirty = false;
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

            _allNamespaces.Add(FullName, this);
        }

        #region 各类型添加/移除操作[添加新类型时,Add/Remove("TypeName")]
        public void AddClassWrap(ClassWrap wrap, bool isDirty = true)
        {
            if (isDirty)
            {
                wrap.SetNodeState(NodeState.Modify);
                SetNodeState(NodeState.Modify);
                _xml.Classes.Add(wrap);
                SetDirty();
            }

            _classes.Add(wrap);
            Add(wrap.Name);
        }
        public void RemoveClassWrap(ClassWrap wrap, bool isDirty = true)
        {
            if (!Contains(wrap.Name)) return;

            if (isDirty)
            {
                SetNodeState(NodeState.Modify);
                _xml.Classes.Remove(wrap);
                SetDirty();
            }

            Remove(wrap.Name);
            _classes.Remove(wrap);
            wrap.Namespace = null;
        }
        public void AddEnumWrap(EnumWrap wrap, bool isDirty = true)
        {
            if (isDirty)
            {
                wrap.SetNodeState(NodeState.Modify);
                SetNodeState(NodeState.Modify);
                _xml.Enums.Add(wrap);
                SetDirty();
            }

            _enums.Add(wrap);
            Add(wrap.Name);
        }
        public void RemoveEnumWrap(EnumWrap wrap, bool isDirty = true)
        {
            if (!Contains(wrap.Name)) return;

            if (isDirty)
            {
                SetNodeState(NodeState.Modify);
                _xml.Enums.Remove(wrap);
                SetDirty();
            }

            Remove(wrap.Name);
            _enums.Remove(wrap);
            wrap.Namespace = null;
        }
        #endregion

        public void SetDirty()
        {
            HasModifyNamespace = true;
            _isDirty = true;
        }
        public override void Dispose()
        {
            base.Dispose();
            try
            {
                string path = Util.GetNamespaceAbsPath(FullName + ".xml");
                File.Delete(path);
            }
            catch (Exception e)
            {
                ConsoleDock.Ins.LogErrorFormat("删除命名空间{0}失败!\n{1}\n{2}\n",
                   FullName, e.Message, e.StackTrace);
            }
            _allNamespaces.Remove(FullName);
            for (int i = 0; i < _classes.Count; i++)
                _classes[i].Dispose();
            for (int i = 0; i < _enums.Count; i++)
                _enums[i].Dispose();
            _classes.Clear();
            _enums.Clear();
            PoolManager.Ins.Push(this);
        }
        public void Save()
        {
            _xml.Name = _name;
            try
            {
                Util.Serialize(_path, _xml);
            }
            catch (Exception e)
            {
                ConsoleDock.Ins.LogErrorFormat("序列化命名空间{0}失败!\n{1}\n{2}\n",
                   _name, e.Message, e.StackTrace);
            }
        }
        public void Cancle()
        {
            _name = "";
            _xml = null;
        }
    }
}
