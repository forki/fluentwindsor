using System.Collections.Generic;
using System.Web.Http;

namespace Example.Web.Classic.Controllers
{
	[RoutePrefix("api/values")]
	public class ValuesController : ApiController
	{
		private readonly IAnyService anyService;

		public ValuesController(IAnyService anyService)
		{
			this.anyService = anyService;
		}

		[HttpGet]
		[Route("")]
		public IEnumerable<string> Get()
		{
			var result = anyService.Anything();
			return new[] { "value1", "value2" };
		}
	}
}