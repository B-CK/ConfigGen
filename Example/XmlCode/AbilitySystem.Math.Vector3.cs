using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Math
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class Vector3 : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		
		public float x;
		/// <summary>
		/// 
		/// <summary>
		
		public float y;
		/// <summary>
		/// 
		/// <summary>
		
		public float z;
		public override void Write(TextWriter _1)
		{
			Write(_1, "x", x);
			Write(_1, "y", y);
			Write(_1, "z", z);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "x": x = ReadFloat(_2); break;
				case "y": y = ReadFloat(_2); break;
				case "z": z = ReadFloat(_2); break;
			}
		}
	}
}
