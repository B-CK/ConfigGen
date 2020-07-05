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
	public class ListSet : CfgObject
	{
		/// <summary>
		/// id
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// 布尔列表
		/// <summary>
		public readonly List<bool> list_bool = new List<bool>();
		/// <summary>
		/// 整形列表
		/// <summary>
		public readonly List<int> list_int = new List<int>();
		/// <summary>
		/// 长整形列表
		/// <summary>
		public readonly List<long> list_long = new List<long>();
		/// <summary>
		/// 浮点型列表
		/// <summary>
		public readonly List<float> list_float = new List<float>();
		/// <summary>
		/// 字符串列表
		/// <summary>
		public readonly List<string> list_string = new List<string>();
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Example.CustomTypes.BuffType> list_enum = new List<Example.CustomTypes.BuffType>();
		public ListSet(DataStream data)
		{
			ID = data.GetInt();
			for (int n = data.GetInt(); n-- > 0;)
			{
				var v = data.GetBool();
				list_bool.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				var v = data.GetInt();
				list_int.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				var v = data.GetLong();
				list_long.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				var v = data.GetFloat();
				list_float.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				var v = data.GetString();
				list_string.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				var v = (Example.CustomTypes.BuffType)data.GetInt();
				list_enum.Add(v);
			}
		}
		public static Dictionary<int, Example.SetTypes.ListSet> Load()
		{
			var dict = new Dictionary<int, Example.SetTypes.ListSet>();
			var path = "SetTypes/ListSet.data";
			try
			{
				var data = new DataStream(path, Encoding.UTF8);
				int length = data.GetInt();
				for (int i = 0; i < length; i++)
				{
					var v = new Example.SetTypes.ListSet(data);
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
