using System;
using System.Text;
using System.Collections.Generic;
namespace Csv
{
	public class CfgManager
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
