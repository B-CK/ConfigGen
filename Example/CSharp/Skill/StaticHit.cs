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
	public class StaticHit : Cfg.Skill.Timeline
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Skill.HitZone Zone;
		/// <summary>
		/// 
		/// <summary>
		public readonly int SequeueID;
		public StaticHit(DataStream data) : base(data)
		{
			Zone = (Cfg.Skill.HitZone)data.GetObject(data.GetString());
			SequeueID = data.GetInt();
		}
	}
}
