using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class Damage : AbilitySystem.Ability.ActionWithTarget
	{
		/// <summary>
		/// 伤害值
		/// <summary>
		
		public float Type;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "Type", Type);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Type": Type = ReadFloat(_2); break;
			}
		}
	}
}
