using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg
{
	public  class Vector3 : XmlObject
	{

		public override void Write(TextWriter _1)
		{
		}

		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
			}
		}

		public static explicit operator Vector3(Cfg.Vector3 _1)
		{
			return new Vector3()
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
