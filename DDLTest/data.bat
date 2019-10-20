set tool=E:\__Github\ConfigGen\DDL\Tool\bin\Debug\Tool.exe
set module=E:\__Github\ConfigGen\DDL\Desc\bin\Debug\Module\Example.xml
%tool% -opt:data -i:.\Excel\ -o:.\Data\ -m:%module% REM -group -check
REM 上述命令为Tool命令,后续移植到MPO代码中进行数据解析,然后再导出数据mpc
REM MPO命令重新设计

pause