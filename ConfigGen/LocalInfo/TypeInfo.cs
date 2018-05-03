﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ConfigGen.LocalInfo
{
    /// <summary>
    /// 表中字段类型分类
    /// </summary>
    public enum FieldTypeType
    {
        None,
        Base,
        Class,
        Enum,
        List,
        Dict,
    }

    [XmlRoot("TypeInfo")]
    class TypeInfo : BaseInfo
    {
        public List<ClassInfo> ClassInfos { get; set; }
        public List<EnumInfo> EnumInfos { get; set; }

        [XmlIgnore]
        public Dictionary<string, ClassInfo> ClassInfoDict { get { return _classInfoDict; } }
        private Dictionary<string, ClassInfo> _classInfoDict = new Dictionary<string, ClassInfo>();
        [XmlIgnore]
        public Dictionary<string, EnumInfo> EnumInfoDict { get { return _enumInfoDict; } }
        private Dictionary<string, EnumInfo> _enumInfoDict = new Dictionary<string, EnumInfo>();

        public void Init()
        {
            if (ClassInfos == null || ClassInfos.Count == 0 || EnumInfos == null || EnumInfos.Count == 0)
            {
                ClassInfos = new List<ClassInfo>();
                EnumInfos = new List<EnumInfo>();
                return;
            }

            for (int i = 0; i < ClassInfos.Count; i++)
            {
                string className = ClassInfos[i].GetClassName();
                if (!_classInfoDict.ContainsKey(className))
                    _classInfoDict.Add(className, ClassInfos[i]);
            }
            for (int i = 0; i < EnumInfos.Count; i++)
            {
                string enumName = EnumInfos[i].Name;
                if (!_enumInfoDict.ContainsKey(enumName))
                    _enumInfoDict.Add(enumName, EnumInfos[i]);
            }
        }
        public void Add(object info)
        {
            if (info is ClassInfo)
            {
                ClassInfo classInfo = info as ClassInfo;
                string className = classInfo.GetClassName();
                if (!_classInfoDict.ContainsKey(className))
                    _classInfoDict.Add(className, classInfo);
                else
                    _classInfoDict[className] = classInfo;
            }
            else if (info is EnumInfo)
            {
                EnumInfo enumInfo = info as EnumInfo;
                if (!_enumInfoDict.ContainsKey(enumInfo.Name))
                    _enumInfoDict.Add(enumInfo.Name, enumInfo);
                else
                    _enumInfoDict[enumInfo.Name] = enumInfo;
            }
            else
                Util.LogError("[添加类型信]信息类型不匹配" + info.GetType().ToString());

        }
        public void Remove(object info)
        {
            if (info is ClassInfo)
            {
                ClassInfo classInfo = info as ClassInfo;
                if (_classInfoDict.ContainsKey(classInfo.Name))
                    _classInfoDict.Remove(classInfo.Name);
            }
            else if (info is EnumInfo)
            {
                EnumInfo enumInfo = info as EnumInfo;
                if (_enumInfoDict.ContainsKey(enumInfo.Name))
                    _enumInfoDict.Remove(enumInfo.Name);
            }
            else
                Util.LogError("[移除类型信息]信息类型不匹配" + info.GetType().ToString());
        }




        ///// <summary>
        ///// 检查类字段信息是否修改
        ///// </summary>
        //public bool CheckFieldInfo(ClassInfo classInfo)
        //{
        //    //检测字段类型修改,存在性
        //    //TODO
        //    //检查字段是否有增减
        //    //TODO
        //    return true;
        //}


        static readonly HashSet<string> BaseType = new HashSet<string>() { "int", "long", "bool", "float", "string" };
        /// <summary>
        /// 完整类名,即带命名空间,基础类型和集合除外
        /// </summary>
        public static FieldTypeType GetFieldTypeType(string type)
        {
            FieldTypeType typeType = FieldTypeType.None;
            if (BaseType.Contains(type))
                typeType = FieldTypeType.Base;
            else if (LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict.ContainsKey(type))
                typeType = FieldTypeType.Class;
            else if (LocalInfoManager.Instance.TypeInfoLib.EnumInfoDict.ContainsKey(type))
                typeType = FieldTypeType.Enum;
            else if (type.StartsWith(FieldTypeType.List.ToString().ToLower()))
                typeType = FieldTypeType.List;
            else if (type.StartsWith(FieldTypeType.Dict.ToString().ToLower()))
                typeType = FieldTypeType.Dict;

            return typeType;
        }
        public static string GetNamespaceName(string relPath)
        {
            string[] nodes = Path.GetDirectoryName(relPath).Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return Util.ListStringSplit(nodes, ".");
        }
        /// <summary>
        /// 检查类命名空间是否修改,true:通过,false:不通过
        /// </summary>
        public static bool CheckNamespaceName(string namespaceName)
        {
            string refPath = namespaceName.Replace(".", "/");
            string path = Util.GetConfigAbsPath(refPath);
            return Directory.Exists(path);
        }
        /// <summary>
        /// 检查类是否存在
        /// </summary>
        public static bool CheckClass(string className)
        {
            return LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict.ContainsKey(className);
        }
        /// <summary>
        /// 检查字段类型,true:通过,false:不通过
        /// </summary>
        /// <param name="type">类型全名,即带命名空间</param>
        public static bool CheckFieldClassName(string type, out string errorString)
        {
            FieldTypeType fieldTypeType = GetFieldTypeType(type);
            switch (fieldTypeType)
            {
                case FieldTypeType.Base:
                    break;
                case FieldTypeType.Class:
                case FieldTypeType.Enum:
                    int endIndex = type.LastIndexOf('.');
                    if (endIndex > -1 && !CheckClass(type))
                    {
                        errorString = string.Format("类型{0}不存在", type);
                        return false;
                    }
                    break;
                case FieldTypeType.List:
                    string element = type.Replace("list<", "").Replace(">", "");
                    FieldTypeType elementType = GetFieldTypeType(element);
                    if (elementType == FieldTypeType.None)
                    {
                        errorString = string.Format("list中元素类型{0}不存在", element);
                        return false;
                    }
                    else if (elementType == FieldTypeType.Dict || elementType == FieldTypeType.List)
                    {
                        errorString = string.Format("list元素类型{0}不能再为集合", element);
                        return false;
                    }
                    break;
                case FieldTypeType.Dict:
                    string keyValue = type.Replace("dict<", "").Replace(">", "");
                    string[] nodes = keyValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (nodes.Length != 2)
                    {
                        errorString = string.Format("dict中只能填写key类型和value类型,错误{0}", type);
                        return false;
                    }
                    FieldTypeType keyType = GetFieldTypeType(nodes[0]);
                    if (keyType != FieldTypeType.Base && keyType != FieldTypeType.Enum)
                    {
                        errorString = string.Format("dict中key类型只能为基础类型或者枚举,错误{0}", type);
                        return false;
                    }
                    FieldTypeType valueType = GetFieldTypeType(nodes[1]);
                    if (valueType == FieldTypeType.None)
                    {
                        errorString = string.Format("dict中value类型{0}不存在", nodes[1]);
                        return false;
                    }
                    else if (valueType == FieldTypeType.Dict || valueType == FieldTypeType.List)
                    {
                        errorString = string.Format("dict中value类型{0}不能再为集合", nodes[1]);
                        return false;
                    }
                    break;
                case FieldTypeType.None:
                default:
                    errorString = string.Format("类型{0}不存在", type);
                    return false;
            }

            errorString = null;
            return true;
        }
    }

    /// <summary>
    /// 类描述
    /// </summary>
    [XmlInclude(typeof(ClassInfo))]
    class ClassInfo
    {
        /// <summary>
        /// 类对应文件的相对路径
        /// </summary>
        public string RelPath { get; set; }
        public string NamespaceName { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public List<FieldInfo> Fields { get; set; }


        Dictionary<string, FieldInfo> _fieldInfoDict = new Dictionary<string, FieldInfo>();
        public Dictionary<string, FieldInfo> GetFieldInfoDict()
        {
            return _fieldInfoDict;
        }
        public void UpdateToDict()
        {
            for (int i = 0; i < Fields.Count; i++)
            {
                string fieldName = Fields[i].Name;
                if (!_fieldInfoDict.ContainsKey(fieldName))
                    _fieldInfoDict.Add(fieldName, Fields[i]);
            }
        }
        public string GetClassName()
        {
            return string.Format("{0}.{1}", NamespaceName, Name);
        }
        public ClassInfo Clone()
        {
            ClassInfo newClassInfo = new ClassInfo();
            newClassInfo.RelPath = RelPath;
            newClassInfo.NamespaceName = NamespaceName;
            newClassInfo.Name = Name;
            newClassInfo.Group = Group;
            newClassInfo.Fields = new List<FieldInfo>(Fields.ToArray());
            return newClassInfo;
        }
    }
    [XmlInclude(typeof(FieldInfo))]
    class FieldInfo
    {
        public string Name { get; set; }
        /// <summary>
        /// 完整类型名,即带命名空间
        /// </summary>
        public string Type { get; set; }
        public string Des { get; set; }
        public string Check { get; set; }
        public string Group { get; set; }
    }

    /// <summary>
    /// 枚举描述
    /// </summary>
    [XmlInclude(typeof(EnumInfo))]
    class EnumInfo
    {
        /// <summary>
        /// 类对应文件的相对路径
        /// </summary>
        public string RelPath { get; set; }
        public string NamespaceName { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public List<EnumKeyValue> KeyValuePair { get; set; }
    }
    [XmlInclude(typeof(EnumKeyValue))]
    class EnumKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Des { get; set; }
    }
}