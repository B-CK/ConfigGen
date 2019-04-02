using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Cfg
{
	/// <summary>
	/// 数据管理类
	/// <summary>
	publicCfgManager class CfgManager
	{
		/// <summary>
		/// 配置文件文件夹路径
		/// <summary>
		public  string = ConfigDir;
		
		
		private  int = _row;
		/// <summary>
		/// constructor参数为指定类型的构造函数
		/// <summary>
		public  List<T> Load<T>(string path, Func<DataStream, T> constructor)
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
								_row = i;
								list.Add(constructor(data));
			}
						return list;
		}
		
		public  void LoadAll()
		{
						string path = "Data Path Empty";
						try
			{
			}
						catch (Exception e)
			{
								UnityEngine.Debug.LogErrorFormat("{0}[r{3}]\n{1}\n{2}", path, e.Message, e.StackTrace, _row);
			}
		}
		
		public  void Clear()
		{
					}
		
	}
}
