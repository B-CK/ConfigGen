using System;
using System.Collections.Generic;
using Example;

namespace SetTypes
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
		public readonly List<CustomTypes.BuffType> list_enum = new List<CustomTypes.BuffType>();
		
		public ListSet(DataStream data)
		{
			ID = data.GetInt();
			for (int n = data.GetInt(); n-- > 0;)
			{
				bool v = data.GetBool();
				list_bool.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				int v = data.GetInt();
				list_int.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				long v = data.GetLong();
				list_long.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				float v = data.GetFloat();
				list_float.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				string v = data.GetString();
				list_string.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				CustomTypes.BuffType v = (CustomTypes.BuffType)data.GetInt();
				list_enum.Add(v);
			}
		}
	}
}
