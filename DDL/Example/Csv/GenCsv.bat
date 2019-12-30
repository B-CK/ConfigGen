@set tool="../Tool.exe"
@set module="Csv\Cfg.xml"
@set data="GamePlayer\Data\config\csv"
@set lua="Code\Scripts\Cfg"

%tool% -module %module% -data %data% -lua %lua%

pause