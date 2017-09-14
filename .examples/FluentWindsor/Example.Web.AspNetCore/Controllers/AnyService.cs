using System;

namespace Example.Web.AspNetCore.Controllers
{
	public interface IAnyService
	{
		string Anything();
	}

	public class AnyService : IAnyService
	{
		public string Anything()
		{
			return Guid.NewGuid().ToString("N");
		}
	}
}