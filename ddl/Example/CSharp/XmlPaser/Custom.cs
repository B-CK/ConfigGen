using Example;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Example.XmlPaser
{
	/// <summary>
	/// 
	/// <summary>
	public class Custom : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Name;
		/// <summary>
		/// 
		/// <summary>
		public readonly int Level;
		public Custom(DataStream data)
		{
			Name = data.GetString();
			Level = data.GetInt();
		}
	}
}
