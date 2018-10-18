using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	public  class StaticHit : XmlCfg.Skill.Timeline
	{
		/// <summary>
		/// 碰撞区域
		/// <summary>
		public XmlCfg.Skill.HitZone Zone;
		/// <summary>
		/// 触发序列容器ID
		/// <summary>
		public int SequeueID;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Zone", this.Zone);
			Write(_1, "SequeueID", this.SequeueID);
		}

		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Zone": this.Zone = ReadObject<XmlCfg.Skill.HitZone>(_2, "XmlCfg.Skill.HitZone"); break;
				case "SequeueID": this.SequeueID = ReadInt(_2); break;
			}
		}

		public static explicit operator StaticHit(Cfg.Skill.StaticHit _1)
		{
			return new StaticHit()
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
				Id = _1.Id,
				Sharp = (XmlCfg.Skill.HitSharpType)_1.Sharp,
				Offset = _1.Offset,
				MaxNum = _1.MaxNum,
				Scale = _1.Scale,
				Radius = _1.Radius,
				Radius = _1.Radius,
				Height = _1.Height,
				Angle = _1.Angle,
				Start = _1.Start,
				End = _1.End,
				Zone = _1.Zone,
				SequeueID = _1.SequeueID,
			};
		}
	}
}
