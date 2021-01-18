using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class ActOnTargets : AbilitySystem.Ability.ActionWithTarget
	{
		/// <summary>
		/// 操作列表
		/// <summary>
		
		public List<AbilitySystem.Ability.Action> Actions = new List<AbilitySystem.Ability.Action>();
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "Actions", Actions);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Actions":
					var Actionss = GetChilds(_2);
					for (int i = 0; i < Actionss.Count; i++)
					{
						var _3 = Actionss[i];
						Actions.Add(ReadObject<AbilitySystem.Ability.Action>(_3, "AbilitySystem.Ability.Action"));
					}
					break;
			}
		}
	}
}
