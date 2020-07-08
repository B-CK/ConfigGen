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
	public class CastObject : Cfg.Skill.Controller
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly bool IsTraceTarget;
		/// <summary>
		/// 
		/// <summary>
		public readonly int CurveId;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool PassBody;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Common.Vector3 Position;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Common.Vector3 EulerAngles;
		public CastObject(DataStream data) : base(data)
		{
			IsTraceTarget = data.GetBool();
			CurveId = data.GetInt();
			PassBody = data.GetBool();
			Position = new Cfg.Common.Vector3(data);
			EulerAngles = new Cfg.Common.Vector3(data);
		}
	}
}
