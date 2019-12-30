local Stream = require("Common.DataStream")
local GetOrCreate = Util.GetOrCreate

local meta
meta= {}
meta.__index = meta
meta.class = 'Character.Model'
GetOrCreate('Character')['Model'] = meta
function Stream:GetCharacterModel()
	local o = {}
	setmetatable(o, Character.Model)
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
meta.class = 'Skill.ActorConfig'
GetOrCreate('Skill')['ActorConfig'] = meta
function Stream:GetSkillActorConfig()
	local o = {}
	setmetatable(o, Skill.ActorConfig)
	o.ModelName = self:GetString()
	o.BaseModelName = self:GetString()
	o.GeneralActions = self:GetDict('String', 'SkillGeneralActionMaker')
	o.SkillActions = self:GetDict('String', 'SkillSkillActionMaker')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.GeneralAction'
GetOrCreate('Skill')['GeneralAction'] = meta
function Stream:GetSkillGeneralActionMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetSkillGeneralAction()
	local o = {}
	setmetatable(o, Skill.GeneralAction)
	o.ActionName = self:GetString()
	o.OtherModelName = self:GetString()
	o.ActionClip = self:GetString()
	o.PreActionFile = self:GetString()
	o.PostActionFile = self:GetString()
	o.ActionSpeed = self:GetFloat()
	o.LoopTimes = self:GetInt()
	o.Timelines = self:GetList('SkillTimelineMaker')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.SkillAction'
GetOrCreate('Skill')['SkillAction'] = meta
function Stream:GetSkillSkillAction()
	local o = self:GetSkillGeneralAction()
	setmetatable(o, Skill.SkillAction)
	o.SkillExpireTime = self:GetFloat()
	o.SkillEndTime = self:GetFloat()
	o.CanInterrupt = self:GetBool()
	o.LockType = self:GetInt()
	o.SkillRange = self:GetFloat()
	o.CanShowSkillRange = self:GetBool()
	o.CanRotate = self:GetBool()
	o.CanMove = self:GetBool()
	o.SequenceDict = self:GetDict('Int', 'SkillSequenceMaker')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.Sequence'
GetOrCreate('Skill')['Sequence'] = meta
function Stream:GetSkillSequence()
	local o = {}
	setmetatable(o, Skill.Sequence)
	o.Id = self:GetString()
	o.HitZones = self:GetList('SkillHitZoneMaker')
	o.Timelines = self:GetList('SkillTimelineMaker')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.HitZone'
GetOrCreate('Skill')['HitZone'] = meta
function Stream:GetSkillHitZoneMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetSkillHitZone()
	local o = {}
	setmetatable(o, Skill.HitZone)
	o.Id = self:GetInt()
	o.Sharp = self:GetInt()
	o.Offset = self:GetCommonVector3Maker()
	o.MaxNum = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.CubeZone'
GetOrCreate('Skill')['CubeZone'] = meta
function Stream:GetSkillCubeZone()
	local o = self:GetSkillHitZone()
	setmetatable(o, Skill.CubeZone)
	o.Scale = self:GetCommonVector3Maker()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.SphereZone'
GetOrCreate('Skill')['SphereZone'] = meta
function Stream:GetSkillSphereZone()
	local o = self:GetSkillHitZone()
	setmetatable(o, Skill.SphereZone)
	o.Radius = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.CylinderZone'
GetOrCreate('Skill')['CylinderZone'] = meta
function Stream:GetSkillCylinderZone()
	local o = self:GetSkillHitZone()
	setmetatable(o, Skill.CylinderZone)
	o.Radius = self:GetFloat()
	o.Height = self:GetFloat()
	o.Angle = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.Timeline'
GetOrCreate('Skill')['Timeline'] = meta
function Stream:GetSkillTimelineMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetSkillTimeline()
	local o = {}
	setmetatable(o, Skill.Timeline)
	o.Start = self:GetInt()
	o.End = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.StaticHit'
GetOrCreate('Skill')['StaticHit'] = meta
function Stream:GetSkillStaticHitMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetSkillStaticHit()
	local o = self:GetSkillTimeline()
	setmetatable(o, Skill.StaticHit)
	o.Zone = self:GetSkillHitZoneMaker()
	o.SequeueID = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.DynamicHit'
GetOrCreate('Skill')['DynamicHit'] = meta
function Stream:GetSkillDynamicHit()
	local o = self:GetSkillStaticHit()
	setmetatable(o, Skill.DynamicHit)
	o.Target = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.Controller'
GetOrCreate('Skill')['Controller'] = meta
function Stream:GetSkillControllerMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetSkillController()
	local o = self:GetSkillTimeline()
	setmetatable(o, Skill.Controller)
	o.Path = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.Active'
GetOrCreate('Skill')['Active'] = meta
function Stream:GetSkillActive()
	local o = self:GetSkillController()
	setmetatable(o, Skill.Active)
	o.Enable = self:GetBool()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.ReplaceObject'
GetOrCreate('Skill')['ReplaceObject'] = meta
function Stream:GetSkillReplaceObject()
	local o = self:GetSkillController()
	setmetatable(o, Skill.ReplaceObject)
	o.NewObject = self:GetString()
	o.Offset = self:GetCommonVector3Maker()
	o.EulerAngles = self:GetCommonVector3Maker()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.Move'
GetOrCreate('Skill')['Move'] = meta
function Stream:GetSkillMove()
	local o = self:GetSkillController()
	setmetatable(o, Skill.Move)
	o.Type = self:GetInt()
	o.IsRelateSelf = self:GetBool()
	o.Offset = self:GetCommonVector3Maker()
	o.Angle = self:GetFloat()
	o.Speed = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.CastObject'
GetOrCreate('Skill')['CastObject'] = meta
function Stream:GetSkillCastObject()
	local o = self:GetSkillController()
	setmetatable(o, Skill.CastObject)
	o.IsTraceTarget = self:GetBool()
	o.CurveId = self:GetInt()
	o.PassBody = self:GetBool()
	o.Position = self:GetCommonVector3Maker()
	o.EulerAngles = self:GetCommonVector3Maker()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.PlayParticle'
GetOrCreate('Skill')['PlayParticle'] = meta
function Stream:GetSkillPlayParticleMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetSkillPlayParticle()
	local o = self:GetSkillTimeline()
	setmetatable(o, Skill.PlayParticle)
	o.Path = self:GetString()
	o.IsRelateSelf = self:GetBool()
	o.FollowDir = self:GetBool()
	o.NodeName = self:GetString()
	o.Position = self:GetCommonVector3Maker()
	o.EulerAngles = self:GetCommonVector3Maker()
	o.Scale = self:GetCommonVector3Maker()
	o.AlignType = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.Buff'
GetOrCreate('Skill')['Buff'] = meta
function Stream:GetSkillBuff()
	local o = self:GetSkillPlayParticle()
	setmetatable(o, Skill.Buff)
	o.Id = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.PlaySound'
GetOrCreate('Skill')['PlaySound'] = meta
function Stream:GetSkillPlaySound()
	local o = self:GetSkillTimeline()
	setmetatable(o, Skill.PlaySound)
	o.Path = self:GetString()
	o.Volume = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Skill.ShakeScreen'
GetOrCreate('Skill')['ShakeScreen'] = meta
function Stream:GetSkillShakeScreen()
	local o = self:GetSkillTimeline()
	setmetatable(o, Skill.ShakeScreen)
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
meta.class = 'Common.Vector2'
GetOrCreate('Common')['Vector2'] = meta
function Stream:GetCommonVector2()
	local o = {}
	setmetatable(o, Common.Vector2)
	o.X = self:GetFloat()
	o.Y = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Common.Vector3'
GetOrCreate('Common')['Vector3'] = meta
function Stream:GetCommonVector3()
	local o = {}
	setmetatable(o, Common.Vector3)
	o.X = self:GetFloat()
	o.Y = self:GetFloat()
	o.Z = self:GetFloat()
	return o
end
GetOrCreate('Character')['GroupType'] = {
	NULL = -9,
	None = 0,
	Base = 1,
	Player = 2,
	Monster = 3,
	NPC = 4,
}
GetOrCreate('Skill')['ActionSourceType'] = {
	NULL = -9,
	SelfModel = 0,
	OtherModel = 1,
}
GetOrCreate('Skill')['LockObjectType'] = {
	NULL = -9,
	None = 0,
	Enemy = 1,
	Teammate = 2,
	Self = 3,
	Other = 4,
}
GetOrCreate('Skill')['HitSharpType'] = {
	NULL = -9,
	Cube = 0,
	Sphere = 1,
	Cylinder = 2,
}
GetOrCreate('Skill')['MoveType'] = {
	NULL = -9,
	MoveToTarget = 0,
	MoveInDirection = 1,
}
GetOrCreate('Skill')['BindType'] = {
	NULL = -9,
	None = 0,
	Body = 1,
	Head = 2,
	Foot = 3,
}
GetOrCreate('Skill')['EffectAlignType'] = {
	NULL = -9,
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
	NULL = -9,
	Horizontal = 0,
	Vertical = 1,
	Mix = 2,
}
return Stream
