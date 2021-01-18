using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	public enum TargetType
	{
		/// <summary>
		/// 自己
		/// <summary>
		SEFL = 0,
		/// <summary>
		/// 敌方
		/// <summary>
		TARGET = 1,
		/// <summary>
		/// 点
		/// <summary>
		POINT = 2,
		/// <summary>
		/// 被选择的任意单位
		/// <summary>
		UNIT = 3,
		/// <summary>
		/// 抛射物
		/// <summary>
		PROJECTILE = 4,
	}
}
