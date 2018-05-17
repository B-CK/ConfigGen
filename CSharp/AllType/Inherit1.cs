using System;
using System.Collections.Generic;
using Csv;

namespace Csv.AllType
{
	public sealed class Inherit1 : AllType.SingleClass
	{
		/// <summary>
		/// Var3
		/// <summary>
		public readonly int Var3;

		pubilic Inherit1(DataStream data) : base(data)
		{
			this.Var3 = data.GetInt();
		}
	}
}
