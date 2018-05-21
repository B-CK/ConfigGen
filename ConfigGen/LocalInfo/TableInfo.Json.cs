using System;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace ConfigGen.LocalInfo
{
    public class TableJsonInfo : TableInfo
    {
        public TableJsonInfo(string relPath, ClassTypeInfo classType)
           : base(relPath, null)
        {

        }
        public override void Analyze()
        {

        }
        private void ReadClass()
        {
           
        }

        public override bool Exist(string content)
        {
            throw new NotImplementedException();
        }
        public override bool Replace(string arg1, string arg2)
        {
            throw new NotImplementedException();
        }
    }
}
