using Example;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Example.CustomTypes
{
	/// <summary>
	/// 伙伴
	/// <summary>
	public class Partner : Example.CustomTypes.Custom
	{
		/// <summary>
		/// 别名
		/// <summary>
		public readonly string Alias;
		/// <summary>
		/// 光环
		/// <summary>
		public readonly Example.CustomTypes.BuffType Buff;
		public Partner(DataStream data) : base(data)
		{
			Alias = data.GetString();
			Buff = (Example.CustomTypes.BuffType)data.GetInt();
		}
	}
}
