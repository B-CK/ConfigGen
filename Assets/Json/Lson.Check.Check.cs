using System;
using System.Collections.Generic;

namespace Lson.Check
{
	public class Check : LsonObject
	{
		/// <summary>
		/// 整形变量
		/// <summary>
		public int VarInt;
		/// <summary>
		/// 浮点变量
		/// <summary>
		public float VarFloat;
		/// <summary>
		/// 存在性检查
		/// <summary>
		public string VarString;
		/// <summary>
		/// 唯一性检查
		/// <summary>
		public string VarUnique;
		/// <summary>
		/// 非空
		/// <summary>
		public string VarNoEmpty;
		/// <summary>
		/// 非空
		/// <summary>
		public long VarNoEmpty2;
		/// <summary>
		/// 列表
		/// <summary>
		public List<string> VarList;
		/// <summary>
		/// 字典
		/// <summary>
		public Dictionary<int, string> VarDict;
	}
}
