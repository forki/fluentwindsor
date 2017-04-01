<img align="left" src="https://avatars0.githubusercontent.com/u/7360948?v=3&s=95" />

&nbsp;FluentWindsor<br /><br />
=============

<br/><br/>

| Version | Build |
|---------|---------|
| <a href= "https://www.nuget.org/packages/FluentWindsor/"><img src="https://img.shields.io/nuget/v/FluentWindsor.svg" /></a> | <a href= "https://ci.appveyor.com/project/fir3pho3nixx/fluentwindsor"><img src="https://ci.appveyor.com/api/projects/status/8nj9cgfnw9spqbpr/branch/master?svg=true" /></a> |

This is a Castle Windsor IoC kick starter for new projects. It scans all the assemblies in your application domain and installs all
implementations of IWindsorInstaller. This is by far the most mature IoC container out there. Period. Let's hope we see more implementations
in the wild as result of this :)

We tried to convert this to dotnet core, and found that Castle.Windsor was not there! 

Watch https://github.com/castleproject/Windsor/issues/145 to see how we go.  

## Castle Windsor

A bit about Castle Windsor.

- https://github.com/castleproject/Windsor/blob/master/docs/README.md

## How it works

An assembly scanner will trawl through your project trying to find every implementation of 'IWindsorInstaller' that it can. These will be
registered automatically for you. It is recommended that you have at least one instance of IWindsorInstaller per assembly. For more please visit
the link below:

- https://github.com/castleproject/Windsor/blob/master/docs/installers.md

You can also use the MVC and WebApi controller extensions. Examples to follow.

## Sample Service (Some other assembly)

Imagine you have sample service that would like to expose to console, mvc, webapi and nunit applications. Let's say we have ServiceA
for example:

``` csharp
public class ServiceA {
public void Execute(){ /* ... does something ... */ }
}
```

And for the same assembly we had the accompanying windsor installation:

``` csharp
public class Installer : IWindsorInstaller {
public void Install(IWindsorContainer container, IConfigurationStore store) {
container.Register(Component.For<ServiceA>().LifeStyle.Transient);
    }
}
```

Then by simply adding the reference to our console, mvc, webapi or nunit project we can then proceed on to the fluent registration. Which
auto wires stuff. You can read more about how you register objects using the link below.

 - https://github.com/castleproject/Windsor/blob/master/docs/fluent-registration-api.md

## Fluent General Purpose Registration(NUnit & Console App)

``` csharp
var container = FluentWindsor
    .NewContainer(Assembly.GetExecutingAssembly())
    .WithArrayResolver()
    .WithInstallers()
    .Create();

var serviceA = container.Resolve<ServiceA>();

serviceA.Execute();
```

## Fluent Mvc & WebApi Registration Including Controllers 

``` csharp
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FluentlyWindsor;
using FluentlyWindsor.Mvc;
using FluentlyWindsor.WebApi;

namespace Example.MVC
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			FluentWindsor
				.NewContainer(Assembly.GetExecutingAssembly())
				.WithArrayResolver()
				.WithInstallers()
				.RegisterApiControllers(GlobalConfiguration.Configuration)
				.RegisterMvcControllers(ControllerBuilder.Current, "Example.MVC.Controllers", "Another.Namespace.For.Controllers")
				.Create();
		}
	}
}
```

## Need a service locator?

No problem, simply do this (you would have to have bootstrapped your application somewhere previously using the examples above). 

``` csharp
var serviceA = FluentWindsor.ServiceLocator.Resolve<ServiceA>();
```

## FluentWindsor.Cachely

A naive caching component

[Click here](https://github.com/cryosharp/fluentwindsor/blob/master/FluentWindsor.Cachely/README.md)

## FluentWindsor.EndersJson

A easy to use Json client

[Click here](https://github.com/cryosharp/fluentwindsor/blob/master/FluentWindsor.EndersJson/README.md)

## FluentWindsor.Hawkeye

A logging component based on log4net

[Click here](https://github.com/cryosharp/fluentwindsor/blob/master/FluentWindsor.Hawkeye/README.md)

## Want ScriptCs!

A bit of fun, but might be something useful if you are thinking of using `Azure Functions`. 

[Click here](https://github.com/cryosharp/fluentwindsor/wiki/Works-with-scriptcs!)

## Credit to castle windsor authors  

Credit to the guys behind Castle Windsor.

[Castle Windsor Contributors](https://github.com/castleproject/Windsor/graphs/contributors)

## Problems?

For any problems please sign into github and raise issues. 
