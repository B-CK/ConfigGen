using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class AttachEffect : AbilitySystem.Ability.ActionWithTarget
	{
		/// <summary>
		/// 特效名称
		/// <summary>
		
		public string effectName;
		/// <summary>
		/// 绑定坐标系类型
		/// <summary>
		
		public AbilitySystem.Ability.AttachType attachType;
		/// <summary>
		/// 偏移量
		/// <summary>
		
		public AbilitySystem.Math.Vector3 point;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "effectName", effectName);
			Write(_1, "attachType", attachType);
			Write(_1, "point", point);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "effectName": effectName = ReadString(_2); break;
				case "attachType": attachType = (AbilitySystem.Ability.AttachType)ReadInt(_2); break;
				case "point": point = ReadObject<AbilitySystem.Math.Vector3>(_2, "AbilitySystem.Math.Vector3"); break;
			}
		}
	}
}
