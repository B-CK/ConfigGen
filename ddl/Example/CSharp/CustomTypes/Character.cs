using System;
using System.Collections.Generic;
using Example;

namespace CustomTypes
{
	/// <summary>
	/// 
	/// <summary>
	public class Character : CfgObject
	{
		/// <summary>
		/// id
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// 角色信息
		/// <summary>
		public readonly CustomTypes.Custom Custom;
		
		public Character(DataStream data)
		{
			ID = data.GetInt();
			Custom = (CustomTypes.Custom)data.GetObject(data.GetString());
		}
	}
}
