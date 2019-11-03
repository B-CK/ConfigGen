using System;
using System.Collections.Generic;
using Example;

namespace CustomTypes
{
	/// <summary>
	/// 自定义
	/// <summary>
	public class Custom : CfgObject
	{
		/// <summary>
		/// ID
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// 名称
		/// <summary>
		public readonly string Name;
		/// <summary>
		/// 等级
		/// <summary>
		public readonly int Level;
		/// <summary>
		/// 类类型
		/// <summary>
		public readonly CustomTypes.BuffType Base;
		
		public Custom(DataStream data)
		{
			ID = data.GetInt();
			Name = data.GetString();
			Level = data.GetInt();
			Base = (CustomTypes.BuffType)data.GetInt();
		}
	}
}
