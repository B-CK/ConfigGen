using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.Base
{
	/// <summary>
	/// 所有类型
	/// <summary>
	public class BaseClass : CfgObject
	{
		/// <summary>
		/// ID
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// Test.TID
		/// <summary>
		public readonly int Index;
		public BaseClass(DataStream data)
		{
			ID = data.GetInt();
			Index = data.GetInt();
		}
		public static Dictionary<int, Cfg.Base.BaseClass> Load()
		{
			var dict = new Dictionary<int, Cfg.Base.BaseClass>();
			var path = "Base/BaseClass.data";
			try
			{
				var data = new DataStream(path, Encoding.UTF8);
				int length = data.GetArrayLength();
				for (int i = 0; i < length; i++)
				{
					var v = new Cfg.Base.BaseClass(data);
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
