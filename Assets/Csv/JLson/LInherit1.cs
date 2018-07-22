using System;
using System.Collections.Generic;
using Csv;

namespace Csv.JLson
{
	public class LInherit1 : Csv.JLson.LSingleClass
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
