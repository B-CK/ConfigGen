using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class ActorConfig : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string ModelName;
		/// <summary>
		/// 
		/// <summary>
		public readonly string BaseModelName;
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<string, Cfg.Skill.GeneralAction> GeneralActions = new Dictionary<string, Cfg.Skill.GeneralAction>();
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<string, Cfg.Skill.SkillAction> SkillActions = new Dictionary<string, Cfg.Skill.SkillAction>();
		public ActorConfig(DataStream data)
		{
			ModelName = data.GetString();
			BaseModelName = data.GetString();
			for (int n = data.GetMapLength(); n-- > 0;)
			{
				var k = data.GetString();
				GeneralActions[k] = (Cfg.Skill.GeneralAction)data.GetObject(data.GetString());
			}
			for (int n = data.GetMapLength(); n-- > 0;)
			{
				var k = data.GetString();
				SkillActions[k] = new Cfg.Skill.SkillAction(data);
			}
		}
		public static Dictionary<string, Cfg.Skill.ActorConfig> Load()
		{
			var dict = new Dictionary<string, Cfg.Skill.ActorConfig>();
			var path = "Skill/ActorConfig.data";
			try
			{
				var data = new DataStream(path, Encoding.UTF8);
				int length = data.GetArrayLength();
				for (int i = 0; i < length; i++)
				{
					var v = new Cfg.Skill.ActorConfig(data);
					dict.Add(v.ModelName, v);
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
