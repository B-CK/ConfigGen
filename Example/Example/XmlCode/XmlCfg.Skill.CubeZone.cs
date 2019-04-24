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
	public class CubeZone : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Vector3 Scale;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Scale", this.Scale);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Scale": Scale = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
			}
		}
	}
}
