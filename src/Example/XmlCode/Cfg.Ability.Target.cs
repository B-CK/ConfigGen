using System;
using XmlEditor;
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
		/// 
		/// <summary>
		public readonly Cfg.Ability.TargetType target;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.UnitTeam teams;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.UnitType types;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.UnitFlag flags;
		public override void Write(TextWriter _1)
		{
			Write(_1, "target", target);
			Write(_1, "teams", teams);
			Write(_1, "types", types);
			Write(_1, "flags", flags);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "target": target = (Cfg.Ability.TargetType)ReadString(_2); break;
				case "teams": teams = (Cfg.Ability.UnitTeam)ReadString(_2); break;
				case "types": types = (Cfg.Ability.UnitType)ReadString(_2); break;
				case "flags": flags = (Cfg.Ability.UnitFlag)ReadString(_2); break;
			}
		}
	}
}
