using System;
using System.Collections.Generic;

namespace Lson.Skill
{
	public class BeAttackEffect : LsonObject
	{
		/// <summary>
		/// 被击效果id
		/// <summary>
		public int Id;
		/// <summary>
		/// 被打击者的抛物曲线
		/// <summary>
		public int Curve;
		/// <summary>
		/// 被打击者的受击动作，null为默认
		/// <summary>
		public string DefencerAction;
		/// <summary>
		/// 被打击者身上出现的被击特效，Null为默认
		/// <summary>
		public int DefencerEffectId;
	}
}
