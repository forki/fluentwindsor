﻿using System.Web.Http;
using Owin;

namespace Example.Console
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
            appBuilder.UseWebApi(config).UseNancy();
        }
    }
}