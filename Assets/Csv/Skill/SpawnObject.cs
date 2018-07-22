using System;
using System.Collections.Generic;
using Csv;

namespace Csv.Skill
{
	public class SpawnObject : Csv.Skill.Action
	{
		/// <summary>
		/// 子物体ID
		/// <summary>
		public readonly int Id;
		/// <summary>
		/// 子物体类型
		/// <summary>
		public readonly int SpawnType;
		/// <summary>
		/// 子物体存活时间
		/// <summary>
		public readonly float Life;

		public SpawnObject(DataStream data) : base(data)
		{
			this.Id = data.GetInt();
			this.SpawnType = data.GetInt();
			this.Life = data.GetFloat();
		}
	}
}
