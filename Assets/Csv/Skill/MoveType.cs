using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	public enum MoveType
	{
		/// <summary>
		/// 方向移动
		/// <summary>
		MoveBack = 0,
		/// <summary>
		/// 向目标移动
		/// <summary>
		MoveToTarget = 1,
		/// <summary>
		/// 按当前方向移动
		/// <summary>
		MoveInDirection = 2,
	}
}
