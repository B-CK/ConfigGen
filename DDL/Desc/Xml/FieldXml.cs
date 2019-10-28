﻿using System.Xml.Serialization;

namespace Desc.Xml
{
    [XmlInclude(typeof(MemberXml))]
    public abstract class MemberXml
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Desc;
        [XmlAttribute]
        public string Group;
    }

    [XmlInclude(typeof(FieldXml))]
    public class FieldXml : MemberXml
    {
        [XmlAttribute]
        public string Type;
        //[XmlAttribute]
        //public bool IsConst;

        /// <summary>
        /// List<T>:T类型
        /// Dict<K,V>:K:V类型
        /// </summary>
        [XmlAttribute]
        public string Value;
        [XmlAttribute]
        public string Checker;//引用资源相对路径
    }

    [XmlInclude(typeof(EnumItemXml))]
    public class EnumItemXml : MemberXml
    {
        [XmlAttribute]
        public int Value;
        [XmlAttribute]
        public string Alias;
    }
}
