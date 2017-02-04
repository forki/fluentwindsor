using System.Web.Http;

namespace Example.Console.Controllers
{
    [RoutePrefix("api/person")]
    public class PersonController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(Person person)
        {
            return Created("api/person/123", person);
        }

        [HttpPut]
        public IHttpActionResult Put(Person person)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            return Ok();
        }
    }
}