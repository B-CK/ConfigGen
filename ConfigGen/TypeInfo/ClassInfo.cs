using ConfigGen.Description;
using System;
using System.Collections.Generic;
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



        public string XmlPath { get { return _xmlDir; } }
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
        private string _xmlDir;
        private string _namespace;
        private string _fullName;

        private readonly List<FieldInfo> _fields;
        private readonly List<ConstInfo> _consts;
        private readonly HashSet<ClassInfo> _children;
        private readonly HashSet<string> _groups;

        public ClassInfo(ClassDes des, string xmlDir, string namespace0)
        {
            _fields = new List<FieldInfo>();
            _consts = new List<ConstInfo>();
            _children = new HashSet<ClassInfo>();
            _groups = new HashSet<string>();

            _des = des;
            _xmlDir = xmlDir;
            _namespace = namespace0;
            _fullName = string.Format("{0}.{1}", namespace0, des.Name);
        }

        public bool IsDynamic()
        {
            return _children.Count > 0;
        }


    }
}
