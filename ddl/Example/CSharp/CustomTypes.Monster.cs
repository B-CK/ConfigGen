using XmlEditor;
using System;
using System.IO
using System.Xml
using System.Linq
using System.Collections.Generic;
namespace Example.CustomTypes
{
	/// <summary>
	/// 怪物
	/// <summary>
public class Monster : XmlEditor.CustomTypes.Custom
	{
		/// <summary>
		/// 攻击
		/// <summary>
		public int Attack;
		public override Write(TextWriter _1)
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
