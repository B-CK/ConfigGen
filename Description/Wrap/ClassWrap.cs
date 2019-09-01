using Description.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Description.Wrap
{
    public class ClassWrap : BaseWrap, IDisposable
    {
        static HashSet<string> ClassHash = new HashSet<string>();

        public static ClassWrap Create(string name, NamespaceWrap ns)
        {
            ClassXml xml = new ClassXml() { Name = name };
            return Create(xml, ns);
        }
        public static ClassWrap Create(ClassXml xml, NamespaceWrap ns)
        {
            var wrap = PoolManager.Ins.Pop<ClassWrap>();
            return wrap ?? new ClassWrap(xml, ns);
        }

        public override string FullName { get { return Util.Format("{0}.{1}", _namespace.Name, Name); } }
        public NamespaceWrap Namespace { get { return _namespace; } set { _namespace = value; } }
        public string Index { get { return _xml.Index; } set { _xml.Index = value; } }
        /// <summary>
        /// 全名称
        /// </summary>
        public string Inherit { get { return _xml.Inherit; } set { _xml.Inherit = value; } }
        public string DataPath
        {
            get { return _xml.DataPath; }
            set
            {
                string dataDir = Path.GetFullPath(Util.DataDir);
                string relPath = value.Replace(dataDir, ".\\");
                _xml.DataPath = Util.Format("{0}\\{1}", dataDir, relPath);
            }
        }
        public string Desc { get { return _xml.Desc; } set { _xml.Desc = value; } }
        public string Group { get { return _xml.Group; } set { _xml.Group = value; } }
        public List<FieldWrap> Fields { get { return _fields; } }

        private ClassXml _xml;
        private NamespaceWrap _namespace;
        private List<FieldWrap> _fields;
        protected ClassWrap(ClassXml xml, NamespaceWrap ns) : base(xml.Name)
        {
            _xml = xml;
            _namespace = ns;
            _fields = new List<FieldWrap>();
            _xml.Fields = _xml.Fields ?? new List<FieldXml>();
            var xfields = _xml.Fields;
            for (int i = 0; i < xfields.Count; i++)
            {
                var xfield = xfields[i];
                var field = FieldWrap.Create(xfield, this);
                _fields.Add(field);
            }
            ClassHash.Add(FullName);
        }
        public bool AddField(FieldWrap wrap)
        {
            if (Contains(wrap.Name))
            {
                Util.MsgError("错误", "该空间中已经存在Class{0}.", wrap.Name);
                return false;
            }

            Add(wrap.Name);
            _fields.Add(wrap);
            _xml.Fields.Add(wrap);
            return true;
        }
        public void RemoveFoield(FieldWrap wrap)
        {
            if (!Contains(wrap.Name)) return;

            Remove(wrap.Name);
            _fields.Remove(wrap);
            _xml.Fields.Remove(wrap);
        }
        public override void Dispose()
        {
            base.Dispose();
            ClassHash.Remove(_name);
            for (int i = 0; i < _fields.Count; i++)
                _fields[i].Dispose();
            _fields.Clear();
        }
        public override bool Valide()
        {
            bool r = base.Valide();
            StringBuilder builder = new StringBuilder();
            bool exist = false;
            for (int i = 0; i < _fields.Count; i++)
            {
                if (_fields[i].Name == Index)
                {
                    exist = true;
                    break;
                }
            }
            if (exist == false)
            {
                builder.AppendFormat("类中不存在{0}关键字|", Index);
                r = false;
            }
            if (!ClassHash.Contains(Inherit))
            {
                r = false;
                builder.AppendFormat("继承类{0}不存在|", Inherit);
            }
            if (!File.Exists(DataPath))
            {
                r = false;
                builder.AppendFormat("数据表{0}不存在|", DataPath);
            }
            if (r == false)
                ConsoleDock.Ins.LogError(builder.ToString());
            return r;
        }

        public static implicit operator ClassXml(ClassWrap wrap)
        {
            return wrap._xml;
        }
    }
}
