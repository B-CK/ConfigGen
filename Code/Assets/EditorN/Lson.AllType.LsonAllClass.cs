using System;
using System.Collections.Generic;

namespace Lson.AllType
{
	public class LsonAllClass : LsonObject
	{
		/// <summary>
		/// ID
		/// <summary>
		public int ID;
		/// <summary>
		/// 长整型
		/// <summary>
		public long VarLong;
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
		/// 字符串列表
		/// <summary>
		public List<string> VarListBase;
		/// <summary>
		/// Elem列表
		/// <summary>
		public List<Lson.Card.CardElement> VarListCardElem;
		/// <summary>
		/// 基础类型字典
		/// <summary>
		public Dictionary<int, string> VarDictBase;
		/// <summary>
		/// 枚举类型字典
		/// <summary>
		public Dictionary<long, Lson.Card.CardElement> VarDictEnum;
	}
}
