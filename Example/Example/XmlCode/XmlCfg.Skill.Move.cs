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
	public class Move : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Skill.MoveType Type;
		/// <summary>
		/// 
		/// <summary>
		public bool IsRelateSelf;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Vector3 Offset;
		/// <summary>
		/// 
		/// <summary>
		public float Angle;
		/// <summary>
		/// 
		/// <summary>
		public float Speed;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Type", this.Type);
			Write(_1, "IsRelateSelf", this.IsRelateSelf);
			Write(_1, "Offset", this.Offset);
			Write(_1, "Angle", this.Angle);
			Write(_1, "Speed", this.Speed);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Type": Type = (XmlCfg.Skill.MoveType)ReadInt(_2); break;
				case "IsRelateSelf": IsRelateSelf = ReadBool(_2); break;
				case "Offset": Offset = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "Angle": Angle = ReadFloat(_2); break;
				case "Speed": Speed = ReadFloat(_2); break;
			}
		}
	}
}
