using Example;
using System;
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
		public readonly List<bool> list_bool;
		/// <summary>
		/// 整形列表
		/// <summary>
		public readonly List<int> list_int;
		/// <summary>
		/// 长整形列表
		/// <summary>
		public readonly List<long> list_long;
		/// <summary>
		/// 浮点型列表
		/// <summary>
		public readonly List<float> list_float;
		/// <summary>
		/// 字符串列表
		/// <summary>
		public readonly List<string> list_string;
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Example.CustomTypes.BuffType> list_enum;
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
	}
}
