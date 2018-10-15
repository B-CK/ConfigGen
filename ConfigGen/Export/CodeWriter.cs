using System;
using System.Text;
using System.Collections.Generic;
using ConfigGen.LocalInfo;

namespace ConfigGen.Export
{
    class CodeWriter
    {
        public const string Public = "public";
        public const string Private = "private";
        public const string Static = "static";
        public const string Const = "const";
        public const string Readonly = "readonly";
        public const string Abstract = "abstract";
        public const string Override = "override";
        public const string Sealed = "sealed";

        private static int _level = 0;
        public static void Reset() { _level = 0; }
        public static void EndLevel() { _level--; }
        public static void AddLevel() { _level++; }
        public static void IntervalLevel(StringBuilder builder)
        {
            for (int i = 0; i < _level; i++)
                builder.Append("\t");
        }
        public static void UsingNamespace(StringBuilder builder, List<string> vs)
        {
            for (int i = 0; i < vs.Count; i++)
                builder.AppendFormat("using {0};\n", vs[i]);
        }
        public static void NameSpace(StringBuilder builder, string name)
        {
            builder.AppendFormat("namespace {0}", name);
            Start(builder);
        }
        public static void ClassBase(StringBuilder builder, string modifier, string className)
        {
            ClassChild(builder, modifier, "", className, null);
        }
        public static void ClassBase(StringBuilder builder, string modifier, string identification, string className)
        {
            ClassChild(builder, modifier, identification, className, null);
        }
        public static void ClassChild(StringBuilder builder, string modifier, string className, string inhert)
        {
            ClassChild(builder, modifier, "", className, inhert);
        }
        public static void ClassChild(StringBuilder builder, string modifier, string identification, string className, string inhert)
        {
            IntervalLevel(builder);
            if (string.IsNullOrWhiteSpace(inhert))
                builder.AppendFormat("{0} {1} class {2}", modifier, identification, className);
            else
                builder.AppendFormat("{0} {1} class {2} : {3}", modifier, identification, className, inhert);
            Start(builder);
        }
        public static void Field(StringBuilder builder, string modifier, string type, string fieldName)
        {
            FieldInit(builder, modifier, "", type, fieldName, null);
        }
        public static void Field(StringBuilder builder, string modifier, string identification, string type, string fieldName)
        {
            FieldInit(builder, modifier, identification, type, fieldName, null);
        }
        public static void FieldInit(StringBuilder builder, string modifier, string type, string fieldName, string initValue)
        {
            FieldInit(builder, modifier, "", type, fieldName, initValue);
        }
        public static void FieldInit(StringBuilder builder, string modifier, string identification, string type, string fieldName, string initValue)
        {
            IntervalLevel(builder);
            modifier = string.IsNullOrWhiteSpace(modifier) ? "" : modifier;
            identification = string.IsNullOrWhiteSpace(identification) ? "" : " " + identification;
            if (string.IsNullOrWhiteSpace(initValue))
                builder.AppendFormat("{0}{1} {2} {3};\n", modifier, identification, type, fieldName);
            else
                builder.AppendFormat("{0}{1} {2} {3} = {4};\n", modifier, identification, type, fieldName, initValue);
        }
        public static void Constructor(StringBuilder builder, string modifier, string funcName, string[] types = null, string[] args = null, string[] baseArgs = null)
        {
            StringBuilder strbuilder = new StringBuilder();
            IntervalLevel(builder);
            if (types == null || args == null || types.Length == 0 || args.Length == 0)
                builder.AppendFormat("{0} {1}()", modifier, funcName);
            else
            {
                if (types.Length != args.Length)
                    throw new Exception("类型长度与参数名长度不一致");

                for (int i = 0; i < types.Length; i++)
                {
                    if (i == 0)
                        strbuilder.AppendFormat("{0} {1}", types[i], args[i]);
                    else
                        strbuilder.AppendFormat(", {0} {1}", types[i], args[i]);
                }
                builder.AppendFormat("{0} {1}({2})", modifier, funcName, strbuilder.ToString());
            }

            if (baseArgs != null)
            {
                strbuilder.Clear();
                for (int i = 0; baseArgs != null && i < baseArgs.Length; i++)
                {
                    if (i == 0)
                        strbuilder.Append(baseArgs[i]);
                    else
                        strbuilder.AppendFormat(", {0}", baseArgs[i]);
                }
                builder.AppendFormat(" : base({0})", strbuilder.ToString());
            }
            Start(builder);
        }
        public static void Function(StringBuilder builder, string modifier, string returnType, string funcName, string[] types = null, string[] args = null)
        {
            Function(builder, modifier, "", returnType, funcName, types, args);
        }
        public static void Function(StringBuilder builder, string modifier, string identification, string returnType, string funcName, string[] types = null, string[] args = null, string whereT = null)
        {
            StringBuilder strbuilder = new StringBuilder();
            IntervalLevel(builder);
            string s1 = string.IsNullOrWhiteSpace(modifier) ? "" : modifier;
            string s2 = string.IsNullOrWhiteSpace(identification) ? "" : " " + identification;

            if (types != null && args != null)
            {
                if (types.Length != args.Length)
                    throw new Exception("类型长度与参数名长度不一致");

                for (int i = 0; i < types.Length; i++)
                {
                    if (i == 0)
                        strbuilder.AppendFormat("{0} {1}", types[i], args[i]);
                    else
                        strbuilder.AppendFormat(", {0} {1}", types[i], args[i]);
                }
            }
            if (string.IsNullOrWhiteSpace(whereT))
                builder.AppendFormat("{0}{1} {2} {3}({4})", s1, s2, returnType, funcName, strbuilder.ToString());
            else
                builder.AppendFormat("{0}{1} {2} {3}({4}) where T :{5}", s1, s2, returnType, funcName, strbuilder.ToString(), whereT);

            Start(builder);
        }

        public static void Enum(StringBuilder builder, string modifier, string enumName)
        {
            IntervalLevel(builder);
            builder.AppendFormat("{0} enum {1}", CodeWriter.Public, enumName);
            Start(builder);
        }
        public static void EnumField(StringBuilder builder, string key, string value = null)
        {
            IntervalLevel(builder);
            if (string.IsNullOrWhiteSpace(value))
                builder.AppendFormat("{0},\n", key);
            else
                builder.AppendFormat("{0} = {1},\n", key, value);
        }
        public static void Start(StringBuilder builder)
        {
            builder.AppendLine();
            IntervalLevel(builder);
            builder.AppendLine("{");
            _level++;
        }
        public static void End(StringBuilder builder)
        {
            _level--;
            IntervalLevel(builder);
            builder.AppendLine("}");
        }
        public static void EndAll(StringBuilder builder)
        {
            for (int i = 0; i <= _level + 1; i++)
                End(builder);
        }
        public static void Comments(StringBuilder builder, string comments)
        {
            IntervalLevel(builder);
            builder.AppendLine("/// <summary>");
            IntervalLevel(builder);
            builder.AppendFormat("/// {0}\n", comments);
            IntervalLevel(builder);
            builder.AppendLine("/// <summary>");
        }
        public static string GetFullNamespace(string root, string relType)
        {
            string type = relType;
            LocalInfo.TypeType typeType = LocalInfo.TypeInfo.GetTypeType(relType);
            if (typeType == LocalInfo.TypeType.Enum || typeType == LocalInfo.TypeType.Class)
                type = string.Format("{0}.{1}", root, relType);
            return type;
        }
    }
}
