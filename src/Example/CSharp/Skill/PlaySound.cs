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
	public class PlaySound : Cfg.Skill.Timeline
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Path;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Volume;
		public PlaySound(DataStream data) : base(data)
		{
			Path = data.GetString();
			Volume = data.GetFloat();
		}
	}
}
