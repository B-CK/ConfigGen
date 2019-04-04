using System;
using System.Linq;
using System.IO;
using XmlCfg;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class M1 : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public long V3;

		public override void Write(TextWriter _1)
		{
			Write(_1, "V3", this.V3);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "V3": V3 = ReadLong(_2); break;
			}
		}
	}
}
