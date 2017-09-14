using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using FluentlyWindsor;
using FluentlyWindsor.AspNetCore;
using Example.Web.AspNetCore.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Example.Web.AspNetCore
{
	public class CustomMiddleware
	{
		private readonly IAnyService anyService;

		public CustomMiddleware(IAnyService anyService)
		{
			this.anyService = anyService;
		}

		public async Task Invoke(HttpContext context, Func<Task> next)
		{
			// Do something before
			await next();
			// Do something after
		}
	}

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			FluentWindsor
				.NewContainer(Assembly.GetExecutingAssembly())
				.WithArrayResolver()
				.WithInstallers()
				.RegisterAspNetCoreControllers(services, 
					c => c.Register(
						Component.For<IAnyService>().ImplementedBy<AnyService>(), 
						Component.For<Example.Web.AspNetCore.CustomMiddleware>()
					)).Create();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.Use(async (context, next) =>
			{
				var middleware = FluentWindsor.ServiceLocator.Resolve<Example.Web.AspNetCore.CustomMiddleware>();
				await middleware.Invoke(context, next);
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					"default",
					"{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}