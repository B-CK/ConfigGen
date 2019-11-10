using Example;
using System;
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
		public readonly Dictionary<int, bool> dict_int;
		/// <summary>
		/// 长整形字典
		/// <summary>
		public readonly Dictionary<long, bool> dict_long;
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<string, bool> dict_string;
		/// <summary>
		/// 枚举字典
		/// <summary>
		public readonly Dictionary<CustomTypes.BuffType, bool> dict_enum;
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
	}
}
