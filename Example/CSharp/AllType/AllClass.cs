using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.AllType
{
	/// <summary>
	/// 所有类型
	/// <summary>
	public class AllClass : CfgObject
	{
		/// <summary>
		/// 常量字符串
		/// <summary>
		public const string ItemString = @"Hello World";
		/// <summary>
		/// 常量浮点值
		/// <summary>
		public const float ItemFloat = 3.141527f;
		/// <summary>
		/// 常量布尔值
		/// <summary>
		public const bool ItemBool = false;
		/// <summary>
		/// 常量枚举值
		/// <summary>
		public const Cfg.AllType.CardElement ItemEnum = Cfg.AllType.CardElement.Renounce;
		/// <summary>
		/// ID
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// Test.TID
		/// <summary>
		public readonly int Index;
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
		public readonly Cfg.AllType.CardElement VarEnum;
		/// <summary>
		/// 类类型
		/// <summary>
		public readonly Cfg.AllType.SingleClass VarClass;
		/// <summary>
		/// 字符串列表
		/// <summary>
		public readonly List<string> VarListBase = new List<string>();
		/// <summary>
		/// Class列表
		/// <summary>
		public readonly List<Cfg.AllType.SingleClass> VarListClass = new List<Cfg.AllType.SingleClass>();
		/// <summary>
		/// 字符串列表
		/// <summary>
		public readonly List<string> VarListCardElem = new List<string>();
		/// <summary>
		/// 浮点数列表
		/// <summary>
		public readonly List<float> VarListFloat = new List<float>();
		/// <summary>
		/// 基础类型字典
		/// <summary>
		public readonly Dictionary<int, float> VarDictBase = new Dictionary<int, float>();
		/// <summary>
		/// 枚举类型字典
		/// <summary>
		public readonly Dictionary<long, string> VarDictEnum = new Dictionary<long, string>();
		/// <summary>
		/// 类类型字典
		/// <summary>
		public readonly Dictionary<string, Cfg.AllType.SingleClass> VarDictClass = new Dictionary<string, Cfg.AllType.SingleClass>();
		public AllClass(DataStream data)
		{
			ID = data.GetInt();
			Index = data.GetInt();
			VarLong = data.GetLong();
			VarFloat = data.GetFloat();
			VarString = data.GetString();
			VarBool = data.GetBool();
			VarEnum = (Cfg.AllType.CardElement)data.GetInt();
			VarClass = (Cfg.AllType.SingleClass)data.GetObject(data.GetString());
			for (int n = data.GetArrayLength(); n-- > 0;)
			{
				var v = data.GetString();
				VarListBase.Add(v);
			}
			for (int n = data.GetArrayLength(); n-- > 0;)
			{
				var v = (Cfg.AllType.SingleClass)data.GetObject(data.GetString());
				VarListClass.Add(v);
			}
			for (int n = data.GetArrayLength(); n-- > 0;)
			{
				var v = data.GetString();
				VarListCardElem.Add(v);
			}
			for (int n = data.GetArrayLength(); n-- > 0;)
			{
				var v = data.GetFloat();
				VarListFloat.Add(v);
			}
			for (int n = data.GetMapLength(); n-- > 0;)
			{
				var k = data.GetInt();
				VarDictBase[k] = data.GetFloat();
			}
			for (int n = data.GetMapLength(); n-- > 0;)
			{
				var k = data.GetLong();
				VarDictEnum[k] = data.GetString();
			}
			for (int n = data.GetMapLength(); n-- > 0;)
			{
				var k = data.GetString();
				VarDictClass[k] = (Cfg.AllType.SingleClass)data.GetObject(data.GetString());
			}
		}
		public static Dictionary<int, Cfg.AllType.AllClass> Load()
		{
			var dict = new Dictionary<int, Cfg.AllType.AllClass>();
			var path = "AllType/AllClass.data";
			try
			{
				var data = new DataStream(path, Encoding.UTF8);
				int length = data.GetArrayLength();
				for (int i = 0; i < length; i++)
				{
					var v = new Cfg.AllType.AllClass(data);
					dict.Add(v.ID, v);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError($"{path}解析异常~\n{e.Message}\n{e.StackTrace}");
#if UNITY_EDITOR
				UnityEngine.Debug.LogError($"最后一条数据Key:{dict.Last().Key}.");
#endif
			}
			return dict;
		}
	}
}
