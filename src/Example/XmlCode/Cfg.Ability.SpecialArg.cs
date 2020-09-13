using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Ability
{
	/// <summary>
	/// 参数类型基类,也可用于Excel数据(单表,如果是多表则需要扩展)引用
	/// <summary>
	public partial class SpecialArg : XmlObject
	{
		/// <summary>
		/// 变量名称
		/// <summary>
		public string name;
		public override void Write(TextWriter _1)
		{
			Write(_1, "name", name);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "name": name = ReadString(_2); break;
			}
		}
	}
}
