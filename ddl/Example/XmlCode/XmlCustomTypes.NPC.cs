using System;
using System.Linq;
using System.IO;
using XmlExample;
using System.Xml;
using System.Collections.Generic;

namespace XmlCustomTypes
{
	/// <summary>
	/// NPC
	/// <summary>
	public class NPC : XmlObject
	{
		/// <summary>
		/// 别名
		/// <summary>
		public string Alias;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Alias", this.Alias);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Alias": Alias = ReadString(_2); break;
			}
		}
	}
}
