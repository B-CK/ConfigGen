using System;
using System.Linq;
using System.IO;
using XmlExample;
using System.Xml;
using System.Collections.Generic;

namespace XmlCustomTypes
{
	/// <summary>
	/// 怪物
	/// <summary>
	public class Monster : XmlObject
	{
		/// <summary>
		/// 攻击
		/// <summary>
		public int Attack;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Attack", this.Attack);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Attack": Attack = ReadInt(_2); break;
			}
		}
	}
}
