using System;
using System.Collections.Generic;
using Example;

namespace CustomTypes
{
	/// <summary>
	/// 怪物
	/// <summary>
	public class Monster : CustomTypes.Custom
	{
		/// <summary>
		/// 攻击
		/// <summary>
		public readonly int Attack;
		
		public Monster(DataStream data) : base(data)
		{
			Attack = data.GetInt();
		}
	}
}
