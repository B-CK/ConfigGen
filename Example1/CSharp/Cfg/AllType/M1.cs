using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class M1 : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly long V3;
		
		public M1(DataStream DataStream) : base(DataStream)
		{
			V3 = data.GetLong();
		}
	}
}
