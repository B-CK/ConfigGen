using XmlEditor;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
namespace XmlExample.CustomTypes
{
	/// <summary>
	/// 自定义
	/// <summary>
	public class Custom : XmlObject
	{
		/// <summary>
		/// 名称
		/// <summary>
		public string Name;
		/// <summary>
		/// 等级
		/// <summary>
		public int Level;
		public override void Write(TextWriter _1)
		{
			Write(_1, "Name", Name);
			Write(_1, "Level", Level);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Name": Name = ReadString(_2); break;
				case "Level": Level = ReadInt(_2); break;
			}
		}
	}
}
