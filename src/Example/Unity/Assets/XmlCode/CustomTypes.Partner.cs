using System;
using XmlEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Editor.CustomTypes
{
	/// <summary>
	/// 伙伴
	/// <summary>
	public class Partner : Editor.CustomTypes.Custom
	{
		/// <summary>
		/// 别名
		/// <summary>
		public string Alias;
		/// <summary>
		/// 光环
		/// <summary>
		public Editor.CustomTypes.BuffType Buff;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "Alias", Alias);
			Write(_1, "Buff", Buff);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Alias": Alias = ReadString(_2); break;
				case "Buff": Buff = (Editor.CustomTypes.BuffType)ReadInt(_2); break;
			}
		}
	}
}
