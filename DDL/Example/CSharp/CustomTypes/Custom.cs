using Example;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Example.CustomTypes
{
	/// <summary>
	/// 自定义
	/// <summary>
	public class Custom : CfgObject
	{
		/// <summary>
		/// 名称
		/// <summary>
		public readonly string Name;
		/// <summary>
		/// 等级
		/// <summary>
		public readonly int Level;
		public Custom(DataStream data)
		{
			Name = data.GetString();
			Level = data.GetInt();
		}
	}
}
