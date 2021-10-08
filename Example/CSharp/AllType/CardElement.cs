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
	public enum CardElement
	{
		/// <summary>
		/// 攻击
		/// <summary>
		Attack = 0,
		/// <summary>
		/// 抽牌
		/// <summary>
		Extract = 1,
		/// <summary>
		/// 弃牌
		/// <summary>
		Renounce = 2,
		/// <summary>
		/// 护甲
		/// <summary>
		Armor = 3,
		/// <summary>
		/// 控制
		/// <summary>
		Control = 4,
		/// <summary>
		/// 治疗
		/// <summary>
		Cure = 5,
		/// <summary>
		/// 自残
		/// <summary>
		Oneself = 6,
		/// <summary>
		/// 手牌
		/// <summary>
		Hand = 7,
		/// <summary>
		/// 牌库
		/// <summary>
		Brary = 8,
		/// <summary>
		/// 手牌攻击
		/// <summary>
		Handack = 9,
	}
}
