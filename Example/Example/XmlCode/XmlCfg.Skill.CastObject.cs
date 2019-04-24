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
	public class CastObject : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public bool IsTraceTarget;
		/// <summary>
		/// 
		/// <summary>
		public int CurveId;
		/// <summary>
		/// 
		/// <summary>
		public bool PassBody;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Vector3 Position;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Vector3 EulerAngles;

		public override void Write(TextWriter _1)
		{
			Write(_1, "IsTraceTarget", this.IsTraceTarget);
			Write(_1, "CurveId", this.CurveId);
			Write(_1, "PassBody", this.PassBody);
			Write(_1, "Position", this.Position);
			Write(_1, "EulerAngles", this.EulerAngles);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "IsTraceTarget": IsTraceTarget = ReadBool(_2); break;
				case "CurveId": CurveId = ReadInt(_2); break;
				case "PassBody": PassBody = ReadBool(_2); break;
				case "Position": Position = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "EulerAngles": EulerAngles = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
			}
		}
	}
}
