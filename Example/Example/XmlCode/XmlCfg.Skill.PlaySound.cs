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
	public class PlaySound : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public string Path;
		/// <summary>
		/// 
		/// <summary>
		public float Volume;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Path", this.Path);
			Write(_1, "Volume", this.Volume);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Path": Path = ReadString(_2); break;
				case "Volume": Volume = ReadFloat(_2); break;
			}
		}
	}
}
