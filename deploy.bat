msbuild  .\.build\BuildTasks\BuildTasks.csproj /p:Configuration=Release
msbuild .build\build-and-deploy.msbuild /p:Properties=build-and-deploy.properties.msbuild
git add --all
git commit -am " -- Nuget Deploy -- "
git push
