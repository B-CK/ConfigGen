using ConfigGen.Description;
using ConfigGen.TypeInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigGen.Export
{
    public class ExportLua
    {
        //--lua中的枚举默认   NULL = -1
        private const string DATA_STRUCT = "DataStruct";
        private const string DATA_STREAM = "DataStream";
        private const string DATA_CONFIG = "Config";
        private const string LUA_ENUM_NULL = "-9";

        public static void Export()
        {
            GenConfigTable();
            GenDataStream();
            GenDataStruct();
        }

        private static void GenConfigTable()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("return {");
            var cit = ConfigInfo.Configs.GetEnumerator();
            while (cit.MoveNext())
            {
                ConfigInfo cfg = cit.Current.Value;
                string method = string.Format("Get{0}", cfg.FullName);
                string index = cfg.Index.Name;
                string relPath = cfg.OutputFile + Setting.CsvFileExt;
                builder.AppendFormat("\t{{ name = '{0}', method = '{1}', index = '{2}', output = '{3}' }},\n",
                    cfg.Name, method, index, relPath.ToLower());
            }
            builder.AppendLine("}");

            string path = Path.Combine(Setting.LuaDir, DATA_CONFIG + ".lua");
            Util.SaveFile(path, builder.ToString());
        }
        private static void GenDataStream()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("local lower = string.lower");
            builder.AppendLine("local setmetatable = setmetatable");
            builder.AppendLine("local tonumber = tonumber");
            builder.AppendLine("local lines = io.lines");
            builder.AppendLine("local split = string.split");
            builder.AppendLine("local format= string.format");
            builder.AppendLine("local Stream = {}");
            builder.AppendLine("Stream.__index = Stream");
            builder.AppendLine("Stream.name = \"Stream\"");

            builder.AppendLine("local Split = function (line)");
            builder.AppendLine("\treturn split(line, '▃')");
            builder.AppendLine("end");

            builder.AppendLine("function Stream.new(dataFile)");
            builder.AppendLine("\tlocal o = {}");
            builder.AppendLine("\tsetmetatable(o, Stream)");
            builder.AppendLine("\to.dataFile = dataFile");
            //----数据解析路径
            builder.AppendFormat("\to.GetLine = lines(dataFile)\n");

            builder.AppendLine("\to.idx = 0");
            builder.AppendLine("\to.line = 0");
            builder.AppendLine("\treturn o");
            builder.AppendLine("end");

            builder.AppendLine("function Stream:Count()");
            builder.AppendLine("\treturn #self.columns");
            builder.AppendLine("end");

            builder.AppendLine("function Stream:NextRow()");
            builder.AppendLine("\tlocal line = self.GetLine()");
            builder.AppendLine("\tif line == nil or #line == 0 then");
            builder.AppendLine("\t\treturn false");
            builder.AppendLine("\tend");
            builder.AppendLine("\tself.columns = Split(line)");
            builder.AppendLine("\tself.idx = 1");
            builder.AppendLine("\tself.line = self.line + 1");
            builder.AppendLine("\treturn true");
            builder.AppendLine("end");

            builder.AppendLine("function Stream:NextColum()");
            builder.AppendLine("\tif self.idx > #self.columns then");
            builder.AppendLine("\t\tlocal status = self:NextRow()");
            builder.AppendLine("\t\tif not status then");
            builder.AppendLine("\t\t\tself.hasNext = false");
            builder.AppendLine("\t\t\treturn nil");
            builder.AppendLine("\t\tend");
            builder.AppendLine("\tend");
            builder.AppendLine("\tlocal result = self.columns[self.idx]");
            builder.AppendLine("\tself.idx = self.idx + 1");
            builder.AppendLine("\treturn result");
            builder.AppendLine("end");

            builder.AppendLine("function Stream:GetInt()");
            builder.AppendLine("\tlocal next = self:NextColum()");
            builder.AppendLine("\treturn tonumber(next)");
            builder.AppendLine("end");

            builder.AppendLine("function Stream:GetLong()");
            builder.AppendLine("\tlocal next = self:NextColum()");
            builder.AppendLine("\treturn tonumber(next)");
            builder.AppendLine("end");

            builder.AppendLine("function Stream:GetFloat()");
            builder.AppendLine("\tlocal next = self:NextColum()");
            builder.AppendLine("\treturn tonumber(next)");
            builder.AppendLine("end");

            builder.AppendLine("function Stream:GetBool()");
            builder.AppendLine("\tlocal next = lower(self:NextColum())");
            builder.AppendLine("\tif next == '0' then");
            builder.AppendLine("\t\treturn false");
            builder.AppendLine("\telse");
            builder.AppendLine("\t\treturn true");
            builder.AppendLine("\tend");
            builder.AppendLine("end");

            builder.AppendLine("function Stream:GetString()");
            builder.AppendLine("\treturn self:NextColum()");
            builder.AppendLine("end");

            builder.AppendLine("function Stream:GetList(type)");
            builder.AppendLine("\tlocal result = {}");
            builder.AppendLine("\tlocal method = self['Get' .. type]");
            builder.AppendLine("\tlocal length = self:GetInt()");
            builder.AppendLine("\tfor i = 1, length do");
            builder.AppendLine("\t\tresult[i] = method(self)");
            builder.AppendLine("\tend");
            builder.AppendLine("\treturn result");
            builder.AppendLine("end");

            builder.AppendLine("function Stream:GetDict(key, value)");
            builder.AppendLine("\tlocal result = {}");
            builder.AppendLine("\tlocal optKey = self['Get' .. key]");
            builder.AppendLine("\tlocal optValue = self['Get' .. value]");
            builder.AppendLine("\tlocal length = self:GetInt()");
            builder.AppendLine("\tfor i = 1, length do");
            builder.AppendLine("\t\tresult[optKey(self)] = optValue(self)");
            builder.AppendLine("\tend");
            builder.AppendLine("\treturn result");
            builder.AppendLine("end");

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

            var cit = ClassInfo.Classes.GetEnumerator();
            while (cit.MoveNext())
            {
                var cls = cit.Current.Value;
                builder.AppendLine("meta= {}");
                builder.AppendLine("meta.__index = meta");
                builder.AppendFormat("meta.class = '{0}'\n", cls.FullName);
                //常量字段
                for (int j = 0; j < cls.Consts.Count; j++)
                {
                    ConstInfo cst = cls.Consts[j];
                    string value = CheckConst(cst.OriginalType, cst.Value);
                    switch (cst.OriginalType)
                    {
                        case Setting.LIST:
                            string[] list = cst.Value.Split(Setting.SplitFlag);
                            for (int k = 0; k < list.Length; k++)
                                list[k] = CheckConst(cst.Types[1], list[k]);
                            value = string.Format("{{ {0} }}", Util.List2String(list));
                            break;
                        case Setting.DICT:
                            string[] dict = cst.Value.Split(Setting.SplitFlag);
                            for (int k = 0; k < dict.Length; k++)
                            {
                                string[] nodes = dict[k].Split(Setting.ArgsSplitFlag);
                                nodes[0] = CheckConst(cst.Types[1], nodes[0]);
                                nodes[1] = CheckConst(cst.Types[2], nodes[1]);
                                dict[k] = string.Format("{0} = {1},", nodes[0], nodes[1]);
                            }
                            value = string.Format("{{ {0} }}", Util.List2String(dict));
                            break;
                    }
                    builder.AppendFormat("meta.{0} = {1}\n", cst.Name, value);
                }

                builder.AppendFormat("GetOrCreate('{0}')['{1}'] = meta\n", cls.Namespace, cls.Name);
                string funcName = cls.FullName.Replace(".", "");
                if (!cls.Inherit.IsEmpty())
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
                builder.AppendFormat("\tsetmetatable(o, {0})\n", cls.FullName);
                //--普通变量
                for (int j = 0; j < cls.Fields.Count; j++)
                {
                    FieldInfo field = cls.Fields[j];
                    if (!Util.MatchGroups(field.Group)) continue;

                    if (field.IsRaw)
                        builder.AppendFormat("\to.{0} = self:Get{1}()\n", field.Name, field.OriginalType);
                    else if (field.IsEnum)
                        builder.AppendFormat("\to.{0} = self:GetInt()\n", field.Name);
                    else if (field.IsClass)
                        builder.AppendFormat("\to.{0} = self:Get{1}Maker()\n", field.Name, Util.List2String(field.Types, ""));
                    else if (field.IsContainer)
                    {
                        if (field.OriginalType == Setting.LIST)
                        {
                            var item = field.GetItemDefine();
                            string index = item.OriginalType.Replace(".", "");
                            if (item.IsClass) index += "Maker";
                            builder.AppendFormat("\to.{0} = self:GetList('{1}')\n", field.Name, index);
                        }
                        else if (field.OriginalType == Setting.DICT)
                        {
                            var k = field.GetKeyDefine();
                            string key = k.OriginalType;
                            if (k.IsEnum) key = "Int";
                            var v = field.GetKeyDefine();
                            string value = v.OriginalType.Replace(".", "");
                            if (k.IsClass) value += "Maker";
                            builder.AppendFormat("\to.{0} = self:GetDict('{1}', '{2}')\n", field.Name, key, value);
                        }
                    }
                }
                builder.AppendFormat("\treturn o\n");
                builder.AppendFormat("end\n");
            }

            var eit = EnumInfo.Enums.GetEnumerator();
            while (cit.MoveNext())
            {
                EnumInfo en = eit.Current.Value;
                builder.AppendFormat("GetOrCreate('{0}.{1}')['{2}'] = {{\n", en.Namespace, en.Name);
                builder.AppendFormat("\tNULL = {0},\n", LUA_ENUM_NULL);
                var vit = en.Values.GetEnumerator();
                while (vit.MoveNext())
                {
                    string key = vit.Current.Key;
                    string value = vit.Current.Value;
                    builder.AppendFormat("\t{0} = {1},\n", key, value);
                }
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
                    value = string.Format("{0}f", value);
                    break;
                case Setting.STRING:
                    value = string.Format("@\"{0}\"", value);
                    break;
            }
            return value;
        }
    }
}
