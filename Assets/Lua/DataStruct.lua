local Stream = require("Cfg.DataStream")
local find = string.find
local sub = string.sub

local function GetOrCreate(namespace)
	local t = _G
	local idx = 1
	while true do
		local start, ends = find(namespace, '.', idx, true)
		local subname = sub(namespace, idx, start and start - 1)
		local subt = t[subname]
		if not subt then
			subt = {}
			t[subname] = subt
		end
		t = subt
		if start then
			idx = ends + 1
		else
			return t
		end
	end
end

local meta
meta= {}
meta.__index = meta
meta.ConstString = "Hello World"
meta.ConstFloat = 3.141527
GetOrCreate('Cfg.AllType')['AllClass'] = meta
function Stream:GetCfgAllTypeAllClass()
	local o = {}
	setmetatable(o, Cfg.AllType.AllClass)
	o.ID = self:GetInt()
	o.VarLong = self:GetInt()
	o.VarFloat = self:GetFloat()
	o.VarString = self:GetString()
	o.VarBool = self:GetBool()
	o.VarEnum = self:GetInt()
	o.VarClass = self:GetObject('CfgAllTypeSingleClass')
	o.VarListBase = self:GetList('String')
	o.VarListClass = self:GetList('CfgAllTypeSingleClass')
	o.VarListCardElem = self:GetList('String')
	o.VarDictBase = self:GetDict('Int', 'String')
	o.VarDictEnum = self:GetDict('Long', 'Int')
	o.VarDictClass = self:GetDict('String', 'CfgAllTypeSingleClass')
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.AllType')['SingleClass'] = meta
function Stream:GetCfgAllTypeSingleClass()
	local o = {}
	setmetatable(o, Cfg.AllType.SingleClass)
	o.Var1 = self:GetString()
	o.Var2 = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Card')['Card'] = meta
function Stream:GetCfgCardCard()
	local o = {}
	setmetatable(o, Cfg.Card.Card)
	o.ID = self:GetInt()
	o.Name = self:GetString()
	o.CardType = self:GetInt()
	o.Cost = self:GetLong()
	o.Elements = self:GetDict('Int', 'Long')
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['Action'] = meta
function Stream:GetCfgSkillAction()
	local o = {}
	setmetatable(o, Cfg.Skill.Action)
	o.Timeline = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['Attack'] = meta
function Stream:GetCfgSkillAttack()
	local o = {}
	setmetatable(o, Cfg.Skill.Attack)
	o.Id = self:GetInt()
	o.HitZoneId = self:GetInt()
	o.BeAttackEffectId = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['ShakeScreen'] = meta
function Stream:GetCfgSkillShakeScreen()
	local o = {}
	setmetatable(o, Cfg.Skill.ShakeScreen)
	o.Type = self:GetString()
	o.Frequency = self:GetInt()
	o.FrequencyDuration = self:GetFloat()
	o.FrequencyAtten = self:GetFloat()
	o.Amplitude = self:GetFloat()
	o.AmplitudeAtten = self:GetFloat()
	o.Life = self:GetFloat()
	o.MinRange = self:GetFloat()
	o.MaxRange = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['Movement'] = meta
function Stream:GetCfgSkillMovement()
	local o = {}
	setmetatable(o, Cfg.Skill.Movement)
	o.Type = self:GetInt()
	o.Duration = self:GetFloat()
	o.Speed = self:GetFloat()
	o.Acceleration = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['ParticleEffect'] = meta
function Stream:GetCfgSkillParticleEffect()
	local o = {}
	setmetatable(o, Cfg.Skill.ParticleEffect)
	o.Id = self:GetInt()
	o.Type = self:GetInt()
	o.FadeOutTime = self:GetFloat()
	o.Path = self:GetString()
	o.Life = self:GetFloat()
	o.FollowDirection = self:GetBool()
	o.FollowBeAttackedDirection = self:GetBool()
	o.Scale = self:GetFloat()
	o.CasterBindType = self:GetInt()
	o.FollowBoneDirection = self:GetBool()
	o.TargetBindType = self:GetInt()
	o.InstanceTraceType = self:GetInt()
	o.WorldOffsetX = self:GetFloat()
	o.WorldOffsetY = self:GetFloat()
	o.WorldOffsetZ = self:GetFloat()
	o.WorldRotateX = self:GetFloat()
	o.WorldRotateY = self:GetFloat()
	o.WorldRotateZ = self:GetFloat()
	o.BonePostionX = self:GetFloat()
	o.BonePostionY = self:GetFloat()
	o.BonePostionZ = self:GetFloat()
	o.BoneRotationX = self:GetFloat()
	o.BoneRotationY = self:GetFloat()
	o.BoneRotationZ = self:GetFloat()
	o.BoneScaleX = self:GetFloat()
	o.BoneScaleY = self:GetFloat()
	o.BoneScaleZ = self:GetFloat()
	o.BoneName = self:GetString()
	o.TraceTime = self:GetFloat()
	o.AlignType = self:GetInt()
	o.IsPoolDestroy = self:GetBool()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['SoundEffect'] = meta
function Stream:GetCfgSkillSoundEffect()
	local o = {}
	setmetatable(o, Cfg.Skill.SoundEffect)
	o.Probability = self:GetFloat()
	o.VolumeMin = self:GetFloat()
	o.VolumeMax = self:GetFloat()
	o.PathList = self:GetList('String')
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['SpawnObject'] = meta
function Stream:GetCfgSkillSpawnObject()
	local o = {}
	setmetatable(o, Cfg.Skill.SpawnObject)
	o.Id = self:GetInt()
	o.SpawnType = self:GetInt()
	o.Life = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.BODY_CORRECT = 0.7
meta.HEAD_CORRECT = 1.3
GetOrCreate('Cfg.Skill')['TraceObject'] = meta
function Stream:GetCfgSkillTraceObject()
	local o = {}
	setmetatable(o, Cfg.Skill.TraceObject)
	o.EffectId = self:GetInt()
	o.IsTraceTarget = self:GetBool()
	o.TraceCurveId = self:GetInt()
	o.OffsetX = self:GetFloat()
	o.OffsetY = self:GetFloat()
	o.OffsetZ = self:GetFloat()
	o.TraceType = self:GetInt()
	o.CasterBindType = self:GetInt()
	o.TargetBindType = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['FlyWeapon'] = meta
function Stream:GetCfgSkillFlyWeapon()
	local o = {}
	setmetatable(o, Cfg.Skill.FlyWeapon)
	o.BulletRadius = self:GetFloat()
	o.PassBody = self:GetBool()
	o.BeAttackEffectId = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['Bomb'] = meta
function Stream:GetCfgSkillBomb()
	local o = {}
	setmetatable(o, Cfg.Skill.Bomb)
	o.Id = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['ModelActions'] = meta
function Stream:GetCfgSkillModelActions()
	local o = {}
	setmetatable(o, Cfg.Skill.ModelActions)
	o.ModelName = self:GetString()
	o.BaseModelName = self:GetString()
	o.ModelActions = self:GetList('CfgSkillModelAction')
	o.SkillActions = self:GetList('CfgSkillSkillAction')
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['ModelAction'] = meta
function Stream:GetCfgSkillModelAction()
	local o = {}
	setmetatable(o, Cfg.Skill.ModelAction)
	o.ActionName = self:GetString()
	o.ActionSource = self:GetInt()
	o.OtherModelName = self:GetString()
	o.ActionFile = self:GetString()
	o.PreActionFile = self:GetString()
	o.PostActionFile = self:GetString()
	o.ActionSpeed = self:GetFloat()
	o.LoopTimes = self:GetInt()
	o.EffectId = self:GetInt()
	o.Actions = self:GetList('CfgSkillAction')
	o.Effects = self:GetList('CfgSkillEffectGroup')
	return o
end
meta= {}
meta.__index = meta
meta.EXPIRE_TIME = 1
GetOrCreate('Cfg.Skill')['SkillAction'] = meta
function Stream:GetCfgSkillSkillAction()
	local o = {}
	setmetatable(o, Cfg.Skill.SkillAction)
	o.SkillExpireTime = self:GetFloat()
	o.SkillEndTime = self:GetFloat()
	o.NeedTarget = self:GetBool()
	o.CanInterrupt = self:GetBool()
	o.SkillRange = self:GetFloat()
	o.CanShowSkillRange = self:GetBool()
	o.CanRotate = self:GetBool()
	o.CanMove = self:GetBool()
	o.StartMoveTime = self:GetFloat()
	o.EndMoveTime = self:GetFloat()
	o.RelateType = self:GetInt()
	o.HitPoints = self:GetList('CfgSkillHitPointGroup')
	o.HitZones = self:GetList('CfgSkillHitZone')
	o.BeAttackEffects = self:GetList('CfgSkillBeAttackEffect')
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['HitZone'] = meta
function Stream:GetCfgSkillHitZone()
	local o = {}
	setmetatable(o, Cfg.Skill.HitZone)
	o.Id = self:GetInt()
	o.Sharp = self:GetInt()
	o.Zoffset = self:GetFloat()
	o.Xlength = self:GetFloat()
	o.BottomHeight = self:GetFloat()
	o.TopHeight = self:GetFloat()
	o.Zlength = self:GetFloat()
	o.YAngle = self:GetFloat()
	o.YRotation = self:GetFloat()
	o.MaxTarget = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['BeAttackEffect'] = meta
function Stream:GetCfgSkillBeAttackEffect()
	local o = {}
	setmetatable(o, Cfg.Skill.BeAttackEffect)
	o.Id = self:GetInt()
	o.Curve = self:GetInt()
	o.DefencerAction = self:GetString()
	o.DefencerEffectId = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['EffectGroup'] = meta
function Stream:GetCfgSkillEffectGroup()
	local o = {}
	setmetatable(o, Cfg.Skill.EffectGroup)
	o.Id = self:GetInt()
	o.Name = self:GetString()
	o.Actions = self:GetList('CfgSkillAction')
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Skill')['HitPointGroup'] = meta
function Stream:GetCfgSkillHitPointGroup()
	local o = {}
	setmetatable(o, Cfg.Skill.HitPointGroup)
	o.Id = self:GetInt()
	o.Name = self:GetString()
	o.Attacks = self:GetList('CfgSkillAttack')
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.LsonAllClass')['LsonAllClass'] = meta
function Stream:GetCfgLsonAllClassLsonAllClass()
	local o = {}
	setmetatable(o, Cfg.LsonAllClass.LsonAllClass)
	o.ID = self:GetInt()
	o.VarLong = self:GetLong()
	o.VarFloat = self:GetFloat()
	o.VarString = self:GetString()
	o.VarBool = self:GetBool()
	o.VarEnum = self:GetInt()
	local _VarClass = 'Cfg' .. self:GetString()
	o.VarClass = self:GetObject(_VarClass)
	o.VarListBase = self:GetList('String')
	o.VarListClass = self:GetList('CfgLsonAllClassLSingleClass')
	o.VarListCardElem = self:GetList('Int')
	o.VarDictBase = self:GetDict('Int', 'String')
	o.VarDictEnum = self:GetDict('Long', 'Int')
	o.VarDictClass = self:GetDict('String', 'CfgLsonAllClassLSingleClass')
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.LsonAllClass')['LSingleClass'] = meta
function Stream:GetCfgLsonAllClassLSingleClass()
	local o = {}
	setmetatable(o, Cfg.LsonAllClass.LSingleClass)
	o.Var1 = self:GetString()
	o.Var2 = self:GetBool()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.LsonAllClass')['LInherit1'] = meta
function Stream:GetCfgLsonAllClassLInherit1()
	local o = {}
	setmetatable(o, Cfg.LsonAllClass.LInherit1)
	o.Var3 = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.LsonAllClass')['LInherit2'] = meta
function Stream:GetCfgLsonAllClassLInherit2()
	local o = {}
	setmetatable(o, Cfg.LsonAllClass.LInherit2)
	o.Var4 = self:GetLong()
	return o
end
GetOrCreate('Cfg.AllType')['CommondEnum'] = {
	NULL = -1,
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
GetOrCreate('Cfg.Card')['CardElement'] = {
	NULL = -1,
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
GetOrCreate('Cfg.Card')['CardType'] = {
	NULL = -1,
	Attack = 0,
}
GetOrCreate('Cfg.Skill')['ActionSourceType'] = {
	NULL = -1,
	SelfModel = 0,
	OtherModel = 1,
}
GetOrCreate('Cfg.Skill')['SkillTargetType'] = {
	NULL = -1,
	Enemy = 0,
	Teammate = 1,
	Self = 2,
}
GetOrCreate('Cfg.Skill')['SkillRelateType'] = {
	NULL = -1,
	Self = 0,
	Target = 1,
}
GetOrCreate('Cfg.Skill')['HitSharpType'] = {
	NULL = -1,
	Cube = 0,
	Cylinder = 1,
	Trangle = 2,
}
GetOrCreate('Cfg.Skill')['ShakeType'] = {
	NULL = -1,
	Horizontal = 0,
	Vertical = 1,
	Mix = 2,
}
GetOrCreate('Cfg.Skill')['MoveType'] = {
	NULL = -1,
	MoveBack = 0,
	MoveToTarget = 1,
	MoveInDirection = 2,
}
GetOrCreate('Cfg.Skill')['EffectType'] = {
	NULL = -1,
	Stand = 0,
	Follow = 1,
	Trace = 2,
	TracePos = 3,
	BindToCamera = 4,
	UIStand = 5,
}
GetOrCreate('Cfg.Skill')['EffectAlignType'] = {
	NULL = -1,
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
GetOrCreate('Cfg.Skill')['SpawnType'] = {
	NULL = -1,
	FlyWeapon = 0,
	Bomb = 1,
	Object = 2,
}
GetOrCreate('Cfg.Skill')['TraceType'] = {
	NULL = -1,
	Fly = 0,
	Fixed = 1,
}
GetOrCreate('Cfg.Skill')['TraceBindType'] = {
	NULL = -1,
	Body = 0,
	Head = 1,
	Foot = 2,
}

return Stream
