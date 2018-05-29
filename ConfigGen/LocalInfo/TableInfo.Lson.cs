using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ConfigGen.LocalInfo
{
    /// <summary>
    /// List:集合中的元素无检查规则
    /// Dict:集合中的元素无检查规则
    /// </summary>
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

            string absPath = Util.GetConfigAbsPath(relPath);
            string[] files = Directory.GetFiles(absPath);
            foreach (var f in files)
            {
                string json = File.ReadAllText(f);
                JObject jObject = JObject.Parse(json);
                _datas.Add(jObject);
            }
        }
        //json文件需要按索引排序,总是以固定顺序生成数据.
        //逐行读取数据,逐列填充,不填充空数据
        public override void Analyze()
        {
            List<JObject> dt = _datas;
            DataClassInfo = new DataClassInfo();
            DataClassInfo.Set(ClassTypeInfo.Name, ClassTypeInfo.GetClassName(), null, null);

            ClassTypeInfo classInfo = DataClassInfo.BaseInfo as ClassTypeInfo;
            //逐行数据存储..
            for (int colum = 0; colum < classInfo.Fields.Count; colum++)
            {
                for (int row = 0; row < dt.Count; row++)
                {
                    //解析数据类字段
                    JObject jClass = dt[row];
                    FieldInfo fieldInfo = classInfo.Fields[colum];

                    AnalyzeField(jClass, DataClassInfo, fieldInfo);
                }
            }
            DataLength = dt.Count;
        }

        /// <summary>
        /// 解析字段
        /// </summary>
        /// <param name="data">类的数据</param>
        /// <param name="dataClass">类</param>
        /// <param name="fieldInfo">类中字段</param>
        private void AnalyzeField(JToken data, DataClassInfo dataClass, FieldInfo fieldInfo)
        {
            BaseTypeInfo baseType = fieldInfo.BaseInfo;
            switch (baseType.TypeType)
            {
                case TypeType.Base:
                case TypeType.Enum:
                    AnalyzeBaseField(dataClass, fieldInfo, data[fieldInfo.Name]);
                    break;
                case TypeType.Class://类的检查规则需要自定义
                    AnalyzeClassField(dataClass, fieldInfo, data[fieldInfo.Name]);
                    break;
                case TypeType.List://集合中元素的规则统一指定,无法重写                          
                    AnalyzeListField(dataClass, fieldInfo, data[fieldInfo.Name]);
                    break;
                case TypeType.Dict://集合中元素的规则统一指定,无法重写
                    AnalyzeDictField(dataClass, fieldInfo, data[fieldInfo.Name]);
                    break;
                case TypeType.None:
                default:
                    break;
            }
        }
        //一下接受的均为指定字段的数据
        private void AnalyzeBaseField(DataClassInfo dataClass, FieldInfo fieldInfo, JToken data)
        {
            DataBaseInfo baseFieldInfo = null;
            if (!dataClass.Fields.ContainsKey(fieldInfo.Name))
            {
                baseFieldInfo = new DataBaseInfo();
                baseFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Check, fieldInfo.Group);
                dataClass.Fields.Add(baseFieldInfo.Name, baseFieldInfo);
                baseFieldInfo.Data = new List<object>();
            }
            else
            {
                baseFieldInfo = dataClass.Fields[fieldInfo.Name] as DataBaseInfo;
            }
            EnumTypeInfo enumType = baseFieldInfo.BaseInfo as EnumTypeInfo;
            if (enumType != null)
            {
                string value = data.Value<string>();
                if (enumType.EnumDict.ContainsValue(value))
                    data = value;
                else
                    Util.LogErrorFormat("枚举{0}中不包含{1}值,字段名:{2},{3}", enumType.GetClassName(), fieldInfo.Name, RelPath);
            }
            baseFieldInfo.Data.Add(data);
        }
        private void AnalyzeClassField(DataClassInfo dataClass, FieldInfo fieldInfo, JToken data)
        {
            JObject jClass = data as JObject;
            ClassTypeInfo classTypeInfo = fieldInfo.BaseInfo as ClassTypeInfo;
            DataClassInfo classFieldInfo = null;
            if (!dataClass.Fields.ContainsKey(fieldInfo.Name))
            {
                classFieldInfo = new DataClassInfo();
                classFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Check, fieldInfo.Group);
                dataClass.Fields.Add(classFieldInfo.Name, classFieldInfo);
            }
            else
            {
                classFieldInfo = dataClass.Fields[fieldInfo.Name] as DataClassInfo;
            }

            AnalyzeClassInfo(classFieldInfo, classTypeInfo, data);
        }
        private void AnalyzeListField(DataClassInfo dataClass, FieldInfo fieldInfo, JToken data)
        {
            JArray array = data as JArray;
            ListTypeInfo listTypeInfo = fieldInfo.BaseInfo as ListTypeInfo;
            DataListInfo listFieldInfo = null;
            if (!dataClass.Fields.ContainsKey(fieldInfo.Name))
            {
                listFieldInfo = new DataListInfo();
                listFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Check, fieldInfo.Group);
                dataClass.Fields.Add(listFieldInfo.Name, listFieldInfo);
            }
            else
            {
                listFieldInfo = dataClass.Fields[fieldInfo.Name] as DataListInfo;
            }

            BaseTypeInfo elemType = TypeInfo.GetTypeInfo(listTypeInfo.ItemType);
            switch (elemType.TypeType)
            {
                case TypeType.List:
                case TypeType.Dict:
                    throw new Exception("Lson 数据List中禁止直接嵌套集合");
                case TypeType.Class:
                    {
                        for (int i = 0; i < array.Count; i++)//i - 集合索引号,表中列号
                        {
                            FieldInfo field = listFieldInfo.DataSet.Find(e => e.Name == i.ToString());
                            ClassTypeInfo classType = elemType as ClassTypeInfo;
                            DataClassInfo elemClass = null;
                            if (field == null)
                            {
                                elemClass = new DataClassInfo();
                                elemClass.Set(i.ToString(), elemType.GetClassName(), null, elemType.Group);
                                listFieldInfo.DataSet.Add(elemClass);
                            }
                            else
                            {
                                elemClass = field as DataClassInfo;
                            }

                            AnalyzeClassInfo(elemClass, classType, array[i]);
                        }
                        if (array.Count == 0)
                        {
                            FieldInfo field = listFieldInfo.DataSet.Find(e => e.Name == "0");
                            ClassTypeInfo classType = elemType as ClassTypeInfo;
                            DataClassInfo elemClass = null;
                            if (field == null)
                            {
                                elemClass = new DataClassInfo();
                                elemClass.Set("0", elemType.GetClassName(), null, elemType.Group);
                                listFieldInfo.DataSet.Add(elemClass);
                            }
                            else
                            {
                                elemClass = field as DataClassInfo;
                            }

                        }
                        break;
                    }
                case TypeType.None:
                case TypeType.Enum:
                case TypeType.Base:
                default:
                    {
                        for (int i = 0; i < array.Count; i++)//i - 集合索引号,表中列号
                        {
                            FieldInfo field = listFieldInfo.DataSet.Find(e => e.Name == i.ToString());
                            DataBaseInfo element = null;
                            if (field == null)
                            {
                                element = new DataBaseInfo();
                                element.Set(i.ToString(), elemType.GetClassName(), null, elemType.Group);
                                listFieldInfo.DataSet.Add(element);
                            }
                            else
                            {
                                element = field as DataBaseInfo;
                            }
                            element.Data.Add(array[i]);
                        }
                        if (array.Count == 0)
                        {
                            FieldInfo field = listFieldInfo.DataSet.Find(e => e.Name == "0");
                            DataBaseInfo element = null;
                            if (field == null)
                            {
                                element = new DataBaseInfo();
                                element.Set("0", elemType.GetClassName(), null, elemType.Group);
                                listFieldInfo.DataSet.Add(element);
                            }
                            else
                            {
                                element = field as DataBaseInfo;
                            }
                            element.Data.Add(Values.DataSetEndFlag);
                        }
                        break;
                    }
            }
        }
        private void AnalyzeDictField(DataClassInfo dataClass, FieldInfo fieldInfo, JToken data)
        {
            JObject jDict = data as JObject;
            DictTypeInfo dictTypeInfo = fieldInfo.BaseInfo as DictTypeInfo;
            DataDictInfo dictFieldInfo = null;
            if (!dataClass.Fields.ContainsKey(fieldInfo.Name))
            {
                dictFieldInfo = new DataDictInfo();
                dictFieldInfo.Set(fieldInfo.Name, fieldInfo.Type, fieldInfo.Check, fieldInfo.Group);
                dataClass.Fields.Add(dictFieldInfo.Name, dictFieldInfo);
            }
            else
            {
                dictFieldInfo = dataClass.Fields[fieldInfo.Name] as DataDictInfo;
            }

            BaseTypeInfo keyType = TypeInfo.GetTypeInfo(dictTypeInfo.KeyType);
            BaseTypeInfo valueType = TypeInfo.GetTypeInfo(dictTypeInfo.ValueType);
            var properties = new List<JProperty>(jDict.Properties());
            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                KeyValuePair<DataBaseInfo, FieldInfo> pair;
                if (i < dictFieldInfo.DataSet.Count)
                    pair = dictFieldInfo.DataSet[i];
                else
                {
                    pair = new KeyValuePair<DataBaseInfo, FieldInfo>();
                    dictFieldInfo.DataSet.Add(pair);
                }

                DataBaseInfo keyData = null;
                if (pair.Key == null)
                {
                    keyData = new DataBaseInfo();
                    keyData.Set(Values.KEY, keyType.GetClassName(), null, keyType.Group);
                    keyData.Data = new List<object>();
                }
                else
                {
                    keyData = pair.Key;
                }
                keyData.Data.Add(property.Name);

                switch (valueType.TypeType)
                {
                    case TypeType.List:
                    case TypeType.Dict:
                        throw new Exception("Lson 数据List中禁止直接嵌套集合");
                    case TypeType.Class:
                        {
                            ClassTypeInfo valueClass = valueType as ClassTypeInfo;
                            DataClassInfo valueData = null;
                            if (pair.Value == null)
                            {
                                valueData = new DataClassInfo();
                                valueData.Set(Values.VALUE, valueType.GetClassName(), null, valueType.Group);
                            }
                            else
                            {
                                valueData = pair.Value as DataClassInfo;
                            }


                            AnalyzeClassInfo(valueData, valueClass, property.Value);
                            pair = new KeyValuePair<DataBaseInfo, FieldInfo>(keyData, valueData);
                            break;
                        }
                    case TypeType.None:
                    case TypeType.Enum:
                    case TypeType.Base:
                    default:
                        {
                            DataBaseInfo valueData = null;
                            if (pair.Value == null)
                            {
                                valueData = new DataBaseInfo();
                                valueData.Set(Values.VALUE, valueType.GetClassName(), null, valueType.Group);
                            }
                            else
                            {
                                valueData = pair.Value as DataBaseInfo;
                            }
                            valueData.Data.Add(property.Value);
                            pair = new KeyValuePair<DataBaseInfo, FieldInfo>(keyData, valueData);
                            break;
                        }
                }
                dictFieldInfo.DataSet[i] = pair;
            }
        }
        private void AnalyzeClassInfo(DataClassInfo dataClass, ClassTypeInfo classType, JToken data)
        {
            JObject jClass = data as JObject;
            for (int colum = 0; colum < classType.Fields.Count; colum++)
            {
                FieldInfo field = classType.Fields[colum];
                AnalyzeField(jClass, dataClass, field);
            }

            if (classType.HasSubClass)
            {
                DataClassInfo polyFieldInfo = dataClass;
                if (jClass.ContainsKey(Values.Polymorphism))
                {
                    string type = jClass[Values.Polymorphism].Value<string>();
                    type = type.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                    type = type.Replace(Values.LsonRootNode + ".", "");
                    ClassTypeInfo polyType = TypeInfo.GetTypeInfo(type) as ClassTypeInfo;
                    polyFieldInfo.Types.Add(type);
                    for (int colum = 0; colum < polyType.Fields.Count; colum++)
                    {
                        FieldInfo field = polyType.Fields[colum];
                        AnalyzeField(jClass, polyFieldInfo, field);
                    }
                }
                else
                    polyFieldInfo.Types.Add(classType.GetClassName());
            }
        }
        private void AddNullClassInfo(DataClassInfo dataClass, ClassTypeInfo classType)
        {
            for (int colum = 0; colum < classType.Fields.Count; colum++)
            {
                FieldInfo field = classType.Fields[colum];
                DataBaseInfo dataBase = null;
                if (!dataClass.Fields.ContainsKey(field.Name))
                {
                    dataBase = new DataBaseInfo();
                    dataBase.Set(field.Name, field.Type, field.Check, field.Group);
                }
                else
                {
                    dataBase = field as DataBaseInfo;
                }
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
    }
}
