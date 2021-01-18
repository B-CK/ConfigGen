using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class Property : XmlObject
	{
		/// <summary>
		/// 属性类型
		/// <summary>
		
		public AbilitySystem.Ability.PropertyType type;
		/// <summary>
		/// 修改量
		/// <summary>
		
		public float value;
		public override void Write(TextWriter _1)
		{
			Write(_1, "type", type);
			Write(_1, "value", value);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "type": type = (AbilitySystem.Ability.PropertyType)ReadInt(_2); break;
				case "value": value = ReadFloat(_2); break;
			}
		}
	}
}
