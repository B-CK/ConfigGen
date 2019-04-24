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
	public class StaticHit : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Skill.HitZone Zone;
		/// <summary>
		/// 
		/// <summary>
		public int SequeueID;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Zone", this.Zone);
			Write(_1, "SequeueID", this.SequeueID);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Zone": Zone = ReadObject<XmlCfg.Skill.HitZone>(_2, "XmlCfg.Skill.HitZone"); break;
				case "SequeueID": SequeueID = ReadInt(_2); break;
			}
		}
	}
}
