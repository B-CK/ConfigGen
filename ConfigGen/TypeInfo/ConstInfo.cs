using ConfigGen.Description;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigGen.TypeInfo
{
    public class ConstInfo
    {
        public string HostName { get { return _hostName; } }
        public string Desc { get { return _des.Desc; } }
        public string Name { get { return _des.Name; } }
        public string FullType { get { return _des.Type; } }
        public string Value { get { return _des.Value; } }
        public string OriginalType { get { return _types[0]; } }
        public string[] Types { get { return _types; } }

        private ConstDes _des;
        private string _hostName;
        private string[] _types;

        public ConstInfo(string fullName, ConstDes des)
        {
            _des = des;
            _hostName = fullName;
            _types = Util.Split(des.Type);
            if (_types.Length == 0)
                _types = new string[1] { "int" };
        }

        public void VerifyDefine()
        {
            if (!Util.MatchName(Name))
                Error("命名不合法:" + Name);
            string type = _types[0];
            if (!Setting.RawTypes.Contains(type)
                && !Setting.ContainerTypes.Contains(type)
                && !EnumInfo.IsEnum(type))
                Error(string.Format("当前常量类型:{0} 非基础/集合/枚举类型", type));
        }
        public override string ToString()
        {
            return string.Format("Const - Name:{0}\tType:{1}\tValue:{2}", Name, FullType, _des.Value);
        }
        void Error(string msg)
        {
            string error = string.Format("Const:{0}.{1} {2}", _hostName, Name, msg);
            throw new System.Exception(error);
        }
    }
}
