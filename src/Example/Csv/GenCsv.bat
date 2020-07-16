cd ..
@set tool="Tool.exe"

@set module=".\Csv\Cfg.xml"
@set data=".\data\csv"
@set binary=".\data\binary"
@set lua=".\Lua"
@set csharp=".\CSharp"

%tool% -module %module% -data %data% -lua %lua% -csharp %csharp% -binary %binary%

pause