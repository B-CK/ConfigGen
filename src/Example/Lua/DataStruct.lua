local Stream = require("Common.DataStream")
local GetOrCreate = Util.GetOrCreate

local meta
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.AllClass'
meta.ItemString = 'Hello World'
meta.ItemFloat = 3.141527
meta.ItemBool = false
meta.ItemEnum = 2
GetOrCreate('Cfg.AllType')['AllClass'] = meta
function Stream:GetCfgAllTypeAllClass()
	local o = {}
	setmetatable(o, Cfg.AllType.AllClass)
	o.ID = self:GetInt()
	o.Index = self:GetInt()
	o.VarLong = self:GetLong()
	o.VarFloat = self:GetFloat()
	o.VarString = self:GetString()
	o.VarBool = self:GetBool()
	o.VarEnum = self:GetInt()
	o.VarClass = self:GetCfgAllTypeSingleClassMaker()
	o.VarListBase = self:GetList('String')
	o.VarListClass = self:GetList('CfgAllTypeSingleClassMaker')
	o.VarListCardElem = self:GetList('String')
	o.VarListFloat = self:GetList('Float')
	o.VarDictBase = self:GetDict('Int', 'Float')
	o.VarDictEnum = self:GetDict('Long', 'String')
	o.VarDictClass = self:GetDict('String', 'CfgAllTypeSingleClassMaker')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.CheckAll'
GetOrCreate('Cfg.AllType')['CheckAll'] = meta
function Stream:GetCfgAllTypeCheckAll()
	local o = {}
	setmetatable(o, Cfg.AllType.CheckAll)
	o.ID = self:GetInt()
	o.Index = self:GetInt()
	o.VarLong = self:GetLong()
	o.VarFloat = self:GetFloat()
	o.VarString = self:GetString()
	o.VarListString = self:GetList('String')
	o.VarListStrEmpty = self:GetList('String')
	o.VarListFloat = self:GetList('Float')
	o.VarDictIntFloat = self:GetDict('Int', 'Float')
	o.VarDictLongString = self:GetDict('Long', 'String')
	o.VarDictStringClass = self:GetDict('String', 'CfgAllTypeSingleClassMaker')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.SingleClass'
GetOrCreate('Cfg.AllType')['SingleClass'] = meta
function Stream:GetCfgAllTypeSingleClassMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgAllTypeSingleClass()
	local o = {}
	setmetatable(o, Cfg.AllType.SingleClass)
	o.Var1 = self:GetString()
	o.Var2 = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.M1'
GetOrCreate('Cfg.AllType')['M1'] = meta
function Stream:GetCfgAllTypeM1()
	local o = self:GetCfgAllTypeSingleClass()
	setmetatable(o, Cfg.AllType.M1)
	o.V3 = self:GetLong()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.M2'
GetOrCreate('Cfg.AllType')['M2'] = meta
function Stream:GetCfgAllTypeM2()
	local o = self:GetCfgAllTypeSingleClass()
	setmetatable(o, Cfg.AllType.M2)
	o.V4 = self:GetBool()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.Test'
GetOrCreate('Cfg.AllType')['Test'] = meta
function Stream:GetCfgAllTypeTest()
	local o = {}
	setmetatable(o, Cfg.AllType.Test)
	o.TID = self:GetInt()
	o.Name = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.ClientConst'
GetOrCreate('Cfg.AllType')['ClientConst'] = meta
function Stream:GetCfgAllTypeClientConst()
	local o = {}
	setmetatable(o, Cfg.AllType.ClientConst)
	o.TID = self:GetInt()
	o.Name = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Character.Model'
GetOrCreate('Cfg.Character')['Model'] = meta
function Stream:GetCfgCharacterModel()
	local o = {}
	setmetatable(o, Cfg.Character.Model)
	o.Name = self:GetString()
	o.GroupType = self:GetInt()
	o.ModelPath = self:GetString()
	o.AvatarPath = self:GetString()
	o.BodyRadius = self:GetFloat()
	o.BodyHeight = self:GetFloat()
	o.ModelScale = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.ActorConfig'
GetOrCreate('Cfg.Skill')['ActorConfig'] = meta
function Stream:GetCfgSkillActorConfig()
	local o = {}
	setmetatable(o, Cfg.Skill.ActorConfig)
	o.ModelName = self:GetString()
	o.BaseModelName = self:GetString()
	o.GeneralActions = self:GetDict('String', 'CfgSkillGeneralActionMaker')
	o.SkillActions = self:GetDict('String', 'CfgSkillSkillAction')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.GeneralAction'
GetOrCreate('Cfg.Skill')['GeneralAction'] = meta
function Stream:GetCfgSkillGeneralActionMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgSkillGeneralAction()
	local o = {}
	setmetatable(o, Cfg.Skill.GeneralAction)
	o.ActionName = self:GetString()
	o.OtherModelName = self:GetString()
	o.ActionClip = self:GetString()
	o.PreActionFile = self:GetString()
	o.PostActionFile = self:GetString()
	o.ActionSpeed = self:GetFloat()
	o.LoopTimes = self:GetInt()
	o.Timelines = self:GetList('CfgSkillTimelineMaker')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.SkillAction'
GetOrCreate('Cfg.Skill')['SkillAction'] = meta
function Stream:GetCfgSkillSkillAction()
	local o = self:GetCfgSkillGeneralAction()
	setmetatable(o, Cfg.Skill.SkillAction)
	o.SkillExpireTime = self:GetFloat()
	o.SkillEndTime = self:GetFloat()
	o.CanInterrupt = self:GetBool()
	o.LockType = self:GetInt()
	o.SkillRange = self:GetFloat()
	o.CanShowSkillRange = self:GetBool()
	o.CanRotate = self:GetBool()
	o.CanMove = self:GetBool()
	o.SequenceDict = self:GetDict('Int', 'CfgSkillSequence')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.Sequence'
GetOrCreate('Cfg.Skill')['Sequence'] = meta
function Stream:GetCfgSkillSequence()
	local o = {}
	setmetatable(o, Cfg.Skill.Sequence)
	o.Id = self:GetString()
	o.HitZones = self:GetList('CfgSkillHitZoneMaker')
	o.Timelines = self:GetList('CfgSkillTimelineMaker')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.HitZone'
GetOrCreate('Cfg.Skill')['HitZone'] = meta
function Stream:GetCfgSkillHitZoneMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgSkillHitZone()
	local o = {}
	setmetatable(o, Cfg.Skill.HitZone)
	o.Id = self:GetInt()
	o.Sharp = self:GetInt()
	o.Offset = self:GetCfgCommonVector3Maker()
	o.MaxNum = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.CubeZone'
GetOrCreate('Cfg.Skill')['CubeZone'] = meta
function Stream:GetCfgSkillCubeZone()
	local o = self:GetCfgSkillHitZone()
	setmetatable(o, Cfg.Skill.CubeZone)
	o.Scale = self:GetCfgCommonVector3Maker()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.SphereZone'
GetOrCreate('Cfg.Skill')['SphereZone'] = meta
function Stream:GetCfgSkillSphereZone()
	local o = self:GetCfgSkillHitZone()
	setmetatable(o, Cfg.Skill.SphereZone)
	o.Radius = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.CylinderZone'
GetOrCreate('Cfg.Skill')['CylinderZone'] = meta
function Stream:GetCfgSkillCylinderZone()
	local o = self:GetCfgSkillHitZone()
	setmetatable(o, Cfg.Skill.CylinderZone)
	o.Radius = self:GetFloat()
	o.Height = self:GetFloat()
	o.Angle = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.Timeline'
GetOrCreate('Cfg.Skill')['Timeline'] = meta
function Stream:GetCfgSkillTimelineMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgSkillTimeline()
	local o = {}
	setmetatable(o, Cfg.Skill.Timeline)
	o.Start = self:GetInt()
	o.End = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.StaticHit'
GetOrCreate('Cfg.Skill')['StaticHit'] = meta
function Stream:GetCfgSkillStaticHitMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgSkillStaticHit()
	local o = self:GetCfgSkillTimeline()
	setmetatable(o, Cfg.Skill.StaticHit)
	o.Zone = self:GetCfgSkillHitZoneMaker()
	o.SequeueID = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.DynamicHit'
GetOrCreate('Cfg.Skill')['DynamicHit'] = meta
function Stream:GetCfgSkillDynamicHit()
	local o = self:GetCfgSkillStaticHit()
	setmetatable(o, Cfg.Skill.DynamicHit)
	o.Target = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.Controller'
GetOrCreate('Cfg.Skill')['Controller'] = meta
function Stream:GetCfgSkillControllerMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgSkillController()
	local o = self:GetCfgSkillTimeline()
	setmetatable(o, Cfg.Skill.Controller)
	o.Path = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.Active'
GetOrCreate('Cfg.Skill')['Active'] = meta
function Stream:GetCfgSkillActive()
	local o = self:GetCfgSkillController()
	setmetatable(o, Cfg.Skill.Active)
	o.Enable = self:GetBool()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.ReplaceObject'
GetOrCreate('Cfg.Skill')['ReplaceObject'] = meta
function Stream:GetCfgSkillReplaceObject()
	local o = self:GetCfgSkillController()
	setmetatable(o, Cfg.Skill.ReplaceObject)
	o.NewObject = self:GetString()
	o.Offset = self:GetCfgCommonVector3Maker()
	o.EulerAngles = self:GetCfgCommonVector3Maker()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.Move'
GetOrCreate('Cfg.Skill')['Move'] = meta
function Stream:GetCfgSkillMove()
	local o = self:GetCfgSkillController()
	setmetatable(o, Cfg.Skill.Move)
	o.Type = self:GetInt()
	o.IsRelateSelf = self:GetBool()
	o.Offset = self:GetCfgCommonVector3Maker()
	o.Angle = self:GetFloat()
	o.Speed = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.CastObject'
GetOrCreate('Cfg.Skill')['CastObject'] = meta
function Stream:GetCfgSkillCastObject()
	local o = self:GetCfgSkillController()
	setmetatable(o, Cfg.Skill.CastObject)
	o.IsTraceTarget = self:GetBool()
	o.CurveId = self:GetInt()
	o.PassBody = self:GetBool()
	o.Position = self:GetCfgCommonVector3Maker()
	o.EulerAngles = self:GetCfgCommonVector3Maker()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.PlayParticle'
GetOrCreate('Cfg.Skill')['PlayParticle'] = meta
function Stream:GetCfgSkillPlayParticleMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgSkillPlayParticle()
	local o = self:GetCfgSkillTimeline()
	setmetatable(o, Cfg.Skill.PlayParticle)
	o.Path = self:GetString()
	o.IsRelateSelf = self:GetBool()
	o.FollowDir = self:GetBool()
	o.NodeName = self:GetString()
	o.Position = self:GetCfgCommonVector3Maker()
	o.EulerAngles = self:GetCfgCommonVector3Maker()
	o.Scale = self:GetCfgCommonVector3Maker()
	o.AlignType = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.Buff'
GetOrCreate('Cfg.Skill')['Buff'] = meta
function Stream:GetCfgSkillBuff()
	local o = self:GetCfgSkillPlayParticle()
	setmetatable(o, Cfg.Skill.Buff)
	o.Id = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.PlaySound'
GetOrCreate('Cfg.Skill')['PlaySound'] = meta
function Stream:GetCfgSkillPlaySound()
	local o = self:GetCfgSkillTimeline()
	setmetatable(o, Cfg.Skill.PlaySound)
	o.Path = self:GetString()
	o.Volume = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Skill.ShakeScreen'
GetOrCreate('Cfg.Skill')['ShakeScreen'] = meta
function Stream:GetCfgSkillShakeScreen()
	local o = self:GetCfgSkillTimeline()
	setmetatable(o, Cfg.Skill.ShakeScreen)
	o.Type = self:GetInt()
	o.Frequency = self:GetInt()
	o.FrequencyDuration = self:GetFloat()
	o.FrequencyAtten = self:GetFloat()
	o.Amplitude = self:GetFloat()
	o.AmplitudeAtten = self:GetFloat()
	o.MinRange = self:GetFloat()
	o.MaxRange = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Common.Vector2'
GetOrCreate('Cfg.Common')['Vector2'] = meta
function Stream:GetCfgCommonVector2()
	local o = {}
	setmetatable(o, Cfg.Common.Vector2)
	o.X = self:GetFloat()
	o.Y = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Common.Vector3'
GetOrCreate('Cfg.Common')['Vector3'] = meta
function Stream:GetCfgCommonVector3()
	local o = {}
	setmetatable(o, Cfg.Common.Vector3)
	o.X = self:GetFloat()
	o.Y = self:GetFloat()
	o.Z = self:GetFloat()
	return o
end
GetOrCreate('AllType')['CardElement'] = {
	Attack = 0,
	Extract = 1,
	Renounce = 2,
	Armor = 3,
	Control = 4,
	Cure = 5,
	Oneself = 6,
	Hand = 7,
	Brary = 8,
	Handack = 9,
}
GetOrCreate('Character')['GroupType'] = {
	None = 0,
	Base = 1,
	Player = 2,
	Monster = 3,
	NPC = 4,
}
GetOrCreate('Skill')['ActionSourceType'] = {
	SelfModel = 0,
	OtherModel = 1,
}
GetOrCreate('Skill')['LockObjectType'] = {
	None = 0,
	Enemy = 1,
	Teammate = 2,
	Self = 3,
	Other = 4,
}
GetOrCreate('Skill')['HitSharpType'] = {
	Cube = 0,
	Sphere = 1,
	Cylinder = 2,
}
GetOrCreate('Skill')['MoveType'] = {
	MoveToTarget = 0,
	MoveInDirection = 1,
}
GetOrCreate('Skill')['BindType'] = {
	None = 0,
	Body = 1,
	Head = 2,
	Foot = 3,
}
GetOrCreate('Skill')['EffectAlignType'] = {
	None = 0,
	LeftTop = 1,
	Left = 2,
	LeftBottom = 3,
	Top = 4,
	Center = 5,
	Bottom = 6,
	RightTop = 7,
	Right = 8,
	RightBottom = 9,
}
GetOrCreate('Skill')['ShakeType'] = {
	Horizontal = 0,
	Vertical = 1,
	Mix = 2,
}
return Stream
