using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class Move : Cfg.Skill.Controller
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Skill.MoveType Type;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool IsRelateSelf;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Common.Vector3 Offset;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Angle;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Speed;
		public Move(DataStream data) : base(data)
		{
			Type = (Cfg.Skill.MoveType)data.GetInt();
			IsRelateSelf = data.GetBool();
			Offset = new Cfg.Common.Vector3(data);
			Angle = data.GetFloat();
			Speed = data.GetFloat();
		}
	}
}
