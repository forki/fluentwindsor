FluentWindsor
=============

<img src="https://raw.githubusercontent.com/cryosharp/fluentwindsor/master/logo.png" align="right" style="border-radius:5px" />


This is a Castle Windsor IoC kick starter for new projects. It scans all the assemblies in your application domain and installs all 
implementations of IWindsorInstaller. This is by far the most mature IoC container out there. Period. Let's hope we see more implementations 
in the wild as result of this :)

##Castle Windsor API

A bit about Castle Windsor.

 - https://github.com/castleproject/Windsor/blob/master/docs/README.md

##How it works

An assembly scanner will trawl through your project trying to find every implementation of 'IWindsorInstaller' that it can. These will be 
registered automatically for you. It is recommended that you have at least one instance of IWindsorInstaller per assembly. For more please visit 
the link below:

 - https://github.com/castleproject/Windsor/blob/master/docs/installers.md

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

 - https://github.com/castleproject/Windsor/blob/master/docs/fluent-registration-api.md

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

##Works with scriptcs!

You can even use Castle Windsor with scriptcs. For more information on how to install scriptcs please navigate to the link below:

 - https://github.com/scriptcs/scriptcs

###Scriptcs example

First dump the following into a temp directory somewhere in a file called 'scriptcs_packages.config'.

	<?xml version="1.0" encoding="utf-8"?>

	<packages>
		<package id="Castle.Core" version="3.3.1" targetFramework="net45" />
		<package id="Castle.Windsor" version="3.3.0" targetFramework="net45" />
		<package id="FluentWindsor" version="1.0.0.34" targetFramework="net45" />
	</packages>


Then dump the following windsor installer into a file called 'windsor-installer.csx'.

	#r "System.dll"
	#r "System.Core.dll"
	#r "Microsoft.CSharp.dll"

	#r "Castle.Core.dll"
	#r "Castle.Windsor.dll"
	#r "FluentWindsor.dll"

	using System;
	using System.Diagnostics;
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.SubSystems.Configuration;
	using Castle.Windsor;

	public interface IAnyService
	{
		void Do();
	}

	public class AnyService : IAnyService
	{
		public void Do()
		{
			Console.WriteLine("IAnyService::Do() - Called");
		}
	}

	public class AnyInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(Component.For<IAnyService>().ImplementedBy<AnyService>().LifeStyle.Transient);
		}
	}


Finally we take this and paste it into a file called 'main.csx'.

    #load "windsor-installer.csx"

    #r "System.dll"
    #r "System.Core.dll"
    #r "Microsoft.CSharp.dll"

    #r "Castle.Core.dll"
    #r "Castle.Windsor.dll"
    #r "FluentWindsor.dll"

    using System;
    using System.Diagnostics;
    using System.Reflection;
    using FluentlyWindsor;

    var container = FluentWindsor.NewContainer(Assembly.GetExecutingAssembly()).WithArrayResolver().WithInstallers().Create();
    container.Resolve<IAnyService>().Do();

Now we have all the component parts to see scriptcs vs Windsor. Lastly run the following commands in a scriptcs installed console.

    scriptcs -install
	scriptcs main.csx

If you see 'IAnyService::Do() - Called' as the final output you know it works.

##Problems?

For any problems please sign into github and raise issues. 
