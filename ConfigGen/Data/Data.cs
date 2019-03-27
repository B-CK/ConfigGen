using ConfigGen.TypeInfo;

namespace ConfigGen.Data
{
    public abstract class Data
    {
        #region 创建数据
        public static Data Create(FClass host, FieldInfo define, string value)
        {
            string type = define.OriginalType;
            if (define.IsRaw)
            {
                switch (type)
                {
                    case "bool":
                    case "int":
                        break;
                    case "long":
                        break;
                    case "float":
                        break;
                    case "string":
                        break;
                }
            }
            else if (define.IsEnum)
            {

            }

            Util.LogError("未知类型" + type);
            return null;
        }
        #endregion


        public FClass Host { get { return _host; } }
        public FieldInfo Define { get { return _define; } }

        protected FClass _host;
        protected FieldInfo _define;

        public Data(FClass host, FieldInfo define)
        {
            _define = define;
            _host = host;
        }
    }
}
