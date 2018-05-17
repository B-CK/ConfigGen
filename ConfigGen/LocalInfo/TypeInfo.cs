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
    public enum TypeType
    {
        None,
        Base,
        Class,
        Enum,
        List,
        Dict,
    }

    [XmlRoot("TypeInfo")]
    public class TypeInfo : BaseInfo
    {

        [XmlArrayItem("Class", typeof(ClassTypeInfo))]
        [XmlArrayItem("List", typeof(ListTypeInfo))]
        [XmlArrayItem("Dict", typeof(DictTypeInfo))]
        public List<ClassTypeInfo> ClassInfos { get; set; }
        [XmlArrayItem("Enum", typeof(EnumTypeInfo))]
        public List<EnumTypeInfo> EnumInfos { get; set; }

        [XmlIgnore]
        public Dictionary<string, BaseTypeInfo> TypeInfoDict { get { return _typeInfoDict; } set { _typeInfoDict = value; } }
        private Dictionary<string, BaseTypeInfo> _typeInfoDict = new Dictionary<string, BaseTypeInfo>();

        [XmlIgnore]
        public Dictionary<string, ClassTypeInfo> ClassInfoDict { get { return _classInfoDict; } }
        private Dictionary<string, ClassTypeInfo> _classInfoDict = new Dictionary<string, ClassTypeInfo>();
        [XmlIgnore]
        public Dictionary<string, EnumTypeInfo> EnumInfoDict { get { return _enumInfoDict; } }
        private Dictionary<string, EnumTypeInfo> _enumInfoDict = new Dictionary<string, EnumTypeInfo>();

        public void Init()
        {
            for (int i = 0; i < ClassInfos.Count; i++)
            {
                string type = ClassInfos[i].GetClassName();
                if (!_classInfoDict.ContainsKey(type))
                {
                    _classInfoDict.Add(type, ClassInfos[i]);
                    _typeInfoDict.Add(type, ClassInfos[i]);
                }
            }
            for (int i = 0; i < EnumInfos.Count; i++)
            {
                string type = EnumInfos[i].GetClassName();
                if (!_enumInfoDict.ContainsKey(type))
                {
                    _enumInfoDict.Add(type, EnumInfos[i]);
                    _typeInfoDict.Add(type, EnumInfos[i]);
                }
            }
        }

        public void Add(object info)
        {
            BaseTypeInfo baseInfo = info as BaseTypeInfo;
            switch (baseInfo.TypeType)
            {
                case TypeType.Base:
                case TypeType.Class:
                case TypeType.List:
                case TypeType.Dict:
                    ClassTypeInfo classInfo = baseInfo as ClassTypeInfo;
                    string className = classInfo.GetClassName();
                    if (!_classInfoDict.ContainsKey(className))
                        _classInfoDict.Add(className, classInfo);
                    else
                        _classInfoDict[className] = classInfo;
                    break;
                case TypeType.Enum:
                    EnumTypeInfo enumInfo = info as EnumTypeInfo;
                    string enumName = enumInfo.GetClassName();
                    if (!_enumInfoDict.ContainsKey(enumName))
                        _enumInfoDict.Add(enumName, enumInfo);
                    else
                        _enumInfoDict[enumName] = enumInfo;
                    break;
                case TypeType.None:
                default:
                    Util.LogErrorFormat("未定义{0}.{1}类型", baseInfo.NamespaceName, baseInfo.Name);
                    break;
            }

            if (baseInfo.TypeType != TypeType.None)
            {
                string type = baseInfo.GetClassName();
                if (!TypeInfoDict.ContainsKey(type))
                    TypeInfoDict.Add(type, baseInfo);
                else
                    TypeInfoDict[type] = baseInfo;
            }
        }
        public void Remove(object info)
        {
            if (info == null) return;

            BaseTypeInfo baseInfo = info as BaseTypeInfo;
            switch (baseInfo.TypeType)
            {
                case TypeType.Base:
                case TypeType.Class:
                case TypeType.List:
                case TypeType.Dict:
                    ClassTypeInfo classInfo = info as ClassTypeInfo;
                    string className = classInfo.GetClassName();
                    if (_classInfoDict.ContainsKey(className))
                        _classInfoDict.Remove(className);
                    break;
                case TypeType.Enum:
                    EnumTypeInfo enumInfo = info as EnumTypeInfo;
                    string enumName = enumInfo.GetClassName();
                    if (_enumInfoDict.ContainsKey(enumName))
                        _enumInfoDict.Remove(enumName);
                    break;
                case TypeType.None:
                default:
                    Util.LogErrorFormat("未定义{0}.{1}类型", baseInfo.NamespaceName, baseInfo.Name);
                    break;
            }
            if (baseInfo.TypeType != TypeType.None)
            {
                string type = baseInfo.GetClassName();
                if (TypeInfoDict.ContainsKey(type))
                    TypeInfoDict.Remove(type);
            }
        }
        public void CorrectClassInfo(List<string> part)
        {
            var infoDict = new Dictionary<string, BaseTypeInfo>(TypeInfoDict);
            foreach (var item in infoDict)
            {
                BaseTypeInfo typeInfo = item.Value;
                if (typeInfo.TypeType == TypeType.Class)
                {
                    string error = TableChecker.CheckType(item.Key);
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Util.LogErrorFormat("{0}类型不存在,错误位置{1}", item.Key, item.Value.RelPath);
                        continue;
                    }

                    ClassTypeInfo classType = typeInfo as ClassTypeInfo;
                    for (int i = 0; i < classType.Fields.Count; i++)
                    {
                        FieldInfo fieldInfo = classType.Fields[i];
                        TypeType typeType = GetTypeType(fieldInfo.Type);
                        if (typeType == TypeType.None)
                        {
                            string combine = Util.Combine(classType.NamespaceName, fieldInfo.Type);
                            typeType = GetTypeType(combine);
                        }
                        BaseTypeInfo baseType = null;
                        switch (typeType)
                        {
                            case TypeType.Class:
                            case TypeType.Enum:
                                fieldInfo.Type = CorrectType(fieldInfo.Type, classType, fieldInfo.Name);
                                continue;
                            case TypeType.List:
                                ListTypeInfo listType = new ListTypeInfo();
                                listType.TypeType = typeType;
                                string type = fieldInfo.Type.Replace("list<", "").Replace(">", "");
                                listType.ItemType = CorrectType(type, classType, fieldInfo.Name);
                                fieldInfo.Type = string.Format("list<{0}>", listType.ItemType);
                                listType.Name = fieldInfo.Type;
                                baseType = listType;
                                break;
                            case TypeType.Dict:
                                DictTypeInfo dictType = new DictTypeInfo();
                                dictType.TypeType = typeType;
                                string[] kv = fieldInfo.Type.Replace("dict<", "").Replace(">", "").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                if (kv.Length != 2)
                                {
                                    Util.LogErrorFormat("{0}类中字段{1}的dict类型格式错误,错误位置{2}",
                                        classType.GetClassName(), fieldInfo.Name, classType.RelPath);
                                    continue;
                                }
                                string key = CorrectType(kv[0], classType, fieldInfo.Name);
                                error = TableChecker.CheckDictKey(key);
                                if (!string.IsNullOrWhiteSpace(error))
                                {
                                    Util.LogErrorFormat("{0}类中字段{1}定义dict key类型错误,错误位置{2}",
                                        classType.GetClassName(), fieldInfo.Name, classType.RelPath);
                                    continue;
                                }
                                dictType.KeyType = key;
                                dictType.ValueType = CorrectType(kv[1], classType, fieldInfo.Name);
                                fieldInfo.Type = string.Format("dict<{0}, {1}>", dictType.KeyType, dictType.ValueType);
                                dictType.Name = fieldInfo.Type;
                                baseType = dictType;
                                break;
                            default:
                                continue;
                        }
                        baseType.IsExist = true;
                        Add(baseType);
                    }
                }
            }
            Clear(part);
            foreach (var item in TypeInfoDict)
            {
                BaseTypeInfo typeInfo = item.Value;
                if (typeInfo.TypeType != TypeType.Class) continue;

                ClassTypeInfo classType = typeInfo as ClassTypeInfo;
                if (!string.IsNullOrWhiteSpace(classType.Inherit))
                {
                    string newType = null;
                    string combine = Util.Combine(classType.NamespaceName, classType.Inherit);
                    string error = TableChecker.CheckType(combine);
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        error = TableChecker.CheckType(classType.Inherit);
                        if (!string.IsNullOrWhiteSpace(error))
                            newType = null;
                        else
                            newType = classType.Inherit;
                    }
                    else
                        newType = combine;

                    if (newType == null)
                        throw new Exception(string.Format("{0}类的继承类型{1}不存在,错误位置{2}",
                                classType.GetClassName(), classType.Inherit, classType.RelPath));
                    else
                        classType.Inherit = newType;

                    ClassTypeInfo pClass = GetTypeInfo(newType) as ClassTypeInfo;
                    pClass.HasSubClass = true;
                }
            }
        }
        private string CorrectType(string type, BaseTypeInfo baseType, string name)
        {
            string newType = null;
            string combine = Util.Combine(baseType.NamespaceName, type);
            string error = TableChecker.CheckType(combine);
            if (!string.IsNullOrWhiteSpace(error))
            {
                error = TableChecker.CheckType(type);
                if (!string.IsNullOrWhiteSpace(error))
                    newType = null;
                else
                    newType = type;
            }
            else
                newType = combine;

            if (newType != null)
            {
                var newInfo = TypeInfo.GetTypeInfo(newType);
                var oldInfo = TypeInfo.GetTypeInfo(type);
                Remove(oldInfo);
                Add(newInfo);
            }
            else
            {
                Util.LogErrorFormat("{0}类字段{1}类型{2}不存在,错误位置{3}",
                   baseType.GetClassName(), name, type, baseType.RelPath);
                throw new Exception();
            }
            return newType;
        }
        private void Clear(List<string> parts)
        {
            HashSet<string> hash = new HashSet<string>(parts);
            List<string> ls = new List<string>();
            foreach (var item in _classInfoDict)
            {
                if (hash.Contains(item.Value.RelPath))
                {
                    if (!item.Value.IsExist)
                        ls.Add(item.Key);
                }
                else if (item.Value.TypeType == TypeType.List
                   || item.Value.TypeType == TypeType.Dict)
                {
                    if (!item.Value.IsExist)
                        ls.Add(item.Key);
                }
            }
            foreach (var item in _enumInfoDict)
            {
                if (hash.Contains(item.Value.RelPath))
                {
                    if (!item.Value.IsExist)
                        ls.Add(item.Key);
                }
                if (item.Value.TypeType == TypeType.List
                   || item.Value.TypeType == TypeType.Dict)
                {
                    if (!item.Value.IsExist)
                        ls.Add(item.Key);
                }
            }
            for (int i = 0; i < ls.Count; i++)
            {
                BaseTypeInfo baseType = TypeInfoDict[ls[i]];
                Remove(baseType);
            }
        }
        private void UpdateList()
        {
            //基础类型信息
            foreach (var item in BaseType)
            {
                if (ClassInfoDict.ContainsKey(item)) continue;

                ClassTypeInfo classInfo = new ClassTypeInfo();
                classInfo.Name = item;
                classInfo.TypeType = TypeType.Base;
                ClassInfoDict.Add(item, classInfo);
            }
            ClassInfos = new List<ClassTypeInfo>(ClassInfoDict.Values);
            EnumInfos = new List<EnumTypeInfo>(EnumInfoDict.Values);
        }


        [XmlIgnore]
        static readonly HashSet<string> BaseType = new HashSet<string>() { "int", "long", "bool", "float", "string" };
        /// <summary>
        /// 完整类名,即带命名空间,基础类型和集合除外
        /// </summary>
        public static TypeType GetTypeType(string type)
        {
            TypeType typeType = TypeType.None;
            if (BaseType.Contains(type))
                typeType = TypeType.Base;
            else if (type.StartsWith("list<"))
                typeType = TypeType.List;
            else if (type.StartsWith("dict<"))
                typeType = TypeType.Dict;
            else if (LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict.ContainsKey(type))
                typeType = TypeType.Class;
            else if (LocalInfoManager.Instance.TypeInfoLib.EnumInfoDict.ContainsKey(type))
                typeType = TypeType.Enum;

            return typeType;
        }
        public static string GetNamespaceName(string relPath)
        {
            string[] nodes = Path.GetDirectoryName(relPath).Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return Util.ListStringSplit(nodes, ".");
        }
        public static BaseTypeInfo GetTypeInfo(string type)
        {
            BaseTypeInfo baseTypeInfo = null;
            var typeDict = LocalInfoManager.Instance.TypeInfoLib.TypeInfoDict;
            if (typeDict.ContainsKey(type))
                baseTypeInfo = typeDict[type];
            //else Util.LogErrorFormat("未定义{0}类型", type);
            return baseTypeInfo;
        }


        public void Save()
        {
            UpdateList();
            string path = LocalInfoManager.GetInfoPath(LocalInfoType.TypeInfo);
            Util.Serialize(path, this);
        }
    }

    public abstract class BaseTypeInfo
    {
        [XmlAttribute]
        public TypeType TypeType { get; set; }
        [XmlAttribute]
        /// <summary>
        /// 类对应文件的相对路径
        /// </summary>
        public string RelPath { get; set; }
        [XmlAttribute]
        public string NamespaceName { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Group { get; set; }

        /// <summary>
        /// 类型信息是否任然存在
        /// </summary>
        [XmlIgnore]
        public bool IsExist { get; set; }

        private string _className;
        /// <summary>
        /// 相对根的目录的命名空间,不包含根节点
        /// </summary>
        public string GetClassName()
        {
            if (_className == null)
            {
                if (string.IsNullOrWhiteSpace(NamespaceName))
                    _className = Name;
                else
                    _className = string.Format("{0}.{1}", NamespaceName, Name);
            }

            return _className;
        }
    }

    /// <summary>
    /// 类描述
    /// </summary>
    [XmlInclude(typeof(ClassTypeInfo))]
    public class ClassTypeInfo : BaseTypeInfo
    {
        [XmlAttribute]
        public string Inherit { get; set; }
        [XmlAttribute]
        public string DataTable { get; set; }
        [XmlElement("Field")]
        public List<FieldInfo> Fields { get; set; }

        [XmlIgnore]
        public FieldInfo IndexField { get; set; }
        [XmlIgnore]
        public bool HasSubClass { get; set; }

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
        public ClassTypeInfo GetInheritTypeInfo()
        {
            ClassTypeInfo classType = null;
            Dictionary<string, ClassTypeInfo> classInfoDict = LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict;
            if (classInfoDict.ContainsKey(Inherit))
                classType = classInfoDict[Inherit];
            else
                throw new Exception(string.Format("{0}类型的继承类型{1}不存在,错误位置{2}",
                    GetClassName(), Inherit, RelPath));
            return classType;
        }
    }
    [XmlInclude(typeof(ListTypeInfo))]
    public class ListTypeInfo : ClassTypeInfo
    {
        [XmlAttribute]
        public string ItemType { get; set; }
    }
    [XmlInclude(typeof(DictTypeInfo))]
    public class DictTypeInfo : ClassTypeInfo
    {
        [XmlAttribute]
        public string KeyType { get; set; }
        [XmlAttribute]
        public string ValueType { get; set; }
    }
    [XmlInclude(typeof(FieldInfo))]
    public class FieldInfo
    {
        [XmlAttribute]
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }
        [XmlAttribute]
        /// <summary>
        /// 完整类型名,即带命名空间
        /// </summary>
        public string Type { get; set; }
        [XmlAttribute]
        public string Des { get; set; }
        [XmlAttribute]
        public string Check { get; set; }
        [XmlAttribute]
        public string Group { get; set; }

        [XmlIgnore]
        public BaseTypeInfo BaseInfo
        {
            get
            {
                if (_typeInfo == null)
                    _typeInfo = TypeInfo.GetTypeInfo(Type);
                return _typeInfo;
            }
        }
        private BaseTypeInfo _typeInfo;
    }

    /// <summary>
    /// 枚举描述
    /// </summary>
    [XmlInclude(typeof(EnumTypeInfo))]
    public class EnumTypeInfo : BaseTypeInfo
    {
        [XmlElement("Item")]
        public List<EnumKeyValue> KeyValuePair { get; set; }
    }
    [XmlInclude(typeof(EnumKeyValue))]
    public class EnumKeyValue
    {
        [XmlAttribute]
        public string Key { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
        [XmlAttribute]
        public string Des { get; set; }
    }
}
