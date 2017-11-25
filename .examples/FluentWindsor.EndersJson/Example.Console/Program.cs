using System.Collections.Generic;
using System.Reflection;
using Example.Console.Controllers;
using FluentlyWindsor;
using FluentlyWindsor.EndersJson.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Example.Console
{
    internal class Program
    {
        private const string BaseUri = "http://localhost:9999";

        private static void Main(string[] args)
        {
            var container = FluentWindsor
                .NewContainer(Assembly.GetExecutingAssembly())
                .WithArrayResolver()
                .WithInstallers()
                .Create();

	        var server = BuildWebHost(args);

			server.Run();

			var json = container.Resolve<ISyncJsonService>();

            var result = json.Get<IEnumerable<Person>>(FormatUri("api/persons"));

            foreach (var item in result)
            {
                System.Console.WriteLine($"Found person! -> {item}");
            }

            System.Console.ReadLine();

            server.Dispose();
        }

	    public static IWebHost BuildWebHost(string[] args) =>
		    WebHost.CreateDefaultBuilder(args)
			    .UseStartup<Startup>()
			    .Build();

		public static string FormatUri(string relativeUri)
        {
            return $"{BaseUri}/{relativeUri}";
        }
    }
}