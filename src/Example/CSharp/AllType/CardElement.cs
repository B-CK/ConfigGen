using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.AllType
{
	/// <summary>
	/// 卡牌枚举
	/// <summary>
	[System.Flags]
	public enum CardElement
	{
		/// <summary>
		/// 攻击
		/// <summary>
		Attack = 1,
		/// <summary>
		/// 抽牌
		/// <summary>
		Extract = 2,
		/// <summary>
		/// 弃牌
		/// <summary>
		Renounce = 3,
		/// <summary>
		/// 护甲
		/// <summary>
		Armor = 4,
		/// <summary>
		/// 控制
		/// <summary>
		Control = 5,
		/// <summary>
		/// 治疗
		/// <summary>
		Cure = 6,
		/// <summary>
		/// 自残
		/// <summary>
		Oneself = 7,
		/// <summary>
		/// 手牌
		/// <summary>
		Hand = 8,
		/// <summary>
		/// 牌库
		/// <summary>
		Brary = 9,
		/// <summary>
		/// 手牌攻击
		/// <summary>
		Handack = 10,
	}
}
