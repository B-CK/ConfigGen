using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.TestModule
{
	/// <summary>
	/// 
	/// <summary>
	public class TClass : CfgObject
	{
		/// <summary>
		/// Var1
		/// <summary>
		public readonly string Var1;
		/// <summary>
		/// Var2
		/// <summary>
		public readonly float Var2;
		public TClass(DataStream data)
		{
			Var1 = data.GetString();
			Var2 = data.GetFloat();
		}
	}
}
