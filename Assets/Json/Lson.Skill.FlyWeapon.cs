using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class FlyWeapon : Lson.Skill.TraceObject
	{
		/// <summary>
		/// 子弹半径
		/// <summary>
		public float BulletRadius;
		/// <summary>
		/// 是否穿透
		/// <summary>
		public bool PassBody;
		/// <summary>
		/// 被击效果ID
		/// <summary>
		public int BeAttackEffectId;
	}
}
