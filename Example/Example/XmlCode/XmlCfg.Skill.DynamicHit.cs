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
	public class DynamicHit : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public string Target;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Target", this.Target);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Target": Target = ReadString(_2); break;
			}
		}
	}
}
