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
	public class ReplaceObject : Cfg.Skill.Controller
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string NewObject;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Common.Vector3 Offset;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Common.Vector3 EulerAngles;
		public ReplaceObject(DataStream data) : base(data)
		{
			NewObject = data.GetString();
			Offset = new Cfg.Common.Vector3(data);
			EulerAngles = new Cfg.Common.Vector3(data);
		}
	}
}
