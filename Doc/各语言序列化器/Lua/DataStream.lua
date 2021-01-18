local error = error
local lines = io.lines
local require = require
local gsub = string.gsub
local tostring = tostring
local lower = string.lower
local tonumber = tonumber
local setmetatable = setmetatable

local Stream = {}
Stream.__index = Stream

if ConstSetting.IsBinaryCfg then
	local msg = require("Common.msgpack")
	function Stream.new(file)
		local o = {}
		setmetatable(o, Stream)
        local f = assert(io.open(file, 'r'))
        o.data = f:read("*all")
        f:close()
		o.position = 1
		return o
	end
	---@return any 仅返回基础类型
	function Stream:GetNext()
		local value, pos = msg.decode_one(self.data, self.position)
		self.position = pos
		return value
	end
	---@return any 返回Array/Map长度
	function Stream:GetLength()
		local value, pos = msg.decode_length(self.data, self.position)
		self.position = pos
		return value
	end

	function Stream:GetBool() return self:GetNext() end
	function Stream:GetInt() return self:GetNext() end
	function Stream:GetLong() return self:GetNext() end
	function Stream:GetFloat() 	return self:GetNext() end
	function Stream:GetString() return self:GetNext() end
	function Stream:GetList(type)
		local result = {}
		local method = self['Get' .. type]
		local length = self:GetLength()
		for i = 1, length do
			result[i] = method(self)
		end
		return result
	end
	function Stream:GetDict(key,value)
		local result = {}
		local optKey = self['Get' .. key]
		local optValue = self['Get' .. value]
		local length = self:GetLength()
		for i = 1, length do
			result[optKey(self)] = optValue(self)
		end
		return result
	end
else
	function Stream.new(file)
		local o = {}
		setmetatable(o, Stream)
		o.dataIter = lines(file)
		return o
	end
	function Stream:Close()
		while self.dataIter() do
		end
	end
	function Stream:GetNext()
		local next = self.dataIter()
		if not next then
			error('Unable to read!')
		end
		return next
	end
	function Stream:GetBool()
		local next = self:GetNext()
		if next == "true" then
			return true
		elseif next == "false" then
			return false
		else
			error(tostring(next) .. " isn't bool! ")
		end
	end
	function Stream:GetInt()
		local next = self:GetNext()
		return tonumber(next)
	end
	function Stream:GetLong()
		local next = self:GetNext()
		return tonumber(next)
	end
	function Stream:GetFloat()
		local next = self:GetNext()
		return tonumber(next)
	end
	function Stream:GetString()
		local next = self:GetNext()
		return next
	end
	function Stream:GetList(type)
		local result = {}
		local method = self['Get' .. type]
		local length = self:GetInt()
		for i = 1, length do
			result[i] = method(self)
		end
		return result
	end
	function Stream:GetDict(key,value)
		local result = {}
		local optKey = self['Get' .. key]
		local optValue = self['Get' .. value]
		local length = self:GetInt()
		for i = 1, length do
			result[optKey(self)] = optValue(self)
		end
		return result
	end
	function Stream:GetLength()
		return self:GetLong()
	end
end



return Stream
