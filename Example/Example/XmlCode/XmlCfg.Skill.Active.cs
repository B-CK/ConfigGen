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
	public class Active : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public bool Enable;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Enable", this.Enable);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Enable": Enable = ReadBool(_2); break;
			}
		}
	}
}
