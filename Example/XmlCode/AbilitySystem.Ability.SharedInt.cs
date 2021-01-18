using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class SharedInt : AbilitySystem.Ability.SharedArg
	{
		/// <summary>
		/// int数据
		/// <summary>
		
		public int value;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "value", value);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "value": value = ReadInt(_2); break;
			}
		}
	}
}
