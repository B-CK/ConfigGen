set ddl=E:\__Github\ConfigGen\DDL\DDL\bin\Debug\DDL.exe
set nsxml=E:\__Github\ConfigGen\DDL\Desc\bin\Debug\Namespace\Example.xml
set output=E:\__Github\ConfigGen\DDL\MPO\Package\
%ddl% -opt:namespace -i:%nsxml% -o:%output% -lan:cs

pause