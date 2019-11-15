using Xml;
using Wrap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Description;

namespace Export
{
    public class ExportLua
    {
        //--lua中的枚举默认   NULL = -1
        private const string DATA_STRUCT = "DataStruct";
        private const string DATA_STREAM = "DataStream";
        private const string DATA_CONFIG = "Config";
        private const string LUA_ENUM_NULL = "-9";
        private const string LOCAL = "local";


        public static void Export()
        {
            GenConfigTable();
            GenDataStream();
            GenDataStruct();
        }

        private static void End(CodeWriter builder)
        {
            builder.EndLevel();
            builder.AppendLine("end");
        }
        private static void GenConfigTable()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("return {");
            var cit = ConfigWrap.Configs.GetEnumerator();
            while (cit.MoveNext())
            {
                ConfigWrap cfg = cit.Current.Value;
                string method = string.Format("Get{0}", cfg.FullType.Replace(new string(Setting.DotSplit), ""));
                string index = cfg.Index.Name;
                string relPath = cfg.OutputFile + Setting.DataFileExt;
                builder.AppendFormat("\t{{ name = '{0}', method = '{1}', index = '{2}', output = '{3}' }},\n",
                    cfg.Name, method, index, relPath.ToLower());
            }
            builder.AppendLine("}");

            string path = Path.Combine(Setting.LuaDir, DATA_CONFIG + ".lua");
            Util.SaveFile(path, builder.ToString());
        }
        private static void GenDataStream()
        {
            CodeWriter builder = new CodeWriter();
            builder.DefineField(null, LOCAL, "tonumber", "tonumber");
            builder.DefineField(null, LOCAL, "gsub", "string.gsub");
            builder.DefineField(null, LOCAL, "lower", "string.lower");
            builder.DefineField(null, LOCAL, "error", "error");
            builder.DefineField(null, LOCAL, "setmetatable", "setmetatable");
            builder.DefineField(null, LOCAL, "tostring", "tostring");
            builder.DefineField(null, LOCAL, "lines", "io.lines");
            builder.AppendLine();

            builder.DefineField(null, LOCAL, "Stream", "{}");
            builder.SetField("Stream.__index", "Stream");
            builder.AppendLine();

            builder.Function("function", "Stream.new", new string[] { "file" });
            builder.DefineField(null, LOCAL, "o", "{}");
            builder.AppendLine("setmetatable(o, Stream)");
            builder.SetField("o.dataIter", "lines(file)");
            builder.AppendLine("return o");
            End(builder);
            builder.AppendLine();

            builder.Function("function", "Stream:Close");
            builder.AppendLine("while self.dataIter() do");
            builder.AddLevel();
            End(builder);
            End(builder);
            builder.AppendLine();

            builder.Function("function", "Stream:GetNext");
            builder.AppendLine("return self.dataIter()");
            End(builder);
            builder.AppendLine();

            builder.Function("function", "Stream:GetBool");
            builder.DefineField(null, LOCAL, "next", "self:GetNext()");
            builder.AppendLine("if next == \"true\" then");
            builder.AddLevel();
            builder.AppendLine("return true");
            builder.EndLevel();
            builder.AppendLine("elseif next == \"false\" then");
            builder.AddLevel();
            builder.AppendLine("return false");
            builder.EndLevel();
            builder.AppendLine("else");
            builder.AddLevel();
            builder.AppendLine("error(tostring(next) .. \" isn't bool! \")");
            End(builder);
            End(builder);
            builder.AppendLine();

            builder.Function("function", "Stream:GetInt");
            builder.DefineField(null, LOCAL, "next", "self:GetNext()");
            builder.AppendLine("return tonumber(next)");
            End(builder);
            builder.AppendLine();

            builder.Function("function", "Stream:GetLong");
            builder.DefineField(null, LOCAL, "next", "self:GetNext()");
            builder.AppendLine("return tonumber(next)");
            End(builder);
            builder.AppendLine();

            builder.Function("function", "Stream:GetFloat");
            builder.DefineField(null, LOCAL, "next", "self:GetNext()");
            builder.AppendLine("return tonumber(next)");
            End(builder);
            builder.AppendLine();

            builder.Function("function", "Stream:GetString");
            builder.DefineField(null, LOCAL, "next", "self:GetNext()");
            builder.AppendLine("return next");
            End(builder);
            builder.AppendLine();

            builder.Function("function", "Stream:GetList", new string[] { "type" });
            builder.DefineField(null, LOCAL, "result", "{}");
            builder.DefineField(null, LOCAL, "method", "self['Get' .. type]");
            builder.DefineField(null, LOCAL, "length", "self:GetInt()");
            builder.AppendLine("for i = 1, length do");
            builder.AddLevel();
            builder.SetField("result[i]", "method(self)");
            End(builder);
            builder.AppendLine("return result");
            End(builder);
            builder.AppendLine();

            builder.Function("function", "Stream:GetDict", new string[] { "key", "value" });
            builder.DefineField(null, LOCAL, "result", "{}");
            builder.DefineField(null, LOCAL, "optKey", "self['Get' .. key]");
            builder.DefineField(null, LOCAL, "optValue", "self['Get' .. value]");
            builder.DefineField(null, LOCAL, "length", "self:GetInt()");
            builder.AppendLine("for i = 1, length do");
            builder.AddLevel();
            builder.SetField("result[optKey(self)]", "optValue(self)");
            End(builder);
            builder.AppendLine("return result");
            End(builder);
            builder.AppendLine();

            builder.AppendLine("return Stream");

            string path = Path.Combine(Setting.LuaDir, DATA_STREAM + ".lua");
            Util.SaveFile(path, builder.ToString());
        }

        private static void GenDataStruct()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("local Stream = require(\"Cfg.DataStream\")");
            builder.AppendLine("local GetOrCreate = Util.GetOrCreate");
            builder.AppendLine();
            builder.AppendLine("local meta");

            var cit = ClassWrap.Classes.GetEnumerator();
            while (cit.MoveNext())
            {
                var cls = cit.Current.Value;
                builder.AppendLine("meta= {}");
                builder.AppendLine("meta.__index = meta");
                builder.AppendFormat("meta.class = '{0}'\n", cls.FullType);

                builder.AppendFormat("GetOrCreate('{0}')['{1}'] = meta\n", cls.Namespace, cls.Name);
                string funcName = cls.FullType.Replace(".", "");
                if (cls.IsDynamic())
                {
                    builder.AppendFormat("function Stream:Get{0}Maker()\n", funcName);
                    builder.AppendFormat("\treturn self['Get' .. self:GetString():gsub('%.', '')](self)\n");
                    builder.AppendFormat("end\n");
                }
                builder.AppendFormat("function Stream:Get{0}()\n", funcName);
                if (!cls.Inherit.IsEmpty())
                    builder.AppendFormat("\tlocal o = self:Get{0}()\n", cls.Inherit.Replace(".", ""));
                else
                    builder.AppendFormat("\tlocal o = {{}}\n");
                builder.AppendFormat("\tsetmetatable(o, {0})\n", cls.FullType);
                //--普通变量
                for (int j = 0; j < cls.Fields.Count; j++)
                {
                    FieldWrap field = cls.Fields[j];
                    if (!Util.MatchGroups(field.Group)) continue;

                    if (field.IsRaw)
                        builder.AppendFormat("\to.{0} = self:Get{1}()\n", field.Name, field.OriginalType.FirstCharUpper());
                    else if (field.IsEnum)
                        builder.AppendFormat("\to.{0} = self:GetInt()\n", field.Name);
                    else if (field.IsClass)
                        builder.AppendFormat("\to.{0} = self:Get{1}Maker()\n", field.Name, field.OriginalType.Replace(".", ""));
                    else if (field.IsContainer)
                    {
                        if (field.OriginalType == Setting.LIST)
                        {
                            var item = field.GetItemDefine();
                            string index = item.OriginalType.Replace(".", "").FirstCharUpper();
                            if (item.IsClass) index += "Maker";
                            builder.AppendFormat("\to.{0} = self:GetList('{1}')\n", field.Name, index);
                        }
                        else if (field.OriginalType == Setting.DICT)
                        {
                            var k = field.GetKeyDefine();
                            string key = k.OriginalType.FirstCharUpper();
                            if (k.IsEnum) key = "Int";
                            var v = field.GetValueDefine();
                            string value = v.OriginalType.Replace(".", "").FirstCharUpper();
                            if (v.IsClass) value += "Maker";
                            else if (v.IsEnum) value = "Int";
                            builder.AppendFormat("\to.{0} = self:GetDict('{1}', '{2}')\n", field.Name, key, value);
                        }
                    }
                }
                builder.AppendFormat("\treturn o\n");
                builder.AppendFormat("end\n");
            }

            var eit = EnumWrap.Enums.GetEnumerator();
            while (eit.MoveNext())
            {
                EnumWrap en = eit.Current.Value;
                builder.AppendFormat("GetOrCreate('{0}')['{1}'] = {{\n", en.Namespace, en.Name);
                builder.AppendFormat("\tNULL = {0},\n", LUA_ENUM_NULL);
                foreach (var item in en.Values)
                    builder.AppendFormat("\t{0} = {1},\n", item.Key, item.Value);
                builder.AppendLine("}");
            }
            builder.AppendLine("return Stream");
            string path = Path.Combine(Setting.LuaDir, DATA_STRUCT + ".lua");
            Util.SaveFile(path, builder.ToString());
        }
        private static string CheckConst(string type, string value)
        {
            switch (type)
            {
                case Setting.FLOAT:
                    value = string.Format("{0}", value);
                    break;
                case Setting.STRING:
                    value = string.Format("'{0}'", value);
                    break;
            }
            return value;
        }
    }
}
