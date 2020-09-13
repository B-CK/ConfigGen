using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Ability
{
	/// <summary>
	/// 
	/// <summary>
	public partial class ActionWithTarget : Cfg.Ability.Action
	{
		/// <summary>
		/// 目标
		/// <summary>
		public Cfg.Ability.Target target;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "target", target);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "target": target = ReadDynamicObject<Cfg.Ability.Target>(_2, "Cfg.Ability"); break;
			}
		}
	}
}
