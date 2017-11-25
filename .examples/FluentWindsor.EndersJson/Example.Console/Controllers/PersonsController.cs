using Microsoft.AspNetCore.Mvc;

namespace Example.Console.Controllers
{
    [Route("api/[controller]")]
    public class PersonsController : Controller
    {
        [HttpGet]
        public IActionResult Get()
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