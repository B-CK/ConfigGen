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
	public partial class SpecialFloat : Cfg.Ability.SpecialArg
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly float value;
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
				case "value": value = ReadFloat(_2); break;
			}
		}
	}
}
