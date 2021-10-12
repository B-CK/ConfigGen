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
	public class TM2 : Cfg.TestModule.TClass
	{
		/// <summary>
		/// 继承2
		/// <summary>
		public readonly bool V4;
		public TM2(DataStream data) : base(data)
		{
			V4 = data.GetBool();
		}
	}
}
