using Nancy;

namespace Example.Console
{
    public class WebApiModule : NancyModule
    {
        public WebApiModule()
        {
            Get["/"] = _ => "hello nancy";
        }
    }
}