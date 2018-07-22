using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class ModelAction : LsonObject
	{
		/// <summary>
		/// 行为名称
		/// <summary>
		public string ActionName;
		/// <summary>
		/// 动作来源
		/// <summary>
		public Lson.Skill.ActionSourceType ActionSource;
		/// <summary>
		/// 其他模型名称,用于套用其他模型动作
		/// <summary>
		public string OtherModelName;
		/// <summary>
		/// 绑定的动作名称
		/// <summary>
		public string ActionFile;
		/// <summary>
		/// 前摇动作名称
		/// <summary>
		public string PreActionFile;
		/// <summary>
		/// 后摇动作名称
		/// <summary>
		public string PostActionFile;
		/// <summary>
		/// 动作播放速率
		/// <summary>
		public float ActionSpeed;
		/// <summary>
		/// 动作循环次数
		/// <summary>
		public int LoopTimes;
		/// <summary>
		/// 特效ID
		/// <summary>
		public int EffectId;
		/// <summary>
		/// 时间事件列表
		/// <summary>
		public List<Lson.Skill.Action> Actions;
		/// <summary>
		/// 特效组列表
		/// <summary>
		public List<Lson.Skill.EffectGroup> Effects;
	}
}
