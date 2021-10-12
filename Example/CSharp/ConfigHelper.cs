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
		public Dictionary<int, AllType.CheckAll> CheckAlls => _checkalls;
		private Dictionary<int, AllType.CheckAll> _checkalls = new Dictionary<int, AllType.CheckAll>();
		public Dictionary<int, TestModule.Test> Tests => _tests;
		private Dictionary<int, TestModule.Test> _tests = new Dictionary<int, TestModule.Test>();
		public void Load()
		{
			_allclasss = AllType.AllClass.Load();
			_checkalls = AllType.CheckAll.Load();
			_tests = TestModule.Test.Load();
		}
	}
}
