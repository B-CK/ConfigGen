using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class DelayedAction : AbilitySystem.Ability.Action
	{
		/// <summary>
		/// 延迟时间
		/// <summary>
		
		public float delay;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "delay", delay);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "delay": delay = ReadFloat(_2); break;
			}
		}
	}
}
