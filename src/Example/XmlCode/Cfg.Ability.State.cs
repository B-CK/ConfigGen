using System;
using XmlEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Ability
{
	/// <summary>
	/// 
	/// <summary>
	public partial class State : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.StateType state;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Ability.StateType value;
		public override void Write(TextWriter _1)
		{
			Write(_1, "state", state);
			Write(_1, "value", value);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "state": state = (Cfg.Ability.StateType)ReadString(_2); break;
				case "value": value = (Cfg.Ability.StateType)ReadString(_2); break;
			}
		}
	}
}
