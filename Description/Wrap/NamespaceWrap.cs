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
        public static Dictionary<string, NamespaceWrap> AllNamespaces { get { return _allNamespaces; } }
        static Dictionary<string, NamespaceWrap> _allNamespaces = new Dictionary<string, NamespaceWrap>();
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
        public static void UpdateImportedFlag(List<string> imports)
        {
            HashSet<string> hash = new HashSet<string>(imports);
            foreach (var item in _allNamespaces)
                item.Value.IsContained = !hash.Contains(item.Key);
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
            wrap.Serialize();
            return wrap;
        }
        public static NamespaceWrap Create(NamespaceXml xml)
        {
            var wrap = PoolManager.Ins.Pop<NamespaceWrap>();
            return wrap ?? new NamespaceWrap(xml);
        }

        /// <summary>
        /// 是否包含在当前模块
        /// </summary>
        public bool IsContained = true;
        /// <summary>
        /// 是否有被修改
        /// </summary>
        public bool IsDirty { get { return _isDirty; } }
        public List<ClassWrap> Classes { get { return _classes; } }
        public List<EnumWrap> Enums { get { return _enums; } }
        public string Desc { get { return _xml.Desc; } }

        private List<ClassWrap> _classes;
        private List<EnumWrap> _enums;
        private NamespaceXml _xml;
        private bool _isDirty = false;
        protected NamespaceWrap(NamespaceXml xml) : base(xml.Name)
        {
            _xml = xml;
            _classes = new List<ClassWrap>();
            _enums = new List<EnumWrap>();
            _xml.Classes = _xml.Classes ?? new List<ClassXml>();
            _xml.Enums = _xml.Enums ?? new List<EnumXml>();

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
        public void AddClass(ClassWrap wrap)
        {
            _classes.Add(wrap);
            _xml.Classes.Add(wrap);
            Add(wrap.Name);
        }
        public void RemoveClass(ClassWrap wrap)
        {
            if (!Contains(wrap.Name)) return;
            Remove(wrap.Name);
            _classes.Remove(wrap);
            _xml.Classes.Remove(wrap);
        }
        public void AddEnum(EnumWrap wrap)
        {
            _enums.Add(wrap);
            _xml.Enums.Add(wrap);
            Add(wrap.Name);
        }
        public void RemoveEnum(EnumWrap wrap)
        {
            if (!Contains(wrap.Name)) return;
            Remove(wrap.Name);
            _enums.Remove(wrap);
            _xml.Enums.Remove(wrap);
        }
        public void Serialize()
        {
            try
            {
                string path = Util.GetNamespaceAbsPath(_name);
                Util.Serialize(path, _xml);
            }
            catch (Exception e)
            {
                ConsoleDock.Ins.LogErrorFormat("创建命名空间{0}失败!\n{1}\n{2}\n",
                   _name, e.Message, e.StackTrace);
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            try
            {
                string path = Util.GetNamespaceAbsPath(_name);
                File.Delete(path);
            }
            catch (Exception e)
            {
                ConsoleDock.Ins.LogErrorFormat("删除命名空间{0}失败!\n{1}\n{2}\n",
                   _name, e.Message, e.StackTrace);
            }
            _allNamespaces.Remove(_name);
            for (int i = 0; i < _classes.Count; i++)
                _classes[i].Dispose();
            for (int i = 0; i < _enums.Count; i++)
                _enums[i].Dispose();
            _classes.Clear();
            _enums.Clear();
        }
    }
}
