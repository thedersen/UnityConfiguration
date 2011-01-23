msbuild.exe src\UnityConfiguration.sln /p:Configuration=Release;OutDir=%cd%\bin\
tools\NuGet.exe update
tools\NuGet.exe pack UnityConfiguration.nuspec -o bin\