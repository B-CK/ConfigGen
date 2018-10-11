using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	public  class FlyWeapon : Cfg.Skill.TraceObject
	{
		/// <summary>
		/// 子弹半径
		/// <summary>
		public readonly float BulletRadius;
		/// <summary>
		/// 是否穿透
		/// <summary>
		public readonly bool PassBody;
		/// <summary>
		/// 被击效果ID
		/// <summary>
		public readonly int BeAttackEffectId;

		public FlyWeapon(DataStream data) : base(data)
		{
			this.BulletRadius = data.GetFloat();
			this.PassBody = data.GetBool();
			this.BeAttackEffectId = data.GetInt();
		}
	}
}
