msbuild .build\build-and-deploy.msbuild /p:Properties=build-and-deploy.properties.v4.5.msbuild
rem git add --all
rem git commit -am " -- Nuget Deploy -- "
rem git push
