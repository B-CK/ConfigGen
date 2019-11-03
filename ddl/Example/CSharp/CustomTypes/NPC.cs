using System;
using System.Collections.Generic;
using Example;

namespace CustomTypes
{
	/// <summary>
	/// NPC
	/// <summary>
	public class NPC : CustomTypes.Custom
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
