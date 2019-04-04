using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class SingleClass : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Var1;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Var2;
		
		public SingleClass(DataStream DataStream)
		{
			Var1 = data.GetString();
			Var2 = data.GetFloat();
		}
	}
}
