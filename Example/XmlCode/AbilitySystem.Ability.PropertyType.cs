using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	public enum PropertyType
	{
		/// <summary>
		/// 无修改
		/// <summary>
		MODIFIER_PROP_NONE  = 0,
		/// <summary>
		/// 修改魔量
		/// <summary>
		MODIFIER_PROP_MANA_BONUS  = 1,
		/// <summary>
		/// 修改血量
		/// <summary>
		MODIFIER_PROP_HEALTH_BONUS = 2,
	}
}
