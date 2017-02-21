.\.paket\paket.bootstrapper.exe
.\.paket\paket.exe install
msbuild  .\.build\BuildTasks\BuildTasks.csproj /p:Configuration=Release
msbuild .build\build-and-deploy.msbuild /p:Properties=build-and-deploy.properties.msbuild /t:tasks\pack
