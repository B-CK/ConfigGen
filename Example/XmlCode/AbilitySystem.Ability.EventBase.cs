using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class EventBase : XmlObject
	{
		/// <summary>
		/// 事件名称
		/// <summary>
		
		public string name;
		/// <summary>
		/// 操作列表
		/// <summary>
		
		public List<AbilitySystem.Ability.Action> actions = new List<AbilitySystem.Ability.Action>();
		public override void Write(TextWriter _1)
		{
			Write(_1, "name", name);
			Write(_1, "actions", actions);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "name": name = ReadString(_2); break;
				case "actions":
					var actionss = GetChilds(_2);
					for (int i = 0; i < actionss.Count; i++)
					{
						var _3 = actionss[i];
						actions.Add(ReadObject<AbilitySystem.Ability.Action>(_3, "AbilitySystem.Ability.Action"));
					}
					break;
			}
		}
	}
}
