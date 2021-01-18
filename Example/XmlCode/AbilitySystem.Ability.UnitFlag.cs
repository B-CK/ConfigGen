using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	[System.Flags]
	public enum UnitFlag
	{
		/// <summary>
		/// 无
		/// <summary>
		FLAG_NONE = 0,
		/// <summary>
		/// 已死亡
		/// <summary>
		FLAG_DEAD = 1,
		/// <summary>
		/// 无敌
		/// <summary>
		FLAG_INVULNERABLE = 2,
		/// <summary>
		/// 不是隐形的
		/// <summary>
		FLAG_NO_INVIS = 4,
		/// <summary>
		/// 不是攻击免疫
		/// <summary>
		FLAG_NOT_ATTACK_IMMUNE = 8,
		/// <summary>
		/// 不是野怪
		/// <summary>
		FLAG_NOT_CREEP_HERO = 16,
		/// <summary>
		/// 不可控制的
		/// <summary>
		FLAG_NOT_DOMINATED = 32,
		/// <summary>
		/// 非召唤的
		/// <summary>
		FLAG_NOT_SUMMONED = 64,
		/// <summary>
		/// 玩家控制的
		/// <summary>
		FLAG_PLAYER_CONTROLLED = 128,
	}
}
