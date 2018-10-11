using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	public  class Bomb : Cfg.Skill.TraceObject
	{
		/// <summary>
		/// 打击点组(HitPointGroup)ID
		/// <summary>
		public readonly int Id;

		public Bomb(DataStream data) : base(data)
		{
			this.Id = data.GetInt();
		}
	}
}
