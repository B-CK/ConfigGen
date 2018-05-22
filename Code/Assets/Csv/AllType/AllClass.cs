using System;
using System.Collections.Generic;
using Csv;

namespace Csv.AllType
{
	public class AllClass : CfgObject
	{
		/// <summary>
		/// ID
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// 长整型
		/// <summary>
		public readonly long VarLong;
		/// <summary>
		/// 浮点型
		/// <summary>
		public readonly float VarFloat;
		/// <summary>
		/// 字符串
		/// <summary>
		public readonly string VarString;
		/// <summary>
		/// 布尔型
		/// <summary>
		public readonly bool VarBool;
		/// <summary>
		/// 枚举类型
		/// <summary>
		public readonly int VarEnum;
		/// <summary>
		/// 类类型
		/// <summary>
		public Csv.AllType.SingleClass VarClass;
		/// <summary>
		/// 字符串列表
		/// <summary>
		public readonly List<string> VarListBase = new List<string>();
		/// <summary>
		/// Class列表
		/// <summary>
		public readonly List<Csv.AllType.SingleClass> VarListClass = new List<Csv.AllType.SingleClass>();
		/// <summary>
		/// Elem列表
		/// <summary>
		public readonly List<int> VarListCardElem = new List<int>();
		/// <summary>
		/// 基础类型字典
		/// <summary>
		public readonly Dictionary<int, string> VarDictBase = new Dictionary<int, string>();
		/// <summary>
		/// 枚举类型字典
		/// <summary>
		public readonly Dictionary<long, int> VarDictEnum = new Dictionary<long, int>();
		/// <summary>
		/// 类类型字典
		/// <summary>
		public readonly Dictionary<string, Csv.AllType.SingleClass> VarDictClass = new Dictionary<string, Csv.AllType.SingleClass>();

		public AllClass(DataStream data)

		{
			this.ID = data.GetInt();
			this.VarLong = data.GetLong();
			this.VarFloat = data.GetFloat();
			this.VarString = data.GetString();
			this.VarBool = data.GetBool();
			this.VarEnum = data.GetInt();
			this.VarClass = (AllType.SingleClass)data.GetObject(data.GetString());
			for (int n = data.GetInt(); n-- > 0; )

			{
				this.VarListBase.Add(data.GetString());
			}
			for (int n = data.GetInt(); n-- > 0; )

			{
				this.VarListClass.Add((AllType.SingleClass)data.GetObject(data.GetString()));
			}
			for (int n = data.GetInt(); n-- > 0; )

			{
				this.VarListCardElem.Add(data.GetInt());
			}
			for (int n = data.GetInt(); n-- > 0;)

			{
				int k = data.GetInt();
				this.VarDictBase[k] = data.GetString();
			}
			for (int n = data.GetInt(); n-- > 0;)

			{
				long k = data.GetLong();
				this.VarDictEnum[k] = data.GetInt();
			}
			for (int n = data.GetInt(); n-- > 0;)

			{
				string k = data.GetString();
				this.VarDictClass[k] = (AllType.SingleClass)data.GetObject(data.GetString());
			}
		}
	}
}
