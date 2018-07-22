using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class ParticleEffect : Lson.Skill.Action
	{
		/// <summary>
		/// 粒子特效id
		/// <summary>
		public int Id;
		/// <summary>
		/// 特效类型
		/// <summary>
		public int Type;
		/// <summary>
		/// 淡出时间
		/// <summary>
		public float FadeOutTime;
		/// <summary>
		/// 粒子资源的路径
		/// <summary>
		public string Path;
		/// <summary>
		/// 粒子的存续时间，-1则与参与动画播放时间等长
		/// <summary>
		public float Life;
		/// <summary>
		/// 是否跟随释放者方向
		/// <summary>
		public bool FollowDirection;
		/// <summary>
		/// 敌人的被击特效是否始终跟随敌我位置变化而转向
		/// <summary>
		public bool FollowBeAttackedDirection;
		/// <summary>
		/// 缩放大小
		/// <summary>
		public float Scale;
		/// <summary>
		/// 释放者施放技能的位置编号
		/// <summary>
		public int CasterBindType;
		/// <summary>
		/// 是否跟随绑定骨骼方向
		/// <summary>
		public bool FollowBoneDirection;
		/// <summary>
		/// 被击者出现被击特效的位置
		/// <summary>
		public int TargetBindType;
		/// <summary>
		/// 跟踪类型(0:Line)
		/// <summary>
		public int InstanceTraceType;
		/// <summary>
		/// 特效世界偏移X
		/// <summary>
		public float WorldOffsetX;
		/// <summary>
		/// 特效世界偏移Y
		/// <summary>
		public float WorldOffsetY;
		/// <summary>
		/// 特效世界偏移Z
		/// <summary>
		public float WorldOffsetZ;
		/// <summary>
		/// 特效世界旋转X
		/// <summary>
		public float WorldRotateX;
		/// <summary>
		/// 特效世界旋转Y
		/// <summary>
		public float WorldRotateY;
		/// <summary>
		/// 特效世界旋转Z
		/// <summary>
		public float WorldRotateZ;
		/// <summary>
		/// 特效骨骼偏移X
		/// <summary>
		public float BonePostionX;
		/// <summary>
		/// 特效骨骼偏移Y
		/// <summary>
		public float BonePostionY;
		/// <summary>
		/// 特效骨骼偏移Z
		/// <summary>
		public float BonePostionZ;
		/// <summary>
		/// 特效骨骼旋转X
		/// <summary>
		public float BoneRotationX;
		/// <summary>
		/// 特效骨骼旋转Y
		/// <summary>
		public float BoneRotationY;
		/// <summary>
		/// 特效骨骼旋转Z
		/// <summary>
		public float BoneRotationZ;
		/// <summary>
		/// 特效骨骼缩放X
		/// <summary>
		public float BoneScaleX;
		/// <summary>
		/// 特效骨骼缩放Y
		/// <summary>
		public float BoneScaleY;
		/// <summary>
		/// 特效骨骼缩放Z
		/// <summary>
		public float BoneScaleZ;
		/// <summary>
		/// 骨骼名称
		/// <summary>
		public string BoneName;
		/// <summary>
		/// 飞行时间
		/// <summary>
		public float TraceTime;
		/// <summary>
		/// 屏幕对齐类型
		/// <summary>
		public Lson.Skill.EffectAlignType AlignType;
		/// <summary>
		/// 是否特效池管理
		/// <summary>
		public bool IsPoolDestroy;
	}
}
