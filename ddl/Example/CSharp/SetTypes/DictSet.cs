using System;
using System.Collections.Generic;
using Example;

namespace SetTypes
{
	/// <summary>
	/// 
	/// <summary>
	public class DictSet : CfgObject
	{
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
		
		public DictSet(DataStream data)
		{
			for (int n = data.GetInt(); n-- > 0;)
			{
				int k = data.GetInt();
				dict_int[k] = data.GetBool();
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				long k = data.GetLong();
				dict_long[k] = data.GetBool();
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				string k = data.GetString();
				dict_string[k] = data.GetBool();
			}
		}
	}
}
