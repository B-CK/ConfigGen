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
	public class CheckAll : CfgObject
	{
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
		/// 字符串列表
		/// <summary>
		public readonly List<string> VarListString = new List<string>();
		/// <summary>
		/// 字符串列表
		/// <summary>
		public readonly List<string> VarListStrEmpty = new List<string>();
		/// <summary>
		/// 浮点数列表
		/// <summary>
		public readonly List<float> VarListFloat = new List<float>();
		/// <summary>
		/// 基础类型字典
		/// <summary>
		public readonly Dictionary<int, float> VarDictIntFloat = new Dictionary<int, float>();
		/// <summary>
		/// 枚举类型字典
		/// <summary>
		public readonly Dictionary<long, string> VarDictLongString = new Dictionary<long, string>();
		/// <summary>
		/// 类类型字典
		/// <summary>
		public readonly Dictionary<string, Cfg.AllType.SingleClass> VarDictStringClass = new Dictionary<string, Cfg.AllType.SingleClass>();
		public CheckAll(DataStream data)
		{
			ID = data.GetInt();
			Index = data.GetInt();
			VarLong = data.GetLong();
			VarFloat = data.GetFloat();
			VarString = data.GetString();
			for (int n = data.GetArrayLength(); n-- > 0;)
			{
				var v = data.GetString();
				VarListString.Add(v);
			}
			for (int n = data.GetArrayLength(); n-- > 0;)
			{
				var v = data.GetString();
				VarListStrEmpty.Add(v);
			}
			for (int n = data.GetArrayLength(); n-- > 0;)
			{
				var v = data.GetFloat();
				VarListFloat.Add(v);
			}
			for (int n = data.GetMapLength(); n-- > 0;)
			{
				var k = data.GetInt();
				VarDictIntFloat[k] = data.GetFloat();
			}
			for (int n = data.GetMapLength(); n-- > 0;)
			{
				var k = data.GetLong();
				VarDictLongString[k] = data.GetString();
			}
			for (int n = data.GetMapLength(); n-- > 0;)
			{
				var k = data.GetString();
				VarDictStringClass[k] = (Cfg.AllType.SingleClass)data.GetObject(data.GetString());
			}
		}
		public static Dictionary<int, Cfg.AllType.CheckAll> Load()
		{
			var dict = new Dictionary<int, Cfg.AllType.CheckAll>();
			var path = "AllType/CheckAll.csv";
			try
			{
				var data = new DataStream(path, Encoding.UTF8);
				int length = data.GetArrayLength();
				for (int i = 0; i < length; i++)
				{
					var v = new Cfg.AllType.CheckAll(data);
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
