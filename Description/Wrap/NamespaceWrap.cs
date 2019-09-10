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
        public static void Init()
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
                Create(Util.EmptyNamespace);
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
            return wrap ?? new NamespaceWrap(xml);
        }

        /// <summary>
        /// 是否有被修改
        /// </summary>
        public bool IsDirty { get { return _isDirty; } }

        public List<ClassWrap> Classes { get { return _classes; } }
        public List<EnumWrap> Enums { get { return _enums; } }
        public string Desc { get { return _desc; } set { _desc = value; } }

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

        private string _desc;

        private List<ClassWrap> _classes;
        private List<EnumWrap> _enums;
        private NamespaceXml _xml;
        private bool _isDirty = false;
        private string _path;
        protected NamespaceWrap(NamespaceXml xml) : base(xml.Name)
        {
            _isDirty = false;
            _xml = xml;
            _classes = new List<ClassWrap>();
            _enums = new List<EnumWrap>();
            _xml.Classes = _xml.Classes ?? new List<ClassXml>();
            _xml.Enums = _xml.Enums ?? new List<EnumXml>();
            _path = Util.GetNamespaceAbsPath(_name + ".xml");
            _desc = _xml.Desc;

            var xclasses = _xml.Classes;
            for (int i = 0; i < xclasses.Count; i++)
            {
                var xclass = xclasses[i];
                var item = ClassWrap.Create(xclass, this);
                _classes.Add(item);
            }
            var xenums = _xml.Enums;
            for (int i = 0; i < xenums.Count; i++)
            {
                var xenum = xenums[i];
                var item = EnumWrap.Create(xenum, this);
                _enums.Add(item);
            }

            _allNamespaces.Add(FullName, this);
        }
        public override bool CheckName()
        {
            if (_name == Util.EmptyNamespace)
                return true;
            return base.CheckName();
        }
        public void AddClass(ClassWrap wrap)
        {
            _classes.Add(wrap);
            _xml.Classes.Add(wrap);
            Add(wrap.Name);
            SetDirty();
        }
        public void RemoveClass(ClassWrap wrap)
        {
            if (!Contains(wrap.Name)) return;
            Remove(wrap.Name);
            _classes.Remove(wrap);
            _xml.Classes.Remove(wrap);
            wrap.Namespace = null;
            SetDirty();
        }
        public void AddEnum(EnumWrap wrap)
        {
            _enums.Add(wrap);
            _xml.Enums.Add(wrap);
            Add(wrap.Name);
            SetDirty();
        }
        public void RemoveEnum(EnumWrap wrap)
        {
            if (!Contains(wrap.Name)) return;
            Remove(wrap.Name);
            _enums.Remove(wrap);
            _xml.Enums.Remove(wrap);
            wrap.Namespace = null;
            SetDirty();
        }
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
            _xml.Desc = _desc;
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
            _desc = "";
            _xml = null;
        }
    }
}
