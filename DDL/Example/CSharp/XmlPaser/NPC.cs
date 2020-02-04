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
	public class NPC : Example.XmlPaser.Custom
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Alias;
		public NPC(DataStream data) : base(data)
		{
			Alias = data.GetString();
		}
	}
}
