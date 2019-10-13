using DataSet;
using Wrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Import
{
    public abstract class Import
    {
        public abstract void Error(string msg);
        public abstract bool GetBool();
        public abstract int GetInt();
        public abstract long GetLong();
        public abstract float GetFloat();
        public abstract string GetString();
        public abstract string GetEnum();
        public abstract void GetClass(FClass data, ClassWrap info);
        public abstract void GetList(FList data, FieldWrap define);
        public abstract void GetDict(FDict data, FieldWrap define);
    }
}
