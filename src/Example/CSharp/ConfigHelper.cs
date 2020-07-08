using Cfg;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Cfg
{
	public partial class ConfigHelper
	{
		public Dictionary<int, AllType.AllClass> AllClasss => _allclasss;
		private Dictionary<int, AllType.AllClass> _allclasss = new Dictionary<int, AllType.AllClass>();
		public Dictionary<int, AllType.Test> Tests => _tests;
		private Dictionary<int, AllType.Test> _tests = new Dictionary<int, AllType.Test>();
		public Dictionary<int, AllType.ClientConst> ClientConsts => _clientconsts;
		private Dictionary<int, AllType.ClientConst> _clientconsts = new Dictionary<int, AllType.ClientConst>();
		public Dictionary<string, Character.Model> Models => _models;
		private Dictionary<string, Character.Model> _models = new Dictionary<string, Character.Model>();
		public Dictionary<string, Skill.ActorConfig> ActorConfigs => _actorconfigs;
		private Dictionary<string, Skill.ActorConfig> _actorconfigs = new Dictionary<string, Skill.ActorConfig>();
		public void Load()
		{
			_allclasss = AllType.AllClass.Load();
			_tests = AllType.Test.Load();
			_clientconsts = AllType.ClientConst.Load();
			_models = Character.Model.Load();
			_actorconfigs = Skill.ActorConfig.Load();
		}
	}
}
