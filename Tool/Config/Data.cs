using Tool.Import;
using System.Xml;
using Tool.Wrap;
using System.Collections.Generic;
using System.IO;

namespace Tool.Config
{
    /// <summary>
    /// 数据导出控制类
    /// </summary>
    public abstract class Data
    {
        /// <summary>
        /// list,dict集合中元素的host为集合的host,而不是集合本身
        /// </summary>
        public FClass Host { get { return _host; } }
        public FieldWrap Define { get { return _define; } }
        /// <summary>
        /// 是否为数据根节点,即数据表最顶层结构.
        /// FList:为ConfigWrap的数据载体,包含完整表数据
        /// FClass:表中一条数据
        /// 以上均为根节点,FList只能通过ConfigWrap获取,而FClass需要遍历树节点
        /// </summary>
        public bool IsRoot => _host == null;

        protected FClass _host;
        protected FieldWrap _define;

        public Data(FClass host, FieldWrap define)
        {
            _define = define;
            _host = host;
        }

        public virtual void ImportData(XmlElement xml) { }
        public virtual void ImportData(ImportExcel excel) { }
        public abstract string ExportData();
        public abstract int ExportBinary(ref byte[] bytes, int offset);
        /// <summary>
        /// 逐数据的检查
        /// </summary>
        public virtual void VerifyData()
        {
            var checkers = _define.Checkers;
            if (checkers == null) return;

            for (int k = 0; k < checkers.Count; k++)
            {
                var checker = checkers[k];
                if (!checker.VerifyData(this))
                    checker.OutputError(this);
            }
        }

        #region 创建数据
        public static Data Create(FClass host, FieldWrap define, ImportExcel excel)
        {
            string type = define.OriginalType;
            if (define.IsRaw)
            {
                switch (type)
                {
                    case Setting.BOOL:
                        return new FBool(host, define, excel);
                    case Setting.INT:
                        return new FInt(host, define, excel);
                    case Setting.LONG:
                        return new FLong(host, define, excel);
                    case Setting.FLOAT:
                        return new FFloat(host, define, excel);
                    case Setting.STRING:
                        return new FString(host, define, excel);
                }
            }
            else if (define.IsEnum)
                return new FEnum(host, define, excel);
            else if (define.IsClass)
            {
                if (!define.IsDynamic)
                    return new FClass(host, define, excel);

                string dType = excel.GetString();
                ClassWrap cls = ClassWrap.Get(define.FullName);
                dType = ClassWrap.CorrectType(cls, dType);
                ClassWrap dynamic = ClassWrap.Get(dType);
                if (dynamic == null)
                    excel.Error("多态类型" + dType + "未知");
                if (cls.FullName != dType && !cls.HasChild(dType))
                    excel.Error(string.Format("数据类型{0}非{1}子类", dType, cls.FullName));
                var define0 = new FieldWrap(define.Host, define.Name, dType, new string[] { define.FullName }, define.Group);
                return new FClass(host, define0, excel);
            }
            else if (define.IsContainer)
            {
                if (define.OriginalType == "list")
                    return new FList(host, define, excel);
                else if (define.OriginalType == "dict")
                    return new FDict(host, define, excel);
            }

            Util.LogError("未知类型" + type);
            return null;
        }
        public static Data Create(FClass host, FieldWrap define, XmlElement xml)
        {
            string type = define.OriginalType;
            if (define.IsRaw)
            {
                switch (type)
                {
                    case Setting.BOOL:
                        return new FBool(host, define, xml);
                    case Setting.INT:
                        return new FInt(host, define, xml);
                    case Setting.LONG:
                        return new FLong(host, define, xml);
                    case Setting.FLOAT:
                        return new FFloat(host, define, xml);
                    case Setting.STRING:
                        return new FString(host, define, xml);
                }
            }
            else if (define.IsEnum)
            {
                return new FEnum(host, define, xml);
            }
            else if (define.IsClass)
            {
                if (!define.IsDynamic)
                    return new FClass(host, define, xml);

                string dType = xml.GetAttribute("Type");
                ClassWrap cls = ClassWrap.Get(define.FullName);
                dType = ClassWrap.CorrectType(cls, dType);
                ClassWrap dynamic = ClassWrap.Get(dType);
                if (dynamic == null)
                    Util.Error("多态类型" + dType + "未知");
                if (cls.FullName != dType && !cls.HasChild(dType))
                    Util.Error(string.Format("数据类型{0}非{1}子类", dType, cls.FullName));
                var define0 = new FieldWrap(define.Host, define.Name, dType, new string[] { define.FullName }, define.Group);
                return new FClass(host, define0, xml);
            }
            else if (define.IsContainer)
            {
                if (define.OriginalType == "list")
                {
                    FList data = new FList(host, define);
                    data.LoadMultiRecord(xml);
                    return data;
                }
                else if (define.OriginalType == "dict")
                {
                    FDict data = new FDict(host, define, xml);
                    return data;
                }
            }

            Util.LogError("未知类型" + type);
            return null;
        }

        public override bool Equals(object obj)
        {
            return obj is Data data &&
                   EqualityComparer<FClass>.Default.Equals(_host, data._host) &&
                   EqualityComparer<FieldWrap>.Default.Equals(_define, data._define);
        }
        public override int GetHashCode()
        {
            var hashCode = -433025342;
            hashCode = hashCode * -1521134295 + EqualityComparer<FClass>.Default.GetHashCode(_host);
            hashCode = hashCode * -1521134295 + EqualityComparer<FieldWrap>.Default.GetHashCode(_define);
            return hashCode;
        }
        #endregion
    }
}
