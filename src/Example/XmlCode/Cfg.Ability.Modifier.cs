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
	public partial class Modifier : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string name;
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Ability.ModifierEvent> events = new List<Cfg.Ability.ModifierEvent>();
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Ability.Property> properties = new List<Cfg.Ability.Property>();
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Ability.State> states = new List<Cfg.Ability.State>();
		public override void Write(TextWriter _1)
		{
			Write(_1, "name", name);
			Write(_1, "events", events);
			Write(_1, "properties", properties);
			Write(_1, "states", states);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "name": name = ReadString(_2); break;
				case "events":
					var eventss = GetChilds(_2);
					for (int i = 0; i < eventss.Count; i++)
					{
						var _3 = eventss[i];
						events.Add(ReadObject<Cfg.Ability.ModifierEvent>(_3, "Cfg.Ability.ModifierEvent"));
					}
					break;
				case "properties":
					var propertiess = GetChilds(_2);
					for (int i = 0; i < propertiess.Count; i++)
					{
						var _3 = propertiess[i];
						properties.Add(ReadObject<Cfg.Ability.Property>(_3, "Cfg.Ability.Property"));
					}
					break;
				case "states":
					var statess = GetChilds(_2);
					for (int i = 0; i < statess.Count; i++)
					{
						var _3 = statess[i];
						states.Add(ReadObject<Cfg.Ability.State>(_3, "Cfg.Ability.State"));
					}
					break;
			}
		}
	}
}
