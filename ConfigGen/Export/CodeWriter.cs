using System;
using System.Text;
using System.Collections.Generic;
using Description.Xml;

namespace Description.Export
{
    public class CodeWriter
    {
        public const string Public = "public";
        public const string Private = "private";
        public const string Static = "static";
        public const string Readonly = "readonly";
        public const string Abstract = "abstract";
        public const string Override = "override";
        public const string Const = "const";
        public const string Sealed = "sealed";

        private StringBuilder _builder;
        public CodeWriter()
        {
            _builder = new StringBuilder();
        }
        public void Clear()
        {
            _builder.Clear();
        }
        public override string ToString()
        {
            return _builder.ToString();
        }

        private int _level = 0;
        public void SetLevel(int level) { _level = level; }
        public void EndLevel() { _level--; }
        public void AddLevel() { _level++; }
        public void IntervalLevel()
        {
            for (int i = 0; i < _level; i++)
                _builder.Append("\t");
        }
        public void UsingNamespace(List<string> vs)
        {
            for (int i = 0; i < vs.Count; i++)
                _builder.AppendFormat("using {0};\n", vs[i]);
        }
        public void NameSpace(string name)
        {
            _builder.AppendFormat("namespace {0}", name);
            Start();
        }
        public void Comments(string comments)
        {
            IntervalLevel();
            _builder.AppendLine("/// <summary>");
            IntervalLevel();
            _builder.AppendFormat("/// {0}\n", comments);
            IntervalLevel();
            _builder.AppendLine("/// <summary>");
        }
        public void DefineClass(string modifier, string className, string inhert = null)
        {
            IntervalLevel();
            inhert = inhert.IsEmpty() ? "" : " : " + inhert;
            _builder.AppendFormat("{0} class {1}{2}", modifier, className, inhert);
            Start();
        }
        public void DefineConst(string type, string name, string value)
        {
            IntervalLevel();
            _builder.AppendFormat("public  readonly {0} {1} = {2};\n", type, name, value);
        }
        //public void DefineField(string type, string fieldName, string value = null)
        //{
        //    DefineField(null, type, fieldName, value);
        //}
        public void DefineField(string modifier, string type, string fieldName, string value = null)
        {
            IntervalLevel();
            modifier = modifier.IsEmpty() ? "" : modifier + " ";
            value = value.IsEmpty() ? "" : " = " + value;
            _builder.AppendFormat("{0}{1} {2}{3};\n", modifier, type, fieldName, value);
        }
        public void Constructor(string modifier, string funcName, string[] types = null, string[] args = null, string[] baseArgs = null)
        {
            StringBuilder strbuilder = new StringBuilder();
            IntervalLevel();
            if (types == null || args == null || types.Length == 0 || args.Length == 0)
                _builder.AppendFormat("{0} {1}()", modifier, funcName);
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
                _builder.AppendFormat("{0} {1}({2})", modifier, funcName, strbuilder.ToString());
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
                _builder.AppendFormat(" : base({0})", strbuilder.ToString());
            }
            Start();
        }
        public void SetField(string fieldName, string value)
        {
            IntervalLevel();
            _builder.AppendFormat("{0} = {1};\n", fieldName, value);
        }
        public void Enum(string modifier, string enumName)
        {
            IntervalLevel();
            _builder.AppendFormat("{0} enum {1}", CodeWriter.Public, enumName);
            Start();
        }
        public void DefineEnum(string key, string value = null)
        {
            IntervalLevel();
            if (string.IsNullOrWhiteSpace(value))
                _builder.AppendFormat("{0},\n", key);
            else
                _builder.AppendFormat("{0} = {1},\n", key, value);
        }
        public void Start()
        {
            _builder.AppendLine();
            IntervalLevel();
            _builder.AppendLine("{");
            _level++;
        }
        public void End(bool isStatement = false)
        {
            _level--;
            IntervalLevel();
            if (isStatement)
                _builder.AppendLine("};");
            else
                _builder.AppendLine("}");
        }
        public void Function(string modifier, string returnType, string funcName, string[] types = null, string[] args = null, string whereT = null)
        {
            StringBuilder strbuilder = new StringBuilder();
            IntervalLevel();
            modifier = modifier.IsEmpty() ? "" : modifier + " ";

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
            if (whereT.IsEmpty())
                _builder.AppendFormat("{0}{1} {2}({3})", modifier, returnType, funcName, strbuilder.ToString());
            else
                _builder.AppendFormat("{0}{1} {2}({3}) where T :{4}", modifier, returnType, funcName, strbuilder.ToString(), whereT);

            Start();
        }
        /// <summary>
        /// 脚本语言使用,例lua
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        public void Function(string modifier, string funcName, params string[] args)
        {
            StringBuilder strbuilder = new StringBuilder();
            IntervalLevel();
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (i == 0)
                        strbuilder.Append(args[i]);
                    else
                        strbuilder.Append("," + args[i]);
                }
            }
            _builder.AppendFormat("{0} {1}({2})", modifier, funcName, strbuilder);

            _builder.AppendLine();
            IntervalLevel();
            _level++;
        }
        public void EndAll()
        {
            for (int i = 0; i <= _level + 1; i++)
                End();
        }


        public void Append(string msg, bool hasLevel = true)
        {
            if (hasLevel)
                IntervalLevel();
            _builder.Append(msg);
        }
        public void AppendLine(bool hasLevel = true) { AppendLine("", hasLevel); }
        public void AppendLine(string msg, bool hasLevel = true)
        {
            if (hasLevel)
                IntervalLevel();
            _builder.AppendLine(msg);
        }
        public void AppendFormat(string fmt, params object[] args)
        {
            IntervalLevel();
            _builder.AppendFormat(fmt, args);
        }
    }
}
