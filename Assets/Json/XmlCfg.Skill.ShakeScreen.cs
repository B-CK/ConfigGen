using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	public  class ShakeScreen : XmlCfg.Skill.Timeline
	{
		/// <summary>
		/// 震屏方式:0水平 1垂直 2混合
		/// <summary>
		public XmlCfg.Skill.ShakeType Type;
		/// <summary>
		/// 每秒震动的次数
		/// <summary>
		public int Frequency;
		/// <summary>
		/// 初始频率维持时间
		/// <summary>
		public float FrequencyDuration;
		/// <summary>
		/// 频率衰减
		/// <summary>
		public float FrequencyAtten;
		/// <summary>
		/// 单次振幅
		/// <summary>
		public float Amplitude;
		/// <summary>
		/// 单次震动的衰减幅度
		/// <summary>
		public float AmplitudeAtten;
		/// <summary>
		/// 最小完整影响范围
		/// <summary>
		public float MinRange;
		/// <summary>
		/// 最大影响范围
		/// <summary>
		public float MaxRange;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Type", (int)this.Type);
			Write(_1, "Frequency", this.Frequency);
			Write(_1, "FrequencyDuration", this.FrequencyDuration);
			Write(_1, "FrequencyAtten", this.FrequencyAtten);
			Write(_1, "Amplitude", this.Amplitude);
			Write(_1, "AmplitudeAtten", this.AmplitudeAtten);
			Write(_1, "MinRange", this.MinRange);
			Write(_1, "MaxRange", this.MaxRange);
		}

		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Type": this.Type = (XmlCfg.Skill.ShakeType)ReadInt(_2); break;
				case "Frequency": this.Frequency = ReadInt(_2); break;
				case "FrequencyDuration": this.FrequencyDuration = ReadFloat(_2); break;
				case "FrequencyAtten": this.FrequencyAtten = ReadFloat(_2); break;
				case "Amplitude": this.Amplitude = ReadFloat(_2); break;
				case "AmplitudeAtten": this.AmplitudeAtten = ReadFloat(_2); break;
				case "MinRange": this.MinRange = ReadFloat(_2); break;
				case "MaxRange": this.MaxRange = ReadFloat(_2); break;
			}
		}

		public static explicit operator ShakeScreen(Cfg.Skill.ShakeScreen _1)
		{
			return new ShakeScreen()
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
				Path = _1.Path,
				IsRelateSelf = _1.IsRelateSelf,
				FollowDir = _1.FollowDir,
				NodeName = _1.NodeName,
				Position = _1.Position,
				EulerAngles = _1.EulerAngles,
				Scale = _1.Scale,
				AlignType = (XmlCfg.Skill.EffectAlignType)_1.AlignType,
				Id = _1.Id,
				Path = _1.Path,
				Volume = _1.Volume,
				Type = (XmlCfg.Skill.ShakeType)_1.Type,
				Frequency = _1.Frequency,
				FrequencyDuration = _1.FrequencyDuration,
				FrequencyAtten = _1.FrequencyAtten,
				Amplitude = _1.Amplitude,
				AmplitudeAtten = _1.AmplitudeAtten,
				MinRange = _1.MinRange,
				MaxRange = _1.MaxRange,
			};
		}
	}
}
