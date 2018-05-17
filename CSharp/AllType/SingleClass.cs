﻿using System;
using System.Collections.Generic;
using Csv;

namespace Csv.AllType
{
	public sealed class SingleClass : CfgObject
	{
		/// <summary>
		/// Var1
		/// <summary>
		public readonly string Var1;
		/// <summary>
		/// Var2
		/// <summary>
		public readonly bool Var2;

		pubilic SingleClass(DataStream data)
		{
			this.Var1 = data.GetString();
			this.Var2 = data.GetBool();
		}
	}
}