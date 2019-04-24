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
	public class Timeline : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public int Start;
		/// <summary>
		/// 
		/// <summary>
		public int End;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Start", this.Start);
			Write(_1, "End", this.End);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Start": Start = ReadInt(_2); break;
				case "End": End = ReadInt(_2); break;
			}
		}
	}
}
