using System;
using System.Collections.Generic;
using Csv;

namespace Csv.AllType
{
	public sealed class SingleClass : CfgObject
	{
		public readonly string Var1;
		public readonly float Var2;

		public SingleClass(DataStream data)
		{
			this.Var1 = data.GetString();
			this.Var2 = data.GetFloat();
		}
	}
}
