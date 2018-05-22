using System;
using System.Collections.Generic;
using Csv;

namespace Csv.AllType
{
	public class Inherit1 : Csv.AllType.SingleClass
	{
		/// <summary>
		/// Var3
		/// <summary>
		public readonly int Var3;

		public Inherit1(DataStream data) : base(data)
		{
			this.Var3 = data.GetInt();
		}
	}
}
