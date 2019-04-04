using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class M2 : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly bool V4;
		
		public M2(DataStream DataStream) : base(DataStream)
		{
			V4 = data.GetBool();
		}
	}
}
