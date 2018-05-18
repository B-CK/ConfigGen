using System;
using LitJson;
using System.Text;
using System.Collections.Generic;

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
