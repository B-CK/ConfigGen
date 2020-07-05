using System;
using XmlEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Editor.SetTypes
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
		public Dictionary<int, bool> dict_int = new Dictionary<int, bool>();
		/// <summary>
		/// 长整形字典
		/// <summary>
		public Dictionary<long, bool> dict_long = new Dictionary<long, bool>();
		/// <summary>
		/// 
		/// <summary>
		public Dictionary<string, bool> dict_string = new Dictionary<string, bool>();
		/// <summary>
		/// 枚举字典
		/// <summary>
		public Dictionary<CustomTypes.BuffType, bool> dict_enum = new Dictionary<CustomTypes.BuffType, bool>();
		public override void Write(TextWriter _1)
		{
			Write(_1, "ID", ID);
			Write(_1, "dict_int", dict_int);
			Write(_1, "dict_long", dict_long);
			Write(_1, "dict_string", dict_string);
			Write(_1, "dict_enum", dict_enum);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "ID": ID = ReadInt(_2); break;
				case "dict_int":
					var dict_ints = GetChilds(_2);
					for (int i = 0; i < dict_ints.Count; i++)
					{
						var _3 = dict_ints[i];
						var key = ReadInt(GetOnlyChild(_3, "Key"));
						var value = ReadBool(GetOnlyChild(_3, "Value"));
						dict_int.Add(key, value);
					}
					break;
				case "dict_long":
					var dict_longs = GetChilds(_2);
					for (int i = 0; i < dict_longs.Count; i++)
					{
						var _3 = dict_longs[i];
						var key = ReadLong(GetOnlyChild(_3, "Key"));
						var value = ReadBool(GetOnlyChild(_3, "Value"));
						dict_long.Add(key, value);
					}
					break;
				case "dict_string":
					var dict_strings = GetChilds(_2);
					for (int i = 0; i < dict_strings.Count; i++)
					{
						var _3 = dict_strings[i];
						var key = ReadString(GetOnlyChild(_3, "Key"));
						var value = ReadBool(GetOnlyChild(_3, "Value"));
						dict_string.Add(key, value);
					}
					break;
				case "dict_enum":
					var dict_enums = GetChilds(_2);
					for (int i = 0; i < dict_enums.Count; i++)
					{
						var _3 = dict_enums[i];
						var key = (Editor.CustomTypes.BuffType)ReadInt(GetOnlyChild(_3, "Key"));
						var value = ReadBool(GetOnlyChild(_3, "Value"));
						dict_enum.Add(key, value);
					}
					break;
			}
		}
	}
}
