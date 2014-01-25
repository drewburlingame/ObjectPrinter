@ECHO OFF
SETLOCAL
SET NUGET=src\.nuget\nuget.exe

 
FOR %%G IN (nuget\*.nupkg) DO (
  %NUGET% push %%G
)