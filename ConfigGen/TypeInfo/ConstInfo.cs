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
        public string Name { get { return _des.Name; } }
        public string FullType { get { return _des.Type; } }
        public string Value { get { return _des.Value; } }

        private ConstDes _des;
        private string _hostName;

        public ConstInfo(string fullName, ConstDes des)
        {
            _des = des;
            _hostName = fullName;
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
