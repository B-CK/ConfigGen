using ConfigGen.LocalInfo;
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
        private const string DATA_CONFIG = "Config";
        private const string LUA_CFG_ROOT = "Cfg";

        static readonly Dictionary<string, string> MapType = new Dictionary<string, string>()
        {
            {"int", "Int" },
            {"long", "Long" },
            {"float", "Float" },
            {"bool", "Bool" },
            {"string", "String" },
        };


        public static void Export()
        {
            StringBuilder dsLoopBuilder = new StringBuilder();
            StringBuilder cfgLoopBuilder = new StringBuilder();
            foreach (var item in Local.Instance.TypeInfoLib.ClassInfoDict)
            {
                ClassTypeInfo classType = item.Value;
                if (classType.TypeType != TypeType.Class) continue;
                string method = string.Format("Get{0}{1}", LUA_CFG_ROOT, classType.GetClassName().Replace(".", ""));
                string index = classType.IndexField.Name;
                string fileName = classType.GetClassName().Replace(".", "/") + Values.CsvFileExt;
                cfgLoopBuilder.AppendFormat("\t{{ name = '{0}', method = '{1}', index = '{2}', output = '{3}' }},\n",
                    classType.Name, method, index, fileName);

                dsLoopBuilder.AppendLine("meta= {}");
                dsLoopBuilder.AppendLine("meta.__index = meta");
                for (int i = 0; i < classType.Consts.Count; i++)
                {
                    ConstFieldInfo cst = classType.Consts[i];
                    dsLoopBuilder.AppendFormat("meta.{0} = {1}\n", cst.Name, cst.Value);
                }
                dsLoopBuilder.AppendFormat("GetOrCreate('{0}.{1}')['{2}'] = meta\n", LUA_CFG_ROOT, classType.NamespaceName, classType.Name);
                dsLoopBuilder.AppendFormat("function Stream:Get{0}{1}{2}()\n", LUA_CFG_ROOT, classType.NamespaceName, classType.Name);
                dsLoopBuilder.AppendFormat("\tlocal o = {{}}\n");
                dsLoopBuilder.AppendFormat("\tsetmetatable(o, {0}.{1}.{2})\n", LUA_CFG_ROOT, classType.NamespaceName, classType.Name);
                for (int i = 0; i < classType.Fields.Count; i++)
                {
                    FieldInfo field = classType.Fields[i];
                    BaseTypeInfo baseType = field.BaseInfo;
                    switch (baseType.TypeType)
                    {
                        case TypeType.Base:
                            dsLoopBuilder.AppendFormat("\to.{0} = self:Get{1}()\n", field.Name, MapType[field.Type]);
                            break;
                        case TypeType.Enum:
                            dsLoopBuilder.AppendFormat("\to.{0} = self:GetInt()\n", field.Name);
                            break;
                        case TypeType.Class:
                            ClassTypeInfo classTypeField = baseType as ClassTypeInfo;
                            if (classTypeField.HasSubClass)
                            {
                                dsLoopBuilder.AppendFormat("\tlocal _{0} = '{1}' .. self:GetString()\n", field.Name, LUA_CFG_ROOT);
                                dsLoopBuilder.AppendFormat("\to.{0} = self:GetObject(_{0})\n", field.Name);
                            }
                            else
                            {
                                string fullName = string.Format("{0}{1}", LUA_CFG_ROOT, field.Type.Replace(".", ""));
                                dsLoopBuilder.AppendFormat("\to.{0} = self:GetObject('{1}')\n", field.Name, fullName);
                            }
                            break;
                        case TypeType.List:
                            {
                                ListTypeInfo listType = baseType as ListTypeInfo;
                                BaseTypeInfo itemInfo = TypeInfo.GetTypeInfo(listType.ItemType);
                                string itemType = null;
                                switch (itemInfo.TypeType)
                                {
                                    case TypeType.Base:
                                        itemType = MapType[listType.ItemType];
                                        break;
                                    case TypeType.Enum:
                                        itemType = MapType["int"];
                                        break;
                                    case TypeType.Class:
                                        itemType = string.Format("{0}{1}", LUA_CFG_ROOT, listType.ItemType.Replace(".", ""));
                                        break;
                                    case TypeType.List:
                                    case TypeType.Dict:
                                    case TypeType.None:
                                    default:
                                        break;
                                }
                                dsLoopBuilder.AppendFormat("\to.{0} = self:GetList('{1}')\n", field.Name, itemType);
                            }
                            break;
                        case TypeType.Dict:
                            {
                                DictTypeInfo dictType = baseType as DictTypeInfo;
                                BaseTypeInfo keyInfo = TypeInfo.GetTypeInfo(dictType.KeyType);
                                BaseTypeInfo valueInfo = TypeInfo.GetTypeInfo(dictType.ValueType);
                                string keyType = null;
                                switch (keyInfo.TypeType)
                                {
                                    case TypeType.Base:
                                        keyType = MapType[dictType.KeyType];
                                        break;
                                    case TypeType.Enum:
                                        keyType = MapType["int"];
                                        break;
                                    case TypeType.Class:
                                    case TypeType.List:
                                    case TypeType.Dict:
                                    case TypeType.None:
                                    default:
                                        break;
                                }
                                string valueType = null;
                                switch (valueInfo.TypeType)
                                {
                                    case TypeType.Base:
                                        valueType = MapType[dictType.ValueType];
                                        break;
                                    case TypeType.Enum:
                                        valueType = MapType["int"];
                                        break;
                                    case TypeType.Class:
                                        valueType = string.Format("{0}{1}", LUA_CFG_ROOT, dictType.ValueType.Replace(".", ""));
                                        break;
                                    case TypeType.List:
                                    case TypeType.Dict:
                                    case TypeType.None:
                                    default:
                                        break;
                                }
                                dsLoopBuilder.AppendFormat("\to.{0} = self:GetDict('{1}', '{2}')\n", field.Name, keyType, valueType);
                            }
                            break;
                        case TypeType.None:
                        default:
                            break;
                    }
                }
                dsLoopBuilder.AppendFormat("\treturn o\n", LUA_CFG_ROOT, classType.NamespaceName, classType.Name);
                dsLoopBuilder.AppendFormat("end\n");
            }
            var enums = Local.Instance.TypeInfoLib.EnumInfoDict;
            foreach (var item in enums)
            {
                EnumTypeInfo enumInfo = item.Value;
                string fullName = string.Format("{0}.{1}", LUA_CFG_ROOT, item.Value.GetClassName());
                dsLoopBuilder.AppendFormat("GetOrCreate('{0}.{1}')['{2}'] = {{\n", LUA_CFG_ROOT, enumInfo.NamespaceName, enumInfo.Name);
                dsLoopBuilder.AppendFormat("\tNULL = -1,\n");
                for (int i = 0; i < enumInfo.KeyValuePair.Count; i++)
                {
                    string key = enumInfo.KeyValuePair[i].Name;
                    string value = enumInfo.KeyValuePair[i].Value;
                    dsLoopBuilder.AppendFormat("\t{0} = {1},\n", key, value);
                }
                dsLoopBuilder.AppendLine("}");
            }
            //--DataStruct
            StringBuilder structBuilder = new StringBuilder();
            structBuilder.AppendLine("local Stream = require(\"Cfg.DataStream\")");
            structBuilder.AppendLine("local find = string.find");
            structBuilder.AppendLine("local sub = string.sub");
            structBuilder.AppendLine();
            structBuilder.AppendLine("local function GetOrCreate(namespace)");
            structBuilder.AppendLine("\tlocal t = _G");
            structBuilder.AppendLine("\tlocal idx = 1");
            structBuilder.AppendLine("\twhile true do");
            structBuilder.AppendLine("\t\tlocal start, ends = find(namespace, '.', idx, true)");
            structBuilder.AppendLine("\t\tlocal subname = sub(namespace, idx, start and start - 1)");
            structBuilder.AppendLine("\t\tlocal subt = t[subname]");
            structBuilder.AppendLine("\t\tif not subt then");
            structBuilder.AppendLine("\t\t\tsubt = {}");
            structBuilder.AppendLine("\t\t\tt[subname] = subt");
            structBuilder.AppendLine("\t\tend");
            structBuilder.AppendLine("\t\tt = subt");
            structBuilder.AppendLine("\t\tif start then");
            structBuilder.AppendLine("\t\t\tidx = ends + 1");
            structBuilder.AppendLine("\t\telse");
            structBuilder.AppendLine("\t\t\treturn t");
            structBuilder.AppendLine("\t\tend");
            structBuilder.AppendLine("\tend");
            structBuilder.AppendLine("end");
            structBuilder.AppendLine();
            structBuilder.AppendLine("local meta");
            structBuilder.AppendLine(dsLoopBuilder.ToString());
            structBuilder.AppendLine("return Stream");
            string path = Path.Combine(Values.ExportLua, DATA_STRUCT + ".lua");
            Util.SaveFile(path, structBuilder.ToString());
            structBuilder.Clear();

            //--Config
            StringBuilder configBuilder = new StringBuilder();
            configBuilder.Append("local Stream = require(\"Cfg.DataStruct\")\n");
            configBuilder.Append("local cfgs = {}\n");
            configBuilder.Append("for _, s in ipairs({\n");
            configBuilder.Append(cfgLoopBuilder.ToString());
            configBuilder.AppendLine("}) do");
            configBuilder.AppendLine("\tlocal data = Stream.new(s.output)");
            configBuilder.AppendLine("\tlocal cfg = {}");
            configBuilder.AppendLine("\twhile data.hasNext do");
            configBuilder.AppendLine("\t\tcfg[s.index] = data[s.method](data)");
            configBuilder.AppendLine("\tend");
            configBuilder.AppendLine("\tcfgs[s.name] = cfg");
            configBuilder.AppendLine("\tdata:Close()");
            configBuilder.AppendLine("end");
            configBuilder.AppendLine();
            configBuilder.AppendLine("return cfgs");
            path = Path.Combine(Values.ExportLua, DATA_CONFIG + ".lua");
            Util.SaveFile(path, configBuilder.ToString());
            configBuilder.Clear();
        }
    }
}
