using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	public  class HitZone : XmlObject
	{
		/// <summary>
		/// 打击区域id
		/// <summary>
		public int Id;
		/// <summary>
		/// 打击范围的形态，0：方盒，1:圆柱,2:球
		/// <summary>
		public XmlCfg.Skill.HitSharpType Sharp;
		/// <summary>
		/// 坐标偏移量
		/// <summary>
		public XmlCfg.Vector3 Offset;
		/// <summary>
		/// 最大数量
		/// <summary>
		public int MaxNum;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Id", this.Id);
			Write(_1, "Sharp", (int)this.Sharp);
			Write(_1, "Offset", this.Offset);
			Write(_1, "MaxNum", this.MaxNum);
		}

		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Id": this.Id = ReadInt(_2); break;
				case "Sharp": this.Sharp = (XmlCfg.Skill.HitSharpType)ReadInt(_2); break;
				case "Offset": this.Offset = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "MaxNum": this.MaxNum = ReadInt(_2); break;
			}
		}

		public static explicit operator HitZone(Cfg.Skill.HitZone _1)
		{
			return new HitZone()
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
			};
		}
	}
}
