using System;
using System.Collections.Generic;
using Csv;

namespace Csv.Lson
{
	public class LInherit2 : Csv.Lson.LSingleClass
	{
		/// <summary>
		/// Var4
		/// <summary>
		public readonly long Var4;

		public LInherit2(DataStream data) : base(data)
		{
			this.Var4 = data.GetLong();
		}
	}
}
