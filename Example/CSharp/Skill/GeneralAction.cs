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
	public class GeneralAction : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string ActionName;
		/// <summary>
		/// 
		/// <summary>
		public readonly string OtherModelName;
		/// <summary>
		/// 
		/// <summary>
		public readonly string ActionClip;
		/// <summary>
		/// 
		/// <summary>
		public readonly string PreActionFile;
		/// <summary>
		/// 
		/// <summary>
		public readonly string PostActionFile;
		/// <summary>
		/// 
		/// <summary>
		public readonly float ActionSpeed;
		/// <summary>
		/// 
		/// <summary>
		public readonly int LoopTimes;
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Skill.Timeline> Timelines = new List<Cfg.Skill.Timeline>();
		public GeneralAction(DataStream data)
		{
			ActionName = data.GetString();
			OtherModelName = data.GetString();
			ActionClip = data.GetString();
			PreActionFile = data.GetString();
			PostActionFile = data.GetString();
			ActionSpeed = data.GetFloat();
			LoopTimes = data.GetInt();
			for (int n = data.GetArrayLength(); n-- > 0;)
			{
				var v = (Cfg.Skill.Timeline)data.GetObject(data.GetString());
				Timelines.Add(v);
			}
		}
	}
}
