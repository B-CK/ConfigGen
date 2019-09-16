using Description.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Description.Wrap
{
    public class ClassWrap : TypeWrap, IDisposable
    {
        public static Dictionary<string, ClassWrap> ClassDict { get { return _classDict; } }
        static Dictionary<string, ClassWrap> _classDict = new Dictionary<string, ClassWrap>();
        public static ClassWrap[] Classes
        {
            get
            {
                if (_classes.Length != _classDict.Count)
                {
                    var ls = new List<ClassWrap>(_classDict.Values);
                    ls.Sort((a, b) => Comparer<string>.Default.Compare(a.DisplayName, b.DisplayName));
                    _classes = ls.ToArray();
                }
                return _classes;
            }
        }
        static ClassWrap[] _classes = new ClassWrap[0];


        public static ClassWrap Create(string name, NamespaceWrap nsw)
        {
            ClassXml xml = new ClassXml() { Name = name };
            return Create(xml, nsw);
        }
        public static ClassWrap Create(ClassXml xml, NamespaceWrap nsw)
        {
            var wrap = PoolManager.Ins.Pop<ClassWrap>();
            if (wrap == null)
                wrap = new ClassWrap(xml, nsw);
            else
                wrap.Init(xml, nsw);
            nsw.AddClassWrap(wrap, false);
            return wrap;
        }

        public string Index { get { return _xml.Index; } set { _xml.Index = value; } }
        /// <summary>
        /// 全名称
        /// </summary>
        public string Inherit { get { return _xml.Inherit; } set { _xml.Inherit = value; } }
        public string DataPath { get { return _xml.DataPath; } set { _xml.DataPath = value; } }
        public string Desc { get { return _xml.Desc; } set { _xml.Desc = value; } }
        public string Group { get { return _xml.Group; } set { _xml.Group = value; } }
        public List<FieldWrap> Fields { get { return _fields; } }

        public string[] Indexes
        {
            get
            {
                if (_indexes.Length != Fields.Count)
                {
                    _indexes = new string[_fields.Count];
                    for (int i = 0; i < _fields.Count; i++)
                        _indexes[i] = _fields[i].DisplayName;
                }
                return _indexes;
            }
        }

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

        string[] _indexes = new string[0];

        private ClassXml _xml;
        private List<FieldWrap> _fields;
        protected ClassWrap(ClassXml xml, NamespaceWrap ns) : base(xml, ns)
        {
            Init(xml, ns);
        }
        protected void Init(ClassXml xml, NamespaceWrap ns)
        {
            base.Init(xml, ns);
            _xml = xml;
            _namespace = ns;

            _fields = new List<FieldWrap>();
            _xml.Fields = _xml.Fields ?? new List<FieldXml>();
            var xfields = _xml.Fields;
            for (int i = 0; i < xfields.Count; i++)
            {
                var xfield = xfields[i];
                var field = FieldWrap.Create(xfield, this);
                Add(field.Name);
                _fields.Add(field);
            }
            _classDict.Add(FullName, this);
        }
        public bool AddField(FieldWrap wrap)
        {
            if (Contains(wrap.Name))
            {
                Util.MsgWarning("该空间中已经存在Class{0}.", wrap.Name);
                return false;
            }

            Add(wrap.Name);
            _fields.Add(wrap);
            _xml.Fields.Add(wrap);
            return true;
        }
        public void RemoveField(FieldWrap wrap)
        {
            if (!Contains(wrap.Name)) return;

            Remove(wrap.Name);
            _fields.Remove(wrap);
            _xml.Fields.Remove(wrap);
        }
        public override void Dispose()
        {
            base.Dispose();
            _classDict.Remove(FullName);
            for (int i = 0; i < _fields.Count; i++)
                _fields[i].Dispose();
            _fields.Clear();
            PoolManager.Ins.Push(this);
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
            if (!_classDict.ContainsKey(Inherit) && Inherit != FullName)
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
