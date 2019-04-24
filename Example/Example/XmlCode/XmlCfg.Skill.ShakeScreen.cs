using System;
using System.Linq;
using System.IO;
using XmlCfg;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class ShakeScreen : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Skill.ShakeType Type;
		/// <summary>
		/// 
		/// <summary>
		public int Frequency;
		/// <summary>
		/// 
		/// <summary>
		public float FrequencyDuration;
		/// <summary>
		/// 
		/// <summary>
		public float FrequencyAtten;
		/// <summary>
		/// 
		/// <summary>
		public float Amplitude;
		/// <summary>
		/// 
		/// <summary>
		public float AmplitudeAtten;
		/// <summary>
		/// 
		/// <summary>
		public float MinRange;
		/// <summary>
		/// 
		/// <summary>
		public float MaxRange;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Type", this.Type);
			Write(_1, "Frequency", this.Frequency);
			Write(_1, "FrequencyDuration", this.FrequencyDuration);
			Write(_1, "FrequencyAtten", this.FrequencyAtten);
			Write(_1, "Amplitude", this.Amplitude);
			Write(_1, "AmplitudeAtten", this.AmplitudeAtten);
			Write(_1, "MinRange", this.MinRange);
			Write(_1, "MaxRange", this.MaxRange);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Type": Type = (XmlCfg.Skill.ShakeType)ReadInt(_2); break;
				case "Frequency": Frequency = ReadInt(_2); break;
				case "FrequencyDuration": FrequencyDuration = ReadFloat(_2); break;
				case "FrequencyAtten": FrequencyAtten = ReadFloat(_2); break;
				case "Amplitude": Amplitude = ReadFloat(_2); break;
				case "AmplitudeAtten": AmplitudeAtten = ReadFloat(_2); break;
				case "MinRange": MinRange = ReadFloat(_2); break;
				case "MaxRange": MaxRange = ReadFloat(_2); break;
			}
		}
	}
}
