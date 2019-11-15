using System;
using XmlEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Editor.CustomTypes
{
	/// <summary>
	/// NPC
	/// <summary>
	public class NPC : Editor.CustomTypes.Custom
	{
		/// <summary>
		/// 别名
		/// <summary>
		public string Alias;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "Alias", Alias);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Alias": Alias = ReadString(_2); break;
			}
		}
	}
}
