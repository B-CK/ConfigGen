using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lson
{
[Serializable]
	public class LsonManager
	{
		public static List<Lson.AllType.AllClass> AllClass = new List<AllType.AllClass>();
		public static List<Lson.Card.Card> Card = new List<Card.Card>();
		public static List<Lson.Skill.ModelActions> ModelActions = new List<Skill.ModelActions>();
		public static List<Lson.LsonAllClass.LsonAllClass> LsonAllClass = new List<LsonAllClass.LsonAllClass>();

		public static T Deserialize<T>(string path)
		{
			string value = File.ReadAllText(path);
			T data = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto
			});
			return data;
		}
		public static void Serialize(string path, object data)
		{
			string value = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto
			});
			File.WriteAllText(path, value);
		}
		public static List<T> Load<T>(string dirPath)
		{
			List<T> list = new List<T>();
			try
			{
				string[] fs = Directory.GetFiles(dirPath);
				foreach (var f in fs)

				{
					list.Add(Deserialize<T>(f));
				}
			}
			catch (System.Exception e)
			{
				UnityEngine.Debug.LogErrorFormat("文件夹路径不存在{0}\n{1}", dirPath, e.StackTrace);
			}
			return list;
		}
		public static void LoadAll()
		{
			AllClass = Load<Lson.AllType.AllClass>("F:/__GitHub/ConfigGen/ConfigGen/bin/Debug/../../../Csv/Data所有Class类型.xlsx");
			Card = Load<Lson.Card.Card>("F:/__GitHub/ConfigGen/ConfigGen/bin/Debug/../../../Csv/卡牌_Card.xlsx");
			ModelActions = Load<Lson.Skill.ModelActions>("F:/__GitHub/ConfigGen/ConfigGen/bin/Debug/../../../Csv/ActionConfig");
			LsonAllClass = Load<Lson.LsonAllClass.LsonAllClass>("F:/__GitHub/ConfigGen/ConfigGen/bin/Debug/../../../Csv/Lson数据");
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
