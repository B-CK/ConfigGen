local Stream = require("Cfg.DataStruct")
local cfgs = {}
for _, s in ipairs({
	{ name = 'AllClass', method = 'GetCfgAllTypeAllClass', index = 'ID', output = 'alltype/allclass.data' },
	{ name = 'Model', method = 'GetCfgCharacterModel', index = 'Name', output = 'character/model.data' },
	{ name = 'ActorConfig', method = 'GetCfgSkillActorConfig', index = 'ModelName', output = 'skill/actorconfig.data' },
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
