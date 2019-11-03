using System;
using System.Linq;
using System.IO;
using XmlExample;
using System.Xml;
using System.Collections.Generic;

namespace XmlBaseTypesInfo0
{
	/// <summary>
	/// 基础类型
	/// <summary>
	public class BaseType : XmlObject
	{
		/// <summary>
		/// int类型
		/// <summary>
		public int int_var;
		/// <summary>
		/// long类型
		/// <summary>
		public long long_var;
		/// <summary>
		/// float类型
		/// <summary>
		public float float_var;
		/// <summary>
		/// bool类型
		/// <summary>
		public bool bool_var;
		/// <summary>
		/// string类型
		/// <summary>
		public string string_var;

		public override void Write(TextWriter _1)
		{
			Write(_1, "int_var", this.int_var);
			Write(_1, "long_var", this.long_var);
			Write(_1, "float_var", this.float_var);
			Write(_1, "bool_var", this.bool_var);
			Write(_1, "string_var", this.string_var);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "int_var": int_var = ReadInt(_2); break;
				case "long_var": long_var = ReadLong(_2); break;
				case "float_var": float_var = ReadFloat(_2); break;
				case "bool_var": bool_var = ReadBool(_2); break;
				case "string_var": string_var = ReadString(_2); break;
			}
		}
	}
}
