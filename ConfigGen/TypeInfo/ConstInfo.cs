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
        public string Type { get { return _des.Type; } }
        public string Value { get { return _des.Value; } }

        private ConstDes _des;
        private string _hostName;

        public ConstInfo(ConstDes des, string hostName)
        {
            _des = des;
            _hostName = hostName;
        }
    }
}
