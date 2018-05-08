using System;
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
    class TypeInfo : BaseInfo
    {
        public List<ClassTypeInfo> ClassInfos { get; set; }
        public List<EnumTypeInfo> EnumInfos { get; set; }

        [XmlIgnore]
        public Dictionary<string, BaseTypeInfo> TypeInfoDict { get { return _typeInfoDict; } }
        private Dictionary<string, BaseTypeInfo> _typeInfoDict = new Dictionary<string, BaseTypeInfo>();

        [XmlIgnore]
        public Dictionary<string, ClassTypeInfo> ClassInfoDict { get { return _classInfoDict; } }
        private Dictionary<string, ClassTypeInfo> _classInfoDict = new Dictionary<string, ClassTypeInfo>();
        [XmlIgnore]
        public Dictionary<string, EnumTypeInfo> EnumInfoDict { get { return _enumInfoDict; } }
        private Dictionary<string, EnumTypeInfo> _enumInfoDict = new Dictionary<string, EnumTypeInfo>();

        public void Init()
        {
            if (ClassInfos == null || ClassInfos.Count == 0 || EnumInfos == null || EnumInfos.Count == 0)
            {
                ClassInfos = new List<ClassTypeInfo>();
                EnumInfos = new List<EnumTypeInfo>();
                return;
            }

            for (int i = 0; i < ClassInfos.Count; i++)
            {
                string type = ClassInfos[i].GetClassName();
                if (!_classInfoDict.ContainsKey(type))
                {
                    _classInfoDict.Add(type, ClassInfos[i]);
                    _typeInfoDict.Add(type, ClassInfos[i]);
                }
                else
                    Util.LogErrorFormat("{0}类型重复定义,路径{1}", type, ClassInfos[i].RelPath);
            }
            for (int i = 0; i < EnumInfos.Count; i++)
            {
                string type = EnumInfos[i].Name;
                if (!_enumInfoDict.ContainsKey(type))
                {
                    _enumInfoDict.Add(type, EnumInfos[i]);
                    _typeInfoDict.Add(type, EnumInfos[i]);
                }
                else
                    Util.LogErrorFormat("{0}类型重复定义,路径{1}", type, EnumInfos[i].RelPath);
            }
            //基础类型信息
            foreach (var item in BaseType)
            {
                ClassTypeInfo classInfo = new ClassTypeInfo();
                classInfo.Name = item;
                classInfo.TypeType = TypeType.Base;
                _typeInfoDict.Add(item, classInfo);
                _typeInfoDict.Add(item, classInfo);
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
                    if (!_enumInfoDict.ContainsKey(enumInfo.Name))
                        _enumInfoDict.Add(enumInfo.Name, enumInfo);
                    else
                        _enumInfoDict[enumInfo.Name] = enumInfo;
                    break;
                case TypeType.None:
                default:
                    Util.LogErrorFormat("未定义{0}.{1}类型", baseInfo.NamespaceName, baseInfo.Name);
                    break;
            }

            if (baseInfo.TypeType != TypeType.None)
            {
                string type = "";
                if (!TypeInfoDict.ContainsKey(type))
                    TypeInfoDict.Add(type, baseInfo);
                else
                    Util.LogErrorFormat("{0}类型重复定义,路径{1}", type, baseInfo.RelPath);
            }
        }
        public void Remove(object info)
        {
            BaseTypeInfo baseInfo = info as BaseTypeInfo;
            switch (baseInfo.TypeType)
            {
                case TypeType.Base:
                case TypeType.Class:
                case TypeType.List:
                case TypeType.Dict:
                    ClassTypeInfo classInfo = info as ClassTypeInfo;
                    if (_classInfoDict.ContainsKey(classInfo.Name))
                        _classInfoDict.Remove(classInfo.Name);
                    break;
                case TypeType.Enum:
                    EnumTypeInfo enumInfo = info as EnumTypeInfo;
                    if (_enumInfoDict.ContainsKey(enumInfo.Name))
                        _enumInfoDict.Remove(enumInfo.Name);
                    break;
                case TypeType.None:
                default:
                    Util.LogErrorFormat("未定义{0}.{1}类型", baseInfo.NamespaceName, baseInfo.Name);
                    break;
            }
            if (baseInfo.TypeType != TypeType.None)
            {
                string type = "";
                if (TypeInfoDict.ContainsKey(type))
                    TypeInfoDict.Remove(type);
            }
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
            else if (LocalInfoManager.Instance.TypeInfoLib.ClassInfoDict.ContainsKey(type))
                typeType = TypeType.Class;
            else if (LocalInfoManager.Instance.TypeInfoLib.EnumInfoDict.ContainsKey(type))
                typeType = TypeType.Enum;
            else if (type.StartsWith(TypeType.List.ToString().ToLower()))
                typeType = TypeType.List;
            else if (type.StartsWith(TypeType.Dict.ToString().ToLower()))
                typeType = TypeType.Dict;

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
            else
                Util.LogErrorFormat("未定义{0}类型", type);
            return baseTypeInfo;
        }
    }

    abstract class BaseTypeInfo
    {
        public TypeType TypeType { get; set; }
        /// <summary>
        /// 类对应文件的相对路径
        /// </summary>
        public string RelPath { get; set; }
        public string NamespaceName { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }

        public string GetClassName()
        {
            return string.Format("{0}.{1}", NamespaceName, Name);
        }
    }

    /// <summary>
    /// 类描述
    /// </summary>
    [XmlInclude(typeof(ClassTypeInfo))]
    class ClassTypeInfo : BaseTypeInfo
    {
        ///// <summary>
        ///// 类对应文件的相对路径
        ///// </summary>
        //public string RelPath { get; set; }
        //public string NamespaceName { get; set; }
        //public string Name { get; set; }
        //public string Group { get; set; }
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
        public ClassTypeInfo Clone()
        {
            ClassTypeInfo newClassInfo = new ClassTypeInfo();
            newClassInfo.RelPath = RelPath;
            newClassInfo.NamespaceName = NamespaceName;
            newClassInfo.Name = Name;
            newClassInfo.Group = Group;
            newClassInfo.Fields = new List<FieldInfo>(Fields.ToArray());
            return newClassInfo;
        }
    }
    [XmlInclude(typeof(ListTypeInfo))]
    class ListTypeInfo : ClassTypeInfo
    {
        public string ElementType { get; set; }
    }
    [XmlInclude(typeof(DictTypeInfo))]
    class DictTypeInfo : ClassTypeInfo
    {
        public string KeyType { get; set; }
        public string ValueType { get; set; }

        public static implicit operator DictTypeInfo(ListTypeInfo v)
        {
            throw new NotImplementedException();
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
    class EnumTypeInfo : BaseTypeInfo
    {
        ///// <summary>
        ///// 类对应文件的相对路径
        ///// </summary>
        //public string RelPath { get; set; }
        //public string NamespaceName { get; set; }
        //public string Name { get; set; }
        //public string Group { get; set; }
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
