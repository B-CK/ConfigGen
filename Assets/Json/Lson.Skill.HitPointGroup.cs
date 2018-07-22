using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class HitPointGroup : LsonObject
	{
		/// <summary>
		/// 打击点组ID
		/// <summary>
		public int Id;
		/// <summary>
		/// 打击点组名称
		/// <summary>
		public string Name;
		/// <summary>
		/// 打击点列表
		/// <summary>
		public List<Lson.Skill.Attack> Attacks;
	}
}
