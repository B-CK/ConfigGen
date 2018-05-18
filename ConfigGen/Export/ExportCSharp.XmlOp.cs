using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigGen.Export
{
    public partial class ExportCSharp
    {
        /// <summary>
        /// 编辑模式下的xml读写操作
        /// </summary>
        public static void Export_XmlOp()
        {

        }
    }

    //Unity 可视化属性记录
    //enum.value    -       直接定义成枚举
    //func          -       可添加功能按钮[Button]

    //字段内容限制(检查规则)
    //int,long,float        -       范围限制
    //文件资源              -       路径,名称前缀等限制[AssetList]
    //读取文件路径          -       [FolderPath],
    //组件数据关联          -       [InlineEditor]



    //--------------------Xml形式
    //<fieldName>
    //	<pair>
    //		<key>1</key>
    //		<value>

    //		</value>
    //	</pair>
    //	<pair>
    //		<key>5</key>
    //		<value>

    //		</value>
    //	</pair>
    //</dict>

    //<fieldName>
    //	<item>1</item>	
    //	<item>3</item>	
    //</fieldName>

    //<fieldName>
    //	<item type = "BCard">
    //
    //  </ item >
    //  < item type = "ACard">
    //
    //	</item>	
    //</fieldName>

    //<fieldName type = "Card">
    //</fieldName>
    //<fieldName> </ fieldName>
}
