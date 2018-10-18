using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Cfg
{
	public  class CfgManager
	{
		/// <summary>
		/// 配置文件文件夹路径
		/// <summary>
		public static string ConfigDir;


		/// <summary>
		/// constructor参数为指定类型的构造函数
		/// <summary>
		public static List<T> Load<T>(string path, Func<DataStream, T> constructor)
		{
			if (!File.Exists(path))
			{
				UnityEngine.Debug.LogError(path + "配置路径不存在");
				return new List<T>();
			}
			DataStream data = new DataStream(path, Encoding.UTF8);
			List<T> list = new List<T>();
			for (int i = 0; i < data.Count; i++)
			{
				list.Add(constructor(data));
			}
			return list;
		}

		public static void LoadAll()
		{
		}

		public static void Clear()
		{
		}

	}
}
