using Tool.Import;
using System.Xml;
using Tool.Wrap;

namespace Tool.Config
{
    public class FEnum : Data
    {
        public readonly string EnumName;
        public readonly string Value;

        public FEnum(FClass host, FieldWrap define, ImportExcel excel) : base(host, define)
        {
            EnumWrap info = EnumWrap.Enums[define.OriginalType];
            string name = excel.GetEnum();
            EnumName = info.GetEnumName(name);
            EnumName = EnumName.IsEmpty() ? name : EnumName;
            Value = info.GetEnumValue(EnumName);

            if (!info.ContainItem(name))
                excel.Error($"未定义枚举(名称/别名){define.FullType}.{name}   !");
        }
        public FEnum(FClass host, FieldWrap define, XmlElement xml) : base(host, define)
        {
            Value = xml.InnerText;//Xml中枚举以int表示
        }

        public override string ExportData()
        {
            return Value;
        }
        public override int ExportBinary(ref byte[] bytes, int offset)
        {
            try
            {
                int v = System.Convert.ToInt32(Value);
                int length = MessagePackBinary.WriteInt32(ref bytes, offset, v);
                return length;
            }
            catch (System.Exception e)
            {
                throw new System.Exception($"无法解析枚举值:{Value}");
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else
                return obj is FEnum ? (obj as FEnum).Value == Value : false;
        }
        public override int GetHashCode()
        {
            return EnumName.GetHashCode();
        }
    }

}
