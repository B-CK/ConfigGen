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
	public class Timeline : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly int Start;
		/// <summary>
		/// 
		/// <summary>
		public readonly int End;
		public Timeline(DataStream data)
		{
			Start = data.GetInt();
			End = data.GetInt();
		}
	}
}
