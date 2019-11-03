local Stream = require("Cfg.DataStream")
local GetOrCreate = Util.GetOrCreate

local meta
meta= {}
meta.__index = meta
meta.class = 'BaseTypesInfo0.BaseType'
GetOrCreate('BaseTypesInfo0')['BaseType'] = meta
function Stream:GetBaseTypesInfo0BaseType()
	local o = {}
	setmetatable(o, BaseTypesInfo0.BaseType)
	o.int_var = self:GetInt()
	o.long_var = self:GetLong()
	o.float_var = self:GetFloat()
	o.bool_var = self:GetBool()
	o.string_var = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'CustomTypes.Monster'
GetOrCreate('CustomTypes')['Monster'] = meta
function Stream:GetCustomTypesMonster()
	local o = self:GetCustomTypesCustom()
	setmetatable(o, CustomTypes.Monster)
	o.Attack = self:GetInt()
	o.ID = self:GetBool()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'CustomTypes.NPC'
GetOrCreate('CustomTypes')['NPC'] = meta
function Stream:GetCustomTypesNPC()
	local o = self:GetCustomTypesCustom()
	setmetatable(o, CustomTypes.NPC)
	o.Alias = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'CustomTypes.Partner'
GetOrCreate('CustomTypes')['Partner'] = meta
function Stream:GetCustomTypesPartner()
	local o = self:GetCustomTypesCustom()
	setmetatable(o, CustomTypes.Partner)
	o.Alias = self:GetString()
	o.Buff = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'CustomTypes.Custom'
GetOrCreate('CustomTypes')['Custom'] = meta
function Stream:GetCustomTypesCustomMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCustomTypesCustom()
	local o = {}
	setmetatable(o, CustomTypes.Custom)
	o.ID = self:GetInt()
	o.Name = self:GetString()
	o.Level = self:GetInt()
	o.Base = self:GetInt()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'SetTypes.ListSet'
GetOrCreate('SetTypes')['ListSet'] = meta
function Stream:GetSetTypesListSet()
	local o = {}
	setmetatable(o, SetTypes.ListSet)
	o.list_bool = self:GetList('Bool')
	o.list_int = self:GetList('Int')
	o.list_long = self:GetList('Long')
	o.list_float = self:GetList('Float')
	o.list_string = self:GetList('String')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'SetTypes.DictSet'
GetOrCreate('SetTypes')['DictSet'] = meta
function Stream:GetSetTypesDictSet()
	local o = {}
	setmetatable(o, SetTypes.DictSet)
	o.dict_int = self:GetDict('Int', 'Bool')
	o.dict_long = self:GetDict('Long', 'Bool')
	o.dict_string = self:GetDict('String', 'Bool')
	return o
end
GetOrCreate('CustomTypes')['BuffType'] = {
	NULL = -9,
	Attack = 1,
	Defense = 2,
	Speed = 3,
}
return Stream
