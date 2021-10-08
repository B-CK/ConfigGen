cd ..
@set tool="Tool.exe"

@set module=".\Excel\Cfg.xml"
@set data=".\data\csv"
@set binary=".\data\binary" REM -binary %binary%
@set lua=".\Lua"
@set csharp=".\CSharp"

%tool% -module %module% -data %data% -lua %lua% -csharp %csharp% 

pause