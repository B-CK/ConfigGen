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
	public class TM1 : Cfg.TestModule.TClass
	{
		/// <summary>
		/// 继承1
		/// <summary>
		public readonly long V3;
		public TM1(DataStream data) : base(data)
		{
			V3 = data.GetLong();
		}
	}
}
