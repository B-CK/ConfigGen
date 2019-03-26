using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ConfigGen.Description
{
    /// <summary>
    /// 表中字段类型分类
    /// </summary>
    public enum TypeType
    {
        None,
        Base,
        Enum,
        Class,
        List,
        Dict,
    }

    public class TypeInfo
    {
        public const string INT = "int";
        public const string LONG = "long";
        public const string BOOL = "bool";
        public const string FLOAT = "float";
        public const string STRING = "string";

        public static TypeInfo Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TypeInfo();
                return _instance;
            }
        }
        private static TypeInfo _instance;

        private HashSet<string> BaseTypeSet = new HashSet<string>();

        public List<ClassTypeInfo> ClassInfos { get; private set; }
        public List<EnumTypeInfo> EnumInfos { get; private set; }
        public Dictionary<string, BaseTypeInfo> TypeInfoDict { get; private set; }
        private Dictionary<string, ClassTypeInfo> ClassInfoDict { get; set; }
        private Dictionary<string, EnumTypeInfo> EnumInfoDict { get; set; }

        public static void Init()
        {
            Instance.ClassInfos = new List<ClassTypeInfo>();
            Instance.EnumInfos = new List<EnumTypeInfo>();
            Instance.TypeInfoDict = new Dictionary<string, BaseTypeInfo>();
            Instance.ClassInfoDict = new Dictionary<string, ClassTypeInfo>();
            Instance.EnumInfoDict = new Dictionary<string, EnumTypeInfo>();

            //解析类型定义
            Dictionary<string, TypeDescription> pairs = new Dictionary<string, TypeDescription>();
            var configXml = Util.Deserialize(Values.ConfigXml, typeof(ConfigXml)) as ConfigXml;
            if (string.IsNullOrWhiteSpace(configXml.Root))
                throw new Exception("数据结构导出时必须指定命名空间根节点<Config Root=\"**\">");
            Values.ConfigRootNode = configXml.Root;
            List<string> defines = configXml.Include;
            string path = "xmlDes";
            try
            {
                for (int i = 0; i < defines.Count; i++)
                {
                    path = Path.Combine(Values.ConfigDir, defines[i]);
                    var typeDes = Util.Deserialize(path, typeof(TypeDescription)) as TypeDescription;
                    if (pairs.ContainsKey(typeDes.Namespace))
                    {
                        pairs[typeDes.Namespace].Classes.AddRange(typeDes.Classes);
                        pairs[typeDes.Namespace].Enums.AddRange(typeDes.Enums);
                    }
                    else
                    {
                        typeDes.XmlDirPath = Path.GetDirectoryName(path);
                        pairs.Add(typeDes.Namespace, typeDes);
                    }
                }
            }
            catch (Exception e)
            {
                Util.LogError(path);
                throw new Exception(e.Message);
            }

            HashSet<string> exclude = new HashSet<string>(configXml.NoStruct);
            //类型重名检查
            HashSet<string> fullHash = new HashSet<string>();
            foreach (var item in pairs)
            {
                foreach (var c in item.Value.Classes)
                {
                    string name = string.Format("{0}.{1}", item.Key, c.Name);
                    if (exclude.Contains(name)) continue;
                    if (fullHash.Contains(name))
                        Util.LogErrorFormat("{0} Class重复定义!\t{1}", name, item.Value.XmlDirPath);

                    fullHash.Add(name);
                    ClassTypeInfo info = new ClassTypeInfo(item.Value.XmlDirPath, item.Key, c);
                    Instance.Add(info);
                }
                foreach (var e in item.Value.Enums)
                {
                    string name = string.Format("{0}.{1}", item.Key, e.Name);
                    if (exclude.Contains(name)) continue;
                    if (fullHash.Contains(name))
                        Util.LogErrorFormat("{0} Enum重复定义!\t{1}", name, item.Value.XmlDirPath);

                    fullHash.Add(name);
                    EnumTypeInfo info = new EnumTypeInfo(item.Value.XmlDirPath, item.Key, e);
                    Instance.Add(info);
                }
            }
            //类型分组
            DoGrouping();
            //过滤导出类型
            //FilterInfo();
            //添加基础类型
            HashSet<string> _baseType = new HashSet<string>() { INT, LONG, BOOL, FLOAT, STRING };
            foreach (var item in _baseType)
            {
                Instance.Add(new BaseTypeInfo("", item));
                Instance.BaseTypeSet.Add(item);
            }
            //初始化类型
            foreach (var item in Instance.ClassInfos)
                item.Init();
            foreach (var item in Instance.EnumInfos)
                item.Init();
        }
        private static void DoGrouping()
        {
            if (Values.ExportGroup == null)
                Values.ExportGroup = new HashSet<string>() { Values.DefualtGroup };

            List<BaseTypeInfo> infoList = new List<BaseTypeInfo>();
            infoList.AddRange(Instance.ClassInfos);
            infoList.AddRange(Instance.EnumInfos);
            for (int i = 0; i < infoList.Count; i++)
            {
                //Class/Enum型分组
                BaseTypeInfo baseType = infoList[i];
                if (baseType.EType == TypeType.Class)
                {
                    ClassTypeInfo classType = baseType as ClassTypeInfo;
                    if (!Values.ExportGroup.Overlaps(classType.GroupHashSet))
                    {
                        Instance.Remove(classType);
                        continue;
                    }

                    var fields = new List<FieldInfo>(classType.Fields);
                    for (int j = 0; j < fields.Count; j++)
                    {
                        FieldInfo field = fields[j];
                        if (!Values.ExportGroup.Overlaps(field.GroupHashSet))
                            classType.Fields.Remove(field);
                    }
                    classType.UpdateFieldDict();
                    fields.Clear();
                    fields.AddRange(classType.Consts);
                    for (int j = 0; j < fields.Count; j++)
                    {
                        ConstInfo field = fields[j] as ConstInfo;
                        if (!Values.ExportGroup.Overlaps(field.GroupHashSet))
                            classType.Consts.Remove(field);
                    }
                    classType.UpdateConstDict();
                }
                else if (baseType.EType == TypeType.Enum)
                {
                    EnumTypeInfo enumType = baseType as EnumTypeInfo;
                    var kvs = new List<ConstInfo>(enumType.Enums);
                    for (int j = 0; j < kvs.Count; j++)
                    {
                        ConstInfo kv = kvs[j];
                        if (!Values.ExportGroup.Overlaps(enumType.GroupHashSet))
                            enumType.Enums.Remove(kv);
                    }
                    enumType.UpdateEnumDict();
                }
            }
            return;
        }
        public void Add(BaseTypeInfo info)
        {
            switch (info.EType)
            {
                case TypeType.Class:
                    ClassTypeInfo classInfo = info as ClassTypeInfo;
                    string className = classInfo.GetFullName();
                    if (!ClassInfoDict.ContainsKey(className))
                    {
                        ClassInfoDict.Add(className, classInfo);
                        ClassInfos.Add(classInfo);
                    }
                    break;
                case TypeType.Enum:
                    EnumTypeInfo enumInfo = info as EnumTypeInfo;
                    string enumName = enumInfo.GetFullName();
                    if (!EnumInfoDict.ContainsKey(enumName))
                    {
                        EnumInfoDict.Add(enumName, enumInfo);
                        EnumInfos.Add(enumInfo);
                    }
                    break;
                case TypeType.Base:

                case TypeType.List:
                case TypeType.Dict:
                    break;
                case TypeType.None:
                default:
                    Util.LogErrorFormat("{0} 类型无法解析!", info.GetFullName());
                    break;
            }

            if (info.EType != TypeType.None)
            {
                string type = info.GetFullName();
                if (!TypeInfoDict.ContainsKey(type))
                    TypeInfoDict.Add(type, info);
                else
                    TypeInfoDict[type] = info;
            }
        }
        public void Remove(BaseTypeInfo info)
        {
            if (info == null) return;

            string fullName = info.GetFullName();
            switch (info.EType)
            {
                case TypeType.Class:
                    ClassTypeInfo classInfo = info as ClassTypeInfo;
                    if (ClassInfoDict.ContainsKey(fullName))
                    {
                        ClassInfoDict.Remove(fullName);
                        ClassInfos.Remove(classInfo);
                    }
                    break;
                case TypeType.Enum:
                    EnumTypeInfo enumInfo = info as EnumTypeInfo;
                    if (EnumInfoDict.ContainsKey(fullName))
                    {
                        EnumInfoDict.Remove(fullName);
                        EnumInfos.Remove(enumInfo);
                    }
                    break;
                case TypeType.Base:
                case TypeType.List:
                case TypeType.Dict:
                    break;
                case TypeType.None:
                default:
                    Util.LogErrorFormat("{0} 类型无法解析!", info.GetFullName());
                    break;
            }
            if (info.EType != TypeType.None)
            {
                if (TypeInfoDict.ContainsKey(fullName))
                    TypeInfoDict.Remove(fullName);
            }
        }

        /// <summary>
        /// 完整类名,即带命名空间
        /// </summary>
        public static TypeType GetTypeType(string fullName)
        {
            TypeType typeType = TypeType.None;
            if (Instance.BaseTypeSet.Contains(fullName))
                typeType = TypeType.Base;
            else if (fullName.StartsWith("list:"))
                typeType = TypeType.List;
            else if (fullName.StartsWith("dict:"))
                typeType = TypeType.Dict;
            else if (Instance.ClassInfoDict.ContainsKey(fullName))
                typeType = TypeType.Class;
            else if (Instance.EnumInfoDict.ContainsKey(fullName))
                typeType = TypeType.Enum;

            return typeType;
        }
        public static BaseTypeInfo GetTypeInfo(string fullName)
        {
            BaseTypeInfo baseTypeInfo = null;
            var typeDict = Instance.TypeInfoDict;
            if (typeDict.ContainsKey(fullName))
                baseTypeInfo = typeDict[fullName];

            return baseTypeInfo;
        }
        /// <summary>
        /// 1.name=全路径,直接识别类型
        /// 2.name=相对路径,组合当前调用空间名去识别类型,未必能识别到类型.
        /// 3.当2无法识别类型时,则只能使用全路径去识别类型.
        /// </summary>
        /// <param name="_namespace">当前调用位置的命名空间</param>
        /// <param name="name">全路径或者相对路径</param>
        /// <returns></returns>
        public static BaseTypeInfo GetBaseTypeInfo(string _namespace, string name)
        {
            BaseTypeInfo bt = GetTypeInfo(name);
            if (bt == null)
            {
                string combineName = Util.Combine(_namespace, name);
                bt = GetTypeInfo(combineName);
            }
            return bt;
        }
        public static HashSet<string> AnalyzeGroup(string group)
        {
            if (string.IsNullOrWhiteSpace(group)) return new HashSet<string>() { Values.DefualtGroup };
            string[] groups = group.Split(Values.ItemSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (groups.Length == 0) return null;
            return new HashSet<string>(groups);
        }
    }

    public class BaseTypeInfo
    {
        /// <summary>
        /// 类对应文件的相对路径
        /// </summary>
        public virtual string XmlDirPath { get; protected set; }

        public TypeType EType = TypeType.Base;
        public string NamespaceName { get; private set; }
        public string Name { get; private set; }

        public BaseTypeInfo(string namespace0, string name)
        {
            NamespaceName = namespace0;
            Name = name;
        }
        public virtual void Init() { }
        /// <summary>
        /// 相对根的目录的命名空间,不包含根节点
        /// </summary>
        public string GetFullName()
        {
            if (string.IsNullOrWhiteSpace(NamespaceName))
                return Name;
            else
                return string.Format("{0}.{1}", NamespaceName, Name);
        }
    }

    /// <summary>
    /// 类描述
    /// </summary>
    public class ClassTypeInfo : BaseTypeInfo
    {
        public enum InhertState
        {
            NonPolyClass,//-非多态类型
            PolyParent,//-多态类型:基类
            PolyChild,//多态类型:子类
        }

        public string Index { get { return _des.Index; } }
        /// <summary>
        /// Excel或者Xml等全路径
        /// </summary>
        public string DataPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_des.DataPath))
                    return null;
                return string.Format("{0}\\{1}", XmlDirPath, Util.NormalizePath(_des.DataPath));
            }
        }
        public string Group { get; private set; }
        /// <summary>
        /// 类的继承状态信息
        /// </summary>
        public InhertState InhertType { get; private set; }
        /// <summary>
        /// 是多态类型,子类或者父类
        /// </summary>
        public bool IsPolyClass { get { return InhertType != InhertState.NonPolyClass; } }
        public List<ConstInfo> Consts { get; private set; }
        public List<FieldInfo> Fields { get; private set; }
        public Dictionary<string, ConstInfo> ConstDict { get; private set; }
        public Dictionary<string, FieldInfo> FieldDict { get; private set; }
        public FieldInfo IndexField { get; private set; }
        public ClassTypeInfo Inherit { get; private set; }
        public HashSet<string> GroupHashSet { get; private set; }

        //继承功能:父类查找,优先查找当前命名空间;其次直接当做全路径类型查找
        //一般子类与父类在同一命名空间,否则需要填写全路径名
        private Dictionary<string, ClassTypeInfo> _allClassDict;

        private ClassDes _des;
        public ClassTypeInfo(string xmlDir, string namespace0, ClassDes des) : base(namespace0, des.Name)
        {
            EType = TypeType.Class;
            _des = des;
            XmlDirPath = Util.NormalizePath(xmlDir);
            string inherit = _des.Inherit;

            Consts = new List<ConstInfo>();
            for (int i = 0; i < des.Consts.Count; i++)
            {
                var info = new ConstInfo(this, des.Consts[i]);
                Consts.Add(info);
            }
            UpdateConstDict();
            Fields = new List<FieldInfo>();
            for (int i = 0; i < des.Fields.Count; i++)
            {
                var info = new FieldInfo(this, des.Fields[i]);
                Fields.Add(info);
            }
            UpdateFieldDict();
            GroupHashSet = TypeInfo.AnalyzeGroup(Group);

            _allClassDict = new Dictionary<string, ClassTypeInfo>();
            if (!string.IsNullOrWhiteSpace(inherit))
            {
                string localSpace = Util.Combine(NamespaceName, inherit);
                Inherit = TypeInfo.GetTypeInfo(localSpace) as ClassTypeInfo;
                if (Inherit == null)
                    Inherit = TypeInfo.GetTypeInfo(inherit) as ClassTypeInfo;
                if (Inherit == null)
                {
                    Util.LogErrorFormat("类型{0}的父类{1}未定义!\t{2}", GetFullName(), inherit, XmlDirPath);
                    return;
                }
                string fullName = GetFullName();
                if (!Inherit._allClassDict.ContainsKey(fullName))
                    Inherit._allClassDict.Add(fullName, this);
            }
        }
        public override void Init()
        {
            if (string.IsNullOrWhiteSpace(_des.Group))
                Group = Values.DefualtGroup;
            else
                Group = string.Format("{0}|{1}", _des.Group, Values.DefualtGroup);
            if (Index != null)
                if (!FieldDict.ContainsKey(Index))
                    Util.LogErrorFormat("{0}类中不包含字段{1}!\t{2}", GetFullName(), Index, XmlDirPath);
                else
                    IndexField = FieldDict[Index];

            for (int i = 0; i < Consts.Count; i++)
                Consts[i].Init();
            for (int i = 0; i < Fields.Count; i++)
                Fields[i].Init();

            if (_allClassDict.Count == 0 && Inherit == null)
                InhertType = InhertState.NonPolyClass;
            else if (_allClassDict.Count > 0 && Inherit == null)
                InhertType = InhertState.PolyParent;
            else if (Inherit != null)
                InhertType = InhertState.PolyChild;
        }
        public void UpdateFieldDict()
        {
            if (FieldDict == null)
                FieldDict = new Dictionary<string, FieldInfo>();
            FieldDict.Clear();

            for (int i = 0; i < Fields.Count; i++)
            {
                var info = Fields[i];
                if (FieldDict.ContainsKey(info.Name))
                    Util.LogErrorFormat("变量{0}.{1}定义重复", GetFullName(), info.Name);
                else
                    FieldDict.Add(info.Name, info);
            }
        }
        public void UpdateConstDict()
        {
            if (ConstDict == null)
                ConstDict = new Dictionary<string, ConstInfo>();
            ConstDict.Clear();

            for (int i = 0; i < Consts.Count; i++)
            {
                var info = Consts[i];
                if (ConstDict.ContainsKey(info.Name))
                    Util.LogErrorFormat("常量{0}.{1}定义重复", GetFullName(), info.Name);
                else
                    ConstDict.Add(info.Name, info);
            }
        }

        public ClassTypeInfo GetRootClassInfo()
        {
            if (!IsPolyClass || InhertType == InhertState.PolyParent) return this;
            return Inherit.GetRootClassInfo();
        }
        public ClassTypeInfo GetSubClass(string fullName)
        {
            if (GetFullName().Equals(fullName))
                return this;
            if (_allClassDict.ContainsKey(fullName))
                return _allClassDict[fullName];
            else
                return null;
        }
        public IEnumerator GetSubClassEnumerator()
        {
            return _allClassDict.GetEnumerator();
        }
        public bool IsTheSame(ClassTypeInfo other)
        {
            return GetFullName().Equals(other.GetFullName());
        }
    }
    public class EnumTypeInfo : BaseTypeInfo
    {
        public string Group { get; private set; }
        public List<ConstInfo> Enums { get; private set; }
        public Dictionary<string, ConstInfo> EnumDict { get; private set; }
        public Dictionary<string, ConstInfo> AliasDict { get; private set; }
        public HashSet<string> GroupHashSet { get; private set; }
        public string this[string key]
        {
            get
            {
                //别名 - 自定名称
                if (AliasDict.ContainsKey(key))
                    return AliasDict[key].Value;
                //字符串 - 枚举字符串
                else if (EnumDict.ContainsKey(key))
                    return EnumDict[key].Value;

                int value = int.MinValue;
                //数值 - 枚举值
                if (int.TryParse(key, out value))
                    return key;
                else
                    return null;
            }
        }


        private EnumDes _des;
        public EnumTypeInfo(string xmlDir, string namespace0, EnumDes des) : base(namespace0, des.Name)
        {
            EType = TypeType.Enum;
            _des = des;
            XmlDirPath = xmlDir;

            Enums = new List<ConstInfo>();
            AliasDict = new Dictionary<string, ConstInfo>();
            for (int i = 0; i < des.Enums.Count; i++)
            {
                var info = new ConstInfo(this, des.Enums[i]);
                info.Type = TypeInfo.INT;
                Enums.Add(info);

                if (string.IsNullOrWhiteSpace(info.Alias)) continue;

                if (!AliasDict.ContainsKey(info.Alias))
                    AliasDict.Add(info.Alias, info);
                else
                    Util.LogErrorFormat("{0} 枚举{1}重名!", GetFullName(), info.Alias);
            }

            UpdateEnumDict();
            GroupHashSet = TypeInfo.AnalyzeGroup(Group);
        }
        public override void Init()
        {
            if (string.IsNullOrWhiteSpace(_des.Group))
                Group = Values.DefualtGroup;
            else
                Group = string.Format("{0}|{1}", _des.Group, Values.DefualtGroup);
            for (int i = 0; i < Enums.Count; i++)
                Enums[i].Init();
        }
        public void UpdateEnumDict()
        {
            if (EnumDict == null)
                EnumDict = new Dictionary<string, ConstInfo>();
            EnumDict.Clear();

            for (int i = 0; i < Enums.Count; i++)
            {
                var info = Enums[i];

                if (EnumDict.ContainsKey(info.Name))
                    Util.LogErrorFormat("枚举{0}.{1}定义重复", GetFullName(), info.Name);
                else
                    EnumDict.Add(info.Name, info);
            }
        }
    }


    public class ListTypeInfo : BaseTypeInfo
    {
        public string ItemType { get; private set; }
        public FieldInfo ItemInfo { get; private set; }
        public BaseTypeInfo Parent { get; private set; }
        public ListTypeInfo(BaseTypeInfo parent, string listType)
            : base("", listType)
        {
            EType = TypeType.List;
            string[] nodes = listType.Split(Values.ArgsSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (nodes.Length != 2)
                Util.LogErrorFormat("{0} list类型格式不正确!", GetFullName());

            Parent = parent;
            ItemType = nodes[1];
            ItemInfo = new FieldInfo(parent, Values.ELEMENT, ItemType, null);
        }
        public override void Init()
        {
            ItemInfo.Init();
        }
    }
    public class DictTypeInfo : BaseTypeInfo
    {
        public string KeyType { get; private set; }
        public string ValueType { get; private set; }
        public FieldInfo KeyInfo { get; private set; }
        public FieldInfo ValueInfo { get; private set; }
        public BaseTypeInfo Parent { get; private set; }

        private HashSet<string> _dictKeyLimit = new HashSet<string>() { TypeInfo.INT, TypeInfo.LONG, TypeInfo.STRING };

        public DictTypeInfo(BaseTypeInfo parent, string dictType)
                : base("", dictType)
        {
            EType = TypeType.Dict;
            string[] nodes = dictType.Split(Values.ArgsSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (nodes.Length != 3)
                Util.LogErrorFormat("{0} dict类型格式不正确!", GetFullName());

            Parent = parent;
            KeyType = nodes[1];
            ValueType = nodes[2];
            if (!_dictKeyLimit.Contains(KeyType) && EType != TypeType.Enum)
                Util.LogErrorFormat("{0} key不能为{1}类型!", GetFullName(), KeyType);
            KeyInfo = new FieldInfo(parent, Values.KEY, KeyType, null);
            ValueInfo = new FieldInfo(parent, Values.VALUE, ValueType, null);


        }
        public override void Init()
        {
            KeyInfo.Init();
            ValueInfo.Init();
        }
    }

    public class FieldInfo
    {
        public virtual string Name { get { return _des.Name; } }
        public virtual string Type { get { return _des.Type; } set { _des.Type = value; } }//直接配置全路径
        public virtual string Des { get { return _des.Des; } }
        public virtual string Check { get { return _des.Check; } }
        public virtual string Group { get; private set; }
        //public virtual string Split { get { return _des.Split; } }

        public virtual BaseTypeInfo Parent { get; private set; }
        public virtual BaseTypeInfo BaseInfo { get; private set; }
        public Dictionary<CheckRuleType, List<string>> RuleDict { get; private set; }
        public HashSet<string> GroupHashSet { get; private set; }

        private FieldDes _des;

        public FieldInfo(BaseTypeInfo parent, string name, string type, string check)
        {
            _des = new FieldDes()
            {
                Name = name,
                Type = type,
                Check = check,
            };
            Parent = parent;
            RuleDict = new Dictionary<CheckRuleType, List<string>>();
            GroupHashSet = TypeInfo.AnalyzeGroup(Group);
            AnalyzeCheckRule();
        }
        public FieldInfo(BaseTypeInfo parent, FieldDes des)
        {
            _des = des;
            Parent = parent;
            RuleDict = new Dictionary<CheckRuleType, List<string>>();
            GroupHashSet = TypeInfo.AnalyzeGroup(Group);
            AnalyzeCheckRule();
        }
        public void Init()
        {
            if (string.IsNullOrWhiteSpace(_des.Group))
                Group = Values.DefualtGroup;
            else
                Group = string.Format("{0}|{1}", _des.Group, Values.DefualtGroup);

            //--基础类型或者集合类型
            string type = Type;
            BaseInfo = TypeInfo.GetTypeInfo(type);
            if (BaseInfo == null)
            {
                //--创建集合类型
                if (Type.StartsWith("list:"))
                {
                    BaseInfo = new ListTypeInfo(Parent, Type);
                    BaseInfo.Init();
                    TypeInfo.Instance.Add(BaseInfo);
                }
                else if (Type.StartsWith("dict:"))
                {
                    BaseInfo = new DictTypeInfo(Parent, Type);
                    BaseInfo.Init();
                    TypeInfo.Instance.Add(BaseInfo);
                }
            }
            if (BaseInfo == null)
            {
                //--Class或者Enum
                type = Util.Combine(Parent.NamespaceName, Type);
                BaseInfo = TypeInfo.GetTypeInfo(type);
                if (BaseInfo == null)
                    BaseInfo = TypeInfo.GetTypeInfo(Type);
                else
                    _des.Type = type;
            }

            if (BaseInfo == null)
                Util.LogErrorFormat("{0}.{1} {2}类型无法解析!", Parent.GetFullName(), Name, Type);
        }
        public void AddCheckRule(string rule)
        {
            if (GroupHashSet != null && GroupHashSet.Contains(rule)) return;
            _des.Check += "|" + rule;
            AnalyzeCheckRule();
        }
        private void AnalyzeCheckRule()
        {
            if (Check == null) return;
            string[] checks = Check.Split(Values.ItemSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (checks.Length == 0) return;

            string refFlag = "ref";
            string[] rangeFlags = { "[", "]", "(", ")" };
            string noEmptyFlag = "noEmpty";
            string uniqueFlag = "unique";
            //string[] relOpFlags = { "<", ">", "<=", ">=", "==" };
            string fileExistFlags = "file";

            foreach (var check in checks)
            {
                CheckRuleType ruleType = CheckRuleType.None;
                List<string> ruleArgs = new List<string>();
                bool isNullOrWhiteSpace = string.IsNullOrWhiteSpace(check);
                if (isNullOrWhiteSpace) continue;

                if (check.StartsWith(refFlag))
                {
                    ruleType = CheckRuleType.Ref;
                    ruleArgs.AddRange(check.Replace(refFlag, "").Split(Values.ArgsSplitFlag.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries));
                }
                else if (check.StartsWith(noEmptyFlag))
                {
                    ruleType = CheckRuleType.NoEmpty;
                    //ruleArgs.AddRange(check.Replace(noEmptyFlag, "").Split(Values.ArgsSplitFlag.ToCharArray(),
                    //    StringSplitOptions.RemoveEmptyEntries));
                }
                else if (check.StartsWith(uniqueFlag))
                {
                    ruleType = CheckRuleType.Unique;
                    //ruleArgs.AddRange(check.Replace(uniqueFlag, "").Split(Values.ArgsSplitFlag.ToCharArray(),
                    //    StringSplitOptions.RemoveEmptyEntries));
                }
                else if (check.StartsWith(fileExistFlags))
                {
                    ruleType = CheckRuleType.FileExist;
                    ruleArgs.AddRange(check.Replace(fileExistFlags, "").Split(Values.ArgsSplitFlag.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries));
                }
                else
                {
                    for (int j = 0; j < rangeFlags.Length; j++)
                    {
                        if (check.StartsWith(rangeFlags[j]) || check.EndsWith(rangeFlags[j]))
                        {
                            ruleType = CheckRuleType.Range;
                            ruleArgs.AddRange(check.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                            break;
                        }
                    }
                    //for (int j = 0; j < relOpFlags.Length; j++)
                    //{
                    //    if (check.StartsWith(relOpFlags[j]) || check.EndsWith(relOpFlags[j]))
                    //    {
                    //        ruleType = CheckRuleType.RelationalOp;
                    //        ruleArgs.Add(check.Replace(relOpFlags[j], ""));
                    //        break;
                    //    }
                    //}
                }

                if (!RuleDict.ContainsKey(ruleType))
                    RuleDict.Add(ruleType, ruleArgs);

                if (!isNullOrWhiteSpace && ruleType == CheckRuleType.None)
                    Util.LogWarningFormat("{0}.{1} 异常:检查规则{2}不存在", Parent.GetFullName(), Name, check);

            }
        }

        public void Set(string name, string type, string check, string group)
        {

        }
    }
    public class ConstInfo : FieldInfo
    {
        public string Value { get { return _des.Value; } }
        public string Alias { get { return _des.Alias; } }

        ConstDes _des;
        public ConstInfo(BaseTypeInfo parent, ConstDes des) : base(parent, des)
        {
            _des = des;
        }


    }
}
