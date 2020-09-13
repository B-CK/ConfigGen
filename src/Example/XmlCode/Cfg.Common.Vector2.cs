using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Common
{
	/// <summary>
	/// 
	/// <summary>
	public partial class Vector2 : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public float x;
		/// <summary>
		/// 
		/// <summary>
		public float y;
		public override void Write(TextWriter _1)
		{
			Write(_1, "x", x);
			Write(_1, "y", y);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "x": x = ReadFloat(_2); break;
				case "y": y = ReadFloat(_2); break;
			}
		}
	}
}
