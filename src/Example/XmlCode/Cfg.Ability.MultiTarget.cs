using System;
using XmlEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Ability
{
	/// <summary>
	/// 
	/// <summary>
	public partial class MultiTarget : Cfg.Ability.Target
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly float radius;
		/// <summary>
		/// 
		/// <summary>
		public readonly int maxTargets;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "radius", radius);
			Write(_1, "maxTargets", maxTargets);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "radius": radius = ReadFloat(_2); break;
				case "maxTargets": maxTargets = ReadInt(_2); break;
			}
		}
	}
}
