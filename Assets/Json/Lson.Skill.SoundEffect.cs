using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class SoundEffect : Lson.Skill.Action
	{
		/// <summary>
		/// 触发概率
		/// <summary>
		public float Probability;
		/// <summary>
		/// 最小音量
		/// <summary>
		public float VolumeMin;
		/// <summary>
		/// 最大音量
		/// <summary>
		public float VolumeMax;
		/// <summary>
		/// 音效资源路径列表
		/// <summary>
		public List<string> PathList;
	}
}
