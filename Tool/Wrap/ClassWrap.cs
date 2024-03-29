﻿using Xml;
using System;
using Tool;
using System.Text;
using System.Collections.Generic;

namespace Tool.Wrap
{
    public class ClassWrap
    {
        private static Dictionary<string, ClassWrap> _classes = new Dictionary<string, ClassWrap>();
        public static Dictionary<string, ClassWrap> Classes { get { return _classes; } }
        public static ClassWrap Get(string fullName)
        {
            return IsClass(fullName) ? _classes[fullName] : null;
        }
        public static bool IsClass(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return false;
            return _classes.ContainsKey(fullName);
        }
        /// <summary>
        /// 当前类有子类
        /// </summary>
        public static bool IsDynamic(string fullName)
        {
            if (IsClass(fullName))
            {
                ClassWrap cls = _classes[fullName];
                return cls != null && cls.IsDynamic();
            }
            return false;
        }
        public static List<ClassWrap> GetExports()
        {
            var exports = new List<ClassWrap>();
            var cit = _classes.GetEnumerator();
            while (cit.MoveNext())
            {
                var cls = cit.Current.Value;
                if (Util.MatchGroups(cls._groups))
                    exports.Add(cls);
            }
            return exports;
        }
        /// <summary>
        /// 仅做局部修正矫正,不包含Module模块名
        /// </summary>
        public static string CorrectType(ClassWrap host, string type)
        {
            if (host == null) return type;

            if (!type.IsEmpty() && type.IndexOfAny(Setting.DotSplit) < 0)
                type = string.Format("{0}.{1}", host.Namespace, type);
            return type;
        }
        static void Add(ClassWrap info)
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
        /// 基类完整类名称;IsEmpty 则无继承
        /// </summary>
        public string Inherit { get { return _inherit; } }
        public string Desc { get { return _des.Desc; } }
        //C#特性描述
        public string Attribute { get { return _des.Attribute; } }
        public List<FieldWrap> Fields { get { return _fields; } }
        public List<ConstWrap> Consts { get { return _consts; } }
        public HashSet<string> Groups { get { return _groups; } }

        private ClassXml _des;
        private string _namespace;
        private string _fullName;
        private string _inherit;

        private readonly List<FieldWrap> _fields;
        private readonly List<ConstWrap> _consts;
        private readonly HashSet<string> _children;
        private readonly HashSet<string> _groups;

        public ClassWrap(ClassXml des, string namespace0)
        {
            _des = des;
            _fields = new List<FieldWrap>();
            _children = new HashSet<string>();
            if (Name.IsEmpty())
                Error("未指定Class名称");
            if (!Util.MatchIdentifier(Name))
                Error("命名不合法:" + Name);

            _namespace = namespace0;
            _fullName = string.Format("{0}.{1}", namespace0, des.Name);
            _inherit = des.Inherit;
            _inherit = CorrectType(this, _inherit);
            _groups = new HashSet<string>(Util.Split(des.Group == null ? "" : des.Group.ToLower()));
            if (_groups.Count == 0)
                _groups.Add(Setting.DefualtGroup);

            Add(this);
            _fields = new List<FieldWrap>();
            for (int i = 0; i < des.Fields.Count; i++)
            {
                var fieldDes = des.Fields[i];
                var info = new FieldWrap(this, fieldDes.Name, fieldDes.Type, fieldDes.Group, fieldDes.Desc, fieldDes.Attribute, _groups);
                info.CreateChecker(fieldDes);
                Fields.Add(info);
            }
            _consts = new List<ConstWrap>();
            for (int i = 0; i < des.Consts.Count; i++)
            {
                var constDes = des.Consts[i];
                var info = new ConstWrap(this, constDes.Name, constDes.Type, constDes.Value, constDes.Group, constDes.Desc, _groups);
                Consts.Add(info);
            }
        }

        /// <summary>
        /// 当前类有子类
        /// </summary>
        public bool IsDynamic()
        {
            return _children.Count > 0;
        }
        /// <summary>
        /// 是否为配置表结构
        /// </summary>
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
            try
            {
                string inhert = Inherit;
                while (!inhert.IsEmpty())
                {
                    var cls = Get(inhert);
                    if (cls == null)
                        Error("未知父类" + inhert);
                    if (!cls.HasChild(_fullName))
                        cls._children.Add(_fullName);

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
                            Error("Const命名重复:" + name);
                        _consts[i].VerifyDefine();
                    }
                    for (int i = 0; i < _fields.Count; i++)
                    {
                        string name = _fields[i].Name;
                        if (!hash.Contains(name))
                            hash.Add(name);
                        else
                            Error("Field命名重复:" + name);
                        _fields[i].VerifyDefine();
                    }
                }

                var git = _groups.GetEnumerator();
                while (git.MoveNext())
                    if (!GroupWrap.IsGroup(git.Current))
                        Error("未定义 Group:" + git.Current);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Class - FullName:{0}\tInherit:{1}\tGroup:{2}\n", FullName, Inherit, _des.Group);
            for (int i = 0; i < _consts.Count; i++)
                builder.AppendFormat("\t{0}\n", _consts[i]);
            for (int i = 0; i < _fields.Count; i++)
                builder.AppendFormat("\t{0}\n", _fields[i]);
            return builder.ToString();
        }
        private void Error(string msg)
        {
            string error = string.Format("Class:{0} 错误:{1}", FullName, msg);
            throw new Exception(error);
        }

    }
}
