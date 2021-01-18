using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace AbilitySystem.Ability
{
	/// <summary>
	/// 
	/// <summary>
	
	public partial class TrackingProjectile : AbilitySystem.Ability.ActionWithTarget
	{
		/// <summary>
		/// 特效名称
		/// <summary>
		
		public string effectName;
		/// <summary>
		/// 移动速度
		/// <summary>
		
		public float MoveSpeed;
		/// <summary>
		/// 绑定点
		/// <summary>
		
		public string TrackNode;
		public override void Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "effectName", effectName);
			Write(_1, "MoveSpeed", MoveSpeed);
			Write(_1, "TrackNode", TrackNode);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "effectName": effectName = ReadString(_2); break;
				case "MoveSpeed": MoveSpeed = ReadFloat(_2); break;
				case "TrackNode": TrackNode = ReadString(_2); break;
			}
		}
	}
}
