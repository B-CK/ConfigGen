set ddl=E:\__Github\ConfigGen\DDL\DDL\bin\Debug\DDL.exe
set mxml=E:\__Github\ConfigGen\DDL\Desc\bin\Debug\Module\Example.xml
set output=E:\__Github\ConfigGen\DDL\MPO\Package\
%ddl% -opt:module -i:%mxml% -o:%output% -lan:cs

pause