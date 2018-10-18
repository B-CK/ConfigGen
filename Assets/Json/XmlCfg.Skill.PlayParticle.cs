using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	public  class PlayParticle : XmlCfg.Skill.Timeline
	{
		/// <summary>
		/// 粒子资源的路径
		/// <summary>
		public string Path = "";
		/// <summary>
		/// 是否相对于自己移动
		/// <summary>
		public bool IsRelateSelf;
		/// <summary>
		/// 特效是否始终跟随目标对象方向变化
		/// <summary>
		public bool FollowDir;
		/// <summary>
		/// 节点名称,如果有配置则绑定到节点局部空间;反之绑定世界空间
		/// <summary>
		public string NodeName = "";
		/// <summary>
		/// 特效结点偏移;特效世界偏移
		/// <summary>
		public XmlCfg.Vector3 Position;
		/// <summary>
		/// 特效结点旋转;特效世界旋转
		/// <summary>
		public XmlCfg.Vector3 EulerAngles;
		/// <summary>
		/// 整体缩放大小
		/// <summary>
		public XmlCfg.Vector3 Scale;
		/// <summary>
		/// 屏幕对齐类型
		/// <summary>
		public XmlCfg.Skill.EffectAlignType AlignType;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Path", this.Path);
			Write(_1, "IsRelateSelf", this.IsRelateSelf);
			Write(_1, "FollowDir", this.FollowDir);
			Write(_1, "NodeName", this.NodeName);
			Write(_1, "Position", this.Position);
			Write(_1, "EulerAngles", this.EulerAngles);
			Write(_1, "Scale", this.Scale);
			Write(_1, "AlignType", (int)this.AlignType);
		}

		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Path": this.Path = ReadString(_2); break;
				case "IsRelateSelf": this.IsRelateSelf = ReadBool(_2); break;
				case "FollowDir": this.FollowDir = ReadBool(_2); break;
				case "NodeName": this.NodeName = ReadString(_2); break;
				case "Position": this.Position = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "EulerAngles": this.EulerAngles = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "Scale": this.Scale = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "AlignType": this.AlignType = (XmlCfg.Skill.EffectAlignType)ReadInt(_2); break;
			}
		}

		public static explicit operator PlayParticle(Cfg.Skill.PlayParticle _1)
		{
			return new PlayParticle()
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
			};
		}
	}
}
