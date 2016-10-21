@echo off
echo Press any key to publish
pause
".nuget\NuGet.exe" push Yort.Http.ClientPipeline.1.0.0.3.nupkg
pause