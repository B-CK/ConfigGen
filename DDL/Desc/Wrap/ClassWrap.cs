using Desc.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desc.Wrap
{
    /// <summary>
    /// 注: MessagePack中多态时,根必须是抽象类/接口
    /// 子类中字段名称将覆盖父类字段名称
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
            wrap.OnNameChange += OnClassNameChange;
            nsw.AddTypeWrap(wrap, false);
            return wrap;
        }      
        private static void OnClassNameChange(BaseWrap wrap, string src)
        {
            if (_dict.ContainsKey(src))
            {
                _dict.Remove(src);
                _dict.Add(wrap.FullName, wrap as ClassWrap);
            }
            else
            {
                Util.MsgError("{0}类修改名称为{1}触发事件异常!", src, wrap.FullName);
            }
        }
        public static implicit operator ClassXml(ClassWrap wrap)
        {
            return wrap.Xml;
        }

        /// <summary>
        /// 类型信息改变
        /// </summary>
        public Action<ClassWrap> OnWrapChange;

        /// <summary>
        /// 父类
        /// </summary>
        public ClassWrap Parent
        {
            get
            {
                if (!Inherit.IsEmpty() && Dict.ContainsKey(Inherit))
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
        public string Group { get { return Xml.Group; } set { Xml.Group = value; } }
        public List<FieldWrap> Fields { get { return _fields; } }

        private ClassXml Xml => base._xml as ClassXml;
        private List<FieldWrap> _fields;
        private List<ClassWrap> _children;
        protected ClassWrap(ClassXml xml, NamespaceWrap ns) : base(xml, ns)
        {
            Init(xml, ns);
        }
        protected void Init(ClassXml xml, NamespaceWrap ns)
        {
            base.Init(xml, ns);

            _children = new List<ClassWrap>();
            _fields = new List<FieldWrap>();
            Xml.Fields = Xml.Fields ?? new List<FieldXml>();
            var xfields = Xml.Fields;
            for (int i = 0; i < xfields.Count; i++)
            {
                var xfield = xfields[i];
                var field = FieldWrap.Create(xfield, this);
                Add(field.Name);
                _fields.Add(field);
            }
            _dict.Add(FullName, this);
        }
        public void AddField(FieldWrap wrap)
        {
            if (Contains(wrap.Name))
            {
                Util.MsgWarning("[Class]类型{0}中已经存在字段{1}.", FullName, wrap.Name);
                return;
            }

            Add(wrap.Name);
            Xml.Fields.Add(wrap);
            _fields.Add(wrap);
        }
        public void RemoveField(FieldWrap wrap)
        {
            Remove(wrap.Name);
            _fields.Remove(wrap);
            Xml.Fields.Remove(wrap);
            wrap.Dispose();
        }

        public void OnSave()
        {
            OnWrapChange?.Invoke(this);
        }

        ///// <summary>
        ///// 保存时(数据有修改),调整xml中字段顺序
        ///// </summary>
        //public void ResortField(List<FieldWrap> fields)
        //{
        //    Xml.Fields.Clear();
        //    _fields = fields;
        //    for (int i = 0; i < fields.Count; i++)
        //        Xml.Fields.Add(fields[i]);
        //    OnWrapChange?.Invoke(this);
        //}
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
            List<string> repeat = new List<string>();
            HashSet<string> hash = new HashSet<string>();
            for (int i = 0; i < _fields.Count; i++)
            {
                var field = _fields[i];
                isOk &= field.Check();
                hasIndex |= field.FullName == Index;
                if (hash.Contains(field.Name))
                    repeat.Add(field.Name);
                else
                    hash.Add(field.Name);
            }
            if (repeat.Count != 0)
            {
                isOk &= false;
                Debug.LogErrorFormat("[Class]类型{0}的字段命名重复:{1}", FullName, string.Join(",", repeat));
            }
            repeat.Clear();
            if (Parent != null)
            {
                var fields = Parent.Fields;
                for (int i = 0; i < fields.Count; i++)
                {
                    var field = fields[i];
                    hasIndex |= field.FullName == Index;
                    if (Contains(field.Name))
                        repeat.Add(field.Name);
                }
            }
            if (repeat.Count != 0)
                Debug.LogWarningFormat("[Class]类型{0}的字段与父类字段重复:{1}", FullName, string.Join(",", repeat));
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
                c = ModuleWrap.Current.Classes.ContainsKey(Inherit);
                isOk &= c;
                if (c == false)
                    Debug.LogErrorFormat("[Class]{0}模块中不包含类型{1}的父类[{2}]!",
                        ModuleWrap.Current.Name, FullName, Inherit);
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
            _dict.Remove(FullName);
            for (int i = 0; i < _fields.Count; i++)
                _fields[i].Dispose();
            _fields.Clear();
            Namespace = null;
            PoolManager.Ins.Push(this);
        }
    }
}
