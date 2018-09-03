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
            dataBase.Data = data.Value;
            return dataBase;
        }
        private Data AnalyzeEnum(XmlElement data, FieldInfo info)
        {
            DataBase dataBase = new DataBase();
            EnumTypeInfo enumType = info.BaseInfo as EnumTypeInfo;
            string value = data.Value;
            if (!enumType.EnumDict.ContainsKey(data.Value))
                Util.LogErrorFormat("Enum:{0} Value:{1}不存在", enumType.GetFullName(), data.Value);
            dataBase.Data = value;
            return dataBase;
        }
        private Data AnalyzeClass(XmlElement data, FieldInfo info)
        {
            ClassTypeInfo classType = info.BaseInfo as ClassTypeInfo;
            DataClass dataClass = new DataClass();

            for (int i = 0; i < classType.Fields.Count; i++)
            {
                FieldInfo fieldInfo = classType.Fields[i];
                Data dataField = AnalyzeField(data[fieldInfo.Name], fieldInfo);
                dataClass.Fields.Add(fieldInfo.Name, dataField);
            }

            if (classType.Inherit != null)
            {
                XmlAttribute polymorphism = data.GetAttributeNode(Values.PolymorphismFlag);
                ClassTypeInfo polyType = classType.GetSubClass(polymorphism.Value);
                if (polyType != null)
                {
                    dataClass.Type = polyType.GetFullName();
                    for (int i = 0; i < polyType.Fields.Count; i++)
                    {
                        FieldInfo fieldInfo = polyType.Fields[i];
                        Data dataField = AnalyzeField(data[fieldInfo.Name], fieldInfo);
                        dataClass.Fields.Add(fieldInfo.Name, dataField);
                    }
                }
                else
                    dataClass.Type = classType.GetFullName();
            }

            return dataClass;
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
