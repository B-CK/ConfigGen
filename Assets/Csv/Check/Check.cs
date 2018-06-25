using System;
using System.Collections.Generic;
using Csv;

namespace Csv.Check
{
	public class Check : CfgObject
	{
		/// <summary>
		/// 整形变量
		/// <summary>
		public readonly int VarInt;
		/// <summary>
		/// 浮点变量
		/// <summary>
		public readonly float VarFloat;
		/// <summary>
		/// 存在性检查
		/// <summary>
		public readonly string VarString;
		/// <summary>
		/// 唯一性检查
		/// <summary>
		public readonly string VarUnique;
		/// <summary>
		/// 非空
		/// <summary>
		public readonly string VarNoEmpty;
		/// <summary>
		/// 非空
		/// <summary>
		public readonly long VarNoEmpty2;
		/// <summary>
		/// 列表
		/// <summary>
		public readonly List<string> VarList = new List<string>();
		/// <summary>
		/// 字典
		/// <summary>
		public readonly Dictionary<int, string> VarDict = new Dictionary<int, string>();

		public Check(DataStream data)
		{
			this.VarInt = data.GetInt();
			this.VarFloat = data.GetFloat();
			this.VarString = data.GetString();
			this.VarUnique = data.GetString();
			this.VarNoEmpty = data.GetString();
			this.VarNoEmpty2 = data.GetLong();
			for (int n = data.GetInt(); n-- > 0; )
			{
				this.VarList.Add(data.GetString());
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				int k = data.GetInt();
				this.VarDict[k] = data.GetString();
			}
		}
	}
}
