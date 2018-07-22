using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class EffectGroup : LsonObject
	{
		/// <summary>
		/// 特效组ID
		/// <summary>
		public int Id;
		/// <summary>
		/// 特效组名称
		/// <summary>
		public string Name;
		/// <summary>
		/// 特效组行为列表
		/// <summary>
		public List<Lson.Skill.Action> Actions;
	}
}
