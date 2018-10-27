using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.AllType
{
	public class M2 : XmlCfg.AllType.SingleClass
	{
		/// <summary>
		/// 继承2
		/// <summary>
		public bool V4;

		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "V4", this.V4);
		}

		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "V4": this.V4 = ReadBool(_2); break;
			}
		}
	}
}
