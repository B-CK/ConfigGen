using System;
using System.Collections.Generic;
using Csv;

namespace Csv.Group
{
	public class GroupField0 : CfgObject
	{
		/// <summary>
		/// 整形变量
		/// <summary>
		public readonly int VarInt;

		public GroupField0(DataStream data)
		{
			this.VarInt = data.GetInt();
		}
	}
}
