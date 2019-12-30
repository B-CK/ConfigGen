using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg
{
	public partial class ConfigHelper
	{
		public Dictionary<string, Character.Model> Models => _models;
		private Dictionary<string, Character.Model> _models = new Dictionary<string, Character.Model>();
		public Dictionary<string, Skill.ActorConfig> ActorConfigs => _actorconfigs;
		private Dictionary<string, Skill.ActorConfig> _actorconfigs = new Dictionary<string, Skill.ActorConfig>();
		public void Load()
		{
			_models = Character.Model.Load();
			_actorconfigs = Skill.ActorConfig.Load();
		}
	}
}
