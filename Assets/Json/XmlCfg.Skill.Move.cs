using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	public  class Move : XmlCfg.Skill.Controller
	{
		/// <summary>
		/// 移动方式:0向目标移动 1按指定方向移动
		/// <summary>
		public XmlCfg.Skill.MoveType Type;
		/// <summary>
		/// 是否相对于自己移动
		/// <summary>
		public bool IsRelateSelf;
		/// <summary>
		/// 起始位置相对目标偏移
		/// <summary>
		public XmlCfg.Vector3 Offset;
		/// <summary>
		/// Y轴顺时针旋转角度
		/// <summary>
		public float Angle;
		/// <summary>
		/// 位移速度m/s
		/// <summary>
		public float Speed;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Type", (int)this.Type);
			Write(_1, "IsRelateSelf", this.IsRelateSelf);
			Write(_1, "Offset", this.Offset);
			Write(_1, "Angle", this.Angle);
			Write(_1, "Speed", this.Speed);
		}

		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Type": this.Type = (XmlCfg.Skill.MoveType)ReadInt(_2); break;
				case "IsRelateSelf": this.IsRelateSelf = ReadBool(_2); break;
				case "Offset": this.Offset = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "Angle": this.Angle = ReadFloat(_2); break;
				case "Speed": this.Speed = ReadFloat(_2); break;
			}
		}

		public static explicit operator Move(Cfg.Skill.Move _1)
		{
			return new Move()
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
				Target = _1.Target,
				Path = _1.Path,
				Enable = _1.Enable,
				NewObject = _1.NewObject,
				Offset = _1.Offset,
				EulerAngles = _1.EulerAngles,
				Type = (XmlCfg.Skill.MoveType)_1.Type,
				IsRelateSelf = _1.IsRelateSelf,
				Offset = _1.Offset,
				Angle = _1.Angle,
				Speed = _1.Speed,
			};
		}
	}
}
