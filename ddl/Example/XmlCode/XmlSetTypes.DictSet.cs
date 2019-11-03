using System;
using System.Linq;
using System.IO;
using XmlExample;
using System.Xml;
using System.Collections.Generic;

namespace XmlSetTypes
{
	/// <summary>
	/// 
	/// <summary>
	public class DictSet : XmlObject
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

		public override void Write(TextWriter _1)
		{
			Write(_1, "dict_int", this.dict_int);
			Write(_1, "dict_long", this.dict_long);
			Write(_1, "dict_string", this.dict_string);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "dict_int": GetChilds(_2).ForEach (_3 => dict_int.Add(ReadInt(GetOnlyChild(_3, "Key")), ReadBool(GetOnlyChild(_3, "Value")))); break;
				case "dict_long": GetChilds(_2).ForEach (_3 => dict_long.Add(ReadLong(GetOnlyChild(_3, "Key")), ReadBool(GetOnlyChild(_3, "Value")))); break;
				case "dict_string": GetChilds(_2).ForEach (_3 => dict_string.Add(ReadString(GetOnlyChild(_3, "Key")), ReadBool(GetOnlyChild(_3, "Value")))); break;
			}
		}
	}
}
