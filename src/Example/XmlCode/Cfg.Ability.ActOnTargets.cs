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
	public partial class ActOnTargets : Cfg.Ability.Action
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.Target Target;
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Ability.Action> Actions = new List<Cfg.Ability.Action>();
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "Target", Target);
			Write(_1, "Actions", Actions);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Target": Target = ReadDynamicObject<Cfg.Ability.Target>(_2, "Cfg.Ability"); break;
				case "Actions":
					var Actionss = GetChilds(_2);
					for (int i = 0; i < Actionss.Count; i++)
					{
						var _3 = Actionss[i];
						Actions.Add(ReadObject<Cfg.Ability.Action>(_3, "Cfg.Ability.Action"));
					}
					break;
			}
		}
	}
}
