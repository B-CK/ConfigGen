using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class ModelActions : LsonObject
	{
		/// <summary>
		/// 模型名称
		/// <summary>
		public string ModelName;
		/// <summary>
		/// 基础模型名称
		/// <summary>
		public string BaseModelName;
		/// <summary>
		/// 普通动作
		/// <summary>
		public List<Lson.Skill.ModelAction> ModelActions;
		/// <summary>
		/// 技能动作
		/// <summary>
		public List<Lson.Skill.SkillAction> SkillActions;
	}
}
