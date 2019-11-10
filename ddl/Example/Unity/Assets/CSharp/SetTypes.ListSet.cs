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
public class ListSet : XmlObject
	{
		/// <summary>
		/// id
		/// <summary>
		public int ID;
		/// <summary>
		/// 布尔列表
		/// <summary>
		public List<bool> list_bool;
		/// <summary>
		/// 整形列表
		/// <summary>
		public List<int> list_int;
		/// <summary>
		/// 长整形列表
		/// <summary>
		public List<long> list_long;
		/// <summary>
		/// 浮点型列表
		/// <summary>
		public List<float> list_float;
		/// <summary>
		/// 字符串列表
		/// <summary>
		public List<string> list_string;
		/// <summary>
		/// 
		/// <summary>
		public List<XmlEditor.CustomTypes.BuffType> list_enum;
		public override Write(TextWriter _1)
		{
			base.Write(_1);
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
			base.Read(_1);
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "ID": ID = ReadInt(_2); break;
				var list_bools = GetChilds(_2);
				for (int i = 0; i < list_bools.Count; i++)
				{
					var _3 = list_bools[i];
					list_bool.Add(ReadBool(_3));
				}
				break;
				var list_ints = GetChilds(_2);
				for (int i = 0; i < list_ints.Count; i++)
				{
					var _3 = list_ints[i];
					list_int.Add(ReadInt(_3));
				}
				break;
				var list_longs = GetChilds(_2);
				for (int i = 0; i < list_longs.Count; i++)
				{
					var _3 = list_longs[i];
					list_long.Add(ReadLong(_3));
				}
				break;
				var list_floats = GetChilds(_2);
				for (int i = 0; i < list_floats.Count; i++)
				{
					var _3 = list_floats[i];
					list_float.Add(ReadFloat(_3));
				}
				break;
				var list_strings = GetChilds(_2);
				for (int i = 0; i < list_strings.Count; i++)
				{
					var _3 = list_strings[i];
					list_string.Add(ReadString(_3));
				}
				break;
				var list_enums = GetChilds(_2);
				for (int i = 0; i < list_enums.Count; i++)
				{
					var _3 = list_enums[i];
					list_enum.Add((XmlEditor.CustomTypes.BuffType)ReadInt(_3));
				}
				break;
			}
		}
	}
}
