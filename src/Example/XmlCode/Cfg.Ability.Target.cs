using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Ability
{
	/// <summary>
	/// 
	/// <summary>
	public partial class Target : XmlObject
	{
		/// <summary>
		/// 目标中心
		/// <summary>
		public Cfg.Ability.TargetType type;
		/// <summary>
		/// 筛选队伍类型
		/// <summary>
		public Cfg.Ability.UnitTeam teams;
		/// <summary>
		/// 标签目标类型
		/// <summary>
		public Cfg.Ability.UnitFlag flags;
		public override void Write(TextWriter _1)
		{
			Write(_1, "type", type);
			Write(_1, "teams", teams);
			Write(_1, "flags", flags);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "type": type = (Cfg.Ability.TargetType)ReadInt(_2); break;
				case "teams": teams = (Cfg.Ability.UnitTeam)ReadInt(_2); break;
				case "flags": flags = (Cfg.Ability.UnitFlag)ReadInt(_2); break;
			}
		}
	}
}
