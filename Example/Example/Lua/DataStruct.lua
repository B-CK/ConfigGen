local Stream = require("Cfg.DataStream")
local GetOrCreate = Util.GetOrCreate

local meta
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.AllClass'
meta.ConstString = 'Hello World'
meta.ConstFloat = 3.141527
GetOrCreate('Cfg.AllType')['AllClass'] = meta
function Stream:GetCfgAllTypeAllClass()
	local o = {}
	setmetatable(o, Cfg.AllType.AllClass)
	o.ID = self:Getint()
	o.Index = self:Getint()
	o.VarLong = self:Getlong()
	o.VarFloat = self:Getfloat()
	o.VarString = self:Getstring()
	o.VarBool = self:Getbool()
	o.VarEnum = self:GetInt()
	o.VarClass = self:GetCfgAllTypeSingleClassMaker()
	o.VarListBase = self:GetList('string')
	o.VarListClass = self:GetList('CfgAllTypeSingleClassMaker')
	o.VarListCardElem = self:GetList('string')
	o.VarDictBase = self:GetDict('int', 'int')
	o.VarDictEnum = self:GetDict('long', 'long')
	o.VarDictClass = self:GetDict('string', 'string')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.SingleClass'
GetOrCreate('Cfg.AllType')['SingleClass'] = meta
function Stream:GetCfgAllTypeSingleClass()
	local o = {}
	setmetatable(o, Cfg.AllType.SingleClass)
	o.Var1 = self:Getstring()
	o.Var2 = self:Getfloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.M1'
GetOrCreate('Cfg.AllType')['M1'] = meta
function Stream:GetCfgAllTypeM1Maker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgAllTypeM1()
	local o = self:GetCfgAllTypeSingleClass()
	setmetatable(o, Cfg.AllType.M1)
	o.V3 = self:Getlong()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.M2'
GetOrCreate('Cfg.AllType')['M2'] = meta
function Stream:GetCfgAllTypeM2Maker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgAllTypeM2()
	local o = self:GetCfgAllTypeSingleClass()
	setmetatable(o, Cfg.AllType.M2)
	o.V4 = self:Getbool()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.Test'
GetOrCreate('Cfg.AllType')['Test'] = meta
function Stream:GetCfgAllTypeTest()
	local o = {}
	setmetatable(o, Cfg.AllType.Test)
	o.TID = self:Getint()
	o.Name = self:Getstring()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Vector2'
GetOrCreate('Cfg')['Vector2'] = meta
function Stream:GetCfgVector2()
	local o = {}
	setmetatable(o, Cfg.Vector2)
	o.X = self:Getfloat()
	o.Y = self:Getfloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Vector3'
GetOrCreate('Cfg')['Vector3'] = meta
function Stream:GetCfgVector3()
	local o = {}
	setmetatable(o, Cfg.Vector3)
	o.X = self:Getfloat()
	o.Y = self:Getfloat()
	o.Z = self:Getfloat()
	return o
end
return Stream
