using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class ClientConst : CfgObject
	{
		/// <summary>
		/// 继承2
		/// <summary>
		public readonly int TID;
		/// <summary>
		/// 继承2
		/// <summary>
		public readonly string Name;
		public ClientConst(DataStream data)
		{
			TID = data.GetInt();
			Name = data.GetString();
		}
		public static Dictionary<int, Cfg.AllType.ClientConst> Load()
		{
			var dict = new Dictionary<int, Cfg.AllType.ClientConst>();
			var path = "AllType/ClientConst.data";
			try
			{
				var data = new DataStream(path, Encoding.UTF8);
				int length = data.GetArrayLength();
				for (int i = 0; i < length; i++)
				{
					var v = new Cfg.AllType.ClientConst(data);
					dict.Add(v.TID, v);
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
