local Stream = require("Common.DataStream")
local GetOrCreate = Util.GetOrCreate

local meta
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
	o.SkillActions = self:GetDict('String', 'CfgSkillSkillActionMaker')
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
	o.SequenceDict = self:GetDict('Int', 'CfgSkillSequenceMaker')
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
	o.Offset = self:GetCfgVector3Maker()
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
	o.Scale = self:GetCfgVector3Maker()
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
	o.Offset = self:GetCfgVector3Maker()
	o.EulerAngles = self:GetCfgVector3Maker()
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
	o.Offset = self:GetCfgVector3Maker()
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
	o.Position = self:GetCfgVector3Maker()
	o.EulerAngles = self:GetCfgVector3Maker()
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
	o.Position = self:GetCfgVector3Maker()
	o.EulerAngles = self:GetCfgVector3Maker()
	o.Scale = self:GetCfgVector3Maker()
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
meta.class = 'Cfg.Vector2'
GetOrCreate('Cfg')['Vector2'] = meta
function Stream:GetCfgVector2()
	local o = {}
	setmetatable(o, Cfg.Vector2)
	o.X = self:GetFloat()
	o.Y = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Vector3'
GetOrCreate('Cfg')['Vector3'] = meta
function Stream:GetCfgVector3()
	local o = {}
	setmetatable(o, Cfg.Vector3)
	o.X = self:GetFloat()
	o.Y = self:GetFloat()
	o.Z = self:GetFloat()
	return o
end
GetOrCreate('Cfg.Character')['GroupType'] = {
	NULL = -9,
}
GetOrCreate('Cfg.Skill')['ActionSourceType'] = {
	NULL = -9,
}
GetOrCreate('Cfg.Skill')['LockObjectType'] = {
	NULL = -9,
}
GetOrCreate('Cfg.Skill')['HitSharpType'] = {
	NULL = -9,
}
GetOrCreate('Cfg.Skill')['MoveType'] = {
	NULL = -9,
}
GetOrCreate('Cfg.Skill')['BindType'] = {
	NULL = -9,
}
GetOrCreate('Cfg.Skill')['EffectAlignType'] = {
	NULL = -9,
}
GetOrCreate('Cfg.Skill')['ShakeType'] = {
	NULL = -9,
}
return Stream
