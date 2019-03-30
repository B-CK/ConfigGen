using ConfigGen.Description;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConfigGen.TypeInfo
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
        public static bool IsDynamic(string fullName)
        {
            if (IsClass(fullName))
            {
                ClassInfo cls = _classes[fullName];
                return cls != null && cls.IsDynamic();
            }
            return false;
        }
        static void Add(ClassInfo info)
        {
            if (_classes.ContainsKey(info._fullName))
                Util.LogWarningFormat("{1} 重复定义!", info._fullName);
            else
                _classes.Add(info._fullName, info);
        }


        public string FullName { get { return _fullName; } }
        public string Namespace { get { return _namespace; } }
        public string Name { get { return _des.Name; } }
        /// <summary>
        /// 基类完整类名称;IsNullOrWhiteSpace 则无继承
        /// </summary>
        public string Inherit { get { return _des.Inherit; } }
        public string Desc { get { return _des.Desc; } }
        public List<FieldInfo> Fields { get { return _fields; } }
        public List<ConstInfo> Consts { get { return _consts; } }
        public HashSet<ClassInfo> Children { get { return _children; } }
        public HashSet<string> Groups { get { return _groups; } }

        private ClassDes _des;
        private string _namespace;
        private string _fullName;

        private readonly List<FieldInfo> _fields;
        private readonly List<ConstInfo> _consts;
        private readonly HashSet<ClassInfo> _children;
        private readonly HashSet<string> _groups;

        public ClassInfo(ClassDes des, string namespace0)
        {
            _fields = new List<FieldInfo>();
            _consts = new List<ConstInfo>();
            _children = new HashSet<ClassInfo>();

            _des = des;
            _namespace = namespace0;
            _fullName = string.Format("{0}.{1}", namespace0, des.Name);
            _groups = new HashSet<string>(Util.Split(des.Group));

            Add(this);
            _consts = new List<ConstInfo>();
            for (int i = 0; i < des.Consts.Count; i++)
            {
                var info = new ConstInfo(_fullName, des.Consts[i]);
                Consts.Add(info);
            }
            _fields = new List<FieldInfo>();
            for (int i = 0; i < des.Fields.Count; i++)
            {
                var info = new FieldInfo(this, des.Fields[i]);
                Fields.Add(info);
            }
        }

        public bool IsDynamic()
        {
            return _children.Count > 0;
        }
        public bool IsConfig(string path)
        {
            return !path.IsEmpty() && File.Exists(path) || Directory.Exists(path);
        }

        public void VerifyDefine()
        {
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
        }
        public void Error(string msg)
        {
            string error = string.Format("Class:{0} 错误:{1}", FullName, msg);
            throw new Exception(error);
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Class - FullName:{0}\tInherit:{1}\tDataPath:{2}\tGroup:{3}\n", FullName, Inherit, _des.Group);
            for (int i = 0; i < _fields.Count; i++)
                builder.AppendFormat("\t{0}\n", _fields[i]);
            for (int i = 0; i < _consts.Count; i++)
                builder.AppendFormat("\t{0}\n", _consts[i]);
            return builder.ToString();
        }

    }
}
