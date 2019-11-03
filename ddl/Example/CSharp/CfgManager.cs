using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Example
{
	/// <summary>
	/// 数据管理类
	/// <summary>
	public class CfgManager
	{
		/// <summary>
		/// 配置文件文件夹路径
		/// <summary>
		public static string ConfigDir;
		
		public static readonly Dictionary<int, BaseTypesInfo0.BaseType> BaseType = new Dictionary<int, BaseTypesInfo0.BaseType>();
		public static readonly Dictionary<int, CustomTypes.Character> Character = new Dictionary<int, CustomTypes.Character>();
		public static readonly Dictionary<int, SetTypes.ListSet> ListSet = new Dictionary<int, SetTypes.ListSet>();
		public static readonly Dictionary<int, SetTypes.DictSet> DictSet = new Dictionary<int, SetTypes.DictSet>();
		
		public static int _row;
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
			int length = data.GetInt();			for (int i = 0; i < length; i++)
			{
				_row = i;
				list.Add(constructor(data));
			}
			return list;
		}
		
		public static void LoadAll()
		{
			string path = "Data Path Empty";
			try
			{
				path = ConfigDir + "basetypesinfo0\basetype.data";
				var basetypes = Load(path, (d) => new BaseTypesInfo0.BaseType(d));
				basetypes.ForEach(v => BaseType.Add(v.int_var, v));
				path = ConfigDir + "customtypes\character.data";
				var characters = Load(path, (d) => new CustomTypes.Character(d));
				characters.ForEach(v => Character.Add(v.ID, v));
				path = ConfigDir + "settypes\listset.data";
				var listsets = Load(path, (d) => new SetTypes.ListSet(d));
				listsets.ForEach(v => ListSet.Add(v.ID, v));
				path = ConfigDir + "settypes\dictset.data";
				var dictsets = Load(path, (d) => new SetTypes.DictSet(d));
				dictsets.ForEach(v => DictSet.Add(v.ID, v));
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogErrorFormat("{0}[r{3}]\n{1}\n{2}", path, e.Message, e.StackTrace, _row);
			}
		}
		
		public static void Clear()
		{
			BaseType.Clear();
Character.Clear();
ListSet.Clear();
DictSet.Clear();
		}
		
	}
}
