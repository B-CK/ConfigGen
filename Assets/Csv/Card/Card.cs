using System;
using System.Collections.Generic;
using Csv;

namespace Csv.Card
{
	public class Card : CfgObject
	{
		/// <summary>
		/// ID编号
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// 名字
		/// <summary>
		public readonly string Name;
		/// <summary>
		/// 卡牌类型
		/// <summary>
		public readonly int CardType;
		/// <summary>
		/// 费用
		/// <summary>
		public readonly long Cost;
		/// <summary>
		/// 元素数据
		/// <summary>
		public readonly Dictionary<int, long> Elements = new Dictionary<int, long>();

		public Card(DataStream data)
		{
			this.ID = data.GetInt();
			this.Name = data.GetString();
			this.CardType = data.GetInt();
			this.Cost = data.GetLong();
			for (int n = data.GetInt(); n-- > 0;)
			{
				int k = data.GetInt();
				this.Elements[k] = data.GetLong();
			}
		}
	}
}
