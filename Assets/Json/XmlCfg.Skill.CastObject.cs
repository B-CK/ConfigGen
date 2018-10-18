using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	public  class CastObject : XmlCfg.Skill.Controller
	{
		/// <summary>
		/// 是否追踪目标
		/// <summary>
		public bool IsTraceTarget;
		/// <summary>
		/// 飞行参数ID,数据有配置表
		/// <summary>
		public int CurveId;
		/// <summary>
		/// 是否穿透
		/// <summary>
		public bool PassBody;
		/// <summary>
		/// 投射起始偏移
		/// <summary>
		public XmlCfg.Vector3 Position;
		/// <summary>
		/// 投射起始旋转角度
		/// <summary>
		public XmlCfg.Vector3 EulerAngles;

		public override void Write(TextWriter _1)
		{
			Write(_1, "IsTraceTarget", this.IsTraceTarget);
			Write(_1, "CurveId", this.CurveId);
			Write(_1, "PassBody", this.PassBody);
			Write(_1, "Position", this.Position);
			Write(_1, "EulerAngles", this.EulerAngles);
		}

		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "IsTraceTarget": this.IsTraceTarget = ReadBool(_2); break;
				case "CurveId": this.CurveId = ReadInt(_2); break;
				case "PassBody": this.PassBody = ReadBool(_2); break;
				case "Position": this.Position = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "EulerAngles": this.EulerAngles = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
			}
		}

		public static explicit operator CastObject(Cfg.Skill.CastObject _1)
		{
			return new CastObject()
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
				IsTraceTarget = _1.IsTraceTarget,
				CurveId = _1.CurveId,
				PassBody = _1.PassBody,
				Position = _1.Position,
				EulerAngles = _1.EulerAngles,
			};
		}
	}
}
