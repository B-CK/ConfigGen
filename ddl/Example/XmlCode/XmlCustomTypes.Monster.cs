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
		/// <summary>
		/// 
		/// <summary>
		public bool ID;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Attack", this.Attack);
			Write(_1, "ID", this.ID);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Attack": Attack = ReadInt(_2); break;
				case "ID": ID = ReadBool(_2); break;
			}
		}
	}
}
