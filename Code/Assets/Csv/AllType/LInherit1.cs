using System;
using System.Collections.Generic;
using Csv;

namespace Csv.AllType
{
	public class LInherit1 : Csv.AllType.LSingleClass
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
