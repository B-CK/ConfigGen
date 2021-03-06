using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	[Editoronly]
	public partial class Ability : XmlObject
	{
		/// <summary>
		/// 能力名称
		/// <summary>
		[Editoronly_FIEILD]
		public string name;
		/// <summary>
		/// 继承类型,默认ability(基类)
		/// <summary>
		
		public string baseClass;
		/// <summary>
		/// 能力行为类型,定义了能力的释放形式;可用|组合多个类型
		/// <summary>
		
		public AbilitySystem.Ability.BehaviorType behaviorType;
		/// <summary>
		/// 持续时间,结束时回收所有相关资源和数据
		/// <summary>
		
		public float duration;
		/// <summary>
		/// 变量列表,可指定常量,也可指定变量引用Excel
		/// <summary>
		
		public List<AbilitySystem.Ability.SharedArg> args = new List<AbilitySystem.Ability.SharedArg>();
		/// <summary>
		/// 资源列表,prefab,AnimationClip,AudioClip等
		/// <summary>
		
		public List<AbilitySystem.Ability.SharedString> assets = new List<AbilitySystem.Ability.SharedString>();
		/// <summary>
		/// 基础事件列表
		/// <summary>
		
		public List<AbilitySystem.Ability.AbilityEvent> events = new List<AbilitySystem.Ability.AbilityEvent>();
		/// <summary>
		/// 修饰器列表,可定义多个修饰器
		/// <summary>
		
		public List<AbilitySystem.Ability.Modifier> modifiers = new List<AbilitySystem.Ability.Modifier>();
		public override void Write(TextWriter _1)
		{
			Write(_1, "name", name);
			Write(_1, "baseClass", baseClass);
			Write(_1, "behaviorType", behaviorType);
			Write(_1, "duration", duration);
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
				case "behaviorType": behaviorType = (AbilitySystem.Ability.BehaviorType)ReadInt(_2); break;
				case "duration": duration = ReadFloat(_2); break;
				case "args":
					var argss = GetChilds(_2);
					for (int i = 0; i < argss.Count; i++)
					{
						var _3 = argss[i];
						args.Add(ReadObject<AbilitySystem.Ability.SharedArg>(_3, "AbilitySystem.Ability.SharedArg"));
					}
					break;
				case "assets":
					var assetss = GetChilds(_2);
					for (int i = 0; i < assetss.Count; i++)
					{
						var _3 = assetss[i];
						assets.Add(ReadObject<AbilitySystem.Ability.SharedString>(_3, "AbilitySystem.Ability.SharedString"));
					}
					break;
				case "events":
					var eventss = GetChilds(_2);
					for (int i = 0; i < eventss.Count; i++)
					{
						var _3 = eventss[i];
						events.Add(ReadObject<AbilitySystem.Ability.AbilityEvent>(_3, "AbilitySystem.Ability.AbilityEvent"));
					}
					break;
				case "modifiers":
					var modifierss = GetChilds(_2);
					for (int i = 0; i < modifierss.Count; i++)
					{
						var _3 = modifierss[i];
						modifiers.Add(ReadObject<AbilitySystem.Ability.Modifier>(_3, "AbilitySystem.Ability.Modifier"));
					}
					break;
			}
		}
	}
}
