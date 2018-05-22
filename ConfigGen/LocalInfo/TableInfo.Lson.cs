using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ConfigGen.LocalInfo
{
    public class TableLsonInfo : TableInfo
    {
        public const string TYPE = "$type";
        public ClassTypeInfo DataClassInfo { get; private set; }
        public List<TableFieldInfo> DataFields { get; private set; }
        private List<JObject> _datas = new List<JObject>();
        public TableLsonInfo(string relPath, ClassTypeInfo classType)
           : base(relPath, null)
        {
            if (classType != null)
                DataClassInfo = classType;
            else
                Util.LogErrorFormat("Lson数据没有指明类型,文件夹名:{0}", RelPath);

            string[] files = Directory.GetFiles(relPath);
            foreach (var f in files)
            {
                string json = File.ReadAllText(f);
                JObject jObject = JObject.Parse(json);
                _datas.Add(jObject);
            }
        }
        public override void Analyze()
        {
            var dataFieldDict = new Dictionary<string, TableFieldInfo>();
            for (int i = 0; i < DataClassInfo.Fields.Count; i++)
            {
                FieldInfo field = DataClassInfo.Fields[i];
                TableFieldInfo tableFieldInfo = new TableFieldInfo();
                tableFieldInfo.Set(field.Name, field.Type, field.Des, field.Check, field.Group, i);

                for (int j = 0; j < _datas.Count; j++)
                {
                    AnalyzeField(tableFieldInfo, _datas[i]);
                }

                if (!dataFieldDict.ContainsKey(tableFieldInfo.Name))
                    dataFieldDict.Add(tableFieldInfo.Name, tableFieldInfo);
                else
                    throw new Exception(string.Format("类型{0}中字段{1}重名", DataClassInfo.GetClassName(), tableFieldInfo.Name));
            }
            DataFields.AddRange(dataFieldDict.Values);
        }
        private void AnalyzeField(TableFieldInfo fieldInfo, JObject jObject)
        {
            fieldInfo.Data = new List<object>();
            BaseTypeInfo baseType = fieldInfo.BaseInfo;
            switch (baseType.TypeType)
            {
                case TypeType.Enum:
                case TypeType.Base:
                    {
                        object value = jObject[fieldInfo.Name];
                        string error = TableChecker.CheckFieldData(fieldInfo, value);
                        if (string.IsNullOrWhiteSpace(error))
                            fieldInfo.Data.Add(value);
                        else
                            throw new Exception(string.Format("类型{0}中字段{1}错误,{2}", DataClassInfo.GetClassName(), fieldInfo.Name, error));
                        break;
                    }
                case TypeType.Class:
                    {
                        List<TableFieldInfo> fieldInfos = fieldInfo.ChildFields;
                        for (int i = 0; i < fieldInfos.Count; i++)
                        {

                        }
                        break;
                    }
                case TypeType.List:
                    {
                        ListTypeInfo listTypeInfo = baseType as ListTypeInfo;
                        switch (listTypeInfo.TypeType)
                        {
                            case TypeType.Enum:
                            case TypeType.Base:
                            case TypeType.Class:

                                break;
                            case TypeType.List:
                            case TypeType.Dict:
                            case TypeType.None:
                            default:
                                throw new Exception(string.Format("Lson解析{0}类型字段异常", baseType.GetClassName()));
                        }
                        break;
                    }
                case TypeType.Dict:
                    {
                        break;
                    }
                case TypeType.None:
                default:
                    break;
            }
        }


        public override bool Exist(string content)
        {
            return true;
        }
        public override bool Replace(string arg1, string arg2)
        {
            return true;
        }



        // case JTokenType.Object:
        //        break;
        //    case JTokenType.Array:
        //        {
        //    if (fieldInfo.BaseInfo.TypeType == TypeType.Class
        //        || fieldInfo.BaseInfo.TypeType == TypeType.Dict)
        //    {

        //    }
        //    else
        //    {

        //    }
        //    break;
        //}
        //    case JTokenType.Integer:
        //    case JTokenType.Float:
        //    case JTokenType.String:
        //    case JTokenType.Boolean:
        //        {
        //    object value = jObject[fieldInfo.Name];
        //    string error = TableChecker.CheckFieldData(fieldInfo, value);
        //    if (string.IsNullOrWhiteSpace(error))
        //        fieldInfo.Data.Add(value);
        //    else
        //        throw new Exception(string.Format("类型{0}中字段{1}错误,{2}", DataClassInfo.GetClassName(), fieldInfo.Name, error));
        //    break;
        //}
        //    case JTokenType.None:
        //    default:
        //        throw new Exception(string.Format("类型{0}中字段{1}类型{2}无法识别", DataClassInfo.GetClassName(), fieldInfo.Name, fieldInfo.Type));
    }
}
