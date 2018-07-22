using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class SpawnObject : Lson.Skill.Action
	{
		/// <summary>
		/// 子物体ID
		/// <summary>
		public int Id;
		/// <summary>
		/// 子物体类型
		/// <summary>
		public int SpawnType;
		/// <summary>
		/// 子物体存活时间
		/// <summary>
		public float Life;
	}
}
