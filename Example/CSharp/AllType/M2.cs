using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class M2 : Cfg.AllType.SingleClass
	{
		/// <summary>
		/// 继承2
		/// <summary>
		public readonly bool V4;
		public M2(DataStream data) : base(data)
		{
			V4 = data.GetBool();
		}
	}
}
