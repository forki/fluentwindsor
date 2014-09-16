FluentWindsor
=============

This is a windsor castle IoC kick starter for new projects. It scans all the assemblies in your application domain and installs all 
implementations of IWindsorInstaller. This is by far the most mature IoC container out there. Period. Let's hope we see more implementations 
in the wild as result of this :)

##Castle Windsor API

A bit about Castle Windsor.

 - http://docs.castleproject.org/Windsor.MainPage.ashx

##How it works

An assembly scanner will trawl through your project trying to find every implementation of 'IWindsorInstaller' that it can. These will be 
registered automatically for you. It is recommended that you have at least one instance of IWindsorInstaller per assembly. For more please visit 
the link below:

 - http://docs.castleproject.org/Windsor.Installers.ashx

You can also use the MVC and WebApi controller extensions. Examples to follow.

##Sample Service (Some other assembly)

Imagine you have sample service that would like to expose to console, mvc, webapi and nunit applications. Let's say we have ServiceA
for example: 

    public class ServiceA {
		public void Execute(){ /* ... does something ... */ }
	}

And for the same assembly we had the accompanying windsor installation:

    public class Installer : IWindsorInstaller {
		public void Install(IWindsorContainer container, IConfigurationStore store) {
            container.Register(Component.For<ServiceA>().LifeStyle.Transient);
        }
	}

Then by simply adding the reference to our console, mvc, webapi or nunit project we can then proceed on to the fluent registration. Which
auto wires stuff. You can read more about how you register objects using the link below.

 - http://docs.castleproject.org/Windsor.Fluent-Registration-API.ashx

##Fluent General Purpose Registration(NUnit & Console App)

    var container = FluentWindsor
        .NewContainer(Assembly.GetExecutingAssembly())
        .WithArrayResolver()
        .WithInstallers()
        .Create();

    var serviceA = container.Resolve<ServiceA>();

    serviceA.Execute();

##Fluent Mvc & WebApi Registration Including Controllers 

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

##Need a service locator?

No problem, simply do this (you would have to have bootstrapped your application somewhere previously using the examples above). 

	var serviceA = FluentWindsor.ServiceLocator.Resolve<ServiceA>();

##Further Reading

Here is where I think we get to the sales pitch about Castle Windsor. These guys have this nailed.

 - http://docs.castleproject.org/Windsor.Interceptors.ashx

##Problems?

For any problems please sign into github and raise issues. 

##If you are upgrading MVC + WebAPI

This project uses the latest distro of WebApi + MVC. Please see the examples of this source for how to setup your web.config's.

 - https://github.com/fir3pho3nixx/FluentWindsor/blob/master/Example.MVC/Web.config
 - https://github.com/fir3pho3nixx/FluentWindsor/blob/master/Example.MVC/Views/Web.config

 You will have to rebuild your web.config's to re-point to the right versions. It looks painful but I can assure you it is not too 
 bad :)

 Still not happy? Have a moan at ol' Rick here ... 

  - http://www.asp.net/mvc/tutorials/mvc-5/how-to-upgrade-an-aspnet-mvc-4-and-web-api-project-to-aspnet-mvc-5-and-web-api-2

##New to C#?

This project in one way or another supports the following principles:

 - http://en.wikipedia.org/wiki/SOLID_(object-oriented_design)
 - http://en.wikipedia.org/wiki/Single_responsibility_principle
 - http://en.wikipedia.org/wiki/Open/closed_principle
 - http://en.wikipedia.org/wiki/Liskov_substitution_principle
 - http://en.wikipedia.org/wiki/Interface_segregation_principle
 - http://en.wikipedia.org/wiki/Dependency_inversion_principle

A standard practice to most seasoned C# developers in the wild. We are doing the dependency inversion piece here.
