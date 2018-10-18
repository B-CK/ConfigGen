local Stream = require("Cfg.DataStream")
local GetOrCreate = Util.GetOrCreate

local meta
meta= {}
meta.__index = meta
meta.class = 'Cfg.Vector2'
GetOrCreate('Cfg.')['Vector2'] = meta
function Stream:GetCfgVector2()
	local o = {}
	setmetatable(o, Cfg..Vector2)
	o.X = self:GetFloat()
	o.Y = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Vector3'
GetOrCreate('Cfg.')['Vector3'] = meta
function Stream:GetCfgVector3()
	local o = {}
	setmetatable(o, Cfg..Vector3)
	o.X = self:GetFloat()
	o.Y = self:GetFloat()
	o.Z = self:GetFloat()
	return o
end

return Stream
