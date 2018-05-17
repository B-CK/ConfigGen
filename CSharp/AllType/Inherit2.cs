using System;
using System.Collections.Generic;
using Csv;

namespace Csv.AllType
{
	public sealed class Inherit2 : AllType.SingleClass
	{
		/// <summary>
		/// Var4
		/// <summary>
		public readonly long Var4;

		pubilic Inherit2(DataStream data) : base(data)
		{
			this.Var4 = data.GetLong();
		}
	}
}
