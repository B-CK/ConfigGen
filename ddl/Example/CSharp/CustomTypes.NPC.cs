using XmlEditor;
using System;
using System.IO
using System.Xml
using System.Linq
using System.Collections.Generic;
namespace Example.CustomTypes
{
	/// <summary>
	/// NPC
	/// <summary>
public class NPC : XmlEditor.CustomTypes.Custom
	{
		/// <summary>
		/// 别名
		/// <summary>
		public string Alias;
		public override Write(TextWriter _1)
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
