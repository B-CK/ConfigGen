using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tool;
using Tool.Wrap;

namespace Tool.Export
{
    public class Gen_Lua
    {
        //--lua中的枚举默认   NULL = -1
        private const string DATA_STRUCT = "DataStruct";
        private const string DATA_CONFIG = "Config";
        private const string LOCAL = "local";

        public static void Gen()
        {
            GenConfigTable();
            GenDataStruct();
        }

        /// <summary>
        /// 数据结构配置表
        /// </summary>
        private static void GenConfigTable()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("return {");
            var cit = ConfigWrap.Configs.GetEnumerator();
            while (cit.MoveNext())
            {
                ConfigWrap cfg = cit.Current.Value;
                string method = string.Format("Get{0}", Util.CorrectFullType(cfg.FullName).Replace(new string(Setting.DotSplit), ""));
                string index = cfg.Index.Name;
                string relPath = cfg.OutputFile + Setting.DataFileExt;
                builder.IntervalLevel().AppendFormat("{{ name = '{0}', method = '{1}', index = '{2}', output = '{3}' }},\n",
                    cfg.Name, method, index, relPath.Replace("\\", "/").ToLower());
            }
            builder.AppendLine("}");

            string path = Path.Combine(Setting.LuaDir, DATA_CONFIG + ".lua");
            var names = ConfigWrap.Configs.Select(item => $"---@field {item.Value.Name} string");
            string content = string.Join("\n", names) + "\n---@class ConfigName\n" + builder.ToString();
            Util.SaveFile(path, content);
        }
        /// <summary>
        /// 数据结构定义
        /// </summary>
        private static void GenDataStruct()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("local Stream = require(\"Common.DataStream\")");
            builder.AppendLine("local GetOrCreate = Util.GetOrCreate");
            builder.AppendLine();
            builder.AppendLine("local meta");

            var cit = ClassWrap.Classes.GetEnumerator();
            while (cit.MoveNext())
            {
                var cls = cit.Current.Value;
                var fullType = Util.CorrectFullType(cls.FullName);
                builder.AppendLine("meta= {}");
                builder.AppendLine("meta.__index = meta");
                builder.AppendFormat("meta.__cname = '{0}'\n", fullType);

                //--常量字段
                for (int j = 0; j < cls.Consts.Count; j++)
                {
                    ConstWrap constant = cls.Consts[j];
                    if (!Util.MatchGroups(constant.Group)) continue;

                    string value = constant.Value;
                    switch (constant.FullName)
                    {
                        case Setting.BOOL:
                            value = value.ToLower();
                            break;
                        case Setting.STRING:
                            value = string.Format("'{0}'", value);
                            break;
                        default:
                            if (EnumWrap.IsEnum(constant.FullName))
                                value = EnumWrap.Enums[constant.FullName].GetValue(constant.Value);
                            break;
                    }
                    builder.AppendFormat("meta.{0} = {1}\n", constant.Name, value);
                }

                builder.AppendFormat("GetOrCreate('{0}.{1}')['{2}'] = meta\n", Setting.ModuleName, cls.Namespace, cls.Name);
                string funcName = fullType.Replace(".", "");
                if (cls.IsDynamic())
                {
                    builder.AppendFormat("function Stream:Get{0}Maker()\n", funcName);
                    builder.IntervalLevel().AppendFormat("return self['Get' .. self:GetString():gsub('%.', '')](self)\n");
                    builder.AppendFormat("end\n");
                }
                builder.AppendFormat("function Stream:Get{0}()\n", funcName);
                if (!cls.Inherit.IsEmpty())
                    builder.IntervalLevel().AppendFormat("local o = self:Get{0}()\n", Util.CorrectFullType(cls.Inherit).Replace(".", ""));
                else
                    builder.IntervalLevel().AppendFormat("local o = {{}}\n");
                builder.IntervalLevel().AppendFormat("setmetatable(o, {0})\n", fullType);

                //--普通字段
                for (int j = 0; j < cls.Fields.Count; j++)
                {
                    FieldWrap field = cls.Fields[j];
                    if (!Util.MatchGroups(field.Group)) continue;

                    if (field.IsRaw)
                        builder.IntervalLevel().AppendFormat("o.{0} = self:Get{1}()\n", field.Name, field.OriginalType.FirstCharUpper());
                    else if (field.IsEnum)
                        builder.IntervalLevel().AppendFormat("o.{0} = self:GetInt()\n", field.Name);
                    else if (field.IsClass)
                    {
                        if (field.IsDynamic)
                            builder.IntervalLevel().AppendFormat("o.{0} = self:Get{1}Maker()\n", field.Name, Util.CorrectFullType(field.OriginalType).Replace(".", ""));
                        else
                            builder.IntervalLevel().AppendFormat("o.{0} = self:Get{1}()\n", field.Name, Util.CorrectFullType(field.OriginalType).Replace(".", ""));
                    }
                    else if (field.IsContainer)
                    {
                        if (field.OriginalType == Setting.LIST)
                        {
                            var item = field.GetItemDefine();
                            string index = Util.CorrectFullType(item.OriginalType).Replace(".", "").FirstCharUpper();
                            if (item.IsClass && item.IsDynamic) index += "Maker";
                            builder.IntervalLevel().AppendFormat("o.{0} = self:GetList('{1}')\n", field.Name, index);
                        }
                        else if (field.OriginalType == Setting.DICT)
                        {
                            var k = field.GetKeyDefine();
                            string key = k.OriginalType.FirstCharUpper();
                            if (k.IsEnum) key = "Int";
                            var v = field.GetValueDefine();
                            string value = Util.CorrectFullType(v.OriginalType).Replace(".", "").FirstCharUpper();
                            if (v.IsClass && v.IsDynamic) value += "Maker";
                            else if (v.IsEnum) value = "Int";
                            builder.IntervalLevel().AppendFormat("o.{0} = self:GetDict('{1}', '{2}')\n", field.Name, key, value);
                        }
                    }
                }
                builder.IntervalLevel().AppendFormat("return o\n");
                builder.AppendFormat("end\n");
            }

            var eit = EnumWrap.Enums.GetEnumerator();
            while (eit.MoveNext())
            {
                EnumWrap en = eit.Current.Value;
                builder.AppendFormat("GetOrCreate('{0}.{1}')['{2}'] = {{\n", Setting.ModuleName, en.Namespace, en.Name);
                foreach (var item in en.Values)
                    builder.IntervalLevel().AppendFormat("{0} = {1},\n", item.Key, item.Value);
                builder.AppendLine("}");
            }
            builder.AppendLine("return Stream");
            string path = Path.Combine(Setting.LuaDir, DATA_STRUCT + ".lua");
            Util.SaveFile(path, builder.ToString());
        }
    }
}