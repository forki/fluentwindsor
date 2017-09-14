using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluentlyWindsor.EndersJson.Tests.Framework
{
	public class Person
    {
        public static readonly Person Any = new Person {Name = "Any", Age = 1};
        public int Age { get; set; }
        public string Name { get; set; }
    }

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