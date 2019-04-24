using System;
using System.Linq;
using System.IO;
using XmlCfg;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Character
{
	/// <summary>
	/// 
	/// <summary>
	public class Model : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public string Name;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Character.GroupType GroupType;
		/// <summary>
		/// 
		/// <summary>
		public string ModelPath;
		/// <summary>
		/// 
		/// <summary>
		public string AvatarPath;
		/// <summary>
		/// 
		/// <summary>
		public float BodyRadius;
		/// <summary>
		/// 
		/// <summary>
		public float BodyHeight;
		/// <summary>
		/// 
		/// <summary>
		public float ModelScale;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Name", this.Name);
			Write(_1, "GroupType", this.GroupType);
			Write(_1, "ModelPath", this.ModelPath);
			Write(_1, "AvatarPath", this.AvatarPath);
			Write(_1, "BodyRadius", this.BodyRadius);
			Write(_1, "BodyHeight", this.BodyHeight);
			Write(_1, "ModelScale", this.ModelScale);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Name": Name = ReadString(_2); break;
				case "GroupType": GroupType = (XmlCfg.Character.GroupType)ReadInt(_2); break;
				case "ModelPath": ModelPath = ReadString(_2); break;
				case "AvatarPath": AvatarPath = ReadString(_2); break;
				case "BodyRadius": BodyRadius = ReadFloat(_2); break;
				case "BodyHeight": BodyHeight = ReadFloat(_2); break;
				case "ModelScale": ModelScale = ReadFloat(_2); break;
			}
		}
	}
}
