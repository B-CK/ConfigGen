using ConfigGen.TypeInfo;
using System.Collections.Generic;

namespace ConfigGen.Data
{
    public class FList : Data
    {
        public List<Data> Values { get { return _values; } }

        private List<Data> _values = new List<Data>();

        public FList(FClass host, FieldInfo define) : base(host, define) { }
    }
}
