using XmlEditor;
using System;
using System.IO
using System.Xml
using System.Linq
using System.Collections.Generic;
namespace Example.CustomTypes
{
	/// <summary>
	/// 伙伴
	/// <summary>
public class Partner : XmlEditor.CustomTypes.Custom
	{
		/// <summary>
		/// 别名
		/// <summary>
		public string Alias;
		/// <summary>
		/// 光环
		/// <summary>
		public XmlEditor.CustomTypes.BuffType Buff;
		public override Write(TextWriter _1)
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
				case "Buff": Buff = (XmlEditor.CustomTypes.BuffType)ReadInt(_2); break;
			}
		}
	}
}
