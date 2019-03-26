using ConfigGen.Description;
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
            GenDataStructAndConfig();
            GenDataStream();
        }

        private static string GetCfgClassPath(ClassTypeInfo classType)
        {
            return string.Format("{0}.{1}", Values.ConfigRootNode, classType.GetFullName());
        }
        private static void GenDataStructAndConfig()
        {
            StringBuilder dsLoopBuilder = new StringBuilder();
            StringBuilder cfgLoopBuilder = new StringBuilder();
            foreach (var item in TableInfo.DataInfoDict)
            {
                ClassTypeInfo classType = item.Value.ClassTypeInfo;
                if (classType.EType != TypeType.Class) continue;
                string method = string.Format("Get{0}", GetCfgClassPath(classType).Replace(".", ""));
                string index = classType.IndexField.Name;
                string fileName = classType.GetFullName().Replace(".", "/") + Values.CsvFileExt;
                cfgLoopBuilder.AppendFormat("\t{{ name = '{0}', method = '{1}', index = '{2}', output = '{3}' }},\n",
                    classType.Name, method, index, fileName.ToLower());
            }
            foreach (var item in TypeInfo.Instance.ClassInfos)
            {
                ClassTypeInfo classInfo = item;
                if (classInfo.EType != TypeType.Class) continue;

                dsLoopBuilder.AppendLine("meta= {}");
                dsLoopBuilder.AppendLine("meta.__index = meta");
                dsLoopBuilder.AppendFormat("meta.class = '{0}'\n", GetCfgClassPath(classInfo));
                //--常量字段
                for (int i = 0; i < classInfo.Consts.Count; i++)
                {
                    ConstInfo cst = classInfo.Consts[i];
                    string value = cst.Value;
                    switch (cst.Type)
                    {
                        case TypeInfo.FLOAT:
                            value = string.Format("{0}", cst.Value);
                            break;
                        case TypeInfo.STRING:
                            value = string.Format("'{0}'", cst.Value);
                            break;
                    }
                    dsLoopBuilder.AppendFormat("meta.{0} = {1}\n", cst.Name, value);
                }
                dsLoopBuilder.AppendFormat("GetOrCreate('{0}.{1}')['{2}'] = meta\n", Values.ConfigRootNode, classInfo.NamespaceName, classInfo.Name);
                if (classInfo.InhertType == ClassTypeInfo.InhertState.PolyParent)
                {
                    //-----类型选择函数,需要动态跳转到指定类型解析函数,以Maker结尾
                    dsLoopBuilder.AppendFormat("function Stream:Get{0}Maker()\n", GetCfgClassPath(classInfo).Replace(".", ""));
                    dsLoopBuilder.AppendFormat("\treturn self['Get' .. self:GetString():gsub('%.', '')](self)\n");
                    dsLoopBuilder.AppendFormat("end\n");
                }
                string noPointClassName = GetCfgClassPath(classInfo).Replace(".", "");
                dsLoopBuilder.AppendFormat("function Stream:Get{0}()\n", noPointClassName);
                if (classInfo.InhertType == ClassTypeInfo.InhertState.PolyChild)
                    dsLoopBuilder.AppendFormat("\tlocal o = self:Get{0}()\n", GetCfgClassPath(classInfo.Inherit).Replace(".", ""));
                else
                    dsLoopBuilder.AppendFormat("\tlocal o = {{}}\n");
                dsLoopBuilder.AppendFormat("\tsetmetatable(o, {0})\n", GetCfgClassPath(classInfo));
                //--普通变量
                GenStructClassCtor(dsLoopBuilder, classInfo);
                dsLoopBuilder.AppendFormat("\treturn o\n");
                dsLoopBuilder.AppendFormat("end\n");
            }
            foreach (var item in TypeInfo.Instance.EnumInfos)
            {
                EnumTypeInfo enumInfo = item;
                string fullName = string.Format("{0}.{1}", Values.ConfigRootNode, enumInfo.GetFullName());
                dsLoopBuilder.AppendFormat("GetOrCreate('{0}.{1}')['{2}'] = {{\n", Values.ConfigRootNode, enumInfo.NamespaceName, enumInfo.Name);
                dsLoopBuilder.AppendFormat("\tNULL = {0},\n", LUA_ENUM_NULL);
                for (int i = 0; i < enumInfo.Enums.Count; i++)
                {
                    string key = enumInfo.Enums[i].Name;
                    string value = enumInfo.Enums[i].Value;
                    dsLoopBuilder.AppendFormat("\t{0} = {1},\n", key, value);
                }
                dsLoopBuilder.AppendLine("}");
            }
            //--DataStruct
            StringBuilder structBuilder = new StringBuilder();
            structBuilder.AppendLine("local Stream = require(\"Cfg.DataStream\")");
            structBuilder.AppendLine("local GetOrCreate = Util.GetOrCreate");
            structBuilder.AppendLine();
            structBuilder.AppendLine("local meta");
            structBuilder.AppendLine(dsLoopBuilder.ToString());
            structBuilder.AppendLine("return Stream");
            string path = Path.Combine(Values.ExportLua, DATA_STRUCT + ".lua");
            Util.SaveFile(path, structBuilder.ToString());
            structBuilder.Clear();

            //--Config
            StringBuilder configBuilder = new StringBuilder();
            configBuilder.AppendLine("local Stream = require(\"Cfg.DataStruct\")");
            configBuilder.AppendLine("local createpath = create_datastream_path");
            configBuilder.AppendLine("local cfgs = {}");
            configBuilder.AppendLine("for _, s in ipairs({");
            configBuilder.Append(cfgLoopBuilder.ToString());
            configBuilder.AppendLine("}) do");
            configBuilder.AppendLine("\tlocal path = createpath(s.output)");
            configBuilder.AppendLine("\tlocal data = Stream.new(path)");
            configBuilder.AppendLine("\tlocal cfg = {}");
            configBuilder.AppendLine("\twhile data:NextRow() do");
            configBuilder.AppendLine("\t\tlocal value = data[s.method](data)");
            configBuilder.AppendLine("\t\tlocal key = value[s.index]");
            configBuilder.AppendLine("\t\tcfg[key] = value");
            configBuilder.AppendLine("\tend");
            configBuilder.AppendLine("\tcfgs[s.name] = cfg");
            configBuilder.AppendLine("end");
            configBuilder.AppendLine();
            configBuilder.AppendLine("return cfgs");
            path = Path.Combine(Values.ExportLua, DATA_CONFIG + ".lua");
            Util.SaveFile(path, configBuilder.ToString());
            configBuilder.Clear();
        }
        private static void GenStructClassCtor(StringBuilder builder, ClassTypeInfo classInfo)
        {
            for (int i = 0; i < classInfo.Fields.Count; i++)
            {
                FieldInfo field = classInfo.Fields[i];
                BaseTypeInfo baseType = field.BaseInfo;
                switch (baseType.EType)
                {
                    case TypeType.Base:
                        builder.AppendFormat("\to.{0} = self:Get{1}()\n", field.Name, Util.FirstCharUpper(field.Type));
                        break;
                    case TypeType.Enum:
                        builder.AppendFormat("\to.{0} = self:GetInt()\n", field.Name);
                        break;
                    case TypeType.Class:
                        ClassTypeInfo classTypeField = baseType as ClassTypeInfo;
                        string fullName = GetCfgClassPath(classTypeField).Replace(".", "");
                        if (classTypeField.IsPolyClass)
                            builder.AppendFormat("\to.{0} = self:Get{1}Maker()\n", field.Name, fullName);
                        else
                            builder.AppendFormat("\to.{0} = self:Get{1}()\n", field.Name, fullName);
                        break;
                    case TypeType.List:
                        {
                            ListTypeInfo listType = baseType as ListTypeInfo;
                            BaseTypeInfo itemInfo = listType.ItemInfo.BaseInfo;
                            string itemType = null;
                            switch (itemInfo.EType)
                            {
                                case TypeType.Base:
                                    itemType = Util.FirstCharUpper(listType.ItemType);
                                    break;
                                case TypeType.Enum:
                                    itemType = Util.FirstCharUpper("int");
                                    break;
                                case TypeType.Class:
                                    ClassTypeInfo itemClass = itemInfo as ClassTypeInfo;
                                    itemType = GetCfgClassPath(itemClass.GetRootClassInfo()).Replace(".", "");
                                    if (itemClass.IsPolyClass)
                                        itemType += "Maker";
                                    break;
                                case TypeType.List:
                                case TypeType.Dict:
                                case TypeType.None:
                                default:
                                    break;
                            }
                            builder.AppendFormat("\to.{0} = self:GetList('{1}')\n", field.Name, itemType);
                        }
                        break;
                    case TypeType.Dict:
                        {
                            DictTypeInfo dictType = baseType as DictTypeInfo;
                            BaseTypeInfo keyInfo = dictType.KeyInfo.BaseInfo;
                            BaseTypeInfo valueInfo = dictType.ValueInfo.BaseInfo;
                            string keyType = null;
                            switch (keyInfo.EType)
                            {
                                case TypeType.Base:
                                    keyType = Util.FirstCharUpper(dictType.KeyType);
                                    break;
                                case TypeType.Enum:
                                    keyType = Util.FirstCharUpper("int");
                                    break;
                                case TypeType.Class:
                                case TypeType.List:
                                case TypeType.Dict:
                                case TypeType.None:
                                default:
                                    break;
                            }
                            string valueType = null;
                            switch (valueInfo.EType)
                            {
                                case TypeType.Base:
                                    valueType = Util.FirstCharUpper(dictType.ValueType);
                                    break;
                                case TypeType.Enum:
                                    valueType = Util.FirstCharUpper("int");
                                    break;
                                case TypeType.Class:
                                    ClassTypeInfo valueClass = valueInfo as ClassTypeInfo;
                                    valueType = GetCfgClassPath(valueClass.GetRootClassInfo()).Replace(".", "");
                                    if (valueClass.IsPolyClass)
                                        valueType += "Maker";
                                    break;
                                case TypeType.List:
                                case TypeType.Dict:
                                case TypeType.None:
                                default:
                                    break;
                            }
                            builder.AppendFormat("\to.{0} = self:GetDict('{1}', '{2}')\n", field.Name, keyType, valueType);
                        }
                        break;
                    case TypeType.None:
                    default:
                        break;
                }
            }
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

            string path = Path.Combine(Values.ExportLua, DATA_STREAM + ".lua");
            Util.SaveFile(path, builder.ToString());
        }
    }
}
