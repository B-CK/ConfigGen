using System;
using System.Linq;
using System.IO;
using XmlExample;
using System.Xml;
using System.Collections.Generic;

namespace XmlCustomTypes
{
	/// <summary>
	/// 自定义
	/// <summary>
	public class Custom : XmlObject
	{
		/// <summary>
		/// ID
		/// <summary>
		public int ID;
		/// <summary>
		/// 名称
		/// <summary>
		public string Name;
		/// <summary>
		/// 等级
		/// <summary>
		public int Level;
		/// <summary>
		/// 类类型
		/// <summary>
		public CustomTypes.BuffType Base;

		public override void Write(TextWriter _1)
		{
			Write(_1, "ID", this.ID);
			Write(_1, "Name", this.Name);
			Write(_1, "Level", this.Level);
			Write(_1, "Base", this.Base);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "ID": ID = ReadInt(_2); break;
				case "Name": Name = ReadString(_2); break;
				case "Level": Level = ReadInt(_2); break;
				case "Base": Base = (CustomTypes.BuffType)ReadInt(_2); break;
			}
		}
	}
}
