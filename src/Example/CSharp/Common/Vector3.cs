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
	public class Vector3 : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly float X;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Y;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Z;
		public Vector3(DataStream data)
		{
			X = data.GetFloat();
			Y = data.GetFloat();
			Z = data.GetFloat();
		}
	}
}
