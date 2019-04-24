using System;
using System.Linq;
using System.IO;
using XmlCfg;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class ActorConfig : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public string ModelName;
		/// <summary>
		/// 
		/// <summary>
		public string BaseModelName;
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<string, XmlCfg.Skill.GeneralAction> GeneralActions = new Dictionary<string, XmlCfg.Skill.GeneralAction>();
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<string, XmlCfg.Skill.SkillAction> SkillActions = new Dictionary<string, XmlCfg.Skill.SkillAction>();

		public override void Write(TextWriter _1)
		{
			Write(_1, "ModelName", this.ModelName);
			Write(_1, "BaseModelName", this.BaseModelName);
			Write(_1, "GeneralActions", this.GeneralActions);
			Write(_1, "SkillActions", this.SkillActions);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "ModelName": ModelName = ReadString(_2); break;
				case "BaseModelName": BaseModelName = ReadString(_2); break;
				case "GeneralActions": GetChilds(_2).ForEach (_3 => GeneralActions.Add(ReadString(GetOnlyChild(_3, "Key")), ReadObject<XmlCfg.Skill.GeneralAction>(GetOnlyChild(_3, "Value"), "XmlCfg.Skill.GeneralAction"))); break;
				case "SkillActions": GetChilds(_2).ForEach (_3 => SkillActions.Add(ReadString(GetOnlyChild(_3, "Key")), ReadObject<XmlCfg.Skill.SkillAction>(GetOnlyChild(_3, "Value"), "XmlCfg.Skill.SkillAction"))); break;
			}
		}
	}
}
