using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ConfigGen.LocalInfo
{
    public class TableLsonInfo : TableDataInfo
    {
        private List<JObject> _datas = new List<JObject>();
        public TableLsonInfo(string relPath, ClassTypeInfo classType)
           : base(relPath, null, classType)
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
        //json文件需要按索引排序,总是以固定顺序生成数据.
        public override void Analyze()
        {
            var dataFieldDict = new Dictionary<string, TableFieldInfo>();
            int startColumn = 0;
            for (int column = 0; column < DataClassInfo.Fields.Count; column++, startColumn++)
            {
                FieldInfo field = DataClassInfo.Fields[column];
                TableFieldInfo fieldInfo = new TableFieldInfo();
                fieldInfo.Set(field.Name, field.Type, field.Des, field.Check, field.Group, startColumn);
                fieldInfo.Data = new List<object>();

                for (int row = 0; row < _datas.Count; row++)
                {
                    startColumn = AnalyzeField(fieldInfo, _datas[row]);
                }

                if (!dataFieldDict.ContainsKey(fieldInfo.Name))
                    dataFieldDict.Add(fieldInfo.Name, fieldInfo);
                else
                    throw new Exception(string.Format("类型{0}中字段{1}重名", DataClassInfo.GetClassName(), fieldInfo.Name));
            }
            DataLength = _datas.Count;
            DataFields.AddRange(dataFieldDict.Values);
        }
        private int AnalyzeField(TableFieldInfo fieldInfo, JObject jObject)
        {
            int startColumn = fieldInfo.ColumnIndex;
            BaseTypeInfo baseType = fieldInfo.BaseInfo;
            switch (baseType.TypeType)
            {
                case TypeType.Enum:
                case TypeType.Base:
                    {
                        AnalyzeData(fieldInfo, jObject[fieldInfo.Name]);
                        break;
                    }
                case TypeType.Class:
                    {
                        JObject jClass = jObject[fieldInfo.Name] as JObject;
                        ClassTypeInfo classType = baseType as ClassTypeInfo;
                        fieldInfo.ChildFields = new Dictionary<string, TableFieldInfo>();
                        startColumn = AnalyzeClass(fieldInfo, classType, jClass);                        
                        break;
                    }
                case TypeType.List:
                    {
                        fieldInfo.ChildFields = new Dictionary<string, TableFieldInfo>();
                        JArray jArray = jObject[fieldInfo.Name] as JArray;
                        ListTypeInfo listTypeInfo = baseType as ListTypeInfo;
                        BaseTypeInfo itemType = TypeInfo.GetTypeInfo(listTypeInfo.ItemType);
                        int count = 0;
                        for (int i = 0; i < jArray.Count; i++)
                        {
                            startColumn += count;
                            TableFieldInfo itemInfo = new TableFieldInfo();
                            itemInfo.AsChildSet(i.ToString(), itemType.GetClassName(), startColumn);
                            count++;
                            switch (itemType.TypeType)
                            {
                                case TypeType.List:
                                case TypeType.Dict:
                                    throw new Exception("Lson 数据List中禁止直接嵌套集合");
                                case TypeType.Class:
                                    {
                                        ClassTypeInfo classType = itemType as ClassTypeInfo;
                                        JObject jClass = jArray[i] as JObject;
                                        startColumn = AnalyzeClass(fieldInfo, classType, jClass);
                                        break;
                                    }
                                case TypeType.None:
                                case TypeType.Enum:
                                case TypeType.Base:
                                default:
                                    AnalyzeData(itemInfo, jArray[i]);
                                    break;
                            }
                            fieldInfo.ChildFields.Add(i.ToString(), itemInfo);
                        }
                        break;
                    }
                case TypeType.Dict:
                    {
                        fieldInfo.ChildFields = new Dictionary<string, TableFieldInfo>();
                        JObject jDict = jObject[fieldInfo.Name] as JObject;
                        DictTypeInfo dictTypeInfo = baseType as DictTypeInfo;
                        BaseTypeInfo keyType = TypeInfo.GetTypeInfo(dictTypeInfo.KeyType);
                        BaseTypeInfo valueType = TypeInfo.GetTypeInfo(dictTypeInfo.ValueType);
                        var properties = new List<JProperty>(jDict.Properties());
                        int count = 0;
                        for (int i = 0; i < properties.Count; i++)
                        {
                            var property = properties[i];
                            TableFieldInfo pair = new TableFieldInfo();
                            pair.ChildFields = new Dictionary<string, TableFieldInfo>();

                            startColumn += count++;
                            TableFieldInfo keyInfo = new TableFieldInfo();
                            keyInfo.AsChildSet("key", keyType.GetClassName(), startColumn);
                            AnalyzeData(keyInfo, property.Name);

                            startColumn += count++;
                            TableFieldInfo valueInfo = new TableFieldInfo();
                            valueInfo.AsChildSet("value", valueType.GetClassName(), startColumn);
                            switch (valueType.TypeType)
                            {
                                case TypeType.List:
                                case TypeType.Dict:
                                    throw new Exception("Lson 数据Dict中禁止直接嵌套集合");
                                case TypeType.Class:
                                    {
                                        ClassTypeInfo classType = valueType as ClassTypeInfo;
                                        JObject jClass = property.Value as JObject;
                                        startColumn = AnalyzeClass(fieldInfo, classType, jClass);
                                        break;
                                    }
                                case TypeType.None:
                                case TypeType.Base:
                                case TypeType.Enum:
                                default:
                                    AnalyzeData(valueInfo, property.Value);
                                    break;
                            }

                            pair.ChildFields.Add(keyInfo.Name, keyInfo);
                            pair.ChildFields.Add(valueInfo.Name, valueInfo);
                            fieldInfo.ChildFields.Add(i.ToString(), pair);
                        }
                        break;
                    }
                case TypeType.None:
                default:
                    break;
            }
            return startColumn;
        }
        private int AnalyzeClass(TableFieldInfo classfieldInfo, ClassTypeInfo classType, JObject jClass)
        {
            int startColumn = classfieldInfo.ColumnIndex + 1;
            int count = 0;
            for (int i = 0; i < classType.Fields.Count; i++)
            {
                startColumn += count;
                FieldInfo field = classType.Fields[i];
                TableFieldInfo subFieldInfo = new TableFieldInfo();
                subFieldInfo.Set(field.Name, field.Type, field.Des, field.Check, field.Group, startColumn);
                count++;

                startColumn = AnalyzeField(subFieldInfo, jClass);

                if (!classfieldInfo.ChildFields.ContainsKey(subFieldInfo.Name))
                    classfieldInfo.ChildFields.Add(subFieldInfo.Name, subFieldInfo);
                else
                    throw new Exception(string.Format("类型{0}中字段{1}重名", classType.GetClassName(), subFieldInfo.Name));
            }
            if (jClass.ContainsKey(Values.Polymorphism))
            {
                string type = jClass[Values.Polymorphism].Value<string>();
                type = type.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                type = type.Replace(Values.LsonRootNode + ".", "");
                ClassTypeInfo polyType = TypeInfo.GetTypeInfo(type) as ClassTypeInfo;
                //startColumn = AnalyzeClass(classfieldInfo, polyType, jClass);
            }

            return startColumn;
        }
        private void AnalyzeData(TableFieldInfo fieldInfo, JToken token)
        {
            string error = TableChecker.CheckFieldData(fieldInfo, token);
            if (string.IsNullOrWhiteSpace(error))
            {
                if (fieldInfo.Data == null)
                    fieldInfo.Data = new List<object>();
                fieldInfo.Data.Add(token);
            }
            else
                throw new Exception(string.Format("{0}类型字段{1}解析错误,{2}", fieldInfo.Type, fieldInfo.Name, error));
        }

        public override bool Exist(string content)
        {
            return true;
        }
        public override bool Replace(string arg1, string arg2)
        {
            return true;
        }
    }
}
