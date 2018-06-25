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
	o.ID = self:Getint()
	o.VarLong = self:Getint()
	o.VarFloat = self:Getfloat()
	o.VarString = self:Getstring()
	o.VarBool = self:Getbool()
	o.VarEnum = self:GetInt()
	o.VarClass = self:GetObject('Cfg.AllType.SingleClass')
	o.VarListBase = self:GetList('string')
	o.VarListClass = self:GetList('Cfg.AllType.SingleClass')
	o.VarListCardElem = self:GetList('string')
	o.VarDictBase = self:GetDict('int', 'string')
	o.VarDictEnum = self:GetDict('long', 'Int')
	o.VarDictClass = self:GetDict('string', 'Cfg.AllType.SingleClass')
	return o
end
meta= {}
meta.__index = meta
GetOrCreate('Cfg.Card')['Card'] = meta
function Stream:GetCfgCardCard()
	local o = {}
	setmetatable(o, Cfg.Card.Card)
	o.ID = self:Getint()
	o.Name = self:Getstring()
	o.CardType = self:GetInt()
	o.Cost = self:Getlong()
	o.Elements = self:GetDict('Int', 'long')
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

return Stream
