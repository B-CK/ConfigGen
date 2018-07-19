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

return Stream
