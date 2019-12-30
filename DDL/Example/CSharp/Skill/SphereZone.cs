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
	public class SphereZone : Cfg.Skill.HitZone
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly float Radius;
		public SphereZone(DataStream data) : base(data)
		{
			Radius = data.GetFloat();
		}
	}
}
