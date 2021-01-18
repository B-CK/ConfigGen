using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class BuffModifier : AbilitySystem.Ability.Modifier
	{
		/// <summary>
		/// 是否为增益魔法
		/// <summary>
		
		public bool isBuff;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "isBuff", isBuff);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "isBuff": isBuff = ReadBool(_2); break;
			}
		}
	}
}
