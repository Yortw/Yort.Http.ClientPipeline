@echo off
echo Press any key to publish
pause
".nuget\NuGet.exe" push Yort.Http.ClientPipeline.1.0.0.6.nupkg
pause