using Example;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Example.CustomTypes
{
	/// <summary>
	/// 
	/// <summary>
	public class Character : CfgObject
	{
		/// <summary>
		/// id
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// 角色信息
		/// <summary>
		public readonly Example.CustomTypes.Custom Custom;
		public Character(DataStream data)
		{
			ID = data.GetInt();
			Custom = (Example.CustomTypes.Custom)data.GetObject(data.GetString());
		}
		public static Dictionary<int, Example.CustomTypes.Character> Load()
		{
			var dict = new Dictionary<int, Example.CustomTypes.Character>();
			var path = "CustomTypes/Character.data";
			try
			{
				var data = new DataStream(path, Encoding.UTF8);
				int length = data.GetInt();
				for (int i = 0; i < length; i++)
				{
					var v = new Example.CustomTypes.Character(data);
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
