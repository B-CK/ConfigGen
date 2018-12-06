using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace ConfigGen.LocalInfo
{
    public class TableXmlInfo : TableInfo
    {
        private List<XmlElement> _datas = new List<XmlElement>();
        public TableXmlInfo(string absPath, ClassTypeInfo classType)
           : base(absPath, classType)
        {
            string[] files = Directory.GetFiles(absPath);
            XmlDocument doc = new XmlDocument();
            foreach (var f in files)
            {
                doc.Load(f);
                _datas.Add(doc.DocumentElement);
            }
        }

        public override void Analyze()
        {
            Datas = new List<DataClass>();
            var fieldDict = ClassTypeInfo.FieldDict;
            //逐行数据存储..
            for (int row = 0; row < _datas.Count; row++)
            {
                XmlElement node = _datas[row];
                DataClass dataClass = new DataClass();

                for (int colum = 0; colum < ClassTypeInfo.Fields.Count; colum++)
                {
                    //解析数据类字段
                    FieldInfo fieldInfo = ClassTypeInfo.Fields[colum];
                    Data dataField = AnalyzeField(node[fieldInfo.Name], fieldInfo);
                    dataClass.Fields.Add(fieldInfo.Name, dataField);
                }

                Datas.Add(dataClass);
            }
        }

        private Data AnalyzeField(XmlElement data, FieldInfo info)
        {
            Data dataField = null;
            BaseTypeInfo baseType = info.BaseInfo;
            switch (baseType.EType)
            {
                case TypeType.Base:
                    dataField = AnalyzeBase(data, info);
                    break;
                case TypeType.Enum:
                    dataField = AnalyzeEnum(data, info);
                    break;
                case TypeType.Class:
                    dataField = AnalyzeClass(data, info);
                    break;
                case TypeType.List:
                    dataField = AnalyzeList(data, info);
                    break;
                case TypeType.Dict:
                    dataField = AnalyzeDict(data, info);
                    break;
                case TypeType.None:
                default:
                    break;
            }
            return dataField;
        }
        private Data AnalyzeBase(XmlElement data, FieldInfo info)
        {
            DataBase dataBase = new DataBase();
            dataBase.Data = data.InnerText;
            return dataBase;
        }
        private Data AnalyzeEnum(XmlElement data, FieldInfo info)
        {
            DataBase dataBase = new DataBase();
            EnumTypeInfo enumType = info.BaseInfo as EnumTypeInfo;
            string key = data.InnerText;
            string value = enumType[key];
            if (string.IsNullOrWhiteSpace(value))
                Util.LogErrorFormat("Enum:{0} Value:{1} 不存在", enumType.GetFullName(), key);
            dataBase.Data = value;
            return dataBase;
        }
        private Data AnalyzeClass(XmlElement data, FieldInfo info)
        {
            ClassTypeInfo classType = info.BaseInfo as ClassTypeInfo;
            DataClass dataClass = new DataClass();

            if (classType.IsPolyClass)
            {
                XmlAttribute polymorphism = data.GetAttributeNode(Values.PolymorphismFlag);
                BaseTypeInfo bti = TypeInfo.GetBaseTypeInfo(classType.NamespaceName, polymorphism.Value);
                ClassTypeInfo subClassType = null;
                if (bti != null)
                {
                    subClassType = bti as ClassTypeInfo;
                    dataClass.Type = subClassType.GetFullName();
                }
                else
                    Util.LogErrorFormat("[解析多态]类型{0} 不存在或者未继承类型{1}再或者类型未定义在同一命名空间下!", polymorphism.Value, classType.GetFullName());
                AnalyzeParentClass(classType, dataClass, data);
                if (!classType.IsTheSame(subClassType))
                {
                    for (int i = 0; i < subClassType.Fields.Count; i++)
                    {
                        FieldInfo fieldInfo = subClassType.Fields[i];
                        Data dataField = AnalyzeField(data[fieldInfo.Name], fieldInfo);
                        dataClass.Fields.Add(fieldInfo.Name, dataField);
                    }
                }
            }
            else
            {
                AnalyzeParentClass(classType, dataClass, data);
            }

            return dataClass;
        }
        private void AnalyzeParentClass(ClassTypeInfo classType, DataClass dataClass, XmlElement data)
        {
            ClassTypeInfo parentClassType = classType.Inherit;
            if (parentClassType == null)
            {
                for (int i = 0; i < classType.Fields.Count; i++)
                {
                    FieldInfo fieldInfo = classType.Fields[i];
                    Data dataField = AnalyzeField(data[fieldInfo.Name], fieldInfo);
                    dataClass.Fields.Add(fieldInfo.Name, dataField);
                }
                return;
            }

            AnalyzeParentClass(parentClassType, dataClass, data);
            for (int i = 0; i < classType.Fields.Count; i++)
            {
                FieldInfo fieldInfo = classType.Fields[i];
                Data dataField = AnalyzeField(data[fieldInfo.Name], fieldInfo);
                dataClass.Fields.Add(fieldInfo.Name, dataField);
            }
        }
        private Data AnalyzeList(XmlElement data, FieldInfo info)
        {
            XmlNodeList list = data.ChildNodes;
            ListTypeInfo listType = info.BaseInfo as ListTypeInfo;
            FieldInfo element = listType.ItemInfo;

            DataList dataList = new DataList();
            for (int i = 0; i < list.Count; i++)
            {
                Data dataField = AnalyzeField(list[i] as XmlElement, element);
                dataList.Elements.Add(dataField);
            }
            return dataList;
        }
        private Data AnalyzeDict(XmlElement data, FieldInfo info)
        {
            XmlNodeList dict = data.ChildNodes;
            DictTypeInfo dictType = info.BaseInfo as DictTypeInfo;
            FieldInfo keyInfo = dictType.KeyInfo;
            FieldInfo valueInfo = dictType.ValueInfo;

            DataDict dataDict = new DataDict();
            for (int i = 0; i < dict.Count; i++)
            {
                XmlNode pair = dict[i];
                XmlElement key = pair[Values.KEY];
                XmlElement value = pair[Values.VALUE];
                DataBase dataKey = AnalyzeField(key, keyInfo) as DataBase;
                Data dataValue = AnalyzeField(value, valueInfo);

                dataDict.Pairs.Add(new KeyValuePair<DataBase, Data>(dataKey, dataValue));
            }

            return dataDict;
        }
    }
}
