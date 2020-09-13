using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Ability
{
	/// <summary>
	/// 
	/// <summary>
	[System.Flags]
	public enum UnitTeam
	{
		/// <summary>
		/// 中立队伍(属于任意一方)
		/// <summary>
		TEAM_EVERYTHING = -1,
		/// <summary>
		/// 无(默认)
		/// <summary>
		TEAM_NONE = 0,
		/// <summary>
		/// 敌方队伍
		/// <summary>
		TEAM_ENEMY = 1,
		/// <summary>
		/// 友方队伍
		/// <summary>
		TEAM_FRIENDLY = 2,
	}
}
