using System;
using XmlEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Ability
{
	/// <summary>
	/// 
	/// <summary>
	public partial class Property : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.PropertyType property;
		/// <summary>
		/// 
		/// <summary>
		public readonly float value;
		public override void Write(TextWriter _1)
		{
			Write(_1, "property", property);
			Write(_1, "value", value);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "property": property = (Cfg.Ability.PropertyType)ReadString(_2); break;
				case "value": value = ReadFloat(_2); break;
			}
		}
	}
}
