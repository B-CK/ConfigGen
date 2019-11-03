set tool=E:/__Github/ConfigGen/ddl/Tool/bin/Debug/Tool.exe
REM %tool% -opt:module -i:./Desc/bin/Debug/Module/Example.xml -o:./Example -lan:cs

%tool% -opt:namespace -i:./Desc/bin/Debug/Namespace/CustomTypes.xml -o:./Example -lan:cs

pause