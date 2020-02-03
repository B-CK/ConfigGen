using Tool.Import;
using System.Xml;
using Tool.Wrap;

namespace Tool.Config
{
    /// <summary>
    /// 数据导出控制类
    /// </summary>
    public abstract class Data
    {
        public FClass Host { get { return _host; } }
        public FieldWrap Define { get { return _define; } }

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
        public virtual void VerifyData()
        {
            if (_define.Refs == null) return;
            for (int i = 0; i < _define.Refs.Length; i++)
            {
                string type = _define.Refs[i];
                if (type.IndexOfAny(Setting.DotSplit) < 0)
                    type = string.Format("{0}.{1}", _define.Host.Namespace, type);
                ConfigWrap cfg = ConfigWrap.Get(type);
                if (!FList.ContainsIndex(cfg.Index, this))
                    Util.LogWarningFormat("Class:{0} {1} {2}主键无法在{3}找到",
                        _host.FullType, _define, ExportData(), type);
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
                ClassWrap cls = ClassWrap.Get(define.FullType);
                dType = ClassWrap.CorrectType(cls, dType);
                ClassWrap dynamic = ClassWrap.Get(dType);
                if (dynamic == null)
                    excel.Error("多态类型" + dType + "未知");
                if (cls.FullType != dType && !cls.HasChild(dType))
                    excel.Error(string.Format("数据类型{0}非{1}子类", dType, cls.FullType));
                var define0 = new FieldWrap(define.Host, define.Name, dType, new string[] { define.FullType }, define.Group);
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
                ClassWrap cls = ClassWrap.Get(define.FullType);
                dType = ClassWrap.CorrectType(cls, dType);
                ClassWrap dynamic = ClassWrap.Get(dType);
                if (dynamic == null)
                    Util.Error("多态类型" + dType + "未知");
                if (cls.FullType != dType && !cls.HasChild(dType))
                    Util.Error(string.Format("数据类型{0}非{1}子类", dType, cls.FullType));
                var define0 = new FieldWrap(define.Host, define.Name, dType, new string[] { define.FullType }, define.Group);
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
        #endregion
    }
}
