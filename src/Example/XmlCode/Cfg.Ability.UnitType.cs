using System;
using XmlEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
namespace Cfg.Ability
{
	/// <summary>
	/// 
	/// <summary>
	public enum UnitType
	{
		UNIT_TYPE_NONE = 1,
		UNIT_TYPE_BUILDING = 2,
		UNIT_TYPE_CREEP = 4,
		UNIT_TYPE_HERO = 8,
		UNIT_TYPE_OTHER = 16,
		UNIT_TYPE_ALL = 32,
	}
}
