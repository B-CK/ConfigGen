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
	public partial class Ability : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string name;
		/// <summary>
		/// 
		/// <summary>
		public readonly string baseClass;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.BehaviorType behaviorType;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.Target target;
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Ability.SpecialArg> args = new List<Cfg.Ability.SpecialArg>();
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Ability.SpecialString> assets = new List<Cfg.Ability.SpecialString>();
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Ability.AbilityEvent> events = new List<Cfg.Ability.AbilityEvent>();
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Ability.Modifier> modifiers = new List<Cfg.Ability.Modifier>();
		public override void Write(TextWriter _1)
		{
			Write(_1, "name", name);
			Write(_1, "baseClass", baseClass);
			Write(_1, "behaviorType", behaviorType);
			Write(_1, "target", target);
			Write(_1, "args", args);
			Write(_1, "assets", assets);
			Write(_1, "events", events);
			Write(_1, "modifiers", modifiers);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "name": name = ReadString(_2); break;
				case "baseClass": baseClass = ReadString(_2); break;
				case "behaviorType": behaviorType = (Cfg.Ability.BehaviorType)ReadString(_2); break;
				case "target": target = ReadDynamicObject<Cfg.Ability.Target>(_2, "Cfg.Ability"); break;
				case "args":
					var argss = GetChilds(_2);
					for (int i = 0; i < argss.Count; i++)
					{
						var _3 = argss[i];
						args.Add(ReadObject<Cfg.Ability.SpecialArg>(_3, "Cfg.Ability.SpecialArg"));
					}
					break;
				case "assets":
					var assetss = GetChilds(_2);
					for (int i = 0; i < assetss.Count; i++)
					{
						var _3 = assetss[i];
						assets.Add(ReadObject<Cfg.Ability.SpecialString>(_3, "Cfg.Ability.SpecialString"));
					}
					break;
				case "events":
					var eventss = GetChilds(_2);
					for (int i = 0; i < eventss.Count; i++)
					{
						var _3 = eventss[i];
						events.Add(ReadObject<Cfg.Ability.AbilityEvent>(_3, "Cfg.Ability.AbilityEvent"));
					}
					break;
				case "modifiers":
					var modifierss = GetChilds(_2);
					for (int i = 0; i < modifierss.Count; i++)
					{
						var _3 = modifierss[i];
						modifiers.Add(ReadObject<Cfg.Ability.Modifier>(_3, "Cfg.Ability.Modifier"));
					}
					break;
			}
		}
	}
}
