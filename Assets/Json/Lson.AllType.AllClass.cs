using System;
using System.Collections.Generic;

namespace Lson.AllType
{
	public class AllClass : LsonObject
	{
		/// <summary>
		/// 常量字符串
		/// <summary>
		public const string ConstString = @"Hello World";
		/// <summary>
		/// 常量浮点值
		/// <summary>
		public const float ConstFloat = 3.141527f;
		/// <summary>
		/// ID
		/// <summary>
		public int ID;
		/// <summary>
		/// 长整型
		/// <summary>
		public int VarLong;
		/// <summary>
		/// 浮点型
		/// <summary>
		public float VarFloat;
		/// <summary>
		/// 字符串
		/// <summary>
		public string VarString;
		/// <summary>
		/// 布尔型
		/// <summary>
		public bool VarBool;
		/// <summary>
		/// 枚举类型
		/// <summary>
		public Lson.Card.CardElement VarEnum;
		/// <summary>
		/// 类类型
		/// <summary>
		public Lson.AllType.SingleClass VarClass;
		/// <summary>
		/// 字符串列表
		/// <summary>
		public List<string> VarListBase;
		/// <summary>
		/// Class列表
		/// <summary>
		public List<Lson.AllType.SingleClass> VarListClass;
		/// <summary>
		/// 字符串列表
		/// <summary>
		public List<string> VarListCardElem;
		/// <summary>
		/// 基础类型字典
		/// <summary>
		public Dictionary<int, string> VarDictBase;
		/// <summary>
		/// 枚举类型字典
		/// <summary>
		public Dictionary<long, Lson.Card.CardElement> VarDictEnum;
		/// <summary>
		/// 类类型字典
		/// <summary>
		public Dictionary<string, Lson.AllType.SingleClass> VarDictClass;
	}
}
