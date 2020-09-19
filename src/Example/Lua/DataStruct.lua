local Stream = require("Common.DataStream")
local GetOrCreate = Util.GetOrCreate

local meta
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.Ability'
GetOrCreate('Cfg.Ability')['Ability'] = meta
function Stream:GetCfgAbilityAbility()
	local o = {}
	setmetatable(o, Cfg.Ability.Ability)
	o.name = self:GetString()
	o.baseClass = self:GetString()
	o.behaviorType = self:GetInt()
	o.duration = self:GetFloat()
	o.args = self:GetList('CfgAbilitySpecialArgMaker')
	o.assets = self:GetList('CfgAbilitySpecialString')
	o.events = self:GetList('CfgAbilityAbilityEvent')
	o.modifiers = self:GetList('CfgAbilityModifierMaker')
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.EventBase'
GetOrCreate('Cfg.Ability')['EventBase'] = meta
function Stream:GetCfgAbilityEventBaseMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgAbilityEventBase()
	local o = {}
	setmetatable(o, Cfg.Ability.EventBase)
	o.name = self:GetString()
	o.actions = self:GetList('CfgAbilityActionMaker')
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.AbilityEvent'
GetOrCreate('Cfg.Ability')['AbilityEvent'] = meta
function Stream:GetCfgAbilityAbilityEvent()
	local o = self:GetCfgAbilityEventBase()
	setmetatable(o, Cfg.Ability.AbilityEvent)
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.ModifierEvent'
GetOrCreate('Cfg.Ability')['ModifierEvent'] = meta
function Stream:GetCfgAbilityModifierEvent()
	local o = self:GetCfgAbilityEventBase()
	setmetatable(o, Cfg.Ability.ModifierEvent)
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.SpecialArg'
GetOrCreate('Cfg.Ability')['SpecialArg'] = meta
function Stream:GetCfgAbilitySpecialArgMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgAbilitySpecialArg()
	local o = {}
	setmetatable(o, Cfg.Ability.SpecialArg)
	o.name = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.SpecialInt'
GetOrCreate('Cfg.Ability')['SpecialInt'] = meta
function Stream:GetCfgAbilitySpecialInt()
	local o = self:GetCfgAbilitySpecialArg()
	setmetatable(o, Cfg.Ability.SpecialInt)
	o.value = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.SpecialLong'
GetOrCreate('Cfg.Ability')['SpecialLong'] = meta
function Stream:GetCfgAbilitySpecialLong()
	local o = self:GetCfgAbilitySpecialArg()
	setmetatable(o, Cfg.Ability.SpecialLong)
	o.value = self:GetLong()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.SpecialFloat'
GetOrCreate('Cfg.Ability')['SpecialFloat'] = meta
function Stream:GetCfgAbilitySpecialFloat()
	local o = self:GetCfgAbilitySpecialArg()
	setmetatable(o, Cfg.Ability.SpecialFloat)
	o.value = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.SpecialString'
GetOrCreate('Cfg.Ability')['SpecialString'] = meta
function Stream:GetCfgAbilitySpecialString()
	local o = self:GetCfgAbilitySpecialArg()
	setmetatable(o, Cfg.Ability.SpecialString)
	o.value = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.SpecialBool'
GetOrCreate('Cfg.Ability')['SpecialBool'] = meta
function Stream:GetCfgAbilitySpecialBool()
	local o = self:GetCfgAbilitySpecialArg()
	setmetatable(o, Cfg.Ability.SpecialBool)
	o.value = self:GetBool()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.Target'
GetOrCreate('Cfg.Ability')['Target'] = meta
function Stream:GetCfgAbilityTargetMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgAbilityTarget()
	local o = {}
	setmetatable(o, Cfg.Ability.Target)
	o.type = self:GetInt()
	o.teams = self:GetInt()
	o.flags = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.MultiTarget'
GetOrCreate('Cfg.Ability')['MultiTarget'] = meta
function Stream:GetCfgAbilityMultiTarget()
	local o = self:GetCfgAbilityTarget()
	setmetatable(o, Cfg.Ability.MultiTarget)
	o.radius = self:GetFloat()
	o.maxTargets = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.Action'
GetOrCreate('Cfg.Ability')['Action'] = meta
function Stream:GetCfgAbilityActionMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgAbilityAction()
	local o = {}
	setmetatable(o, Cfg.Ability.Action)
	o.name = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.ActionWithTarget'
GetOrCreate('Cfg.Ability')['ActionWithTarget'] = meta
function Stream:GetCfgAbilityActionWithTargetMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgAbilityActionWithTarget()
	local o = self:GetCfgAbilityAction()
	setmetatable(o, Cfg.Ability.ActionWithTarget)
	o.target = self:GetCfgAbilityTargetMaker()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.ApplyModifier'
GetOrCreate('Cfg.Ability')['ApplyModifier'] = meta
function Stream:GetCfgAbilityApplyModifier()
	local o = self:GetCfgAbilityActionWithTarget()
	setmetatable(o, Cfg.Ability.ApplyModifier)
	o.modifier = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.PlayAnimation'
GetOrCreate('Cfg.Ability')['PlayAnimation'] = meta
function Stream:GetCfgAbilityPlayAnimation()
	local o = self:GetCfgAbilityActionWithTarget()
	setmetatable(o, Cfg.Ability.PlayAnimation)
	o.clip = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.PlaySound'
GetOrCreate('Cfg.Ability')['PlaySound'] = meta
function Stream:GetCfgAbilityPlaySound()
	local o = self:GetCfgAbilityActionWithTarget()
	setmetatable(o, Cfg.Ability.PlaySound)
	o.sound = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.PlayEffect'
GetOrCreate('Cfg.Ability')['PlayEffect'] = meta
function Stream:GetCfgAbilityPlayEffect()
	local o = self:GetCfgAbilityActionWithTarget()
	setmetatable(o, Cfg.Ability.PlayEffect)
	o.effectName = self:GetString()
	o.attachType = self:GetInt()
	o.point = self:GetCfgMathVector3()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.AttachEffect'
GetOrCreate('Cfg.Ability')['AttachEffect'] = meta
function Stream:GetCfgAbilityAttachEffect()
	local o = self:GetCfgAbilityActionWithTarget()
	setmetatable(o, Cfg.Ability.AttachEffect)
	o.effectName = self:GetString()
	o.attachType = self:GetInt()
	o.point = self:GetCfgMathVector3()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.LinearProjectile'
GetOrCreate('Cfg.Ability')['LinearProjectile'] = meta
function Stream:GetCfgAbilityLinearProjectile()
	local o = self:GetCfgAbilityAction()
	setmetatable(o, Cfg.Ability.LinearProjectile)
	o.effectName = self:GetString()
	o.moveSpeed = self:GetFloat()
	o.distance = self:GetFloat()
	o.endTarget = self:GetCfgAbilityTargetMaker()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.DelayedAction'
GetOrCreate('Cfg.Ability')['DelayedAction'] = meta
function Stream:GetCfgAbilityDelayedAction()
	local o = self:GetCfgAbilityAction()
	setmetatable(o, Cfg.Ability.DelayedAction)
	o.delay = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.TrackingProjectile'
GetOrCreate('Cfg.Ability')['TrackingProjectile'] = meta
function Stream:GetCfgAbilityTrackingProjectile()
	local o = self:GetCfgAbilityActionWithTarget()
	setmetatable(o, Cfg.Ability.TrackingProjectile)
	o.effectName = self:GetString()
	o.MoveSpeed = self:GetFloat()
	o.TrackNode = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.RemoveModifier'
GetOrCreate('Cfg.Ability')['RemoveModifier'] = meta
function Stream:GetCfgAbilityRemoveModifier()
	local o = self:GetCfgAbilityActionWithTarget()
	setmetatable(o, Cfg.Ability.RemoveModifier)
	o.Modifier = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.Damage'
GetOrCreate('Cfg.Ability')['Damage'] = meta
function Stream:GetCfgAbilityDamage()
	local o = self:GetCfgAbilityActionWithTarget()
	setmetatable(o, Cfg.Ability.Damage)
	o.Type = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.ActOnTargets'
GetOrCreate('Cfg.Ability')['ActOnTargets'] = meta
function Stream:GetCfgAbilityActOnTargets()
	local o = self:GetCfgAbilityActionWithTarget()
	setmetatable(o, Cfg.Ability.ActOnTargets)
	o.Actions = self:GetList('CfgAbilityActionMaker')
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.Modifier'
GetOrCreate('Cfg.Ability')['Modifier'] = meta
function Stream:GetCfgAbilityModifierMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgAbilityModifier()
	local o = {}
	setmetatable(o, Cfg.Ability.Modifier)
	o.name = self:GetString()
	o.events = self:GetList('CfgAbilityModifierEvent')
	o.properties = self:GetList('CfgAbilityProperty')
	o.states = self:GetList('CfgAbilityState')
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.BuffModifier'
GetOrCreate('Cfg.Ability')['BuffModifier'] = meta
function Stream:GetCfgAbilityBuffModifier()
	local o = self:GetCfgAbilityModifier()
	setmetatable(o, Cfg.Ability.BuffModifier)
	o.isBuff = self:GetBool()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.Property'
GetOrCreate('Cfg.Ability')['Property'] = meta
function Stream:GetCfgAbilityProperty()
	local o = {}
	setmetatable(o, Cfg.Ability.Property)
	o.type = self:GetInt()
	o.value = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Ability.State'
GetOrCreate('Cfg.Ability')['State'] = meta
function Stream:GetCfgAbilityState()
	local o = {}
	setmetatable(o, Cfg.Ability.State)
	o.type = self:GetInt()
	o.value = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Math.Vector2'
GetOrCreate('Cfg.Math')['Vector2'] = meta
function Stream:GetCfgMathVector2()
	local o = {}
	setmetatable(o, Cfg.Math.Vector2)
	o.x = self:GetFloat()
	o.y = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'Cfg.Math.Vector3'
GetOrCreate('Cfg.Math')['Vector3'] = meta
function Stream:GetCfgMathVector3()
	local o = {}
	setmetatable(o, Cfg.Math.Vector3)
	o.x = self:GetFloat()
	o.y = self:GetFloat()
	o.z = self:GetFloat()
	return o
end
GetOrCreate('Ability')['BehaviorType'] = {
	BEHAVIOR_PASSIVE = 1,
	BEHAVIOR_TARGET = 4,
	BEHAVIOR_NO_TARGET = 8,
	BEHAVIOR_CHANNELLED = 16,
	BEHAVIOR_POINT = 32,
	BEHAVIOR_AOE = 64,
}
GetOrCreate('Ability')['TargetType'] = {
	SEFL = 0,
	TARGET = 1,
	POINT = 2,
	UNIT = 3,
	PROJECTILE = 4,
}
GetOrCreate('Ability')['UnitTeam'] = {
	TEAM_EVERYTHING = -1,
	TEAM_NONE = 0,
	TEAM_ENEMY = 1,
	TEAM_FRIENDLY = 2,
}
GetOrCreate('Ability')['UnitFlag'] = {
	FLAG_NONE = 0,
	FLAG_DEAD = 1,
	FLAG_INVULNERABLE = 2,
	FLAG_NO_INVIS = 4,
	FLAG_NOT_ATTACK_IMMUNE = 8,
	FLAG_NOT_CREEP_HERO = 16,
	FLAG_NOT_DOMINATED = 32,
	FLAG_NOT_SUMMONED = 64,
	FLAG_PLAYER_CONTROLLED = 128,
}
GetOrCreate('Ability')['AttachType'] = {
	NONE = 0,
	HEAD = 1,
	LEFT_HAND = 2,
	RIGHT_HAND = 3,
	FOOT = 4,
	CENTER = 5,
	CHEST = 6,
}
GetOrCreate('Ability')['StateValue'] = {
	MODIFIER_STATE_VALUE_NONE = 0,
	MODIFIER_STATE_VALUE_ENABLE = 1,
	MODIFIER_STATE_VALUE_DISABLE = 2,
}
GetOrCreate('Ability')['PropertyType'] = {
	MODIFIER_PROP_NONE  = 0,
	MODIFIER_PROP_MANA_BONUS  = 1,
	MODIFIER_PROP_HEALTH_BONUS = 2,
}
GetOrCreate('Ability')['StateType'] = {
	MODIFIER_STATE_NONE  = 0,
	MODIFIER_STATE_INVISIBLE  = 1,
	MODIFIER_STATE_INVULNERABLE  = 2,
}
return Stream
