using System;
using System.Collections.Generic;

namespace Lson.Card
{
	public class Card : LsonObject
	{
		/// <summary>
		/// ID编号
		/// <summary>
		public int ID;
		/// <summary>
		/// 名字
		/// <summary>
		public string Name;
		/// <summary>
		/// 卡牌类型
		/// <summary>
		public Lson.Card.CardType CardType;
		/// <summary>
		/// 费用
		/// <summary>
		public long Cost;
		/// <summary>
		/// 元素数据
		/// <summary>
		public Dictionary<Lson.Card.CardElement, long> Elements;
	}
}
