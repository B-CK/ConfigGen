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
	public class GeneralAction : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public string ActionName;
		/// <summary>
		/// 
		/// <summary>
		public string OtherModelName;
		/// <summary>
		/// 
		/// <summary>
		public string ActionClip;
		/// <summary>
		/// 
		/// <summary>
		public string PreActionFile;
		/// <summary>
		/// 
		/// <summary>
		public string PostActionFile;
		/// <summary>
		/// 
		/// <summary>
		public float ActionSpeed;
		/// <summary>
		/// 
		/// <summary>
		public int LoopTimes;
		/// <summary>
		/// 
		/// <summary>
		public readonly List<XmlCfg.Skill.Timeline> Timelines = new List<XmlCfg.Skill.Timeline>();

		public override void Write(TextWriter _1)
		{
			Write(_1, "ActionName", this.ActionName);
			Write(_1, "OtherModelName", this.OtherModelName);
			Write(_1, "ActionClip", this.ActionClip);
			Write(_1, "PreActionFile", this.PreActionFile);
			Write(_1, "PostActionFile", this.PostActionFile);
			Write(_1, "ActionSpeed", this.ActionSpeed);
			Write(_1, "LoopTimes", this.LoopTimes);
			Write(_1, "Timelines", this.Timelines);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "ActionName": ActionName = ReadString(_2); break;
				case "OtherModelName": OtherModelName = ReadString(_2); break;
				case "ActionClip": ActionClip = ReadString(_2); break;
				case "PreActionFile": PreActionFile = ReadString(_2); break;
				case "PostActionFile": PostActionFile = ReadString(_2); break;
				case "ActionSpeed": ActionSpeed = ReadFloat(_2); break;
				case "LoopTimes": LoopTimes = ReadInt(_2); break;
				case "Timelines": GetChilds(_2).ForEach (_3 => Timelines.Add(ReadObject<XmlCfg.Skill.Timeline>(_3, "XmlCfg.Skill.Timeline"))); break;
			}
		}
	}
}
