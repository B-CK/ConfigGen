using System;
using XmlEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Ability
{
	/// <summary>
	/// 
	/// <summary>
	public enum UnitFlag
	{
		UNIT_FLAG_NONE = 1,
		UNIT_FLAG_DEAD = 2,
		UNIT_FLAG_INVULNERABLE = 4,
		UNIT_FLAG_NO_INVIS = 8,
		UNIT_FLAG_NOT_ATTACK_IMMUNE = 16,
		UNIT_FLAG_NOT_CREEP_HERO = 32,
		UNIT_FLAG_NOT_DOMINATED = 64,
		UNIT_FLAG_NOT_SUMMONED = 128,
		UNIT_FLAG_PLAYER_CONTROLLED = 256,
	}
}
