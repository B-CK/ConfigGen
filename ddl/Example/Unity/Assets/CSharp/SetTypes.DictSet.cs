using XmlEditor;
using System;
using System.IO
using System.Xml
using System.Linq
using System.Collections.Generic;
namespace Example.SetTypes
{
	/// <summary>
	/// 
	/// <summary>
public class DictSet : XmlObject
	{
		/// <summary>
		/// id
		/// <summary>
		public int ID;
		/// <summary>
		/// 整形字典
		/// <summary>
		public Dictionary<int, bool> dict_int;
		/// <summary>
		/// 长整形字典
		/// <summary>
		public Dictionary<long, bool> dict_long;
		/// <summary>
		/// 
		/// <summary>
		public Dictionary<string, bool> dict_string;
		/// <summary>
		/// 枚举字典
		/// <summary>
		public Dictionary<CustomTypes.BuffType, bool> dict_enum;
		public override Write(TextWriter _1)
		{
			base.Write(_1);
			Write(_1, "ID", ID);
			Write(_1, "dict_int", dict_int);
			Write(_1, "dict_long", dict_long);
			Write(_1, "dict_string", dict_string);
			Write(_1, "dict_enum", dict_enum);
		}
		public override void Read(XmlNode _1)
		{
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "ID": ID = ReadInt(_2); break;
				var dict_ints = GetChilds(_2);
				for (int i = 0; i < dict_ints.Count; i++)
				{
					var _3 = dict_ints[i];
					var key = ReadInt(GetOnlyChild(_3, "Key"));
					var value = ReadBool(GetOnlyChild(_3, "Value"));
					dict_int.Add(key, value);
				}
				break;
				var dict_longs = GetChilds(_2);
				for (int i = 0; i < dict_longs.Count; i++)
				{
					var _3 = dict_longs[i];
					var key = ReadLong(GetOnlyChild(_3, "Key"));
					var value = ReadBool(GetOnlyChild(_3, "Value"));
					dict_long.Add(key, value);
				}
				break;
				var dict_strings = GetChilds(_2);
				for (int i = 0; i < dict_strings.Count; i++)
				{
					var _3 = dict_strings[i];
					var key = ReadString(GetOnlyChild(_3, "Key"));
					var value = ReadBool(GetOnlyChild(_3, "Value"));
					dict_string.Add(key, value);
				}
				break;
				var dict_enums = GetChilds(_2);
				for (int i = 0; i < dict_enums.Count; i++)
				{
					var _3 = dict_enums[i];
					var key = (XmlEditor.CustomTypes.BuffType)ReadInt(GetOnlyChild(_3, "Key"));
					var value = ReadBool(GetOnlyChild(_3, "Value"));
					dict_enum.Add(key, value);
				}
				break;
			}
		}
	}
}
