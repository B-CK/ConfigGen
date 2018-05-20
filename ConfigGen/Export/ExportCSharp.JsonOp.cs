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
        private const string ROOT_SPACE = "Lson";


        /// <summary>
        /// 编辑模式下的Json读写操作
        /// </summary>
        public static void Export_JsonOp()
        {
            StringBuilder builder = new StringBuilder();
            List<string> NameSpaces = new List<string>() { "System", "System.Collections.Generic" };

            List<BaseTypeInfo> typeInfos = new List<BaseTypeInfo>();
            typeInfos.AddRange(LocalInfoManager.Instance.TypeInfoLib.ClassInfos);
            typeInfos.AddRange(LocalInfoManager.Instance.TypeInfoLib.EnumInfos);
            foreach (BaseTypeInfo baseType in typeInfos)
            {
                CodeWriter.UsingNamespace(builder, NameSpaces);
                builder.AppendLine();

                if (baseType.TypeType == TypeType.Class)
                {

                }
                else if (baseType.TypeType == TypeType.Enum)
                {

                    EnumTypeInfo enumType = baseType as EnumTypeInfo;
                    foreach (EnumKeyValue item in enumType.KeyValuePair)
                    {

                    }
                }

                string relDir = Path.GetDirectoryName(baseType.RelPath);
                string path = Path.Combine(Values.ExportCSharp, relDir, baseType.Name + ".cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
        private static void CreateJsonFormat(StringBuilder builder, ClassTypeInfo classType)
        {
            switch (classType.TypeType)
            {
                case TypeType.Base:

                    break;
                case TypeType.Class:

                    break;
                case TypeType.Enum:

                    break;
                case TypeType.List:

                    break;
                case TypeType.Dict:

                    break;
                case TypeType.None:
                default:
                    break;
            }
        }
        private static void Json_Class(StringBuilder builder, ClassTypeInfo classType)
        {

        }
        private static void Json_List(StringBuilder builder, ClassTypeInfo classType)
        {

        }
        private static void Json_Dict(StringBuilder builder, ClassTypeInfo classType)
        {

        }

        /// <summary>
        /// Json数据管理类
        /// </summary>
        private static void DefineLsonManager()
        {
            StringBuilder builder = new StringBuilder();
            List<string> NameSpaces = new List<string>() { "System", "System.IO", "Newtonsoft.Json", "System.Collections.Generic" };

            const string lson = "lson";
            CodeWriter.UsingNamespace(builder, NameSpaces);
            builder.AppendLine();
            CodeWriter.NameSpace(builder, ROOT_SPACE);
            CodeWriter.ClassBase(builder, CodeWriter.Public, CodeWriter.Abstract, LSON_STREAM);
            List<ClassTypeInfo> classes = LocalInfoManager.Instance.TypeInfoLib.ClassInfos;
            foreach (var info in classes)
            {
                if (info.TypeType == TypeType.Class && !string.IsNullOrWhiteSpace(info.DataTable))
                {
                    string setType = string.Format("List<{0}>", info.GetClassName());
                    string initValue = string.Format("new List<{0}>()", info.GetClassName());
                    CodeWriter.FieldInit(builder, CodeWriter.Public, CodeWriter.Static, setType, info.Name, initValue);
                }
            }
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, Base.Void, "Deserialize<T>", new string[] { "T" }, new string[] { lson });
            CodeWriter.IntervalLevel(builder);

            CodeWriter.End(builder);

            CodeWriter.EndAll(builder);
            string path = Path.Combine(Values.ExportCsJson, LSON_STREAM + ".cs");
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
