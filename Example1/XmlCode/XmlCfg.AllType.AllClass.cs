using System;
using System.IO;
using Xml;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class AllClass : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public  readonly string ConstString = @"Hello World";
		/// <summary>
		/// 
		/// <summary>
		public  readonly float ConstFloat = 3.141527f;
		/// <summary>
		/// 
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// 
		/// <summary>
		public readonly long VarLong;
		/// <summary>
		/// 
		/// <summary>
		public readonly float VarFloat;
		/// <summary>
		/// 
		/// <summary>
		public readonly string VarString;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool VarBool;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.AllType.CardElement VarEnum;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.AllType.SingleClass VarClass;
		/// <summary>
		/// 
		/// <summary>
		public readonly List<string> VarListBase = new List<string>();
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.AllType.SingleClass> VarListClass = new List<Cfg.AllType.SingleClass>();
		/// <summary>
		/// 
		/// <summary>
		public readonly List<string> VarListCardElem = new List<string>();
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<int, float> VarDictBase = new Dictionary<int, float>();
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<long, Cfg.AllType.CardElement> VarDictEnum = new Dictionary<long, Cfg.AllType.CardElement>();
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<string, Cfg.AllType.SingleClass> VarDictClass = new Dictionary<string, Cfg.AllType.SingleClass>();

		public override void Write(TextWriter _1)
		{
			Write(_1, "ID", this.ID);
			Write(_1, "VarLong", this.VarLong);
			Write(_1, "VarFloat", this.VarFloat);
			Write(_1, "VarString", this.VarString);
			Write(_1, "VarBool", this.VarBool);
			Write(_1, "VarEnum", this.VarEnum);
			Write(_1, "VarClass", this.VarClass);
			Write(_1, "VarListBase", this.VarListBase);
			Write(_1, "VarListClass", this.VarListClass);
			Write(_1, "VarListCardElem", this.VarListCardElem);
			Write(_1, "VarDictBase", this.VarDictBase);
			Write(_1, "VarDictEnum", this.VarDictEnum);
			Write(_1, "VarDictClass", this.VarDictClass);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "ID": Readint(_2);
				case "VarLong": Readlong(_2);
				case "VarFloat": Readfloat(_2);
				case "VarString": Readstring(_2);
				case "VarBool": Readbool(_2);
				case "VarEnum": (Cfg.AllType.CardElement)ReadInt(_2);
				case "VarClass": ReadObject<Cfg.AllType.SingleClass>(_2, Cfg.AllType.SingleClass);
				case "VarListBase": VarListBase = GetChilds(_2).ForEach (_3 => VarListBase.Add(Readstring(_3)));
				case "VarListClass": VarListClass = GetChilds(_2).ForEach (_3 => VarListClass.Add(ReadObject<Cfg.AllType.SingleClass>(_3, Cfg.AllType.SingleClass)));
				case "VarListCardElem": VarListCardElem = GetChilds(_2).ForEach (_3 => VarListCardElem.Add(Readstring(_3)));
				case "VarDictBase": VarDictBase = GetChilds(_2).ForEach (_3 => VarDictBase.Add(Readint(_GetOnlyChild(_3, "Key")), Readfloat(_GetOnlyChild(_3, "Value"))));
				case "VarDictEnum": VarDictEnum = GetChilds(_2).ForEach (_3 => VarDictEnum.Add(Readlong(_GetOnlyChild(_3, "Key")), (Cfg.AllType.CardElement)ReadInt(_GetOnlyChild(_3, "Value"))));
				case "VarDictClass": VarDictClass = GetChilds(_2).ForEach (_3 => VarDictClass.Add(Readstring(_GetOnlyChild(_3, "Key")), ReadObject<Cfg.AllType.SingleClass>(_GetOnlyChild(_3, "Value"), Cfg.AllType.SingleClass)));
			}
		}
	}
}
