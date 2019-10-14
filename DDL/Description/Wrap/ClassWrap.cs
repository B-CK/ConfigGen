using Description.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Description.Wrap
{
    /// <summary>
    ///注: MessagePack中多态时,根必须是抽象类/接口
    /// </summary>
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

        /// <summary>
        /// 父类
        /// </summary>
        public ClassWrap Parent
        {
            get
            {
                if (Dict.ContainsKey(Inherit))
                    return Dict[Inherit];
                return null;
            }
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
        private Dictionary<string, LinkedListNode<FieldWrap>> _fieldDict;
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
            _fieldDict = new Dictionary<string, LinkedListNode<FieldWrap>>();
            _fields = new LinkedList<FieldWrap>();
            Xml.Fields = Xml.Fields ?? new List<FieldXml>();
            var xfields = Xml.Fields;
            for (int i = 0; i < xfields.Count; i++)
            {
                var xfield = xfields[i];
                var field = FieldWrap.Create(xfield, this);
                Add(field.Name);
                if (_fields.Last == null)
                    _fieldDict.Add(field.Name, _fields.AddLast(field));
                else
                    _fieldDict.Add(field.Name, _fields.AddAfter(_fields.Last, field));
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
            if (_fields.Last == null)
                _fieldDict.Add(wrap.Name, _fields.AddLast(wrap));
            else
                _fieldDict.Add(wrap.Name, _fields.AddAfter(_fields.Last, wrap));
            Xml.Fields.Add(wrap);
            return true;
        }
        public void RemoveField(FieldWrap wrap)
        {
            if (!Contains(wrap.Name)) return;

            Remove(wrap.Name);
            _fields.Remove(wrap);
            _fieldDict.Remove(wrap.Name);
            Xml.Fields.Remove(wrap);
        }
        public void UpField(string name)
        {
            var current = _fieldDict[name];
            if (current.Previous != null)
            {
                var previous = current.Previous;
                _fields.Remove(current);
                _fields.AddBefore(previous, current);
            }
        }
        public void DownField(string name)
        {
            var current = _fieldDict[name];
            if (current.Next != null)
            {
                var next = current.Next;
                _fields.Remove(current);
                _fields.AddAfter(next, current);
            }
        }
        /// <summary>
        /// 旧数据直接重写,新数据直接添加
        /// </summary>
        /// <param name="wrap"></param>
        public void OverrideField(FieldWrap wrap)
        {
            var current = _fields.First;
            while (current != null)
            {
                if (current.Value.Name == wrap.Name)
                {
                    current.Value.Override(wrap);
                    return;
                }
                current = current.Next;
            }
            AddField(wrap);
        }
        /// <summary>
        /// 保存时,调整xml中字段顺序
        /// </summary>
        public void ResortField()
        {
            Xml.Fields.Clear();
            var current = _fields.First;
            while (current != null)
            {
                Xml.Fields.Add(current.Value);
                current = current.Next;
            }
        }
        /// <summary>
        /// 取消保存时还原字段顺序
        /// </summary>
        public void RecoverField()
        {
            var current = _fields.First;
            while (current != null)
            {
                var next = current.Next;
                var wrap = current.Value;
                Remove(wrap.Name);
                _fields.Remove(wrap);
                _fieldDict.Remove(wrap.Name);
                current = next;
            }
            var xfields = Xml.Fields;
            for (int i = 0; i < xfields.Count; i++)
            {
                var wrap = FieldWrap.Create(xfields[i], this);
                Add(wrap.Name);
                if (_fields.Last == null)
                    _fieldDict.Add(wrap.Name, _fields.AddLast(wrap));
                else
                    _fieldDict.Add(wrap.Name, _fields.AddAfter(_fields.Last, wrap));
            }
        }
        ///// <summary>
        ///// 重置字段在列表中的顺序编号
        ///// </summary>
        //public void InitFieldSeq()
        //{
        //    int i = 0;
        //    var current = _fields.First;
        //    while (current != null)
        //    {
        //        current.Value.Seq = i++;
        //        current = current.Next;
        //    }
        //}
        public void AddChildClass(ClassWrap child)
        {
            if (!_children.Contains(child))
                _children.Add(child);
        }
        /// <summary>
        /// 子类排序
        /// </summary>
        /// <returns></returns>
        public List<ClassWrap> SortChildren()
        {
            _children.Sort((a, b) => string.Compare(a.FullName, b.FullName));
            return _children;
        }
        public override bool Check()
        {
            bool isOk = base.Check();
            bool hasIndex = false;
            var current = _fields.First;
            while (current != null)
            {
                isOk &= current.Value.Check();
                hasIndex = current.Value.FullName == Index;
                current = current.Next;
            }
            if (!DataPath.IsEmpty())
            {
                string path = Util.GetDataDirAbsPath(DataPath);
                bool a = File.Exists(path);
                if (a == false)
                    Debug.LogErrorFormat("[Class]类型{0}的数据文件[{1}]不存在!", FullName, path);
                if (hasIndex == false)
                    Debug.LogErrorFormat("[Class]类型{0}的关键字[{1}]未定义!", FullName, Index);
                isOk &= a;
                isOk &= hasIndex;
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
            var current = _fields.First;
            while (current != null)
            {
                current.Value.Dispose();
                current = current.Next;
            }
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
