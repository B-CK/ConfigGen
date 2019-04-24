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
	public class SkillAction : XmlObject
	{
		/// <summary>
		/// 
		/// <summary>
		public  readonly float EXPIRE_TIME = 1f;
		/// <summary>
		/// 
		/// <summary>
		public float SkillExpireTime;
		/// <summary>
		/// 
		/// <summary>
		public float SkillEndTime;
		/// <summary>
		/// 
		/// <summary>
		public bool CanInterrupt;
		/// <summary>
		/// 
		/// <summary>
		public XmlCfg.Skill.LockObjectType LockType;
		/// <summary>
		/// 
		/// <summary>
		public float SkillRange;
		/// <summary>
		/// 
		/// <summary>
		public bool CanShowSkillRange;
		/// <summary>
		/// 
		/// <summary>
		public bool CanRotate;
		/// <summary>
		/// 
		/// <summary>
		public bool CanMove;
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<int, XmlCfg.Skill.Sequence> SequenceDict = new Dictionary<int, XmlCfg.Skill.Sequence>();

		public override void Write(TextWriter _1)
		{
			Write(_1, "SkillExpireTime", this.SkillExpireTime);
			Write(_1, "SkillEndTime", this.SkillEndTime);
			Write(_1, "CanInterrupt", this.CanInterrupt);
			Write(_1, "LockType", this.LockType);
			Write(_1, "SkillRange", this.SkillRange);
			Write(_1, "CanShowSkillRange", this.CanShowSkillRange);
			Write(_1, "CanRotate", this.CanRotate);
			Write(_1, "CanMove", this.CanMove);
			Write(_1, "SequenceDict", this.SequenceDict);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "SkillExpireTime": SkillExpireTime = ReadFloat(_2); break;
				case "SkillEndTime": SkillEndTime = ReadFloat(_2); break;
				case "CanInterrupt": CanInterrupt = ReadBool(_2); break;
				case "LockType": LockType = (XmlCfg.Skill.LockObjectType)ReadInt(_2); break;
				case "SkillRange": SkillRange = ReadFloat(_2); break;
				case "CanShowSkillRange": CanShowSkillRange = ReadBool(_2); break;
				case "CanRotate": CanRotate = ReadBool(_2); break;
				case "CanMove": CanMove = ReadBool(_2); break;
				case "SequenceDict": GetChilds(_2).ForEach (_3 => SequenceDict.Add(ReadInt(GetOnlyChild(_3, "Key")), ReadObject<XmlCfg.Skill.Sequence>(GetOnlyChild(_3, "Value"), "XmlCfg.Skill.Sequence"))); break;
			}
		}
	}
}
