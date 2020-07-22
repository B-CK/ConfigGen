using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.Base
{
	/// <summary>
	/// 
	/// <summary>
	public class BClass : Cfg.Base.BaseClass
	{
		/// <summary>
		/// Int
		/// <summary>
		public readonly string BIndex;
		public BClass(DataStream data) : base(data)
		{
			BIndex = data.GetString();
		}
	}
}
