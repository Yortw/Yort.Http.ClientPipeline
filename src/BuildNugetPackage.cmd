del /F /Q /S *.CodeAnalysisLog.xml

".nuget\NuGet.exe" pack -sym Yort.Http.ClientPipeline.nuspec -BasePath .\
pause

copy *.nupkg C:\Nuget.LocalRepository\
pause