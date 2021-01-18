using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class SharedBool : AbilitySystem.Ability.SharedArg
	{
		/// <summary>
		/// bool数据
		/// <summary>
		
		public bool value;
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
				case "value": value = ReadBool(_2); break;
			}
		}
	}
}
