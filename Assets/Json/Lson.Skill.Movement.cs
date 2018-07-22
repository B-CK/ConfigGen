using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class Movement : Lson.Skill.Action
	{
		/// <summary>
		/// 移动方式
		/// <summary>
		public Lson.Skill.MoveType Type;
		/// <summary>
		/// 持续时间
		/// <summary>
		public float Duration;
		/// <summary>
		/// 速度
		/// <summary>
		public float Speed;
		/// <summary>
		/// 加速度
		/// <summary>
		public float Acceleration;
	}
}
