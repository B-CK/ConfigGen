using System;
using XmlEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Editor.CustomTypes
{
	/// <summary>
	/// 怪物
	/// <summary>
	public class Monster : Editor.CustomTypes.Custom
	{
		/// <summary>
		/// 攻击
		/// <summary>
		public int Attack;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "Attack", Attack);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Attack": Attack = ReadInt(_2); break;
			}
		}
	}
}
