using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class ShakeScreen : Lson.Skill.Action
	{
		/// <summary>
		/// 震屏方式
		/// <summary>
		public string Type;
		/// <summary>
		/// 每秒震动的次数
		/// <summary>
		public int Frequency;
		/// <summary>
		/// 初始频率维持时间
		/// <summary>
		public float FrequencyDuration;
		/// <summary>
		/// 频率衰减
		/// <summary>
		public float FrequencyAtten;
		/// <summary>
		/// 单次振幅
		/// <summary>
		public float Amplitude;
		/// <summary>
		/// 单次震动的衰减幅度
		/// <summary>
		public float AmplitudeAtten;
		/// <summary>
		/// 本次震动持续时间，-1表持续到下一次震动触发或技能结束为止
		/// <summary>
		public float Life;
		/// <summary>
		/// 最小完整影响范围
		/// <summary>
		public float MinRange;
		/// <summary>
		/// 最大影响范围
		/// <summary>
		public float MaxRange;
	}
}
