using System;
using System.Collections.Generic;
using Csv;

namespace Csv.Lson
{
	public class LInherit1 : Csv.Lson.LSingleClass
	{
		/// <summary>
		/// Var3
		/// <summary>
		public readonly int Var3;

		public LInherit1(DataStream data) : base(data)
		{
			this.Var3 = data.GetInt();
		}
	}
}
