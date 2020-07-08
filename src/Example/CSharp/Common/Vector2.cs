using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.Common
{
	/// <summary>
	/// 
	/// <summary>
	public class Vector2 : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly float X;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Y;
		public Vector2(DataStream data)
		{
			X = data.GetFloat();
			Y = data.GetFloat();
		}
	}
}
