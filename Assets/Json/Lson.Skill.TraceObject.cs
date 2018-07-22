using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class TraceObject : Lson.Skill.SpawnObject
	{
		/// <summary>
		/// 身体矫正值
		/// <summary>
		public const float BODY_CORRECT = 0.7f;
		/// <summary>
		/// 头部矫正值
		/// <summary>
		public const float HEAD_CORRECT = 1.3f;
		/// <summary>
		/// 特效ID
		/// <summary>
		public int EffectId;
		/// <summary>
		/// 是否追踪目标
		/// <summary>
		public bool IsTraceTarget;
		/// <summary>
		/// 飞行参数ID,数据有配置表
		/// <summary>
		public int TraceCurveId;
		/// <summary>
		/// 目标偏移X
		/// <summary>
		public float OffsetX;
		/// <summary>
		/// 目标偏移Y
		/// <summary>
		public float OffsetY;
		/// <summary>
		/// 目标偏移Z
		/// <summary>
		public float OffsetZ;
		/// <summary>
		/// 追踪类型
		/// <summary>
		public Lson.Skill.TraceType TraceType;
		/// <summary>
		/// 释放者绑定位置
		/// <summary>
		public Lson.Skill.TraceBindType CasterBindType;
		/// <summary>
		/// 被击者绑定位置
		/// <summary>
		public Lson.Skill.TraceBindType TargetBindType;
	}
}
