using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace Lson.Skill
{
	public  class Movement : Lson.Skill.Action
	{
		/// <summary>
		/// 移动方式
		/// <summary>
		public Lson.Skill.MoveType Type;
		/// <summary>
		/// 持续时间
		/// <summary>
		public float Duration;
		/// <summary>
		/// 速度
		/// <summary>
		public float Speed;
		/// <summary>
		/// 加速度
		/// <summary>
		public float Acceleration;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Type", (int)this.Type);
			Write(_1, "Duration", this.Duration);
			Write(_1, "Speed", this.Speed);
			Write(_1, "Acceleration", this.Acceleration);
		}

		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Type": this.Type = (Lson.Skill.MoveType)ReadInt(_2); break;
				case "Duration": this.Duration = ReadFloat(_2); break;
				case "Speed": this.Speed = ReadFloat(_2); break;
				case "Acceleration": this.Acceleration = ReadFloat(_2); break;
			}
		}
	}
}
