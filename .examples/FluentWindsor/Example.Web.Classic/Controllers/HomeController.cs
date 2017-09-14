using System.Web.Mvc;

namespace Example.Web.Classic.Controllers
{
	public class HomeController : Controller
	{
		private readonly IAnyService anyService;

		public HomeController(IAnyService anyService)
		{
			this.anyService = anyService;
		}

		public ActionResult Index()
		{
			var result = anyService.Anything();
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}