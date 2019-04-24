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
	public class PlayParticle : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public string Path;
		/// <summary>
		/// 
		/// <summary>
		public bool IsRelateSelf;
		/// <summary>
		/// 
		/// <summary>
		public bool FollowDir;
		/// <summary>
		/// 
		/// <summary>
		public string NodeName;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Vector3 Position;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Vector3 EulerAngles;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Vector3 Scale;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Skill.EffectAlignType AlignType;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Path", this.Path);
			Write(_1, "IsRelateSelf", this.IsRelateSelf);
			Write(_1, "FollowDir", this.FollowDir);
			Write(_1, "NodeName", this.NodeName);
			Write(_1, "Position", this.Position);
			Write(_1, "EulerAngles", this.EulerAngles);
			Write(_1, "Scale", this.Scale);
			Write(_1, "AlignType", this.AlignType);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Path": Path = ReadString(_2); break;
				case "IsRelateSelf": IsRelateSelf = ReadBool(_2); break;
				case "FollowDir": FollowDir = ReadBool(_2); break;
				case "NodeName": NodeName = ReadString(_2); break;
				case "Position": Position = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "EulerAngles": EulerAngles = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "Scale": Scale = ReadObject<XmlCfg.Vector3>(_2, "XmlCfg.Vector3"); break;
				case "AlignType": AlignType = (XmlCfg.Skill.EffectAlignType)ReadInt(_2); break;
			}
		}
	}
}
