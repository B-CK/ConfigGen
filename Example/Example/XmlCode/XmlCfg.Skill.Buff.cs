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
	public class Buff : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public int Id;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Id", this.Id);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Id": Id = ReadInt(_2); break;
			}
		}
	}
}
