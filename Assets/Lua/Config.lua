local Stream = require("Cfg.DataStream")
local cfgs = {}
for _, s in ipairs({
	{ name = 'CfgAllType.AllClass', index = 'ID', output = 'AllType/AllClass.xml' },
	{ name = 'CfgCard.Card', index = 'ID', output = 'Card/Card.xml' },
}) do
	local data = Stream.New(s.output)
	local method = 'Get' .. s.name
	while data.hasNext do
		cfg[s.index] = data[method](data)
	end
	cfgs[s.name] = cfg
end

return cfgs
