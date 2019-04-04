using System;
using System.IO;
using Xml;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg
{
	/// <summary>
	/// 
	/// <summary>
	public class Vector2 : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly float X;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Y;

		public override void Write(TextWriter _1)
		{
			Write(_1, "X", this.X);
			Write(_1, "Y", this.Y);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "X": Readfloat(_2);
				case "Y": Readfloat(_2);
			}
		}
	}
}
