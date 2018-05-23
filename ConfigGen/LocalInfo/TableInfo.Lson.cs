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
                ClassTypeInfo = classType;
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
            List<JObject> dt = _datas;
            DataClassInfo = new DataClassInfo();
            DataClassInfo.Set(ClassTypeInfo.Name, ClassTypeInfo.GetClassName(), null, null);

            DataClassInfo.Fields = new Dictionary<string, FieldInfo>();
            ClassTypeInfo classInfo = DataClassInfo.BaseInfo as ClassTypeInfo;
            for (int colum = 0; colum < classInfo.Fields.Count; colum++)
            {
                for (int row = 0; row < dt.Count; row++)
                {
                    //解析数据类字段
                    FieldInfo fieldInfo = classInfo.Fields[colum];
                    BaseTypeInfo baseType = fieldInfo.BaseInfo;
                    switch (baseType.TypeType)
                    {
                        case TypeType.Base:
                        case TypeType.Enum:
                            DataBaseInfo baseFieldInfo = new DataBaseInfo();
                            baseFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Check, fieldInfo.Group);
                            DataClassInfo.Fields.Add(baseFieldInfo.Name, baseFieldInfo);
                            AnalyzeBaseField(dt, baseFieldInfo);
                            break;
                        case TypeType.Class://类的检查规则需要自定义
                            DataClassInfo subClassFieldInfo = new DataClassInfo();
                            subClassFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Check, fieldInfo.Group);
                            DataClassInfo.Fields.Add(subClassFieldInfo.Name, subClassFieldInfo);
                            AnalyzeClassField(dt[row], subClassFieldInfo);
                            break;
                        case TypeType.List://集合中元素的规则统一指定,无法重写
                            DataListInfo listFieldInfo = new DataListInfo();
                            listFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Check, fieldInfo.Group);
                            DataClassInfo.Fields.Add(listFieldInfo.Name, listFieldInfo);
                            AnalyzeListField(dt, listFieldInfo);
                            break;
                        case TypeType.Dict://集合中元素的规则统一指定,无法重写
                            DataDictInfo dictFieldInfo = new DataDictInfo();
                            dictFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Check, fieldInfo.Group);
                            DataClassInfo.Fields.Add(dictFieldInfo.Name, dictFieldInfo);
                            AnalyzeDictField(dt, dictFieldInfo);
                            break;
                        case TypeType.None:
                        default:
                            break;
                    }
                }
            }

            for (int i = 0; i < dt.Count; i++)
            {
                AnalyzeClassField(dt[i], DataClassInfo);
            }
            DataLength = dt.Count;
        }
        private void AnalyzeClassField(JObject jObject, DataClassInfo classFieldInfo)
        {
            
            //----多态
            if (jClass.ContainsKey(Values.Polymorphism))
            {
                string type = jClass[Values.Polymorphism].Value<string>();
                type = type.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                type = type.Replace(Values.LsonRootNode + ".", "");
                ClassTypeInfo polyType = TypeInfo.GetTypeInfo(type) as ClassTypeInfo;
                //startColumn = AnalyzeClass(classfieldInfo, polyType, jClass);
            }
        }
        private void AnalyzeBaseField(List<JObject> dt, DataBaseInfo baseFieldInfo)
        {
            //BaseTypeInfo baseType = fieldInfo.BaseInfo;
            //switch (baseType.TypeType)
            //{
            //    case TypeType.Enum:
            //    case TypeType.Base:
            //        {
            //            AnalyzeData(fieldInfo, jObject[fieldInfo.Name]);
            //            break;
            //        }
            //    case TypeType.Class:
            //        {
            //            JObject jClass = jObject[fieldInfo.Name] as JObject;
            //            ClassTypeInfo classType = baseType as ClassTypeInfo;
            //            fieldInfo.ChildFields = new Dictionary<string, TableFieldInfo>();
            //            startColumn = AnalyzeClass(fieldInfo, classType, jClass);
            //            break;
            //        }
            //    case TypeType.List:
            //        {
            //            fieldInfo.ChildFields = new Dictionary<string, TableFieldInfo>();
            //            JArray jArray = jObject[fieldInfo.Name] as JArray;
            //            ListTypeInfo listTypeInfo = baseType as ListTypeInfo;
            //            BaseTypeInfo itemType = TypeInfo.GetTypeInfo(listTypeInfo.ItemType);
            //            int count = 0;
            //            for (int i = 0; i < jArray.Count; i++)
            //            {
            //                startColumn += count;
            //                TableFieldInfo itemInfo = new TableFieldInfo();
            //                itemInfo.AsChildSet(i.ToString(), itemType.GetClassName());
            //                count++;
            //                switch (itemType.TypeType)
            //                {
            //                    case TypeType.List:
            //                    case TypeType.Dict:
            //                        throw new Exception("Lson 数据List中禁止直接嵌套集合");
            //                    case TypeType.Class:
            //                        {
            //                            ClassTypeInfo classType = itemType as ClassTypeInfo;
            //                            JObject jClass = jArray[i] as JObject;
            //                            startColumn = AnalyzeClass(fieldInfo, classType, jClass);
            //                            break;
            //                        }
            //                    case TypeType.None:
            //                    case TypeType.Enum:
            //                    case TypeType.Base:
            //                    default:
            //                        AnalyzeData(itemInfo, jArray[i]);
            //                        break;
            //                }
            //                fieldInfo.ChildFields.Add(i.ToString(), itemInfo);
            //            }
            //            break;
            //        }
            //    case TypeType.Dict:
            //        {
            //            fieldInfo.ChildFields = new Dictionary<string, TableFieldInfo>();
            //            JObject jDict = jObject[fieldInfo.Name] as JObject;
            //            DictTypeInfo dictTypeInfo = baseType as DictTypeInfo;
            //            BaseTypeInfo keyType = TypeInfo.GetTypeInfo(dictTypeInfo.KeyType);
            //            BaseTypeInfo valueType = TypeInfo.GetTypeInfo(dictTypeInfo.ValueType);
            //            var properties = new List<JProperty>(jDict.Properties());
            //            int count = 0;
            //            for (int i = 0; i < properties.Count; i++)
            //            {
            //                var property = properties[i];
            //                TableFieldInfo pair = new TableFieldInfo();
            //                pair.ChildFields = new Dictionary<string, TableFieldInfo>();

            //                startColumn += count++;
            //                TableFieldInfo keyInfo = new TableFieldInfo();
            //                keyInfo.AsChildSet("key", keyType.GetClassName());
            //                AnalyzeData(keyInfo, property.Name);

            //                startColumn += count++;
            //                TableFieldInfo valueInfo = new TableFieldInfo();
            //                valueInfo.AsChildSet("value", valueType.GetClassName());
            //                switch (valueType.TypeType)
            //                {
            //                    case TypeType.List:
            //                    case TypeType.Dict:
            //                        throw new Exception("Lson 数据Dict中禁止直接嵌套集合");
            //                    case TypeType.Class:
            //                        {
            //                            ClassTypeInfo classType = valueType as ClassTypeInfo;
            //                            JObject jClass = property.Value as JObject;
            //                            startColumn = AnalyzeClass(fieldInfo, classType, jClass);
            //                            break;
            //                        }
            //                    case TypeType.None:
            //                    case TypeType.Base:
            //                    case TypeType.Enum:
            //                    default:
            //                        AnalyzeData(valueInfo, property.Value);
            //                        break;
            //                }

            //                pair.ChildFields.Add(keyInfo.Name, keyInfo);
            //                pair.ChildFields.Add(valueInfo.Name, valueInfo);
            //                fieldInfo.ChildFields.Add(i.ToString(), pair);
            //            }
            //            break;
            //        }
            //    case TypeType.None:
            //    default:
            //        break;
            //}
        }
        private void AnalyzeListField(List<JObject> dt, DataListInfo listFieldInfo)
        {
        }
        private void AnalyzeDictField(List<JObject> dt, DataDictInfo dictFieldInfo)
        {

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
