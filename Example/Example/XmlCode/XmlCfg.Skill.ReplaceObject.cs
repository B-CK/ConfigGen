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
	public class ReplaceObject : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public string NewObject;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Vector3 Offset;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Vector3 EulerAngles;

		public override void Write(TextWriter _1)
		{
			Write(_1, "NewObject", this.NewObject);
			Write(_1, "Offset", this.Offset);
			Write(_1, "EulerAngles", this.EulerAngles);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "NewObject": NewObject = ReadString(_2); break;
				case "Offset": Offset = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "EulerAngles": EulerAngles = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
			}
		}
	}
}
