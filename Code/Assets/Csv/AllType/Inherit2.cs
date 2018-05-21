using System;
using System.Collections.Generic;
using Csv;

namespace Csv.AllType
{
	public class Inherit2 : Csv.AllType.SingleClass
	{
		/// <summary>
		/// Var4
		/// <summary>
		public readonly long Var4;

		public Inherit2(DataStream data) : base(data)

		{
			this.Var4 = data.GetLong();
		}
	}
}
