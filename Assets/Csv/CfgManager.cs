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
		public static readonly Dictionary<string, Csv.Skill.ModelActions> ModelActions = new Dictionary<string, Csv.Skill.ModelActions>();
		public static readonly Dictionary<int, Csv.LsonAllClass.LsonAllClass> LsonAllClass = new Dictionary<int, Csv.LsonAllClass.LsonAllClass>();

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
			var allclasss = Load(ConfigDir + "AllType/AllClass.data", (d) => new AllType.AllClass(d));
			allclasss.ForEach(v => AllClass.Add(v.ID, v));
			var cards = Load(ConfigDir + "Card/Card.data", (d) => new Card.Card(d));
			cards.ForEach(v => Card.Add(v.ID, v));
			var modelactionss = Load(ConfigDir + "Skill/ModelActions.data", (d) => new Skill.ModelActions(d));
			modelactionss.ForEach(v => ModelActions.Add(v.ModelName, v));
			var lsonallclasss = Load(ConfigDir + "LsonAllClass/LsonAllClass.data", (d) => new LsonAllClass.LsonAllClass(d));
			lsonallclasss.ForEach(v => LsonAllClass.Add(v.ID, v));
		}

		public static void Clear()
		{
			AllClass.Clear();
			Card.Clear();
			ModelActions.Clear();
			LsonAllClass.Clear();
		}

	}
}
