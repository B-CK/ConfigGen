using System;
using System.Collections.Generic;
using Csv;

namespace Csv.Card
{
	public sealed class Card : CfgObject
	{
		public readonly int ID;
		public readonly string Name;
		public readonly int CardType;
		public readonly long Cost;
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
				this.Elements[k] = new data.GetLong();
			}
		}
	}
}
