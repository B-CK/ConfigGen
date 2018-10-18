using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	public  class SkillAction : XmlCfg.Skill.GeneralAction
	{
		/// <summary>
		/// 默认后续技能使用期限,用于单个技能多段输出
		/// <summary>
		public const float EXPIRE_TIME = 1f;
		/// <summary>
		/// 后续技能使用期限,用于单个技能多段输出
		/// <summary>
		public float SkillExpireTime;
		/// <summary>
		/// 技能结束时间
		/// <summary>
		public float SkillEndTime;
		/// <summary>
		/// 是否可被打断
		/// <summary>
		public bool CanInterrupt;
		/// <summary>
		/// 技能锁定对象类型(0不需要目标 1敌方目标 ,2己方目标 3自己 4中立方)
		/// <summary>
		public XmlCfg.Skill.LockObjectType LockType;
		/// <summary>
		/// 技能作用范围[半径]
		/// <summary>
		public float SkillRange;
		/// <summary>
		/// 是否显示技能范围
		/// <summary>
		public bool CanShowSkillRange;
		/// <summary>
		/// 放技能时人是否可以转动
		/// <summary>
		public bool CanRotate;
		/// <summary>
		/// 放技能时人是否可以移动
		/// <summary>
		public bool CanMove;
		/// <summary>
		/// 序列字典集合
		/// <summary>
		public Dictionary<int, Sequence> SequenceDict = new Dictionary<int, Sequence>();

		public override void Write(TextWriter _1)
		{
			Write(_1, "SkillExpireTime", this.SkillExpireTime);
			Write(_1, "SkillEndTime", this.SkillEndTime);
			Write(_1, "CanInterrupt", this.CanInterrupt);
			Write(_1, "LockType", (int)this.LockType);
			Write(_1, "SkillRange", this.SkillRange);
			Write(_1, "CanShowSkillRange", this.CanShowSkillRange);
			Write(_1, "CanRotate", this.CanRotate);
			Write(_1, "CanMove", this.CanMove);
			Write(_1, "SequenceDict", this.SequenceDict);
		}

		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "SkillExpireTime": this.SkillExpireTime = ReadFloat(_2); break;
				case "SkillEndTime": this.SkillEndTime = ReadFloat(_2); break;
				case "CanInterrupt": this.CanInterrupt = ReadBool(_2); break;
				case "LockType": this.LockType = (XmlCfg.Skill.LockObjectType)ReadInt(_2); break;
				case "SkillRange": this.SkillRange = ReadFloat(_2); break;
				case "CanShowSkillRange": this.CanShowSkillRange = ReadBool(_2); break;
				case "CanRotate": this.CanRotate = ReadBool(_2); break;
				case "CanMove": this.CanMove = ReadBool(_2); break;
				case "SequenceDict": GetChilds(_2).ForEach (_3 => this.SequenceDict.Add(ReadInt(GetOnlyChild(_3, "Key")), ReadObject<XmlCfg.Skill.Sequence>(_3, "XmlCfg.Skill.Sequence"))); break;
			}
		}

		public static explicit operator SkillAction(Cfg.Skill.SkillAction _1)
		{
			return new SkillAction()
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
			};
		}
	}
}
