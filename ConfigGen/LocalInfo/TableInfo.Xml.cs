using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ConfigGen.LocalInfo
{
    public class TableXmlInfo : TableInfo
    {
        public TableXmlInfo(string relPath, ClassTypeInfo classType)
           : base(relPath, null)
        {

        }
        public override void Analyze()
        {

        }
        private void ReadClass()
        {
            XElement e = new XElement("");
            //Xml每个元素都是key-value形式
            //遇见多态,则使用属性type标注类型
            //每一个xml文件就是一条数据,文件名最好与key值相同
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
