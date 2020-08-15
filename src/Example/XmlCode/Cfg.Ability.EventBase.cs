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
	public partial class EventBase : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string name;
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Ability.Action> actions = new List<Cfg.Ability.Action>();
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
						actions.Add(ReadObject<Cfg.Ability.Action>(_3, "Cfg.Ability.Action"));
					}
					break;
			}
		}
	}
}
