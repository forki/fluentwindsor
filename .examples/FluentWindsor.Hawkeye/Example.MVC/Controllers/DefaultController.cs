using System.Web.Mvc;

namespace Example.MVC.Controllers
{
    public class DefaultController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}