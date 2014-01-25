@ECHO OFF
SETLOCAL

ECHO starting with:
dir nuget\. /B
ECHO.

del nuget\. /Q
ECHO.

SET NUGET=src\.nuget\nuget.exe
SET OP=src\ObjectPrinter\ObjectPrinter.csproj
SET L4J10=src\ObjectPrinter.Log4Net.v1210\ObjectPrinter.Log4Net.v1210.csproj
SET L4J11=src\ObjectPrinter.Log4Net.v1211\ObjectPrinter.Log4Net.v1211.csproj
SET L4J12=src\ObjectPrinter.Log4Net.v1212\ObjectPrinter.Log4Net.v1212.csproj

%NUGET% pack %OP% -OutputDirectory nuget -Prop Configuration=Release
%NUGET% pack %L4J10% -OutputDirectory nuget -Prop Configuration=Release
%NUGET% pack %L4J11% -OutputDirectory nuget -Prop Configuration=Release
%NUGET% pack %L4J12% -OutputDirectory nuget -Prop Configuration=Release

ECHO.
ECHO ending with:
dir nuget\. /B