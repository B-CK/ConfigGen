using Description.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Description.Wrap
{
    public class ClassWrap : TypeWrap
    {
        public static Dictionary<string, ClassWrap> Dict { get { return _dict; } }
        static Dictionary<string, ClassWrap> _dict = new Dictionary<string, ClassWrap>();
        public static ClassWrap[] Array
        {
            get
            {
                if (_array.Length != _dict.Count)
                {
                    var ls = new List<ClassWrap>(_dict.Values);
                    ls.Sort((a, b) => Comparer<string>.Default.Compare(a.DisplayName, b.DisplayName));
                    _array = ls.ToArray();
                }
                return _array;
            }
        }
        static ClassWrap[] _array = new ClassWrap[0];
        public static void ClearAll()
        {
            _array = new ClassWrap[0];
            _dict.Clear();
        }
        /// <summary>
        /// 记录类之间的继承
        /// </summary>
        public static void RecordChildren()
        {
            foreach (var item in Dict)
            {
                if (Dict.ContainsKey(item.Key))
                {
                    var parent = Dict[item.Key];
                    parent.AddChildClass(item.Value);
                }
            }
        }
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
            nsw.AddTypeWrap(wrap, false);
            return wrap;
        }

        public override string FullName => base.FullName;
        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                Xml.Name = value;
            }
        }
        public string Index { get { return Xml.Index; } set { Xml.Index = value; } }
        /// <summary>
        /// 全名称
        /// </summary>
        public string Inherit { get { return Xml.Inherit; } set { Xml.Inherit = value; } }
        public string DataPath { get { return Xml.DataPath; } set { Xml.DataPath = value; } }
        public string Desc { get { return Xml.Desc; } set { Xml.Desc = value; } }
        public string Group { get { return Xml.Group; } set { Xml.Group = value; } }
        public LinkedList<FieldWrap> Fields { get { return _fields; } }

        private ClassXml Xml => base._xml as ClassXml;
        private LinkedList<FieldWrap> _fields;
        private List<ClassWrap> _children;
        protected ClassWrap(ClassXml xml, NamespaceWrap ns) : base(xml, ns)
        {
            Init(xml, ns);
        }
        protected void Init(ClassXml xml, NamespaceWrap ns)
        {
            base.Init(xml, ns);

            _children = new List<ClassWrap>();
            _fields = new SortedList<int, FieldWrap>();
            Xml.Fields = Xml.Fields ?? new List<FieldXml>();
            var xfields = Xml.Fields;
            for (int i = 0; i < xfields.Count; i++)
            {
                var xfield = xfields[i];
                var field = FieldWrap.Create(xfield, this);
                Add(field.Name);
                _fields.Add(i, field);
            }
            _dict.Add(FullName, this);
        }
        public bool AddField(FieldWrap wrap)
        {
            if (Contains(wrap.Name))
            {
                Util.MsgWarning("[Class]类型{0}中已经存在字段{0}.", wrap.Name);
                return false;
            }

            Add(wrap.Name);
            _fields.Add(wrap.Seq, wrap);
            Xml.Fields.Add(wrap);
            return true;
        }
        public void RemoveField(FieldWrap wrap)
        {
            if (!Contains(wrap.Name)) return;

            Remove(wrap.Name);
            _fields.Remove(wrap.Seq);
            Xml.Fields.Remove(wrap);
        }
        /// <summary>
        /// 旧数据直接重写,新数据直接添加
        /// </summary>
        /// <param name="wrap"></param>
        public void OverrideField(FieldWrap wrap)
        {
            FieldWrap old = null;
            for (int i = 0; i < _fields.Count; i++)
            {
                if (_fields[i].Name == wrap.Name)
                {
                    old = wrap;
                    break;
                }
            }
            if (old != null)
                RemoveField(old);
            AddField(wrap);
        }
        public void AddChildClass(ClassWrap child)
        {
            if (!_children.Contains(child))
                _children.Add(child);
        }
        public List<ClassWrap> SortChildren()
        {
            _children.Sort((a, b) => string.Compare(a.FullName, b.FullName));
            return _children;
        }
        public override bool Check()
        {
            bool isOk = base.Check();
            for (int i = 0; i < _fields.Count; i++)
                isOk &= _fields[i].Check();
            if (!DataPath.IsEmpty())
            {
                string path = Util.GetDataDirAbsPath(DataPath);
                bool a = File.Exists(path);
                if (a == false)
                    Debug.LogErrorFormat("[Class]类型{0}的数据文件[{1}]不存在!", FullName, path);
                bool b = false;
                for (int i = 0; i < _fields.Count; i++)
                {
                    if (_fields[i].FullName == Index)
                    {
                        b = true;
                        break;
                    }
                }
                if (b == false)
                    Debug.LogErrorFormat("[Class]类型{0}的关键字[{1}]未定义!", FullName, Index);
                isOk &= a;
                isOk &= b;
            }
            if (!Inherit.IsEmpty())
            {
                bool c = Dict.ContainsKey(Inherit);
                isOk &= c;
                if (c == false)
                    Debug.LogErrorFormat("[Class]类型{0}的父类[{1}]不存在!", FullName, Inherit);
                c = ModuleWrap.Current.CheckType(Inherit);
                isOk &= c;
                if (c == false)
                    Debug.LogErrorFormat("[Class]{0}模块中不包含类型{1}的父类[{2}]!",
                        ModuleWrap.Current.Name, FullName, Inherit);
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
            _dict.Remove(FullName);
            for (int i = 0; i < _fields.Count; i++)
                _fields[i].Dispose();
            _fields.Clear();
            Namespace = null;
            PoolManager.Ins.Push(this);
        }
        public static implicit operator ClassXml(ClassWrap wrap)
        {
            return wrap.Xml;
        }
    }
}
