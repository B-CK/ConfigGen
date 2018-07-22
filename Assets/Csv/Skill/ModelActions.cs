using System;
using System.Collections.Generic;
using Csv;

namespace Csv.Skill
{
	public class ModelActions : CfgObject
	{
		/// <summary>
		/// 模型名称
		/// <summary>
		public readonly string ModelName;
		/// <summary>
		/// 基础模型名称
		/// <summary>
		public readonly string BaseModelName;
		/// <summary>
		/// 普通动作
		/// <summary>
		public readonly List<Csv.Skill.ModelAction> ModelActions = new List<Csv.Skill.ModelAction>();
		/// <summary>
		/// 技能动作
		/// <summary>
		public readonly List<Csv.Skill.SkillAction> SkillActions = new List<Csv.Skill.SkillAction>();

		public ModelActions(DataStream data)
		{
			this.ModelName = data.GetString();
			this.BaseModelName = data.GetString();
			for (int n = data.GetInt(); n-- > 0; )
			{
				this.ModelActions.Add((Skill.ModelAction)data.GetObject(data.GetString()));
			}
			for (int n = data.GetInt(); n-- > 0; )
			{
				this.SkillActions.Add(new Skill.SkillAction(data));
			}
		}
	}
}
