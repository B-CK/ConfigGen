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
	public class PlayParticle : Cfg.Skill.Timeline
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Path;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool IsRelateSelf;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool FollowDir;
		/// <summary>
		/// 
		/// <summary>
		public readonly string NodeName;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Common.Vector3 Position;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Common.Vector3 EulerAngles;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Common.Vector3 Scale;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Skill.EffectAlignType AlignType;
		public PlayParticle(DataStream data) : base(data)
		{
			Path = data.GetString();
			IsRelateSelf = data.GetBool();
			FollowDir = data.GetBool();
			NodeName = data.GetString();
			Position = new Cfg.Common.Vector3(data);
			EulerAngles = new Cfg.Common.Vector3(data);
			Scale = new Cfg.Common.Vector3(data);
			AlignType = (Cfg.Skill.EffectAlignType)data.GetInt();
		}
	}
}
