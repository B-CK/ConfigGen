using System;
using System.Collections.Generic;
using Csv;

namespace Csv.Group
{
	public class GroupClassField : CfgObject
	{
		/// <summary>
		/// 整形变量
		/// <summary>
		public readonly int VarInt;
		/// <summary>
		/// 浮点变量
		/// <summary>
		public readonly float VarFloat;

		public GroupClassField(DataStream data)
		{
			this.VarInt = data.GetInt();
			this.VarFloat = data.GetFloat();
		}
	}
}
