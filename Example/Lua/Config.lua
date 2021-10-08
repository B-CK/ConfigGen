---@field AllClass string
---@field CheckAll string
---@field Test string
---@field ClientConst string
---@class ConfigName
return {
	{ name = 'AllClass', method = 'GetCfgAllTypeAllClass', index = 'ID', output = 'alltype/allclass.data' },
	{ name = 'CheckAll', method = 'GetCfgAllTypeCheckAll', index = 'ID', output = 'alltype/checkall.data' },
	{ name = 'Test', method = 'GetCfgAllTypeTest', index = 'TID', output = 'alltype/test.data' },
	{ name = 'ClientConst', method = 'GetCfgAllTypeClientConst', index = 'TID', output = 'alltype/clientconst.data' },
}
