using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.Character
{
	/// <summary>
	/// 
	/// <summary>
	public class Model : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Name;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Character.GroupType GroupType;
		/// <summary>
		/// 
		/// <summary>
		public readonly string ModelPath;
		/// <summary>
		/// 
		/// <summary>
		public readonly string AvatarPath;
		/// <summary>
		/// 
		/// <summary>
		public readonly float BodyRadius;
		/// <summary>
		/// 
		/// <summary>
		public readonly float BodyHeight;
		/// <summary>
		/// 
		/// <summary>
		public readonly float ModelScale;
		public Model(DataStream data)
		{
			Name = data.GetString();
			GroupType = (Cfg.Character.GroupType)data.GetInt();
			ModelPath = data.GetString();
			AvatarPath = data.GetString();
			BodyRadius = data.GetFloat();
			BodyHeight = data.GetFloat();
			ModelScale = data.GetFloat();
		}
		public static Dictionary<string, Cfg.Character.Model> Load()
		{
			var dict = new Dictionary<string, Cfg.Character.Model>();
			var path = "Character/Model.data";
			try
			{
				var data = new DataStream(path, Encoding.UTF8);
				int length = data.GetInt();
				for (int i = 0; i < length; i++)
				{
					var v = new Cfg.Character.Model(data);
					dict.Add(v.Name, v);
				}
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError($"{path}解析异常~\n{e.Message}");
#if UNITY_EDITOR
				UnityEngine.Debug.LogError($"最后一条数据Key:{dict.Last().Key}.");
#endif
			}
			return dict;
		}
	}
}
