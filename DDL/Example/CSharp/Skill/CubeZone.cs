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
	public class CubeZone : Cfg.Skill.HitZone
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Common.Vector3 Scale;
		public CubeZone(DataStream data) : base(data)
		{
			Scale = new Cfg.Common.Vector3(data);
		}
	}
}
