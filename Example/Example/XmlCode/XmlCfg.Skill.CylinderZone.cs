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
	public class CylinderZone : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public float Radius;
		/// <summary>
		/// 
		/// <summary>
		public float Height;
		/// <summary>
		/// 
		/// <summary>
		public float Angle;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Radius", this.Radius);
			Write(_1, "Height", this.Height);
			Write(_1, "Angle", this.Angle);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Radius": Radius = ReadFloat(_2); break;
				case "Height": Height = ReadFloat(_2); break;
				case "Angle": Angle = ReadFloat(_2); break;
			}
		}
	}
}
