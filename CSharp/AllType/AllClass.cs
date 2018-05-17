using System;
using System.Collections.Generic;
using Csv;

namespace Csv.AllType
{
	public sealed class AllClass : CfgObject
	{
		public readonly int ID;
		public readonly long VarLong;
		public readonly float VarFloat;
		public readonly string VarString;
		public readonly bool VarBool;
		public readonly int VarEnum;
		public readonly Csv.AllType.SingleClass VarClass;
		public readonly List<string> VarListBase = new List<string>();
		public readonly List<Csv.AllType.SingleClass> VarListClass = new List<Csv.AllType.SingleClass>();
		public readonly List<int> VarListCardElem = new List<int>();
		public readonly Dictionary<int, string> VarDictBase = new Dictionary<int, string>();
		public readonly Dictionary<long, int> VarDictEnum = new Dictionary<long, int>();
		public readonly Dictionary<string, Csv.AllType.SingleClass> VarDictClass = new Dictionary<string, Csv.AllType.SingleClass>();

		public AllClass(DataStream data)
		{
			this.ID = data.GetInt();
			this.VarLong = data.GetLong();
			this.VarFloat = data.GetFloat();
			this.VarString = data.GetString();
			this.VarBool = data.GetBool();
			this.VarEnum = data.GetInt();
			this.VarClass =  new Csv.AllType.SingleClass(data);
			for (int n = data.GetInt(); n-- > 0; )
			{
				this.VarListBase.Add(data.GetString());
			}
			for (int n = data.GetInt(); n-- > 0; )
			{
				this.VarListClass.Add(new Csv.AllType.SingleClass(data));
			}
			for (int n = data.GetInt(); n-- > 0; )
			{
				this.VarListCardElem.Add(data.GetInt());
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				int k = data.GetInt();
				this.VarDictBase[k] = new data.GetString();
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				long k = data.GetLong();
				this.VarDictEnum[k] = data.GetInt();
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				string k = data.GetString();
				this.VarDictClass[k] = new Csv.AllType.SingleClass(data);
			}
		}
	}
}
