using System;
using System.Collections.Generic;
using Csv;

namespace Csv.Group
{
	public class GroupClass0 : CfgObject
	{
		/// <summary>
		/// 整形变量
		/// <summary>
		public readonly int VarInt;

		public GroupClass0(DataStream data)
		{
			this.VarInt = data.GetInt();
		}
	}
}
