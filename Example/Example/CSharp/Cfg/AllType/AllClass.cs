using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class AllClass : CfgObject
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
		
		public AllClass(DataStream data)
		{
			ID = data.GetInt();
			VarLong = data.GetLong();
			VarFloat = data.GetFloat();
			VarString = data.GetString();
			VarBool = data.GetBool();
			VarEnum = (Cfg.AllType.CardElement)data.GetInt();
			VarClass = (Cfg.AllType.SingleClass)data.GetObject(data.GetString());
			for (int n = data.GetInt(); n-- > 0;)
			{
				string v = data.GetString();
				VarListBase.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				Cfg.AllType.SingleClass v = (Cfg.AllType.SingleClass)data.GetObject(data.GetString());
				VarListClass.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				string v = data.GetString();
				VarListCardElem.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				int k = data.GetInt();
				float v = data.GetFloat();
				VarDictBase[k] = v;
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				long k = data.GetLong();
				Cfg.AllType.CardElement v = (Cfg.AllType.CardElement)data.GetInt();
				VarDictEnum[k] = v;
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				string k = data.GetString();
				Cfg.AllType.SingleClass v = (Cfg.AllType.SingleClass)data.GetObject(data.GetString());
				VarDictClass[k] = v;
			}
		}
	}
}
