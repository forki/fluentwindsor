using System.Collections.Generic;
using System.Web.Http;

namespace Example.MVC.Controllers.Api
{
    [RoutePrefix("api/default")]
    public class ThingmeApiController : ApiController
    {
        [HttpGet()]
        [Route("")]
        public List<string> Get()
        {
            return new List<string>()
            {
                "Something",
                "Another Thing",
                "And Another Thing"
            };
        }
    }
}