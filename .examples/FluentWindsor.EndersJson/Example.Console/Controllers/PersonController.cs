using Microsoft.AspNetCore.Mvc;

namespace Example.Console.Controllers
{
	[Route("api/[controller]")]
    public class PersonController : Controller
    {
        [HttpPost]
        public IActionResult Post(Person person)
        {
            return Created("api/person/123", person);
        }

        [HttpPut]
        public IActionResult Put(Person person)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}