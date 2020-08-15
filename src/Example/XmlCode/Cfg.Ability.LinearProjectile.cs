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
	public partial class LinearProjectile : Cfg.Ability.Action
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Effect;
		/// <summary>
		/// 
		/// <summary>
		public readonly float MoveSpeed;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.Target StartPosition;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.Target EndPosition;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "Effect", Effect);
			Write(_1, "MoveSpeed", MoveSpeed);
			Write(_1, "StartPosition", StartPosition);
			Write(_1, "EndPosition", EndPosition);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Effect": Effect = ReadString(_2); break;
				case "MoveSpeed": MoveSpeed = ReadFloat(_2); break;
				case "StartPosition": StartPosition = ReadDynamicObject<Cfg.Ability.Target>(_2, "Cfg.Ability"); break;
				case "EndPosition": EndPosition = ReadDynamicObject<Cfg.Ability.Target>(_2, "Cfg.Ability"); break;
			}
		}
	}
}
