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
	public class Buff : Cfg.Skill.PlayParticle
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly int Id;
		public Buff(DataStream data) : base(data)
		{
			Id = data.GetInt();
		}
	}
}
