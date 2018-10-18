using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	public  class Sequence : XmlObject
	{
		/// <summary>
		/// 序列ID
		/// <summary>
		public int Id;
		/// <summary>
		/// 碰撞区域定义列表
		/// <summary>
		public List<HitZone> HitZones = new List<HitZone>();
		/// <summary>
		/// 时间事件列表
		/// <summary>
		public List<Timeline> Timelines = new List<Timeline>();

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
				case "Id": this.Id = ReadInt(_2); break;
				case "HitZones": GetChilds(_2).ForEach (_3 => this.HitZones.Add(ReadObject<XmlCfg.Skill.HitZone>(_3, "XmlCfg.Skill.HitZone"))); break;
				case "Timelines": GetChilds(_2).ForEach (_3 => this.Timelines.Add(ReadObject<XmlCfg.Skill.Timeline>(_3, "XmlCfg.Skill.Timeline"))); break;
			}
		}

		public static explicit operator Sequence(Cfg.Skill.Sequence _1)
		{
			return new Sequence()
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
				SkillExpireTime = _1.SkillExpireTime,
				SkillEndTime = _1.SkillEndTime,
				CanInterrupt = _1.CanInterrupt,
				LockType = (XmlCfg.Skill.LockObjectType)_1.LockType,
				SkillRange = _1.SkillRange,
				CanShowSkillRange = _1.CanShowSkillRange,
				CanRotate = _1.CanRotate,
				CanMove = _1.CanMove,
				SequenceDict = _1.SequenceDict,
				Id = _1.Id,
				HitZones = _1.HitZones,
				Timelines = _1.Timelines,
			};
		}
	}
}
