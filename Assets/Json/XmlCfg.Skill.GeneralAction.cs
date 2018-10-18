using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	public  class GeneralAction : XmlObject
	{
		/// <summary>
		/// 行为名称
		/// <summary>
		public string ActionName = "";
		/// <summary>
		/// 动作来源
		/// <summary>
		public bool IsFromOther;
		/// <summary>
		/// 其他模型名称,用于套用其他模型动作
		/// <summary>
		public string OtherModelName = "";
		/// <summary>
		/// 绑定的动作名称
		/// <summary>
		public string ActionFile = "";
		/// <summary>
		/// 前摇动作名称
		/// <summary>
		public string PreActionFile = "";
		/// <summary>
		/// 后摇动作名称
		/// <summary>
		public string PostActionFile = "";
		/// <summary>
		/// 动作播放速率
		/// <summary>
		public float ActionSpeed;
		/// <summary>
		/// 动作循环次数
		/// <summary>
		public int LoopTimes;
		/// <summary>
		/// 时间事件列表
		/// <summary>
		public List<Timeline> Timelines = new List<Timeline>();

		public override void Write(TextWriter _1)
		{
			Write(_1, "ActionName", this.ActionName);
			Write(_1, "IsFromOther", this.IsFromOther);
			Write(_1, "OtherModelName", this.OtherModelName);
			Write(_1, "ActionFile", this.ActionFile);
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
				case "ActionName": this.ActionName = ReadString(_2); break;
				case "IsFromOther": this.IsFromOther = ReadBool(_2); break;
				case "OtherModelName": this.OtherModelName = ReadString(_2); break;
				case "ActionFile": this.ActionFile = ReadString(_2); break;
				case "PreActionFile": this.PreActionFile = ReadString(_2); break;
				case "PostActionFile": this.PostActionFile = ReadString(_2); break;
				case "ActionSpeed": this.ActionSpeed = ReadFloat(_2); break;
				case "LoopTimes": this.LoopTimes = ReadInt(_2); break;
				case "Timelines": GetChilds(_2).ForEach (_3 => this.Timelines.Add(ReadObject<XmlCfg.Skill.Timeline>(_3, "XmlCfg.Skill.Timeline"))); break;
			}
		}

		public static explicit operator GeneralAction(Cfg.Skill.GeneralAction _1)
		{
			return new GeneralAction()
			{
				Name = _1.Name,
				GroupType = (XmlCfg.Character.GroupType)_1.GroupType,
				Level = _1.Level,
				ModelName = _1.ModelName,
				BaseModelName = _1.BaseModelName,
				GeneralActions = _1.GeneralActions,
				SkillActions = _1.SkillActions,
				ActionName = _1.ActionName,
				IsFromOther = _1.IsFromOther,
				OtherModelName = _1.OtherModelName,
				ActionFile = _1.ActionFile,
				PreActionFile = _1.PreActionFile,
				PostActionFile = _1.PostActionFile,
				ActionSpeed = _1.ActionSpeed,
				LoopTimes = _1.LoopTimes,
				Timelines = _1.Timelines,
			};
		}
	}
}
