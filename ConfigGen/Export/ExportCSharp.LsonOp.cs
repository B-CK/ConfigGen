using System;
using System.Text;
using Newtonsoft.Json;
using ConfigGen.LocalInfo;
using System.Collections.Generic;
using System.IO;

namespace ConfigGen.Export
{
    public partial class ExportCSharp
    {
        private const string LSON_STREAM = "LsonManager";
        private const string CLASS_LSON_OBJECT = "LsonObject";

        /// <summary>
        /// 编辑模式下的Json读写操作
        /// </summary>
        public static void Export_LsonOp()
        {
            StringBuilder builder = new StringBuilder();
            List<string> NameSpaces = new List<string>() { "System", "System.Collections.Generic" };

            //构建Lson存储类基础类LsonObject.cs
            string path = Path.Combine(Values.ExportCsLson, CLASS_LSON_OBJECT + ".cs");
            CodeWriter.UsingNamespace(builder, NameSpaces);
            CodeWriter.NameSpace(builder, Values.LsonRootNode);
            CodeWriter.ClassBase(builder, CodeWriter.Public, CodeWriter.Abstract, CLASS_LSON_OBJECT);
            CodeWriter.EndAll(builder);
            Util.SaveFile(path, builder.ToString());
            builder.Clear();

            List<BaseTypeInfo> typeInfos = new List<BaseTypeInfo>();
            typeInfos.AddRange(LocalInfoManager.Instance.TypeInfoLib.ClassInfos);
            typeInfos.AddRange(LocalInfoManager.Instance.TypeInfoLib.EnumInfos);
            foreach (BaseTypeInfo baseType in typeInfos)
            {
                CodeWriter.UsingNamespace(builder, NameSpaces);
                builder.AppendLine();
                CodeWriter.NameSpace(builder, string.Format("{0}.{1}", Values.LsonRootNode, baseType.NamespaceName));

                bool isWrited = false;
                if (baseType.TypeType == TypeType.Class)
                {
                    ClassTypeInfo classType = baseType as ClassTypeInfo;
                    bool isEmpty = string.IsNullOrWhiteSpace(classType.Inherit);
                    if (isEmpty)
                        CodeWriter.ClassChild(builder, CodeWriter.Public, classType.Name, CLASS_LSON_OBJECT);
                    else
                    {
                        string fullType = CodeWriter.GetFullNamespace(Values.LsonRootNode, classType.Inherit);
                        CodeWriter.ClassChild(builder, CodeWriter.Public, classType.Name, fullType);
                    }

                    isWrited = true;
                    for (int j = 0; j < classType.Fields.Count; j++)
                    {
                        FieldInfo field = classType.Fields[j];
                        switch (field.BaseInfo.TypeType)
                        {
                            case TypeType.Base:
                            case TypeType.Enum:
                                {
                                    CodeWriter.Comments(builder, field.Des);
                                    string fullType = CodeWriter.GetFullNamespace(Values.LsonRootNode, field.Type);
                                    CodeWriter.Field(builder, CodeWriter.Public, fullType, field.Name);
                                    break;
                                }
                            case TypeType.Class:
                                {
                                    CodeWriter.Comments(builder, field.Des);
                                    string fullType = CodeWriter.GetFullNamespace(Values.LsonRootNode, field.Type);
                                    CodeWriter.Field(builder, CodeWriter.Public, fullType, field.Name);
                                }
                                break;
                            case TypeType.List:
                                {
                                    string type = field.Type.Replace("list", "List");
                                    ListTypeInfo listType = field.BaseInfo as ListTypeInfo;
                                    string initValue = string.Format("new {0}()", type);
                                    CodeWriter.Comments(builder, field.Des);
                                    TypeType itemType = TypeInfo.GetTypeType(listType.ItemType);
                                    string fullType = CodeWriter.GetFullNamespace(Values.LsonRootNode, listType.ItemType);
                                    type = type.Replace(listType.ItemType, fullType);

                                    CodeWriter.Field(builder, CodeWriter.Public, type, field.Name);
                                    break;
                                }
                            case TypeType.Dict:
                                {
                                    string type = field.Type.Replace("dict", "Dictionary");
                                    DictTypeInfo dictType = field.BaseInfo as DictTypeInfo;
                                    string initValue = string.Format("new {0}()", type);
                                    CodeWriter.Comments(builder, field.Des);
                                    string fullType = CodeWriter.GetFullNamespace(Values.LsonRootNode, dictType.KeyType);
                                    type = type.Replace(dictType.KeyType, fullType);
                                    fullType = CodeWriter.GetFullNamespace(Values.LsonRootNode, dictType.ValueType);
                                    type = type.Replace(dictType.ValueType, fullType);

                                    CodeWriter.Field(builder, CodeWriter.Public, type, field.Name);
                                    break;
                                }
                            case TypeType.None:
                            default:
                                break;
                        }
                    }
                }
                else if (baseType.TypeType == TypeType.Enum)
                {
                    isWrited = true;
                    EnumTypeInfo enumType = baseType as EnumTypeInfo;
                    CodeWriter.Enum(builder, CodeWriter.Public, enumType.Name);
                    foreach (EnumKeyValue item in enumType.KeyValuePair)
                    {
                        CodeWriter.EnumField(builder, item.Key, item.Value);
                    }
                }

                CodeWriter.EndAll(builder);

                if (isWrited)
                {
                    string file = string.Format("{0}.{1}.cs", Values.LsonRootNode, baseType.GetClassName());
                    path = Path.Combine(Values.ExportCsLson, file);
                    Util.SaveFile(path, builder.ToString());
                }

                builder.Clear();
                CodeWriter.Reset();
            }

            DefineLsonManager();
        }

        /// <summary>
        /// Json数据管理类
        /// </summary>
        private static void DefineLsonManager()
        {
            StringBuilder builder = new StringBuilder();
            List<string> NameSpaces = new List<string>() { "System", "System.IO", "Newtonsoft.Json", "System.Collections.Generic" };

            CodeWriter.UsingNamespace(builder, NameSpaces);
            builder.AppendLine();
            CodeWriter.NameSpace(builder, Values.LsonRootNode);
            CodeWriter.ClassBase(builder, CodeWriter.Public, CodeWriter.Abstract, LSON_STREAM);
            List<ClassTypeInfo> classes = LocalInfoManager.Instance.TypeInfoLib.ClassInfos;
            foreach (var info in classes)
            {
                if (info.TypeType == TypeType.Class && !string.IsNullOrWhiteSpace(info.DataTable))
                {
                    string fullType = CodeWriter.GetFullNamespace(Values.LsonRootNode, info.GetClassName());
                    string setType = string.Format("List<{0}>", fullType);
                    string initValue = string.Format("new List<{0}>()", info.GetClassName());
                    CodeWriter.FieldInit(builder, CodeWriter.Public, CodeWriter.Static, setType, info.Name, initValue);
                }
            }
            builder.AppendLine();

            string[] types = new string[] { Base.String };
            string[] args = new string[] { "path" };
            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, "T", "Deserialize<T>", types, args);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("string value = File.ReadAllText({0});\n", args[0]);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("T data = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings");
            CodeWriter.Start(builder);
            CodeWriter.IntervalLevel(builder); builder.AppendFormat("TypeNameHandling = TypeNameHandling.Auto\n");
            CodeWriter.EndLevel(); CodeWriter.IntervalLevel(builder); builder.Append("});\n");
            CodeWriter.IntervalLevel(builder); builder.AppendFormat("return data;\n");
            CodeWriter.End(builder);

            types = new string[] { Base.String, Base.Object };
            args = new string[] { "path", "data" };
            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Void, "Serialize", types, args);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("string value = JsonConvert.SerializeObject({0}, Formatting.Indented, new JsonSerializerSettings", args[1]);
            CodeWriter.Start(builder);
            CodeWriter.IntervalLevel(builder); builder.AppendFormat("TypeNameHandling = TypeNameHandling.Auto\n");
            CodeWriter.EndLevel(); CodeWriter.IntervalLevel(builder); builder.Append("});\n");
            CodeWriter.IntervalLevel(builder); builder.AppendFormat("File.WriteAllText({0}, value);\n", args[0]);
            CodeWriter.End(builder);

            types = new string[] { Base.String };
            args = new string[] { "dirPath" };
            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, "List<T>", "Load<T>", types, args);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("List<T> list = new List<T>();\n");
            CodeWriter.IntervalLevel(builder); builder.AppendFormat("try");
            CodeWriter.Start(builder);
            CodeWriter.IntervalLevel(builder); builder.AppendFormat("string[] fs = Directory.GetFiles({0});\n", args[0]);
            CodeWriter.IntervalLevel(builder); builder.AppendFormat("foreach (var f in fs)\n");
            CodeWriter.Start(builder);
            CodeWriter.IntervalLevel(builder); builder.AppendFormat("list.Add(Deserialize<T>(f));\n");
            CodeWriter.End(builder);
            CodeWriter.End(builder);
            CodeWriter.IntervalLevel(builder); builder.AppendFormat("catch (System.Exception e)");
            CodeWriter.Start(builder);
            CodeWriter.IntervalLevel(builder); builder.AppendFormat("UnityEngine.Debug.LogErrorFormat(\"文件夹路径不存在{{0}}\\n{{1}}\", dirPath, e.StackTrace);\n");
            CodeWriter.End(builder);
            CodeWriter.IntervalLevel(builder); builder.AppendFormat("return list;\n");
            CodeWriter.End(builder);

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Void, "LoadAll", null, null);
            foreach (var info in classes)
            {
                if (info.TypeType == TypeType.Class && !string.IsNullOrWhiteSpace(info.DataTable))
                {
                    CodeWriter.IntervalLevel(builder);
                    string dirPath = string.Format(@"{0}{1}", Values.ConfigDir.Replace("\\", "/"), info.DataTable);
                    string fullType = CodeWriter.GetFullNamespace(Values.LsonRootNode, info.GetClassName());
                    builder.AppendFormat("{0} = Load<{1}>(\"{2}\");\n", info.Name, fullType, dirPath);
                }
            }
            CodeWriter.End(builder);

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Void, "Clear", null, null);
            foreach (var info in classes)
            {
                if (info.TypeType == TypeType.Class && !string.IsNullOrWhiteSpace(info.DataTable))
                {
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendFormat("{0}.Clear();\n", info.Name);
                }
            }
            CodeWriter.End(builder);

            CodeWriter.EndAll(builder);
            string path = Path.Combine(Values.ExportCsLson, LSON_STREAM + ".cs");
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }
    }

    //Unity 可视化属性记录
    //enum.value    -       直接定义成枚举,[ShowIf],[EnumToggleButtons]
    //func          -       可添加功能按钮[Button]
    //dict          -       [DictionaryDrawerSettings]

    //字段内容限制(检查规则)
    //int,long,float        -       范围限制
    //文件资源              -       路径,名称前缀等限制[AssetList]
    //读取文件路径          -       [FolderPath],
    //组件数据关联          -       [InlineEditor]
}
