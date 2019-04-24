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
	public class Sequence : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public string Id;
		/// <summary>
		/// 
		/// <summary>
		public readonly List<XmlCfg.Skill.HitZone> HitZones = new List<XmlCfg.Skill.HitZone>();
		/// <summary>
		/// 
		/// <summary>
		public readonly List<XmlCfg.Skill.Timeline> Timelines = new List<XmlCfg.Skill.Timeline>();

		public override void Write(TextWriter _1)
		{
			Write(_1, "Id", this.Id);
			Write(_1, "HitZones", this.HitZones);
			Write(_1, "Timelines", this.Timelines);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Id": Id = ReadString(_2); break;
				case "HitZones": GetChilds(_2).ForEach (_3 => HitZones.Add(ReadObject<XmlCfg.Skill.HitZone>(_3, "XmlCfg.Skill.HitZone"))); break;
				case "Timelines": GetChilds(_2).ForEach (_3 => Timelines.Add(ReadObject<XmlCfg.Skill.Timeline>(_3, "XmlCfg.Skill.Timeline"))); break;
			}
		}
	}
}
