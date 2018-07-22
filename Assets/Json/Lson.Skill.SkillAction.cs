using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class SkillAction : Lson.Skill.ModelAction
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
		/// 是否需要目标
		/// <summary>
		public bool NeedTarget;
		/// <summary>
		/// 是否可被打断
		/// <summary>
		public bool CanInterrupt;
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
		/// 开始位移时间，如果改值为-1 则改值等于技能开始时间
		/// <summary>
		public float StartMoveTime;
		/// <summary>
		/// 结束位移时间，如果改值为-1 则改值等于技能结束时间
		/// <summary>
		public float EndMoveTime;
		/// <summary>
		/// 施放相对位置(1 自己 , 2 目标)
		/// <summary>
		public Lson.Skill.SkillRelateType RelateType;
		/// <summary>
		/// 打击点组列表
		/// <summary>
		public List<Lson.Skill.HitPointGroup> HitPoints;
		/// <summary>
		/// 打击区域列表
		/// <summary>
		public List<Lson.Skill.HitZone> HitZones;
		/// <summary>
		/// 被击效果列表
		/// <summary>
		public List<Lson.Skill.BeAttackEffect> BeAttackEffects;
	}
}
