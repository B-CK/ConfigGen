using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Ability
{
	/// <summary>
	/// 能力/行为类型:决定能力释放方式
	/// <summary>
	[System.Flags]
	public enum BehaviorType
	{
		/// <summary>
		/// 被动能力，不能被使用，但是会在HUD上显示
		/// <summary>
		BEHAVIOR_PASSIVE = 1,
		/// <summary>
		/// 需要指定一个目标来释放
		/// <summary>
		BEHAVIOR_TARGET = 4,
		/// <summary>
		/// 不需要指定目标就能释放的能力，当按下能力按钮的时候，这个能力就会被释放
		/// <summary>
		BEHAVIOR_NO_TARGET = 8,
		/// <summary>
		/// 持续性施法能力
		/// <summary>
		BEHAVIOR_CHANNELLED = 16,
		/// <summary>
		/// 能力将会在鼠标指定的位置释放
		/// <summary>
		BEHAVIOR_POINT = 32,
		/// <summary>
		/// 范围型能力,将会显示能力释放的范围
		/// <summary>
		BEHAVIOR_AOE = 64,
	}
}
