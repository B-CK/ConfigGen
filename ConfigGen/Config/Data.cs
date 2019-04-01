using ConfigGen.Import;
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



        #region 创建数据
        public static Data Create(FClass host, FieldInfo define, ImportExcel excel)
        {
            string type = define.OriginalType;
            if (define.IsRaw)
            {
                switch (type)
                {
                    case Consts.BOOL:
                        return new FBool(host, define, excel.GetBool());
                    case Consts.INT:
                        return new FInt(host, define, excel.GetInt());
                    case Consts.LONG:
                        return new FLong(host, define, excel.GetLong());
                    case Consts.FLOAT:
                        return new FFloat(host, define, excel.GetFloat());
                    case Consts.STRING:
                        return new FString(host, define, excel.GetString());
                }
            }
            else if (define.IsEnum)
            {
                return new FEnum(host, define, excel.GetString());
            }
            else if (define.IsClass)
            {
                FClass data = new FClass(host, define);
                excel.GetClass(data, ClassInfo.Get(define.FullType));
            }
            else if (define.IsContainer)
            {
                if (define.OriginalType == "list")
                {
                    FList data = new FList(host, define);
                    excel.GetList(data, define);
                }
                else if (define.OriginalType == "dict")
                {
                    FDict data = new FDict(host, define);
                    excel.GetDict(data, define);
                }
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
                    case Consts.BOOL:
                        return new FBool(host, define, xml);
                    case Consts.INT:
                        return new FInt(host, define, xml);
                    case Consts.LONG:
                        return new FLong(host, define, xml);
                    case Consts.FLOAT:
                        return new FFloat(host, define, xml);
                    case Consts.STRING:
                        return new FString(host, define, xml);
                }
            }
            else if (define.IsEnum)
            {
                return new FEnum(host, define, xml);
            }
            else if (define.IsClass)
            {
                FClass data = new FClass(host, define, xml);
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
