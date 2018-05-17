using System;
using System.Text;
using System.Collections.Generic;

namespace Csv
{
	public class CfgManager
	{
		/// <summary>
		/// 配置文件文件夹路径
		/// <summary>
		public static string ConfigDir;

		public static readonly Dictionary<int, AllType.AllClass> AllClass = new Dictionary<int, AllType.AllClass>();
		public static readonly Dictionary<int, Card.Card> Card = new Dictionary<int, Card.Card>();

		public static List<T> Load<T>(string path, Encoding encoding)
		{
			DataStream data = new DataStream(path, encoding);
			List<T> list = new List<T>();
			for (int i = 0; i < data.Count; i++)
			{
				list.Add(new T(data));
			}
			return list;
		}

		public static void LoadAll()
		{
			var allclasss = Load<AllType.AllClass>(ConfigDir + "AllType/AllClass.xml", Encoding.UTF8);
			allclasss.ForEach(v => AllClass.Add(v.ID, v));
			var cards = Load<Card.Card>(ConfigDir + "Card/Card.xml", Encoding.UTF8);
			cards.ForEach(v => Card.Add(v.ID, v));
		}

		public static void Clear()
		{
			AllClass.Clear();
			Card.Clear();
		}

	}
}
