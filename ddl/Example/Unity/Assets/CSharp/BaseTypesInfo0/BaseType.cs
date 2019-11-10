using Example;
using System;
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
	}
}
