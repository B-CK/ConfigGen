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
	public class AClass : Cfg.Base.BaseClass
	{
		/// <summary>
		/// Int
		/// <summary>
		public readonly int AIndex;
		public AClass(DataStream data) : base(data)
		{
			AIndex = data.GetInt();
		}
	}
}
