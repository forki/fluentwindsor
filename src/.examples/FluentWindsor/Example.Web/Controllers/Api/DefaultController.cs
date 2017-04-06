using System.Collections.Generic;
using System.Web.Http;
using Example.Test.AssemblyC;

namespace Example.Web.Controllers.Api
{
    public class DefaultController : ApiController
    {
        private readonly ServiceC _serviceC;

        public DefaultController(ServiceC serviceC)
        {
            _serviceC = serviceC;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            _serviceC.Execute();

            return Ok(
                new List<string>
                {
                    "Item 1",
                    "Item 2",
                    "Item 3"
                });
        }
    }
}