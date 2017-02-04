using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Example.Console.Controllers
{
    [RoutePrefix("api/Persons")]
    public class PersonsController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Person>))]
        public IHttpActionResult Get()
        {
            return Ok(new[]
            {
                new Person {Name = "Jon", Age = 20},
                new Person {Name = "Jill", Age = 22},
                new Person {Name = "Jeff", Age = 24}
            });
        }
    }
}