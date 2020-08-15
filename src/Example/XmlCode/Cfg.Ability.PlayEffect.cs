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
	public partial class PlayEffect : Cfg.Ability.Action
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Effect;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.Target Target;
		/// <summary>
		/// 
		/// <summary>
		public readonly string AttackPath;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "Effect", Effect);
			Write(_1, "Target", Target);
			Write(_1, "AttackPath", AttackPath);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Effect": Effect = ReadString(_2); break;
				case "Target": Target = ReadDynamicObject<Cfg.Ability.Target>(_2, "Cfg.Ability"); break;
				case "AttackPath": AttackPath = ReadString(_2); break;
			}
		}
	}
}
