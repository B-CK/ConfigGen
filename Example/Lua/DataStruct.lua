local Stream = require("Common.DataStream")
local GetOrCreate = Util.GetOrCreate

local meta
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.Ability'
GetOrCreate('AbilitySystem.Ability')['Ability'] = meta
function Stream:GetAbilitySystemAbilityAbility()
	local o = {}
	setmetatable(o, AbilitySystem.Ability.Ability)
	o.name = self:GetString()
	o.baseClass = self:GetString()
	o.behaviorType = self:GetInt()
	o.duration = self:GetFloat()
	o.args = self:GetList('AbilitySystemAbilitySharedArgMaker')
	o.assets = self:GetList('AbilitySystemAbilitySharedString')
	o.events = self:GetList('AbilitySystemAbilityAbilityEvent')
	o.modifiers = self:GetList('AbilitySystemAbilityModifierMaker')
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.SharedArg'
GetOrCreate('AbilitySystem.Ability')['SharedArg'] = meta
function Stream:GetAbilitySystemAbilitySharedArgMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetAbilitySystemAbilitySharedArg()
	local o = {}
	setmetatable(o, AbilitySystem.Ability.SharedArg)
	o.name = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.SharedInt'
GetOrCreate('AbilitySystem.Ability')['SharedInt'] = meta
function Stream:GetAbilitySystemAbilitySharedInt()
	local o = self:GetAbilitySystemAbilitySharedArg()
	setmetatable(o, AbilitySystem.Ability.SharedInt)
	o.value = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.SharedLong'
GetOrCreate('AbilitySystem.Ability')['SharedLong'] = meta
function Stream:GetAbilitySystemAbilitySharedLong()
	local o = self:GetAbilitySystemAbilitySharedArg()
	setmetatable(o, AbilitySystem.Ability.SharedLong)
	o.value = self:GetLong()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.SharedFloat'
GetOrCreate('AbilitySystem.Ability')['SharedFloat'] = meta
function Stream:GetAbilitySystemAbilitySharedFloat()
	local o = self:GetAbilitySystemAbilitySharedArg()
	setmetatable(o, AbilitySystem.Ability.SharedFloat)
	o.value = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.SharedString'
GetOrCreate('AbilitySystem.Ability')['SharedString'] = meta
function Stream:GetAbilitySystemAbilitySharedString()
	local o = self:GetAbilitySystemAbilitySharedArg()
	setmetatable(o, AbilitySystem.Ability.SharedString)
	o.value = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.SharedBool'
GetOrCreate('AbilitySystem.Ability')['SharedBool'] = meta
function Stream:GetAbilitySystemAbilitySharedBool()
	local o = self:GetAbilitySystemAbilitySharedArg()
	setmetatable(o, AbilitySystem.Ability.SharedBool)
	o.value = self:GetBool()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.TargetFilter'
GetOrCreate('AbilitySystem.Ability')['TargetFilter'] = meta
function Stream:GetAbilitySystemAbilityTargetFilterMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetAbilitySystemAbilityTargetFilter()
	local o = {}
	setmetatable(o, AbilitySystem.Ability.TargetFilter)
	o.type = self:GetInt()
	o.teams = self:GetInt()
	o.flags = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.MultiTargetFilter'
GetOrCreate('AbilitySystem.Ability')['MultiTargetFilter'] = meta
function Stream:GetAbilitySystemAbilityMultiTargetFilter()
	local o = self:GetAbilitySystemAbilityTargetFilter()
	setmetatable(o, AbilitySystem.Ability.MultiTargetFilter)
	o.radius = self:GetFloat()
	o.maxTargets = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.Modifier'
GetOrCreate('AbilitySystem.Ability')['Modifier'] = meta
function Stream:GetAbilitySystemAbilityModifierMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetAbilitySystemAbilityModifier()
	local o = {}
	setmetatable(o, AbilitySystem.Ability.Modifier)
	o.name = self:GetString()
	o.events = self:GetList('AbilitySystemAbilityModifierEvent')
	o.properties = self:GetList('AbilitySystemAbilityProperty')
	o.states = self:GetList('AbilitySystemAbilityState')
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.BuffModifier'
GetOrCreate('AbilitySystem.Ability')['BuffModifier'] = meta
function Stream:GetAbilitySystemAbilityBuffModifier()
	local o = self:GetAbilitySystemAbilityModifier()
	setmetatable(o, AbilitySystem.Ability.BuffModifier)
	o.isBuff = self:GetBool()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.Property'
GetOrCreate('AbilitySystem.Ability')['Property'] = meta
function Stream:GetAbilitySystemAbilityProperty()
	local o = {}
	setmetatable(o, AbilitySystem.Ability.Property)
	o.type = self:GetInt()
	o.value = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.State'
GetOrCreate('AbilitySystem.Ability')['State'] = meta
function Stream:GetAbilitySystemAbilityState()
	local o = {}
	setmetatable(o, AbilitySystem.Ability.State)
	o.type = self:GetInt()
	o.value = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.EventBase'
GetOrCreate('AbilitySystem.Ability')['EventBase'] = meta
function Stream:GetAbilitySystemAbilityEventBaseMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetAbilitySystemAbilityEventBase()
	local o = {}
	setmetatable(o, AbilitySystem.Ability.EventBase)
	o.name = self:GetString()
	o.actions = self:GetList('AbilitySystemAbilityActionMaker')
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.AbilityEvent'
GetOrCreate('AbilitySystem.Ability')['AbilityEvent'] = meta
function Stream:GetAbilitySystemAbilityAbilityEvent()
	local o = self:GetAbilitySystemAbilityEventBase()
	setmetatable(o, AbilitySystem.Ability.AbilityEvent)
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.ModifierEvent'
GetOrCreate('AbilitySystem.Ability')['ModifierEvent'] = meta
function Stream:GetAbilitySystemAbilityModifierEvent()
	local o = self:GetAbilitySystemAbilityEventBase()
	setmetatable(o, AbilitySystem.Ability.ModifierEvent)
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.Action'
GetOrCreate('AbilitySystem.Ability')['Action'] = meta
function Stream:GetAbilitySystemAbilityActionMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetAbilitySystemAbilityAction()
	local o = {}
	setmetatable(o, AbilitySystem.Ability.Action)
	o.name = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.ActionWithTarget'
GetOrCreate('AbilitySystem.Ability')['ActionWithTarget'] = meta
function Stream:GetAbilitySystemAbilityActionWithTargetMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetAbilitySystemAbilityActionWithTarget()
	local o = self:GetAbilitySystemAbilityAction()
	setmetatable(o, AbilitySystem.Ability.ActionWithTarget)
	o.target = self:GetAbilitySystemAbilityTargetFilterMaker()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.ApplyModifier'
GetOrCreate('AbilitySystem.Ability')['ApplyModifier'] = meta
function Stream:GetAbilitySystemAbilityApplyModifier()
	local o = self:GetAbilitySystemAbilityActionWithTarget()
	setmetatable(o, AbilitySystem.Ability.ApplyModifier)
	o.modifier = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.PlayAnimation'
GetOrCreate('AbilitySystem.Ability')['PlayAnimation'] = meta
function Stream:GetAbilitySystemAbilityPlayAnimation()
	local o = self:GetAbilitySystemAbilityActionWithTarget()
	setmetatable(o, AbilitySystem.Ability.PlayAnimation)
	o.clip = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.PlaySound'
GetOrCreate('AbilitySystem.Ability')['PlaySound'] = meta
function Stream:GetAbilitySystemAbilityPlaySound()
	local o = self:GetAbilitySystemAbilityActionWithTarget()
	setmetatable(o, AbilitySystem.Ability.PlaySound)
	o.sound = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.PlayEffect'
GetOrCreate('AbilitySystem.Ability')['PlayEffect'] = meta
function Stream:GetAbilitySystemAbilityPlayEffect()
	local o = self:GetAbilitySystemAbilityActionWithTarget()
	setmetatable(o, AbilitySystem.Ability.PlayEffect)
	o.effectName = self:GetString()
	o.attachType = self:GetInt()
	o.point = self:GetAbilitySystemMathVector3()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.AttachEffect'
GetOrCreate('AbilitySystem.Ability')['AttachEffect'] = meta
function Stream:GetAbilitySystemAbilityAttachEffect()
	local o = self:GetAbilitySystemAbilityActionWithTarget()
	setmetatable(o, AbilitySystem.Ability.AttachEffect)
	o.effectName = self:GetString()
	o.attachType = self:GetInt()
	o.point = self:GetAbilitySystemMathVector3()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.LinearProjectile'
GetOrCreate('AbilitySystem.Ability')['LinearProjectile'] = meta
function Stream:GetAbilitySystemAbilityLinearProjectile()
	local o = self:GetAbilitySystemAbilityAction()
	setmetatable(o, AbilitySystem.Ability.LinearProjectile)
	o.effectName = self:GetString()
	o.moveSpeed = self:GetFloat()
	o.distance = self:GetFloat()
	o.endTarget = self:GetAbilitySystemAbilityTargetFilterMaker()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.DelayedAction'
GetOrCreate('AbilitySystem.Ability')['DelayedAction'] = meta
function Stream:GetAbilitySystemAbilityDelayedAction()
	local o = self:GetAbilitySystemAbilityAction()
	setmetatable(o, AbilitySystem.Ability.DelayedAction)
	o.delay = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.TrackingProjectile'
GetOrCreate('AbilitySystem.Ability')['TrackingProjectile'] = meta
function Stream:GetAbilitySystemAbilityTrackingProjectile()
	local o = self:GetAbilitySystemAbilityActionWithTarget()
	setmetatable(o, AbilitySystem.Ability.TrackingProjectile)
	o.effectName = self:GetString()
	o.MoveSpeed = self:GetFloat()
	o.TrackNode = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.RemoveModifier'
GetOrCreate('AbilitySystem.Ability')['RemoveModifier'] = meta
function Stream:GetAbilitySystemAbilityRemoveModifier()
	local o = self:GetAbilitySystemAbilityActionWithTarget()
	setmetatable(o, AbilitySystem.Ability.RemoveModifier)
	o.Modifier = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.Damage'
GetOrCreate('AbilitySystem.Ability')['Damage'] = meta
function Stream:GetAbilitySystemAbilityDamage()
	local o = self:GetAbilitySystemAbilityActionWithTarget()
	setmetatable(o, AbilitySystem.Ability.Damage)
	o.Type = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Ability.ActOnTargets'
GetOrCreate('AbilitySystem.Ability')['ActOnTargets'] = meta
function Stream:GetAbilitySystemAbilityActOnTargets()
	local o = self:GetAbilitySystemAbilityActionWithTarget()
	setmetatable(o, AbilitySystem.Ability.ActOnTargets)
	o.Actions = self:GetList('AbilitySystemAbilityActionMaker')
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Math.Vector2'
GetOrCreate('AbilitySystem.Math')['Vector2'] = meta
function Stream:GetAbilitySystemMathVector2()
	local o = {}
	setmetatable(o, AbilitySystem.Math.Vector2)
	o.x = self:GetFloat()
	o.y = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.__cname = 'AbilitySystem.Math.Vector3'
GetOrCreate('AbilitySystem.Math')['Vector3'] = meta
function Stream:GetAbilitySystemMathVector3()
	local o = {}
	setmetatable(o, AbilitySystem.Math.Vector3)
	o.x = self:GetFloat()
	o.y = self:GetFloat()
	o.z = self:GetFloat()
	return o
end
GetOrCreate('AbilitySystem.Ability')['BehaviorType'] = {
	BEHAVIOR_PASSIVE = 1,
	BEHAVIOR_TARGET = 4,
	BEHAVIOR_NO_TARGET = 8,
	BEHAVIOR_CHANNELLED = 16,
	BEHAVIOR_POINT = 32,
	BEHAVIOR_AOE = 64,
}
GetOrCreate('AbilitySystem.Ability')['TargetType'] = {
	SEFL = 0,
	TARGET = 1,
	POINT = 2,
	UNIT = 3,
	PROJECTILE = 4,
}
GetOrCreate('AbilitySystem.Ability')['UnitTeam'] = {
	TEAM_EVERYTHING = -1,
	TEAM_NONE = 0,
	TEAM_ENEMY = 1,
	TEAM_FRIENDLY = 2,
}
GetOrCreate('AbilitySystem.Ability')['UnitFlag'] = {
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
GetOrCreate('AbilitySystem.Ability')['StateValue'] = {
	MODIFIER_STATE_VALUE_NONE = 0,
	MODIFIER_STATE_VALUE_ENABLE = 1,
	MODIFIER_STATE_VALUE_DISABLE = 2,
}
GetOrCreate('AbilitySystem.Ability')['PropertyType'] = {
	MODIFIER_PROP_NONE  = 0,
	MODIFIER_PROP_MANA_BONUS  = 1,
	MODIFIER_PROP_HEALTH_BONUS = 2,
}
GetOrCreate('AbilitySystem.Ability')['StateType'] = {
	MODIFIER_STATE_NONE  = 0,
	MODIFIER_STATE_INVISIBLE  = 1,
	MODIFIER_STATE_INVULNERABLE  = 2,
}
GetOrCreate('AbilitySystem.Ability')['AttachType'] = {
	NONE = 0,
	HEAD = 1,
	LEFT_HAND = 2,
	RIGHT_HAND = 3,
	FOOT = 4,
	CENTER = 5,
	CHEST = 6,
}
return Stream
