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

		public static readonly Dictionary<int, Csv.AllType.AllClass> AllClass = new Dictionary<int, Csv.AllType.AllClass>();
		public static readonly Dictionary<int, Csv.Card.Card> Card = new Dictionary<int, Csv.Card.Card>();

		/// <summary>
		/// constructor参数为指定类型的构造函数
		/// <summary>
		public static List<T> Load<T>(string path, Func<DataStream, T> constructor)
		{
			DataStream data = new DataStream(path, Encoding.UTF8);
			List<T> list = new List<T>();
			for (int i = 0; i < data.Count; i++)
			{
				list.Add(constructor(data));
			}
			return list;
		}

		public static void LoadAll()
		{
			var allclasss = Load(ConfigDir + "AllType/AllClass.xml", (d) => new AllType.AllClass(d));
			allclasss.ForEach(v => AllClass.Add(v.ID, v));
			var cards = Load(ConfigDir + "Card/Card.xml", (d) => new Card.Card(d));
			cards.ForEach(v => Card.Add(v.ID, v));
		}

		public static void Clear()
		{
			AllClass.Clear();
			Card.Clear();
		}

	}
}
