using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Example.MVC1.Startup))]

namespace Example.MVC1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
