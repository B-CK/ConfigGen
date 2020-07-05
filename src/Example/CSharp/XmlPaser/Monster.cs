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
	public class Monster : Example.XmlPaser.Custom
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly int Attack;
		public Monster(DataStream data) : base(data)
		{
			Attack = data.GetInt();
		}
	}
}
