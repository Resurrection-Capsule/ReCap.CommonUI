@echo off
taskkill /f /im dotnet.exe > NUL


echo 'https://github.com/dotnet/sdk/issues/14503' > NUL
dotnet build -c Debug -p:DisableWinExeOutputInference=true
echo Finished building...
cd "bin\Debug\net5.0"
dotnet "ReCap.CommonUI.Demo.dll"


pause
taskkill /f /im dotnet.exe