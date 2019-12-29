using Description.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Description.TypeInfo
{
    public class ClassInfo
    {
        private static Dictionary<string, ClassInfo> _classes = new Dictionary<string, ClassInfo>();
        public static Dictionary<string, ClassInfo> Classes { get { return _classes; } }
        public static ClassInfo Get(string fullName)
        {
            return IsClass(fullName) ? _classes[fullName] : null;
        }
        public static bool IsClass(string fullName)
        {
            return _classes.ContainsKey(fullName);
        }
        /// <summary>
        /// 当前类有子类
        /// </summary>
        public static bool IsDynamic(string fullName)
        {
            if (IsClass(fullName))
            {
                ClassInfo cls = _classes[fullName];
                return cls != null && cls.IsDynamic();
            }
            return false;
        }
        public static List<ClassInfo> GetExports()
        {
            var exports = new List<ClassInfo>();
            var cit = _classes.GetEnumerator();
            while (cit.MoveNext())
            {
                var cls = cit.Current.Value;
                if (Util.MatchGroups(cls._groups))
                    exports.Add(cls);
            }
            return exports;
        }
        public static string CorrectType(ClassInfo host, string type)
        {
            if (host == null) return type;

            if (!type.IsEmpty() && type.IndexOfAny(Setting.DotSplit) < 0)
                type = string.Format("{0}.{1}", host.Namespace, type);
            return type;
        }
        static void Add(ClassInfo info)
        {
            if (_classes.ContainsKey(info._fullType))
                Util.LogWarningFormat("{1} 重复定义!", info._fullType);
            else
                _classes.Add(info._fullType, info);
        }


        public string FullType { get { return _fullType; } }
        public string Namespace { get { return _namespace; } }
        public string Name { get { return _des.Name; } }
        /// <summary>
        /// 基类完整类名称;IsEmpty 则无继承
        /// </summary>
        public string Inherit { get { return _inherit; } }
        public string Desc { get { return _des.Desc; } }
        public List<FieldInfo> Fields { get { return _fields; } }
        public List<ConstInfo> Consts { get { return _consts; } }
        public HashSet<string> Groups { get { return _groups; } }

        private ClassXml _des;
        private string _namespace;
        private string _fullType;
        private string _inherit;

        private readonly List<FieldInfo> _fields;
        private readonly List<ConstInfo> _consts;
        private readonly HashSet<string> _children;
        private readonly HashSet<string> _groups;

        public ClassInfo(ClassXml des, string namespace0)
        {
            _des = des;
            _fields = new List<FieldInfo>();
            _consts = new List<ConstInfo>();
            _children = new HashSet<string>();
            if (Name.IsEmpty())
                Error("未指定Class名称");
            if (!Util.MatchIdentifier(Name))
                Error("命名不合法:" + Name);

            _namespace = namespace0;
            _fullType = string.Format("{0}.{1}", namespace0, des.Name);
            _inherit = des.Inherit;
            _inherit = CorrectType(this, _inherit);
            _groups = new HashSet<string>(Util.Split(des.Group));
            if (_groups.Count == 0)
                _groups.Add(global::Description.Setting.DefualtGroup);

            Add(this);
            _consts = new List<ConstInfo>();
            for (int i = 0; i < des.Consts.Count; i++)
            {
                var info = new ConstInfo(_fullType, des.Consts[i]);
                Consts.Add(info);
            }
            _fields = new List<FieldInfo>();
            for (int i = 0; i < des.Fields.Count; i++)
            {
                var fieldDes = des.Fields[i];
                var info = new FieldInfo(this, fieldDes.Name, fieldDes.Type, fieldDes.Group, fieldDes.Desc, _groups);
                info.InitCheck(fieldDes.Ref, fieldDes.RefPath);
                Fields.Add(info);
            }
        }

        /// <summary>
        /// 当前类有子类
        /// </summary>
        public bool IsDynamic()
        {
            return _children.Count > 0;
        }
        public bool IsConfig()
        {
            return !_des.DataPath.IsEmpty();
        }
        /// <summary>
        /// 判断是否存在该子类
        /// </summary>
        public bool HasChild(string child)
        {
            return _children.Contains(child);
        }
        public void VerifyDefine()
        {
            string inhert = Inherit;
            while (!inhert.IsEmpty())
            {
                var cls = Get(inhert);
                if (!cls.HasChild(_fullType))
                    cls._children.Add(_fullType);

                inhert = cls.Inherit;
            }

            if (!Inherit.IsEmpty() && !IsClass(Inherit))
                Error("未知父类" + Inherit);
            else
            {
                HashSet<string> hash = new HashSet<string>();
                for (int i = 0; i < _consts.Count; i++)
                {
                    string name = _consts[i].Name;
                    if (!hash.Contains(name))
                        hash.Add(name);
                    else
                        Error("Const名重复:" + name);
                    _consts[i].VerifyDefine();
                }
                hash.Clear();
                for (int i = 0; i < _fields.Count; i++)
                {
                    string name = _fields[i].Name;
                    if (!hash.Contains(name))
                        hash.Add(name);
                    else
                        Error("Field名重复:" + name);
                    _fields[i].VerifyDefine();
                }
            }

            var git = _groups.GetEnumerator();
            while (git.MoveNext())
                if (!GroupInfo.IsGroup(git.Current))
                    Error("未知 Group:" + git.Current);
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Class - FullName:{0}\tInherit:{1}\tGroup:{2}\n", FullType, Inherit, _des.Group);
            for (int i = 0; i < _fields.Count; i++)
                builder.AppendFormat("\t{0}\n", _fields[i]);
            for (int i = 0; i < _consts.Count; i++)
                builder.AppendFormat("\t{0}\n", _consts[i]);
            return builder.ToString();
        }
        private void Error(string msg)
        {
            string error = string.Format("Class:{0} 错误:{1}", FullType, msg);
            throw new Exception(error);
        }

    }
}
