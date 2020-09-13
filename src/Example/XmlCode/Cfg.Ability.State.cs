using System;
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
		/// 状态类型
		/// <summary>
		public Cfg.Ability.StateType type;
		/// <summary>
		/// 状态值:三种值
		/// <summary>
		public Cfg.Ability.StateValue value;
		public override void Write(TextWriter _1)
		{
			Write(_1, "type", type);
			Write(_1, "value", value);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "type": type = (Cfg.Ability.StateType)ReadInt(_2); break;
				case "value": value = (Cfg.Ability.StateValue)ReadInt(_2); break;
			}
		}
	}
}
