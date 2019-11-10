using Example;
using System;
using System.Collections.Generic;
namespace Example.CustomTypes
{
	/// <summary>
	/// NPC
	/// <summary>
public class NPC : Example.CustomTypes.Custom
	{
		/// <summary>
		/// 别名
		/// <summary>
		public readonly string Alias;
		public NPC(DataStream data) : base(data)
		{
			Alias = data.GetString();
		}
	}
}
