using System;
using System.Collections.Generic;
using Csv;

namespace Csv.JLson
{
	public class LSingleClass : CfgObject
	{
		/// <summary>
		/// Var1
		/// <summary>
		public readonly string Var1;
		/// <summary>
		/// Var2
		/// <summary>
		public readonly bool Var2;

		public LSingleClass(DataStream data)
		{
			this.Var1 = data.GetString();
			this.Var2 = data.GetBool();
		}
	}
}
