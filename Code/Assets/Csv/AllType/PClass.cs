using System;
using System.Collections.Generic;
using Csv;

namespace Csv.AllType
{
	public class PClass : CfgObject
	{
		/// <summary>
		/// Var5
		/// <summary>
		public readonly long Var5;

		public PClass(DataStream data)

		{
			this.Var5 = data.GetLong();
		}
	}
}
