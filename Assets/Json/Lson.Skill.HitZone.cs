using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class HitZone : LsonObject
	{
		/// <summary>
		/// 打击区域id
		/// <summary>
		public int Id;
		/// <summary>
		/// 打击范围的形态，0：方盒，1:圆柱,2:三棱柱
		/// <summary>
		public Lson.Skill.HitSharpType Sharp;
		/// <summary>
		/// 打击范围的中心点在Z轴上的偏移量，向前为正
		/// <summary>
		public float Zoffset;
		/// <summary>
		/// X方向长度
		/// <summary>
		public float Xlength;
		/// <summary>
		/// 底边距地面的高度
		/// <summary>
		public float BottomHeight;
		/// <summary>
		/// 顶部距地面的高度
		/// <summary>
		public float TopHeight;
		/// <summary>
		/// Z方向长度
		/// <summary>
		public float Zlength;
		/// <summary>
		/// 以y轴为中心的切面角度
		/// <summary>
		public float YAngle;
		/// <summary>
		/// 打击区域绕y轴旋转角度（顺时针）,构成扇形
		/// <summary>
		public float YRotation;
		/// <summary>
		/// 最大数量
		/// <summary>
		public int MaxTarget;
	}
}
