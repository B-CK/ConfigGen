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
	public partial class SpecialArg : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Name;
		public override void Write(TextWriter _1)
		{
			Write(_1, "Name", Name);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Name": Name = ReadString(_2); break;
			}
		}
	}
}
