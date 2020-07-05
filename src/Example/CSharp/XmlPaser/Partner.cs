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
	public class Partner : Example.XmlPaser.Custom
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Alias;
		/// <summary>
		/// 
		/// <summary>
		public readonly Example.XmlPaser.BuffType Buff;
		public Partner(DataStream data) : base(data)
		{
			Alias = data.GetString();
			Buff = (Example.XmlPaser.BuffType)data.GetInt();
		}
	}
}
