using System;
using System.Linq;
using System.IO;
using XmlExample;
using System.Xml;
using System.Collections.Generic;

namespace XmlCustomTypes
{
	/// <summary>
	/// 伙伴
	/// <summary>
	public class Partner : XmlObject
	{
		/// <summary>
		/// 别名
		/// <summary>
		public string Alias;
		/// <summary>
		/// 光环
		/// <summary>
		public CustomTypes.BuffType Buff;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Alias", this.Alias);
			Write(_1, "Buff", this.Buff);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Alias": Alias = ReadString(_2); break;
				case "Buff": Buff = (CustomTypes.BuffType)ReadInt(_2); break;
			}
		}
	}
}
