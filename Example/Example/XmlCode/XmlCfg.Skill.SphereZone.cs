using System;
using System.Linq;
using System.IO;
using XmlCfg;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class SphereZone : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public float Radius;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Radius", this.Radius);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Radius": Radius = ReadFloat(_2); break;
			}
		}
	}
}
