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
	public class HitZone : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public int Id;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Skill.HitSharpType Sharp;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Vector3 Offset;
		/// <summary>
		/// 
		/// <summary>
		public int MaxNum;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Id", this.Id);
			Write(_1, "Sharp", this.Sharp);
			Write(_1, "Offset", this.Offset);
			Write(_1, "MaxNum", this.MaxNum);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Id": Id = ReadInt(_2); break;
				case "Sharp": Sharp = (XmlCfg.Skill.HitSharpType)ReadInt(_2); break;
				case "Offset": Offset = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "MaxNum": MaxNum = ReadInt(_2); break;
			}
		}
	}
}
