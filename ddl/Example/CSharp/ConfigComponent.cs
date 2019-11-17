using Example;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace Example
{
	public partial class ConfigComponent
	{
		public Dictionary<int, BaseTypesInfo0.BaseType> BaseTypes => _basetypes;
		private Dictionary<int, BaseTypesInfo0.BaseType> _basetypes = new Dictionary<int, BaseTypesInfo0.BaseType>();
		public Dictionary<int, CustomTypes.Character> Characters => _characters;
		private Dictionary<int, CustomTypes.Character> _characters = new Dictionary<int, CustomTypes.Character>();
		public Dictionary<int, SetTypes.ListSet> ListSets => _listsets;
		private Dictionary<int, SetTypes.ListSet> _listsets = new Dictionary<int, SetTypes.ListSet>();
		public Dictionary<int, SetTypes.DictSet> DictSets => _dictsets;
		private Dictionary<int, SetTypes.DictSet> _dictsets = new Dictionary<int, SetTypes.DictSet>();
		public Dictionary<int, XmlPaser.Character> Characters => _characters;
		private Dictionary<int, XmlPaser.Character> _characters = new Dictionary<int, XmlPaser.Character>();
		public void Load()
		{
			_basetypes = BaseTypesInfo0.BaseType.Load();
			_characters = CustomTypes.Character.Load();
			_listsets = SetTypes.ListSet.Load();
			_dictsets = SetTypes.DictSet.Load();
			_characters = XmlPaser.Character.Load();
		}
	}
}
