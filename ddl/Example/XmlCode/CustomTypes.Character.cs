using XmlEditor;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
namespace XmlExample.CustomTypes
{
	/// <summary>
	/// 
	/// <summary>
	public class Character : XmlObject
	{
		/// <summary>
		/// id
		/// <summary>
		public int ID;
		/// <summary>
		/// 角色信息
		/// <summary>
		public XmlExample.CustomTypes.Custom Custom;
		public override void Write(TextWriter _1)
		{
			Write(_1, "ID", ID);
			Write(_1, "Custom", Custom);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "ID": ID = ReadInt(_2); break;
			}
		}
	}
}
