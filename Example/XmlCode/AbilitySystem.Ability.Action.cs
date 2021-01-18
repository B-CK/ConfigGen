using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class Action : XmlObject
	{
		/// <summary>
		/// 名称
		/// <summary>
		
		public string name;
		public override void Write(TextWriter _1)
		{
			Write(_1, "name", name);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "name": name = ReadString(_2); break;
			}
		}
	}
}
