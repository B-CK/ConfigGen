﻿using ConfigGen.Import;
using ConfigGen.TypeInfo;
using System.Xml;

namespace ConfigGen.Config
{
    /// <summary>
    /// 数据导出控制类
    /// </summary>
    public abstract class Data
    {
        public FClass Host { get { return _host; } }
        public FieldInfo Define { get { return _define; } }

        protected FClass _host;
        protected FieldInfo _define;

        public Data(FClass host, FieldInfo define)
        {
            _define = define;
            _host = host;
        }

        public virtual void ImportData(XmlElement xml) { }
        public virtual void ImportData(ImportExcel excel) { }
        public abstract string ExportData();
        public virtual void CheckData(Data data)
        {

        }

        #region 创建数据
        public static Data Create(FClass host, FieldInfo define, ImportExcel excel)
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
                ClassInfo cls = ClassInfo.Get(define.FullType);
                dType = ClassInfo.CorrectType(cls, dType);
                ClassInfo dynamic = ClassInfo.Get(dType);
                if (dynamic == null)
                    excel.Error("多态类型" + dType + "未知");
                if (cls.FullName != dType && !cls.HasChild(dType))
                    excel.Error(string.Format("数据类型{0}非{1}子类", dType, cls.FullName));
                var define0 = new FieldInfo(define.Host, define.Name, dType, define.Group);
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
        public static Data Create(FClass host, FieldInfo define, XmlElement xml)
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
                ClassInfo cls = ClassInfo.Get(define.FullType);
                dType = ClassInfo.CorrectType(cls, dType);
                ClassInfo dynamic = ClassInfo.Get(dType);
                if (dynamic == null)
                    Util.Error("多态类型" + dType + "未知");
                if (cls.FullName != dType && !cls.HasChild(dType))
                    Util.Error(string.Format("数据类型{0}非{1}子类", dType, cls.FullName));
                var define0 = new FieldInfo(define.Host, define.Name, dType, define.Group);
                return new FClass(host, define0, xml);
            }
            else if (define.IsContainer)
            {
                if (define.OriginalType == "list")
                {
                    FList data = new FList(host, define, xml);
                }
                else if (define.OriginalType == "dict")
                {
                    FDict data = new FDict(host, define, xml);
                }
            }

            Util.LogError("未知类型" + type);
            return null;
        }
        #endregion
    }
}