using Example;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Example.XmlPaser
{
	/// <summary>
	/// 
	/// <summary>
	public class Character : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// 
		/// <summary>
		public readonly Example.XmlPaser.Custom Custom;
		public Character(DataStream data)
		{
			ID = data.GetInt();
			Custom = (Example.XmlPaser.Custom)data.GetObject(data.GetString());
		}
		public static Dictionary<int, Example.XmlPaser.Character> Load()
		{
			var dict = new Dictionary<int, Example.XmlPaser.Character>();
			var path = "XmlPaser/Character.data";
			try
			{
				var data = new DataStream(path, Encoding.UTF8);
				int length = data.GetInt();
				for (int i = 0; i < length; i++)
				{
					var v = new Example.XmlPaser.Character(data);
					dict.Add(v.ID, v);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError($"{path}解析异常~\n{e.Message}");
#if UNITY_EDITOR
				UnityEngine.Debug.LogError($"最后一条数据Key:{dict.Last().Key}.");
#endif
			}
			return dict;
		}
	}
}
