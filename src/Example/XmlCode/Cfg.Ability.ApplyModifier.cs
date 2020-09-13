using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Ability
{
	/// <summary>
	/// 
	/// <summary>
	public partial class ApplyModifier : Cfg.Ability.ActionWithTarget
	{
		/// <summary>
		/// 修饰器名称
		/// <summary>
		public string modifier;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "modifier", modifier);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "modifier": modifier = ReadString(_2); break;
			}
		}
	}
}
