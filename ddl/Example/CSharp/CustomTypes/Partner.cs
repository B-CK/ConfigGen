using System;
using System.Collections.Generic;
using Example;

namespace CustomTypes
{
	/// <summary>
	/// 伙伴
	/// <summary>
	public class Partner : CustomTypes.Custom
	{
		/// <summary>
		/// 别名
		/// <summary>
		public readonly string Alias;
		/// <summary>
		/// 光环
		/// <summary>
		public readonly int Buff;
		
		public Partner(DataStream data) : base(data)
		{
			Alias = data.GetString();
			Buff = data.GetInt();
		}
	}
}
