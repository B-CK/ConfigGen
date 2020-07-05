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
	public class ListSet : XmlObject
	{
		/// <summary>
		/// id
		/// <summary>
		public int ID;
		/// <summary>
		/// 布尔列表
		/// <summary>
		public List<bool> list_bool = new List<bool>();
		/// <summary>
		/// 整形列表
		/// <summary>
		public List<int> list_int = new List<int>();
		/// <summary>
		/// 长整形列表
		/// <summary>
		public List<long> list_long = new List<long>();
		/// <summary>
		/// 浮点型列表
		/// <summary>
		public List<float> list_float = new List<float>();
		/// <summary>
		/// 字符串列表
		/// <summary>
		public List<string> list_string = new List<string>();
		/// <summary>
		/// 
		/// <summary>
		public List<Editor.CustomTypes.BuffType> list_enum = new List<Editor.CustomTypes.BuffType>();
		public override void Write(TextWriter _1)
		{
			Write(_1, "ID", ID);
			Write(_1, "list_bool", list_bool);
			Write(_1, "list_int", list_int);
			Write(_1, "list_long", list_long);
			Write(_1, "list_float", list_float);
			Write(_1, "list_string", list_string);
			Write(_1, "list_enum", list_enum);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "ID": ID = ReadInt(_2); break;
				case "list_bool":
					var list_bools = GetChilds(_2);
					for (int i = 0; i < list_bools.Count; i++)
					{
						var _3 = list_bools[i];
						list_bool.Add(ReadBool(_3));
					}
					break;
				case "list_int":
					var list_ints = GetChilds(_2);
					for (int i = 0; i < list_ints.Count; i++)
					{
						var _3 = list_ints[i];
						list_int.Add(ReadInt(_3));
					}
					break;
				case "list_long":
					var list_longs = GetChilds(_2);
					for (int i = 0; i < list_longs.Count; i++)
					{
						var _3 = list_longs[i];
						list_long.Add(ReadLong(_3));
					}
					break;
				case "list_float":
					var list_floats = GetChilds(_2);
					for (int i = 0; i < list_floats.Count; i++)
					{
						var _3 = list_floats[i];
						list_float.Add(ReadFloat(_3));
					}
					break;
				case "list_string":
					var list_strings = GetChilds(_2);
					for (int i = 0; i < list_strings.Count; i++)
					{
						var _3 = list_strings[i];
						list_string.Add(ReadString(_3));
					}
					break;
				case "list_enum":
					var list_enums = GetChilds(_2);
					for (int i = 0; i < list_enums.Count; i++)
					{
						var _3 = list_enums[i];
						list_enum.Add((Editor.CustomTypes.BuffType)ReadInt(_3));
					}
					break;
			}
		}
	}
}