local Stream = require("Cfg.DataStruct")
local cfgs = {}
for _, s in ipairs({
	{ name = 'Card', method = 'GetCfgCardCard', index = 'ID', output = 'Card/Card.data' },
	{ name = 'AllClass', method = 'GetCfgAllTypeAllClass', index = 'ID', output = 'AllType/AllClass.data' },
}) do
	local data = Stream.new(s.output)
	local cfg = {}
	while data:NextRow() do
		local value = data[s.method](data)
		local key = value[s.index]
		cfg[key] = value
	end
	cfgs[s.name] = cfg
end

return cfgs