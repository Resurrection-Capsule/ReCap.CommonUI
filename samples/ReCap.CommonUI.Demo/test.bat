@echo off
taskkill /f /im dotnet.exe > NUL


echo 'https://github.com/dotnet/sdk/issues/14503' > NUL
dotnet build -c Debug
if %ERRORLEVEL% EQU 0 (GOTO :SUCCESS) ELSE GOTO :FAIL


:FAIL
echo Build failed.
GOTO :END


:SUCCESS
echo Finished building.
cd "bin\Debug\net5.0"
dotnet "ReCap.CommonUI.Demo.dll"
GOTO :END


:END
pause
taskkill /f /im dotnet.exe