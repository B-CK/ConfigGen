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
	public class ListSet : XmlObject
	{
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

		public override void Write(TextWriter _1)
		{
			Write(_1, "list_bool", this.list_bool);
			Write(_1, "list_int", this.list_int);
			Write(_1, "list_long", this.list_long);
			Write(_1, "list_float", this.list_float);
			Write(_1, "list_string", this.list_string);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "list_bool": GetChilds(_2).ForEach (_3 => list_bool.Add(ReadBool(_3))); break;
				case "list_int": GetChilds(_2).ForEach (_3 => list_int.Add(ReadInt(_3))); break;
				case "list_long": GetChilds(_2).ForEach (_3 => list_long.Add(ReadLong(_3))); break;
				case "list_float": GetChilds(_2).ForEach (_3 => list_float.Add(ReadFloat(_3))); break;
				case "list_string": GetChilds(_2).ForEach (_3 => list_string.Add(ReadString(_3))); break;
			}
		}
	}
}
