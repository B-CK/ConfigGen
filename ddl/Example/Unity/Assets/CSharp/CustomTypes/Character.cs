using Example;
using System;
using System.Collections.Generic;
namespace Example.CustomTypes
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
		public readonly Example.CustomTypes.Custom Custom;
		public Character(DataStream data)
		{
			ID = data.GetInt();
			Custom = (Example.CustomTypes.Custom)data.GetObject(data.GetString());
		}
	}
}
