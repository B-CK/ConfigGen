using Example;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Example.SetTypes
{
	/// <summary>
	/// 
	/// <summary>
	public class DictSet : CfgObject
	{
		/// <summary>
		/// id
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// 整形字典
		/// <summary>
		public readonly Dictionary<int, bool> dict_int = new Dictionary<int, bool>();
		/// <summary>
		/// 长整形字典
		/// <summary>
		public readonly Dictionary<long, bool> dict_long = new Dictionary<long, bool>();
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<string, bool> dict_string = new Dictionary<string, bool>();
		/// <summary>
		/// 枚举字典
		/// <summary>
		public readonly Dictionary<CustomTypes.BuffType, bool> dict_enum = new Dictionary<CustomTypes.BuffType, bool>();
		public DictSet(DataStream data)
		{
			ID = data.GetInt();
			for (int n = data.GetInt(); n-- > 0;)
			{
				var k = data.GetInt();
				dict_int[k] = data.GetBool();
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				var k = data.GetLong();
				dict_long[k] = data.GetBool();
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				var k = data.GetString();
				dict_string[k] = data.GetBool();
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				var k = (Example.CustomTypes.BuffType)data.GetInt();
				dict_enum[k] = data.GetBool();
			}
		}
		public static Dictionary<int, Example.SetTypes.DictSet> Load()
		{
			var dict = new Dictionary<int, Example.SetTypes.DictSet>();
			var path = "SetTypes/DictSet.data";
			try
			{
				var data = new DataStream(path, Encoding.UTF8);
				int length = data.GetInt();
				for (int i = 0; i < length; i++)
				{
					var v = new Example.SetTypes.DictSet(data);
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
