using System;
using System.IO;
using Xml;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class M2 : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly bool V4;

		public override void Write(TextWriter _1)
		{
			Write(_1, "V4", this.V4);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "V4": Readbool(_2);
			}
		}
	}
}
