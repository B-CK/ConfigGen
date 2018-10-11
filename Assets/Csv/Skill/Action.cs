using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	public  class Action : CfgObject
	{
		/// <summary>
		/// 时间点
		/// <summary>
		public readonly float Timeline;

		public Action(DataStream data)
		{
			this.Timeline = data.GetFloat();
		}
	}
}
