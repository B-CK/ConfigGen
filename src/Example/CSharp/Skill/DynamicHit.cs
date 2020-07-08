using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class DynamicHit : Cfg.Skill.StaticHit
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Target;
		public DynamicHit(DataStream data) : base(data)
		{
			Target = data.GetString();
		}
	}
}
