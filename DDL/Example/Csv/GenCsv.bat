@set tool="../Tool.exe"

@set module="Cfg.xml"
@set data="..\data\csv"
@set lua="..\Lua"
@set csharp="..\CSharp"

%tool% -module %module% -data %data% -lua %lua% -csharp %csharp%

pause