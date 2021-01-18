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
	public class Active : Cfg.Skill.Controller
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly bool Enable;
		public Active(DataStream data) : base(data)
		{
			Enable = data.GetBool();
		}
	}
}
