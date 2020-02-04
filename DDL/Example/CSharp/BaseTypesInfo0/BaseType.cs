using Example;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Example.BaseTypesInfo0
{
	/// <summary>
	/// 基础类型
	/// <summary>
	public class BaseType : CfgObject
	{
		/// <summary>
		/// int类型
		/// <summary>
		public readonly int int_var;
		/// <summary>
		/// long类型
		/// <summary>
		public readonly long long_var;
		/// <summary>
		/// float类型
		/// <summary>
		public readonly float float_var;
		/// <summary>
		/// bool类型
		/// <summary>
		public readonly bool bool_var;
		/// <summary>
		/// string类型
		/// <summary>
		public readonly string string_var;
		public BaseType(DataStream data)
		{
			int_var = data.GetInt();
			long_var = data.GetLong();
			float_var = data.GetFloat();
			bool_var = data.GetBool();
			string_var = data.GetString();
		}
		public static Dictionary<int, Example.BaseTypesInfo0.BaseType> Load()
		{
			var dict = new Dictionary<int, Example.BaseTypesInfo0.BaseType>();
			var path = "BaseTypesInfo0/BaseType.data";
			try
			{
				var data = new DataStream(path, Encoding.UTF8);
				int length = data.GetInt();
				for (int i = 0; i < length; i++)
				{
					var v = new Example.BaseTypesInfo0.BaseType(data);
					dict.Add(v.int_var, v);
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
