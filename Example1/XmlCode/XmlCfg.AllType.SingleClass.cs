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
	public class SingleClass : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Var1;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Var2;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Var1", this.Var1);
			Write(_1, "Var2", this.Var2);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Var1": Readstring(_2);
				case "Var2": Readfloat(_2);
			}
		}
	}
}
