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
	public class HitZone : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly int Id;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Skill.HitSharpType Sharp;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Common.Vector3 Offset;
		/// <summary>
		/// 
		/// <summary>
		public readonly int MaxNum;
		public HitZone(DataStream data)
		{
			Id = data.GetInt();
			Sharp = (Cfg.Skill.HitSharpType)data.GetInt();
			Offset = new Cfg.Common.Vector3(data);
			MaxNum = data.GetInt();
		}
	}
}
