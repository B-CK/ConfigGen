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
	public class Controller : Cfg.Skill.Timeline
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Path;
		public Controller(DataStream data) : base(data)
		{
			Path = data.GetString();
		}
	}
}
