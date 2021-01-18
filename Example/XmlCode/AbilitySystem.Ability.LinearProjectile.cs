using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class LinearProjectile : AbilitySystem.Ability.Action
	{
		/// <summary>
		/// 特效名称
		/// <summary>
		
		public string effectName;
		/// <summary>
		/// 移动速度
		/// <summary>
		
		public float moveSpeed;
		/// <summary>
		/// 可移动距离:可转化成时间去做定时计算
		/// <summary>
		
		public float distance;
		/// <summary>
		/// 结束目标:抵达该类型目标则结束
		/// <summary>
		
		public AbilitySystem.Ability.TargetFilter endTarget;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "effectName", effectName);
			Write(_1, "moveSpeed", moveSpeed);
			Write(_1, "distance", distance);
			Write(_1, "endTarget", endTarget);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "effectName": effectName = ReadString(_2); break;
				case "moveSpeed": moveSpeed = ReadFloat(_2); break;
				case "distance": distance = ReadFloat(_2); break;
				case "endTarget": endTarget = ReadDynamicObject<AbilitySystem.Ability.TargetFilter>(_2, "AbilitySystem.Ability"); break;
			}
		}
	}
}
