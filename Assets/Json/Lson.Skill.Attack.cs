using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class Attack : Lson.Skill.Action
	{
		/// <summary>
		/// 打击点id
		/// <summary>
		public int Id;
		/// <summary>
		/// 打击区域id
		/// <summary>
		public int HitZoneId;
		/// <summary>
		/// 被击效果id
		/// <summary>
		public int BeAttackEffectId;
	}
}
