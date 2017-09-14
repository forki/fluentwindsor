using Nancy;

namespace FluentlyWindsor.EndersJson.Tests.Framework
{
    public class WebApiModule : NancyModule
    {
        public WebApiModule()
        {
            Get("/", _ => "hello nancy");
        }
    }
}