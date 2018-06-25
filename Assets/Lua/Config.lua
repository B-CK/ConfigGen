local Stream = require("Cfg.DataStruct")
local sub = string.sub
local find = string.find
local cfgs = {}
for _, s in ipairs({
	{ name = 'Card', method = 'GetCfgCardCard', index = 'ID', output = 'Card/Card.xml' },
	{ name = 'AllClass', method = 'GetCfgAllTypeAllClass', index = 'ID', output = 'AllType/AllClass.xml' },
}) do
	local data = Stream.New(s.output)
	local i, j = find(s.name,'%w+%.', 1)
	local method = 'Get' .. sub(s.name, i, j - 1)
	local cfg = nil
	while data.hasNext do
		cfg[s.index] = data[method](data)
	end
	cfgs[s.name] = cfg
end

return cfgs
