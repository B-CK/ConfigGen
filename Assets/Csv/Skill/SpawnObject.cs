using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	public  class SpawnObject : Cfg.Skill.Action
	{
		/// <summary>
		/// 子物体ID
		/// <summary>
		public readonly float Id;
		/// <summary>
		/// 子物体类型
		/// <summary>
		public readonly float SpawnType;
		/// <summary>
		/// 子物体存活时间
		/// <summary>
		public readonly float Life;

		public SpawnObject(DataStream data) : base(data)
		{
			this.Id = data.GetFloat();
			this.SpawnType = data.GetFloat();
			this.Life = data.GetFloat();
		}
	}
}
