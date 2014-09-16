rm -rf *.nupkg

nuget pack ./FluentWindsor/nuget.nuspec
nuget pack ./FluentWindsor.Mvc/nuget.nuspec
nuget pack ./FluentWindsor.WebApi/nuget.nuspec

nuget push ./FluentWindsor.1.0.0.*.nupkg
nuget push ./FluentWindsor.Mvc.1.0.0.*.nupkg
nuget push ./FluentWindsor.WebApi.1.0.0.*.nupkg
